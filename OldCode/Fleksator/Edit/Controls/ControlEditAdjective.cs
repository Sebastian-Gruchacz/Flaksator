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
            InitializeComponent();
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
            LoadCasesControls(this.panelSingularEqual, DecliantionNumber.Singular, AdjectiveLevel.Equal);
            LoadCasesControls(this.panelPluralEqual, DecliantionNumber.Plural, AdjectiveLevel.Equal);
            LoadCasesControls(this.panelSingularHigher, DecliantionNumber.Singular, AdjectiveLevel.Higher);
            LoadCasesControls(this.panelPluralHigher, DecliantionNumber.Plural, AdjectiveLevel.Higher);
            LoadCasesControls(this.panelSingularHighest, DecliantionNumber.Singular, AdjectiveLevel.Highest);
            LoadCasesControls(this.panelPluralHighest, DecliantionNumber.Plural, AdjectiveLevel.Highest);

            LoadCategories();
            LoadGenres();
        }

        private Adjective edited = null;

        public void EditAdjective(Adjective adj)
        {
            edited = adj;

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
                RefreshFields();
            }
        }

        private void RefreshFields()
        {
            // "loose" fields
            this.eRoot.Text = edited.Root;
            this.eRootHigher.Text = edited.LevelHigherForm;
            this.eRootHighest.Text = edited.LevelHighestForm;
            this.checkConstant.Checked = edited.IsConstant;
            this.checkOnlyEqualLevel.Checked = !edited.CanBeLevelled;

            // categories
            for (int i = 0; i < chlistCategories.Items.Count; i++)
            {
                if (edited.Categories.Contains(((Option<int>)chlistCategories.Items[i]).Key))
                    chlistCategories.SetItemChecked(i, true);
                else
                    chlistCategories.SetItemChecked(i, false);
            }

            // Forms
            if (cboGenres.Items.Count > 0 && cboGenres.SelectedIndex == -1)
                cboGenres.SelectedIndex = 0; // select first
            else
                cboGenres_SelectedIndexChanged(cboGenres, EventArgs.Empty); // dip0lsay for selected genre
        }

        private void cboGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            Option<GrammaticalGender> opt = (Option<GrammaticalGender>)cboGenres.SelectedItem;
            if (opt != null)
            {
                GrammaticalGender genre = opt.Key;

                SetEdited(this.panelSingularEqual, edited, genre);
                SetEdited(this.panelPluralEqual, edited, genre);
                SetEdited(this.panelSingularHigher, edited, genre);
                SetEdited(this.panelPluralHigher, edited, genre);
                SetEdited(this.panelSingularHighest, edited, genre);
                SetEdited(this.panelPluralHighest, edited, genre);
            }
            else
            {
                SetEdited(this.panelSingularEqual, null, GrammaticalGender._Unknown);
                SetEdited(this.panelPluralEqual, null, GrammaticalGender._Unknown);
                SetEdited(this.panelSingularHigher, null, GrammaticalGender._Unknown);
                SetEdited(this.panelPluralHigher, null, GrammaticalGender._Unknown);
                SetEdited(this.panelSingularHighest, null, GrammaticalGender._Unknown);
                SetEdited(this.panelPluralHighest, null, GrammaticalGender._Unknown);
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
