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
            this.InitializeComponent();            
        }

        private void ControlEditNoun_Load(object sender, EventArgs e)
        {
            this.LoadCasesControls(this.panelSingular, DecliantionNumber.Singular);
            this.LoadCasesControls(this.panelPlural, DecliantionNumber.Plural);

            this.LoadCategories();
            this.LoadGenres(this.cboGenres, false);
            this.LoadGenres(this.cboGenresIrregular, true);
        }

        void LoadGenres(ComboBox cbo, bool addUnknown)
        {
            if (this.translator == null)
                return;

            cbo.Items.Clear();
            
            Array arr = Enum.GetValues(typeof(GrammaticalGender));
            foreach (GrammaticalGender aCase in arr)
            {
                if (!addUnknown && aCase == GrammaticalGender._Unknown)
                    continue;

                cbo.Items.Add(new Option<GrammaticalGender>(aCase, this.translator.TranslateWordGenre(aCase)));
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
                edit.PostfixButtonPressed += new EventHandler(this.edit_PostfixButtonPressed);
                edit.IrregularSet += new EventHandler(this.edit_IrregularSet);

                targetPanel.Controls.Add(edit);
            }
        }

        void edit_PostfixButtonPressed(object sender, EventArgs e)
        {
            ControlNounCaseEdit edit = (ControlNounCaseEdit)sender;
            NounGrammar.SetPostIndex(this.edited, edit.InflectionCase, edit.DecliantionNumber, edit.PostFixIndex);
            this.RefreshFields();
        }

        void edit_IrregularSet(object sender, EventArgs e)
        {
            ControlNounCaseEdit edit = (ControlNounCaseEdit)sender;
            WordToken token = new WordToken(edit.Value, edit.InflectionCase, edit.DecliantionNumber);
            NounGrammar.UpdateIrregular(this.edited, token);
            this.RefreshFields();
        }

        private Noun edited = null;

        public void EditNoun(Noun noun)
        {
            this.edited = noun;
            

            if (noun == null)
            {
                this.editing = true;

                this.eRoot.Text = "";
                this.eRootIrregular.Text = "";
                this.checkConstant.Checked = false;
                this.checkNoPlural.Checked = false;
                this.checkNoSingular.Checked = false;

                this.editing = false;
            }
            else
            {
                this.RefreshFields();
            }

        }

        private bool editing = false;

        private void RefreshFields()
        {
            this.editing = true;

            // "loose" fields
            this.eRoot.Text = this.edited.Root;
            this.eRootIrregular.Text = this.edited.RootOther;
            this.checkConstant.Checked = this.edited.IsConstant;
            this.checkNoPlural.Checked = !this.edited.CanBePlural;
            this.checkNoSingular.Checked = !this.edited.CanBeSingular;

            // categories
            for (int i = 0; i < this.chlistCategories.Items.Count; i++)
            {
                if (this.edited.Categories.Contains(((Option<int>) this.chlistCategories.Items[i]).Key))
                    this.chlistCategories.SetItemChecked(i, true);
                else
                    this.chlistCategories.SetItemChecked(i, false);
            }

            // Forms
            this.cboGenres.SelectedIndex = -1;
            for (int i = 0; i < this.cboGenres.Items.Count; i++)
            {
                if (((Option<GrammaticalGender>) this.cboGenres.Items[i]).Key == this.edited.Genre)
                {
                    this.cboGenres.SelectedIndex = i;                    
                    break;
                }
            }

            if (this.edited.HasIrregularGenre)
            {
                for (int i = 0; i < this.cboGenresIrregular.Items.Count; i++)
                {
                    if (((Option<GrammaticalGender>) this.cboGenresIrregular.Items[i]).Key == this.edited.IrregularGenre)
                    {
                        this.cboGenresIrregular.SelectedIndex = i;
                        break;
                    }
                }
            }
            else
                this.cboGenresIrregular.SelectedIndex = -1;

            this.cboPostfixes.SelectedIndex = -1;
            for (int i = 0; i < this.cboPostfixes.Items.Count; i++)
            {
                if ((string) this.cboPostfixes.Items[i] == this.edited.DeclinationType)
                {
                    this.cboPostfixes.SelectedIndex = i;
                    break;
                }
            }

            this.editing = false;

            this.cboGenres_SelectedIndexChanged(this.cboGenres, EventArgs.Empty);
        }

        private void cboGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.editing)
                return;

            Option<GrammaticalGender> opt = (Option<GrammaticalGender>) this.cboGenres.SelectedItem;
            if (opt != null)
            {
                this.edited.Genre = opt.Key;

                this.SetEdited(this.panelSingular, this.edited);
                this.SetEdited(this.panelPlural, this.edited);
            }
            else
            {
                this.SetEdited(this.panelSingular, null);
                this.SetEdited(this.panelPlural, null);
            }
        }

        private void cboGenresIrregular_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.editing)
                return;

            Option<GrammaticalGender> opt = (Option<GrammaticalGender>) this.cboGenresIrregular.SelectedItem;
            if (opt != null)
            {
                this.edited.IrregularGenre = opt.Key;
                this.edited.HasIrregularGenre = this.edited.IrregularGenre != GrammaticalGender._Unknown;
            }
            else
            {
                this.edited.IrregularGenre = GrammaticalGender._Unknown;
                this.edited.HasIrregularGenre = false;
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
                if (this.edited.HasIrregularGenre)
                {
                    for (int i = 0; i < this.cboGenresIrregular.Items.Count; i++)
                    {
                        if (((Option<GrammaticalGender>) this.cboGenresIrregular.Items[i]).Key == this.edited.IrregularGenre)
                        {
                            this.cboGenresIrregular.SelectedIndex = i;
                            this.cboGenresIrregular_SelectedIndexChanged(this.cboGenresIrregular, EventArgs.Empty);
                            break;
                        }
                    }
                }
            }
        }

        private void eRoot_TextChanged(object sender, EventArgs e)
        {
            if (this.editing)
                return;

            this.edited.Root = this.eRoot.Text;
            this.cboGenres_SelectedIndexChanged(this.cboGenres, EventArgs.Empty);
        }

        private void eRootIrregular_TextChanged(object sender, EventArgs e)
        {
            if (this.editing)
                return;

            this.edited.RootOther = this.eRootIrregular.Text;
            this.cboGenres_SelectedIndexChanged(this.cboGenres, EventArgs.Empty);
        }

        private void cboPostfixes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.editing)
                return;

            this.edited.DeclinationType = (string) this.cboPostfixes.SelectedItem;
            this.cboGenres_SelectedIndexChanged(this.cboGenres, EventArgs.Empty);
        }

        private void chlistCategories_Click(object sender, EventArgs e)
        {
            if (this.editing)
                return;

            this.edited.Categories.Clear();
            foreach (Option<int> opt in this.chlistCategories.CheckedItems)
                this.edited.Categories.Add(opt.Key);
        }

        private void chlistCategories_Leave(object sender, EventArgs e)
        {
            if (this.editing)
                return;

            this.edited.Categories.Clear();
            foreach (Option<int> opt in this.chlistCategories.CheckedItems)
                this.edited.Categories.Add(opt.Key);
        }
    }
}
