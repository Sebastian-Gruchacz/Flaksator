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
            InitializeComponent();

            LoadGenres(this.cboGenre, true);
        }

        private Noun edited = null;
        private Adjective adj = null;

        public void SetNoun(Noun noun)
        {
            edited = noun;

            RandomAdjective();

            for (int i = 0; i < cboGenre.Items.Count; i++)
            {
                if (((Option<GrammaticalGender>)cboGenre.Items[i]).Key == edited.IrregularGenre)
                {
                    cboGenre.SelectedIndex = i;
                    cboGenre_SelectedIndexChanged(cboGenre, EventArgs.Empty);
                    break;
                }
            }

            //Display();
        }

        private void bRandom_Click(object sender, EventArgs e)
        {
            RandomAdjective();
            Display();
        }

        private void Display()
        {
            this.textNoun.Text = edited.Root;
            while (adj == null)
                RandomAdjective();
            this.textAdjective.Text = adj.Root;

            // cbo...

            this.textResult.Clear();

            foreach (InflectionCase wCase in Enum.GetValues(typeof(InflectionCase)))
            {
                string str = String.Format("{0} {1}{2}",
                    AdjectiveDecliner.Decliner.MakeWord(adj, edited, wCase, DecliantionNumber.Singular),
                    NounDecliner.Decliner.MakeWord(edited, wCase, DecliantionNumber.Singular),
                    Environment.NewLine);

                this.textResult.AppendText(str);
            }

            this.textResult.AppendText(Environment.NewLine);

            foreach (InflectionCase wCase in Enum.GetValues(typeof(InflectionCase)))
            {
                string str = String.Format("{0} {1}{2}",
                    AdjectiveDecliner.Decliner.MakeWord(adj, edited, wCase, DecliantionNumber.Plural),
                    NounDecliner.Decliner.MakeWord(edited, wCase, DecliantionNumber.Plural),
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

        private void cboGenre_SelectedIndexChanged(object sender, EventArgs e)
        {
            Option<GrammaticalGender> opt = (Option<GrammaticalGender>)cboGenre.SelectedItem;
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

            Display();
        }
    }
}