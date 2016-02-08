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
    public partial class ControlEditNoun : UserControl
    {
        EnumTranslator translator = EnumTranslator.GetTranslator();

        public ControlEditNoun()
        {
            InitializeComponent();            
        }

        private void ControlEditNoun_Load(object sender, EventArgs e)
        {
            LoadCasesControls(this.panelSingular, DecliantionNumber.Singular);
            LoadCasesControls(this.panelPlural, DecliantionNumber.Plural);

            LoadCategories();
            LoadGenres(this.cboGenres, false);
            LoadGenres(this.cboGenresIrregular, true);
        }

        void LoadGenres(ComboBox cbo, bool addUnknown)
        {
            if (translator == null)
                return;

            cbo.Items.Clear();
            
            Array arr = Enum.GetValues(typeof(GrammaticalGender));
            foreach (GrammaticalGender aCase in arr)
            {
                if (!addUnknown && aCase == GrammaticalGender._Unknown)
                    continue;

                cbo.Items.Add(new Option<GrammaticalGender>(aCase, translator.TranslateWordGenre(aCase)));
            }
        }

        void LoadCategories()
        {
            this.chlistCategories.Items.Clear();
            foreach (int key in WordCategories.Categories.NounCategories.Keys)
            {
                this.chlistCategories.Items.Add(new Option<int>(key,
                    WordCategories.Categories.NounCategories[key]));
            }
        }

        void LoadCasesControls(Panel targetPanel, DecliantionNumber amount)
        {
            targetPanel.Controls.Clear();

            Array arr = Enum.GetValues(typeof(InflectionCase));
            foreach (InflectionCase aCase in arr)
            {
                if (aCase == InflectionCase._Unknown)
                    continue;

                ControlNounCaseEdit edit = new ControlNounCaseEdit();
                edit.Dock = DockStyle.Bottom;
                edit.Case = aCase;
                edit.Value = "";
                edit.DecliantionNumber = amount;
                edit.InflectionCase = aCase;
                edit.PostfixButtonPressed += new EventHandler(edit_PostfixButtonPressed);
                edit.IrregularSet += new EventHandler(edit_IrregularSet);

                targetPanel.Controls.Add(edit);
            }
        }

        void edit_PostfixButtonPressed(object sender, EventArgs e)
        {
            ControlNounCaseEdit edit = (ControlNounCaseEdit)sender;
            NounGrammar.SetPostIndex(this.edited, edit.InflectionCase, edit.DecliantionNumber, edit.PostFixIndex);
            RefreshFields();
        }

        void edit_IrregularSet(object sender, EventArgs e)
        {
            ControlNounCaseEdit edit = (ControlNounCaseEdit)sender;
            WordToken token = new WordToken(edit.Value, edit.InflectionCase, edit.DecliantionNumber);
            NounGrammar.UpdateIrregular(this.edited, token);
            RefreshFields();
        }

        private Noun edited = null;

        public void EditNoun(Noun noun)
        {
            edited = noun;
            

            if (noun == null)
            {
                editing = true;

                this.eRoot.Text = "";
                this.eRootIrregular.Text = "";
                this.checkConstant.Checked = false;
                this.checkNoPlural.Checked = false;
                this.checkNoSingular.Checked = false;

                editing = false;
            }
            else
            {
                RefreshFields();
            }

        }

        private bool editing = false;

        private void RefreshFields()
        {
            editing = true;

            // "loose" fields
            this.eRoot.Text = edited.Root;
            this.eRootIrregular.Text = edited.RootOther;
            this.checkConstant.Checked = edited.IsConstant;
            this.checkNoPlural.Checked = !edited.CanBePlural;
            this.checkNoSingular.Checked = !edited.CanBeSingular;

            // categories
            for (int i = 0; i < chlistCategories.Items.Count; i++)
            {
                if (edited.Categories.Contains(((Option<int>)chlistCategories.Items[i]).Key))
                    chlistCategories.SetItemChecked(i, true);
                else
                    chlistCategories.SetItemChecked(i, false);
            }

            // Forms
            cboGenres.SelectedIndex = -1;
            for (int i = 0; i < cboGenres.Items.Count; i++)
            {
                if (((Option<GrammaticalGender>)cboGenres.Items[i]).Key == edited.Genre)
                {
                    cboGenres.SelectedIndex = i;                    
                    break;
                }
            }

            if (edited.HasIrregularGenre)
            {
                for (int i = 0; i < cboGenresIrregular.Items.Count; i++)
                {
                    if (((Option<GrammaticalGender>)cboGenresIrregular.Items[i]).Key == edited.IrregularGenre)
                    {
                        cboGenresIrregular.SelectedIndex = i;
                        break;
                    }
                }
            }
            else
                cboGenresIrregular.SelectedIndex = -1;

            cboPostfixes.SelectedIndex = -1;
            for (int i = 0; i < cboPostfixes.Items.Count; i++)
            {
                if ((string)cboPostfixes.Items[i] == edited.DeclinationType)
                {
                    cboPostfixes.SelectedIndex = i;
                    break;
                }
            }

            editing = false;

            cboGenres_SelectedIndexChanged(cboGenres, EventArgs.Empty);
        }

        private void cboGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (editing)
                return;

            Option<GrammaticalGender> opt = (Option<GrammaticalGender>)cboGenres.SelectedItem;
            if (opt != null)
            {
                edited.Genre = opt.Key;

                SetEdited(this.panelSingular, edited);
                SetEdited(this.panelPlural, edited);
            }
            else
            {
                SetEdited(this.panelSingular, null);
                SetEdited(this.panelPlural, null);
            }
        }

        private void cboGenresIrregular_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (editing)
                return;

            Option<GrammaticalGender> opt = (Option<GrammaticalGender>)cboGenresIrregular.SelectedItem;
            if (opt != null)
            {
                edited.IrregularGenre = opt.Key;
                edited.HasIrregularGenre = edited.IrregularGenre != GrammaticalGender._Unknown;
            }
            else
            {
                edited.IrregularGenre = GrammaticalGender._Unknown;
                edited.HasIrregularGenre = false;
            }
        }

        private void SetEdited(Panel targetPanel, Noun noun)
        {
            foreach (Control ctrl in targetPanel.Controls)
            {
                if (!(ctrl is ControlNounCaseEdit))
                    continue;

                ControlNounCaseEdit edit = ctrl as ControlNounCaseEdit;
                if (noun == null)
                    edit.Value = "";
                else
                {
                    string form = NounDecliner.Decliner.MakeWord(
                        noun, edit.InflectionCase, edit.DecliantionNumber);
                    edit.Value = form;
                    edit.PostFixes = NounDecliner.Decliner.GetPostFix(noun.Genre, noun.DeclinationType,
                        edit.InflectionCase, DecliantionNumber.Singular).Split(' ');
                    edit.PostFixIndex = NounGrammar.GetPostFixIndex(noun, DecliantionNumber.Singular, edit.InflectionCase);
                }
            }
        }

        private void bAdjectiveTest_Click(object sender, EventArgs e)
        {
            FormTestNounAndAdjective frm = new FormTestNounAndAdjective();
            frm.SetNoun(this.edited);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                if (edited.HasIrregularGenre)
                {
                    for (int i = 0; i < cboGenresIrregular.Items.Count; i++)
                    {
                        if (((Option<GrammaticalGender>)cboGenresIrregular.Items[i]).Key == edited.IrregularGenre)
                        {
                            cboGenresIrregular.SelectedIndex = i;
                            cboGenresIrregular_SelectedIndexChanged(cboGenresIrregular, EventArgs.Empty);
                            break;
                        }
                    }
                }
            }
        }

        private void eRoot_TextChanged(object sender, EventArgs e)
        {
            if (editing)
                return;

            edited.Root = eRoot.Text;
            cboGenres_SelectedIndexChanged(cboGenres, EventArgs.Empty);
        }

        private void eRootIrregular_TextChanged(object sender, EventArgs e)
        {
            if (editing)
                return;

            edited.RootOther = eRootIrregular.Text;
            cboGenres_SelectedIndexChanged(cboGenres, EventArgs.Empty);
        }

        private void cboPostfixes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (editing)
                return;

            edited.DeclinationType = (string)cboPostfixes.SelectedItem;
            cboGenres_SelectedIndexChanged(cboGenres, EventArgs.Empty);
        }

        private void chlistCategories_Click(object sender, EventArgs e)
        {
            if (editing)
                return;

            this.edited.Categories.Clear();
            foreach (Option<int> opt in this.chlistCategories.CheckedItems)
                this.edited.Categories.Add(opt.Key);
        }

        private void chlistCategories_Leave(object sender, EventArgs e)
        {
            if (editing)
                return;

            this.edited.Categories.Clear();
            foreach (Option<int> opt in this.chlistCategories.CheckedItems)
                this.edited.Categories.Add(opt.Key);
        }
    }
}
