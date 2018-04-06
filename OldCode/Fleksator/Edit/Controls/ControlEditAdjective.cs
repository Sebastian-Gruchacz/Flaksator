using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using SharpDevs.Fleksator.Grammar;
using SharpDevs.Helpers.Presentation;

namespace SharpDevs.Fleksator.Edit.Controls
{
    public partial class ControlEditAdjective : UserControl
    {
        public ControlEditAdjective()
        {
            this.InitializeComponent();
        }

        void LoadCasesControls(Panel targetPanel, DecliantionNumber amount, AdjectiveLevel level)
        {
            targetPanel.Controls.Clear();

            Array arr = Enum.GetValues(typeof(InflectionCase));
            foreach (InflectionCase aCase in arr)
            {
                if (aCase == InflectionCase._Unknown)
                    continue;

                ControlAdjectiveCaseEdit edit = new ControlAdjectiveCaseEdit();
                edit.Dock = DockStyle.Bottom;
                edit.Case = aCase;
                edit.Value = "";
                edit.DecliantionNumber = amount;
                edit.InflectionCase = aCase;
                edit.Level = level;
                // edit.PostfixButtonPressed += new EventHandler(edit_PostfixButtonPressed);
                // edit.IrregularSet += new EventHandler(edit_IrregularSet);

                targetPanel.Controls.Add(edit);
            }
        }

        void LoadCategories()
        {
            this.chlistCategories.Items.Clear();
            foreach (int key in WordCategories.Categories.AdjectiveCategories.Keys)
            {
                this.chlistCategories.Items.Add(new Option<int>(key,
                    WordCategories.Categories.AdjectiveCategories[key]));
            }
        }

        void LoadGenres()
        {
            this.cboGenres.Items.Clear();

            Array arr = Enum.GetValues(typeof(GrammaticalGender));
            foreach (GrammaticalGender aCase in arr)
            {
                if (aCase == GrammaticalGender._Unknown)
                    continue;

                this.cboGenres.Items.Add(new Option<GrammaticalGender>(aCase, aCase.ToString()));
            }
        }

        private void ControlEditAdjective_Load(object sender, EventArgs e)
        {
            this.LoadCasesControls(this.panelSingularEqual, DecliantionNumber.Singular, AdjectiveLevel.Equal);
            this.LoadCasesControls(this.panelPluralEqual, DecliantionNumber.Plural, AdjectiveLevel.Equal);
            this.LoadCasesControls(this.panelSingularHigher, DecliantionNumber.Singular, AdjectiveLevel.Higher);
            this.LoadCasesControls(this.panelPluralHigher, DecliantionNumber.Plural, AdjectiveLevel.Higher);
            this.LoadCasesControls(this.panelSingularHighest, DecliantionNumber.Singular, AdjectiveLevel.Highest);
            this.LoadCasesControls(this.panelPluralHighest, DecliantionNumber.Plural, AdjectiveLevel.Highest);

            this.LoadCategories();
            this.LoadGenres();
        }

        private Adjective edited = null;

        public void EditAdjective(Adjective adj)
        {
            this.edited = adj;

            if (adj == null)
            {
                this.eRoot.Text = "";
                this.eRootHigher.Text = "";
                this.eRootHighest.Text = "";
                this.checkConstant.Checked = false;
                this.checkOnlyEqualLevel.Checked = false;

            }
            else
            {
                this.RefreshFields();
            }
        }

        private void RefreshFields()
        {
            // "loose" fields
            this.eRoot.Text = this.edited.Root;
            this.eRootHigher.Text = this.edited.LevelHigherForm;
            this.eRootHighest.Text = this.edited.LevelHighestForm;
            this.checkConstant.Checked = this.edited.IsConstant;
            this.checkOnlyEqualLevel.Checked = !this.edited.CanBeLevelled;

            // categories
            for (int i = 0; i < this.chlistCategories.Items.Count; i++)
            {
                if (this.edited.Categories.Contains(((Option<int>) this.chlistCategories.Items[i]).Key))
                    this.chlistCategories.SetItemChecked(i, true);
                else
                    this.chlistCategories.SetItemChecked(i, false);
            }

            // Forms
            if (this.cboGenres.Items.Count > 0 && this.cboGenres.SelectedIndex == -1)
                this.cboGenres.SelectedIndex = 0; // select first
            else
                this.cboGenres_SelectedIndexChanged(this.cboGenres, EventArgs.Empty); // dip0lsay for selected genre
        }

        private void cboGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            Option<GrammaticalGender> opt = (Option<GrammaticalGender>) this.cboGenres.SelectedItem;
            if (opt != null)
            {
                GrammaticalGender genre = opt.Key;

                this.SetEdited(this.panelSingularEqual, this.edited, genre);
                this.SetEdited(this.panelPluralEqual, this.edited, genre);
                this.SetEdited(this.panelSingularHigher, this.edited, genre);
                this.SetEdited(this.panelPluralHigher, this.edited, genre);
                this.SetEdited(this.panelSingularHighest, this.edited, genre);
                this.SetEdited(this.panelPluralHighest, this.edited, genre);
            }
            else
            {
                this.SetEdited(this.panelSingularEqual, null, GrammaticalGender._Unknown);
                this.SetEdited(this.panelPluralEqual, null, GrammaticalGender._Unknown);
                this.SetEdited(this.panelSingularHigher, null, GrammaticalGender._Unknown);
                this.SetEdited(this.panelPluralHigher, null, GrammaticalGender._Unknown);
                this.SetEdited(this.panelSingularHighest, null, GrammaticalGender._Unknown);
                this.SetEdited(this.panelPluralHighest, null, GrammaticalGender._Unknown);
            }

        }

        private void SetEdited(Panel targetPanel, Adjective adj, GrammaticalGender genre)
        {
            foreach (Control ctrl in targetPanel.Controls)
            {
                if (!(ctrl is ControlAdjectiveCaseEdit))
                    continue;

                ControlAdjectiveCaseEdit edit = ctrl as ControlAdjectiveCaseEdit;
                if (adj == null)
                    edit.Value = "";
                else
                {
                    string form = AdjectiveDecliner.Decliner.MakeWord(
                        adj, genre, edit.InflectionCase, edit.DecliantionNumber, edit.Level);
                    edit.Value = form;
                }
            }
        }
    }
}
