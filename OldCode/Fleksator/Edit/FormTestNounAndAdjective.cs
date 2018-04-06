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
    public partial class FormTestNounAndAdjective : Form
    {
        EnumTranslator translator = EnumTranslator.GetTranslator();

        public FormTestNounAndAdjective()
        {
            this.InitializeComponent();

            this.LoadGenres(this.cboGenre, true);
        }

        private Noun edited = null;
        private Adjective adj = null;

        public void SetNoun(Noun noun)
        {
            this.edited = noun;

            this.RandomAdjective();

            for (int i = 0; i < this.cboGenre.Items.Count; i++)
            {
                if (((Option<GrammaticalGender>) this.cboGenre.Items[i]).Key == this.edited.IrregularGenre)
                {
                    this.cboGenre.SelectedIndex = i;
                    this.cboGenre_SelectedIndexChanged(this.cboGenre, EventArgs.Empty);
                    break;
                }
            }

            //Display();
        }

        private void bRandom_Click(object sender, EventArgs e)
        {
            this.RandomAdjective();
            this.Display();
        }

        private void Display()
        {
            this.textNoun.Text = this.edited.Root;
            while (this.adj == null)
                this.RandomAdjective();
            this.textAdjective.Text = this.adj.Root;

            // cbo...

            this.textResult.Clear();

            foreach (InflectionCase wCase in Enum.GetValues(typeof(InflectionCase)))
            {
                string str = String.Format("{0} {1}{2}",
                    AdjectiveDecliner.Decliner.MakeWord(this.adj, this.edited, wCase, DecliantionNumber.Singular),
                    NounDecliner.Decliner.MakeWord(this.edited, wCase, DecliantionNumber.Singular),
                    Environment.NewLine);

                this.textResult.AppendText(str);
            }

            this.textResult.AppendText(Environment.NewLine);

            foreach (InflectionCase wCase in Enum.GetValues(typeof(InflectionCase)))
            {
                string str = String.Format("{0} {1}{2}",
                    AdjectiveDecliner.Decliner.MakeWord(this.adj, this.edited, wCase, DecliantionNumber.Plural),
                    NounDecliner.Decliner.MakeWord(this.edited, wCase, DecliantionNumber.Plural),
                    Environment.NewLine);

                this.textResult.AppendText(str);
            }
        }

        private void RandomAdjective()
        {
            this.adj = AdjectiveCollection.Collection.GetRandomAdjective();
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

        private void cboGenre_SelectedIndexChanged(object sender, EventArgs e)
        {
            Option<GrammaticalGender> opt = (Option<GrammaticalGender>) this.cboGenre.SelectedItem;
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

            this.Display();
        }
    }
}