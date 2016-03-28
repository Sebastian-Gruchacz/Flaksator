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
            InitializeComponent();            
        }

        private bool editing = false;

        private IGrammarSerializers _grammarSerializers;

        private void FormEditNouns_Load(object sender, EventArgs e)
        {
            // workaround - let it work
            _grammarSerializers = new GrammarSerializersFactory().GetOldSerializers();

            this.chlistCategories.Items.Clear();
            foreach (int key in WordCategories.Categories.NounCategories.Keys)
            {
                this.chlistCategories.Items.Add(new Option<int>(key,
                    WordCategories.Categories.NounCategories[key]));
            }

            FilterAll();
        }

        private void FilterAll()
        {
            editing = true;

            for (int i = 0; i < this.chlistCategories.Items.Count; i++)
                this.chlistCategories.SetItemChecked(i, true);

            editing = false;

            chlistCategories_SelectedIndexChanged(this.chlistCategories, EventArgs.Empty);
        }

        private void chlistCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (editing)
                return;

            DoFilterNouns();
        }

        Noun selectedNoun = null;
        Noun tmp = null;

        private void listNouns_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (editing)
                return;

            Option<Noun> selected = (Option<Noun>)listNouns.SelectedItem;

            if (selected != null)
            {
                if (selectedNoun != null && tmp != null)
                {
                    // save changes
                    selectedNoun.AnalyzeLine(tmp.WriteNoun());

                    // refresh display correctly   
                    int indx = listNouns.SelectedIndex;
                    editing = true;
                    DoFilterNouns();                  
                    listNouns.SelectedIndex = indx;
                    editing = false;
                }

                selectedNoun = selected.Key;

                tmp = _grammarSerializers.NounSerializer.Load(_grammarSerializers.NounSerializer.Write(selectedNoun)); // clone...

                this.controlEditNoun.EditNoun(tmp);

            }
            else
                this.controlEditNoun.EditNoun(null);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (editing)
                return;

            DoFilterNouns();
        }

        private void DoFilterNouns()
        {
            // read selected categories
            List<int> usedCategories = new List<int>();
            for (int i = 0; i < this.chlistCategories.Items.Count; i++)
                if (this.chlistCategories.GetItemChecked(i))
                    usedCategories.Add(((Option<int>)this.chlistCategories.Items[i]).Key);

            string maskText = this.textBox1.Text.ToLower();
            bool maskFilter = maskText.Length > 0;

            // Filter out adjectives
            listNouns.Items.Clear();
            foreach (Noun noun in NounCollection.Collection.Nouns)
            {
                //skip nouns that do not contain specified substring
                if (maskFilter && !noun.Root.ToLower().Contains(maskText))
                    continue;

                // Display Noun only if valid category is selected
                bool validCat = false;
                if (noun.Categories.Count == 0)
                    validCat = true; // not assigned Adj's are always displayed
                else
                    foreach (int category in noun.Categories)
                        if (usedCategories.Contains(category))
                        {
                            validCat = true;
                            break;
                        }

                if (validCat)
                {
                    listNouns.Items.Add(new Option<Noun>
                        (noun, noun.Root));
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (selectedNoun != null && tmp != null)
            {
                // save changes in existing noun
                selectedNoun.AnalyzeLine(tmp.WriteNoun());
                DoFilterNouns();
            }

            selectedNoun = new Noun();
            selectedNoun.Root = "<nowy>";
            tmp = selectedNoun;
            NounCollection.Collection.Nouns.Add(selectedNoun);
            this.controlEditNoun.EditNoun(tmp);

            DoFilterNouns();

            foreach (Option<Noun> opt in this.listNouns.Items)
                if (opt.Key == selectedNoun)
                {
                    this.listNouns.SelectedItem = opt;
                    break;
                }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // cancle changes
            tmp.AnalyzeLine(selectedNoun.WriteNoun());
            this.controlEditNoun.EditNoun(tmp);
        }

        private void bDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Na pewno?", "Usuwanie rzeczownika", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                NounCollection.Collection.Nouns.Remove(selectedNoun);
                this.controlEditNoun.EditNoun(null);

                selectedNoun = null;
                tmp = null;

                DoFilterNouns();
            }
        }
    }
}