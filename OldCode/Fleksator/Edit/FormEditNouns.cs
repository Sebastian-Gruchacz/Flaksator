using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SharpDevs.Fleksator.Grammar;
using SharpDevs.Fleksator.IO;
using SharpDevs.Helpers.Presentation;

namespace SharpDevs.Fleksator.Edit
{
    public partial class FormEditNouns : Form
    {
        public FormEditNouns()
        {
            this.InitializeComponent();            
        }

        private bool editing = false;

        private IGrammarSerializers _grammarSerializers;

        private void FormEditNouns_Load(object sender, EventArgs e)
        {
            // workaround - let it work
            this._grammarSerializers = new GrammarSerializersFactory().GetOldSerializers();

            this.chlistCategories.Items.Clear();
            foreach (int key in WordCategories.Categories.NounCategories.Keys)
            {
                this.chlistCategories.Items.Add(new Option<int>(key,
                    WordCategories.Categories.NounCategories[key]));
            }

            this.FilterAll();
        }

        private void FilterAll()
        {
            this.editing = true;

            for (int i = 0; i < this.chlistCategories.Items.Count; i++)
                this.chlistCategories.SetItemChecked(i, true);

            this.editing = false;

            this.chlistCategories_SelectedIndexChanged(this.chlistCategories, EventArgs.Empty);
        }

        private void chlistCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.editing)
                return;

            this.DoFilterNouns();
        }

        Noun selectedNoun = null;
        Noun tmp = null;

        private void listNouns_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.editing)
                return;

            Option<Noun> selected = (Option<Noun>) this.listNouns.SelectedItem;

            if (selected != null)
            {
                if (selected.Key == this.selectedNoun)
                    return;

                this.SavePendingNoun(true);

                this.selectedNoun = selected.Key;

                this.tmp = this._grammarSerializers.NounSerializer.Load(this._grammarSerializers.NounSerializer.Write(this.selectedNoun)); // clone...

                this.controlEditNoun.EditNoun(this.tmp);

            }
            else
            {
                this.SavePendingNoun(false);
                this.selectedNoun = null;
                this.tmp = null;
                this.controlEditNoun.EditNoun(null);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (this.editing)
                return;

            this.DoFilterNouns();
        }

        private void DoFilterNouns()
        {
            this.SavePendingNoun(false);

            // read selected categories
            HashSet<int> usedCategories = new HashSet<int>();
            for (int i = 0; i < this.chlistCategories.Items.Count; i++)
                if (this.chlistCategories.GetItemChecked(i))
                    usedCategories.Add(((Option<int>)this.chlistCategories.Items[i]).Key);

            string maskText = this.textBox1.Text;
            bool maskFilter = maskText.Length > 0;
            Option<Noun> selectedOption = null;
            bool wasEditing = this.editing;

            this.editing = true;
            this.listNouns.BeginUpdate();
            try
            {
                this.listNouns.Items.Clear();
                foreach (Noun noun in NounCollection.Collection.Nouns)
                {
                    string displayText = DictionaryDisplayText.ForNoun(noun);
                    if (maskFilter && displayText.IndexOf(maskText, StringComparison.OrdinalIgnoreCase) < 0)
                        continue;

                    bool validCat = noun.Categories.Count == 0;
                    if (!validCat)
                        foreach (int category in noun.Categories)
                            if (usedCategories.Contains(category))
                            {
                                validCat = true;
                                break;
                            }

                    if (validCat)
                    {
                        Option<Noun> option = new Option<Noun>(noun, displayText);
                        this.listNouns.Items.Add(option);
                        if (noun == this.selectedNoun)
                            selectedOption = option;
                    }
                }

                this.listNouns.SelectedItem = selectedOption;
            }
            finally
            {
                this.listNouns.EndUpdate();
                this.editing = wasEditing;
            }

            if (this.selectedNoun != null && selectedOption == null)
            {
                this.selectedNoun = null;
                this.tmp = null;
                this.controlEditNoun.EditNoun(null);
            }
        }

        private void SavePendingNoun(bool updateDisplay)
        {
            if (this.selectedNoun == null || this.tmp == null)
                return;

            this.selectedNoun.AnalyzeLine(this.tmp.WriteNoun());
            if (updateDisplay)
                this.UpdateNounDisplay(this.selectedNoun);
        }

        private void UpdateNounDisplay(Noun noun)
        {
            for (int i = 0; i < this.listNouns.Items.Count; i++)
            {
                Option<Noun> option = (Option<Noun>) this.listNouns.Items[i];
                if (option.Key != noun)
                    continue;

                string displayText = DictionaryDisplayText.ForNoun(noun);
                if (option.Text != displayText)
                {
                    bool wasEditing = this.editing;
                    this.editing = true;
                    this.listNouns.Items[i] = new Option<Noun>(noun, displayText);
                    this.editing = wasEditing;
                }
                break;
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            this.SavePendingNoun(false);
            this.editing = true;
            base.OnFormClosing(e);
            if (e.Cancel)
                this.editing = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.selectedNoun != null && this.tmp != null)
            {
                // save changes in existing noun
                this.SavePendingNoun(false);
                this.DoFilterNouns();
            }

            this.selectedNoun = new Noun();
            this.selectedNoun.Root = "<nowy>";
            this.tmp = this.selectedNoun;
            NounCollection.Collection.Nouns.Add(this.selectedNoun);
            this.controlEditNoun.EditNoun(this.tmp);

            this.DoFilterNouns();

            foreach (Option<Noun> opt in this.listNouns.Items)
                if (opt.Key == this.selectedNoun)
                {
                    this.listNouns.SelectedItem = opt;
                    break;
                }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // cancle changes
            this.tmp.AnalyzeLine(this.selectedNoun.WriteNoun());
            this.controlEditNoun.EditNoun(this.tmp);
        }

        private void bDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Na pewno?", "Usuwanie rzeczownika", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                NounCollection.Collection.Nouns.Remove(this.selectedNoun);
                this.controlEditNoun.EditNoun(null);

                this.selectedNoun = null;
                this.tmp = null;

                this.DoFilterNouns();
            }
        }
    }
}