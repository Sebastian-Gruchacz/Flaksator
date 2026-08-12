using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SharpDevs.Fleksator.Grammar;
using SharpDevs.Helpers.Presentation;

namespace SharpDevs.Fleksator.Edit
{
    public partial class FormEditAdjectives : Form
    {
        public FormEditAdjectives()
        {
            this.InitializeComponent();
        }

        private void FormEditAdjectives_Load(object sender, EventArgs e)
        {
            this.chlistCategories.Items.Clear();
            foreach (int key in WordCategories.Categories.AdjectiveCategories.Keys)
            {
                this.chlistCategories.Items.Add(new Option<int>(key,
                    WordCategories.Categories.AdjectiveCategories[key]));
            }

            this.FilterAll();
        }

        private bool editing = false;

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

            this.DoFilterAdjectives();
        }

        Option<Adjective> selected = null;

        private void listAdjectives_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.editing)
                return;

            this.selected = (Option<Adjective>) this.listAdjectives.SelectedItem;

            if (this.selected != null)
            {
                this.controlEditAdjective1.EditAdjective(this.selected.Key);
            }
            else
                this.controlEditAdjective1.EditAdjective(null);
        }

        private void checkOnlyLevelled_CheckedChanged(object sender, EventArgs e)
        {
            if (this.editing)
                return;

            this.chlistCategories_SelectedIndexChanged(this.chlistCategories, EventArgs.Empty);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (this.editing)
                return;

            this.DoFilterAdjectives();
        }

        private void DoFilterAdjectives()
        {
            // read selected categories
            HashSet<int> usedCategories = new HashSet<int>();
            for (int i = 0; i < this.chlistCategories.Items.Count; i++)
                if (this.chlistCategories.GetItemChecked(i))
                    usedCategories.Add(((Option<int>)this.chlistCategories.Items[i]).Key);

            bool onlyLevelled = this.checkOnlyLevelled.Checked;
            string maskText = this.textBox1.Text;
            bool maskFilter = maskText.Length > 0;
            Adjective selectedAdjective = this.selected == null ? null : this.selected.Key;
            Option<Adjective> selectedOption = null;
            bool wasEditing = this.editing;

            this.editing = true;
            this.listAdjectives.BeginUpdate();
            try
            {
                this.listAdjectives.Items.Clear();
                foreach (Adjective adj in AdjectiveCollection.Collection.Adjectives)
                {
                    if (onlyLevelled && !adj.CanBeLevelled)
                        continue;

                    string displayText = DictionaryDisplayText.ForAdjective(adj);
                    if (maskFilter && displayText.IndexOf(maskText, StringComparison.OrdinalIgnoreCase) < 0)
                        continue;

                    bool validCat = adj.Categories.Count == 0;
                    if (!validCat)
                        foreach (int category in adj.Categories)
                            if (usedCategories.Contains(category))
                            {
                                validCat = true;
                                break;
                            }

                    if (validCat)
                    {
                        Option<Adjective> option = new Option<Adjective>(adj, displayText);
                        this.listAdjectives.Items.Add(option);
                        if (adj == selectedAdjective)
                            selectedOption = option;
                    }
                }

                this.listAdjectives.SelectedItem = selectedOption;
                this.selected = selectedOption;
            }
            finally
            {
                this.listAdjectives.EndUpdate();
                this.editing = wasEditing;
            }

            if (selectedAdjective != null && selectedOption == null)
                this.controlEditAdjective1.EditAdjective(null);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            this.editing = true;
            base.OnFormClosing(e);
            if (e.Cancel)
                this.editing = false;
        }
    }
}