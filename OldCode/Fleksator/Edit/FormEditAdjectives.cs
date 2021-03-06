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
            List<int> usedCategories = new List<int>();
            for (int i = 0; i < this.chlistCategories.Items.Count; i++)
                if (this.chlistCategories.GetItemChecked(i))
                    usedCategories.Add(((Option<int>)this.chlistCategories.Items[i]).Key);

            bool onlyLevelled = this.checkOnlyLevelled.Checked;
            string maskText = this.textBox1.Text.ToLower();
            bool maskFilter = maskText.Length > 0;

            // Filter out adjectives
            this.listAdjectives.Items.Clear();
            foreach (Adjective adj in AdjectiveCollection.Collection.Adjectives)
            {
                // skip Adjectives that cannot be levelled when user specifies so
                if (onlyLevelled && !adj.CanBeLevelled)
                    continue;

                //skip adjcectives that do not contain specified substring
                if (maskFilter && !adj.Root.ToLower().Contains(maskText))
                    continue;

                // Display Adjective only if valid category is selected
                bool validCat = false;
                if (adj.Categories.Count == 0)
                    validCat = true; // not assigned Adj's are always displayed
                else
                    foreach (int category in adj.Categories)
                        if (usedCategories.Contains(category))
                        {
                            validCat = true;
                            break;
                        }

                if (validCat)
                {
                    this.listAdjectives.Items.Add(new Option<Adjective>
                        (adj, adj.Root));
                }
            }
        }
    }
}