using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using SharpDevs.Fleksator;
#if NETFRAMEWORK
using SharpDevs.Fleksator.Edit;
using Flaksator.Edit;
#endif

//using SpeechLib;

namespace Flaksator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            this.InitializeComponent();
        }

        private void nounsToolStripMenuItem_Click(object sender, EventArgs e)
        {
#if NETFRAMEWORK
            FormEditNouns frm = new FormEditNouns();
            frm.Show();
#else
            MessageBox.Show("The legacy dictionary editor is available in the .NET Framework build.");
#endif
        }

        private void adjectivesToolStripMenuItem_Click(object sender, EventArgs e)
        {
#if NETFRAMEWORK
            FormEditAdjectives frm = new FormEditAdjectives();
            frm.Show();
#else
            MessageBox.Show("The legacy dictionary editor is available in the .NET Framework build.");
#endif
        }

        private void verbsToolStripMenuItem_Click(object sender, EventArgs e)
        {
#if NETFRAMEWORK
            FormEditVerbs frm = new FormEditVerbs();
            frm.Show();
#else
            MessageBox.Show("The legacy dictionary editor is available in the .NET Framework build.");
#endif
        }

        private void versesToolStripMenuItem_Click(object sender, EventArgs e)
        {
#if NETFRAMEWORK
            FormEditVerses frm = new FormEditVerses();
            frm.Show();
#else
            MessageBox.Show("The legacy verse editor is available in the .NET Framework build.");
#endif
        }

        private void bGenerationFilters_Click(object sender, EventArgs e)
        {
#if NETFRAMEWORK
            using (FormGenerationFilters form = new FormGenerationFilters())
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                    this.UpdateGenerationFilterCaption();
            }
#else
            MessageBox.Show("Generation category filters are available in the .NET Framework build.");
#endif
        }

        private void UpdateGenerationFilterCaption()
        {
            int count = GenerationCategoryFilter.ExcludedNounCategories.Count +
                GenerationCategoryFilter.ExcludedAdjectiveCategories.Count +
                GenerationCategoryFilter.ExcludedVerbCategories.Count;
            this.bGenerationFilters.Text = count == 0
                ? "Wykluczenia..."
                : "Wykluczenia (" + count + ")...";
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            // Load dictionaries...
            WordCategories.Categories.LoadFromFile(".\\DataFiles\\Categories.ini");
            AdjectiveCollection.Collection.LoadFromFile(".\\DataFiles\\Adjectives.txt");
            NounCollection.Collection.LoadFromFile(".\\DataFiles\\Nouns.txt");
            VerbCollection.Collection.LoadFromFile(".\\DataFiles\\Verbs.txt");
            PronounCollection.Collection.LoadFromFile(".\\DataFiles\\Pronouns.txt");
            NounDecliner.Decliner.LoadPostfixes(".\\DataFiles\\NounPostfixes.txt");


            VersesCreator.Creator.LoadFromFile(".\\DataFiles\\Verses.txt");
            TitleCreator.Creator.LoadFromFile(".\\DataFiles\\_Title.txt");
            SongCreator.Creator.LoadFromFile(".\\DataFiles\\_Title.txt");
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            NounCollection.Collection.SaveToFile(".\\DataFiles\\Nouns.txt");
            VerbCollection.Collection.SaveToFile(".\\DataFiles\\Verbs.txt");
            // TODO: other saves
        }

        private void bGenSong_Click(object sender, EventArgs e)
        {
            this.textResults.AppendText(SongCreator.Creator.GenerateRandomSong());
            this.textResults.AppendText(Environment.NewLine);
            this.textResults.AppendText(Environment.NewLine);
            this.textResults.AppendText("------------------------------------------------");
            this.textResults.AppendText(Environment.NewLine);
            this.textResults.AppendText(Environment.NewLine);
        }

        private void bTitleGen_Click(object sender, EventArgs e)
        {
            this.textResults.AppendText(TitleCreator.Creator.GetRandomTitle().ToUpper());
            this.textResults.AppendText(Environment.NewLine);
        }

        private void bClear_Click(object sender, EventArgs e)
        {
            this.textResults.Clear();
        }

        private void bSpeak_Click(object sender, EventArgs e)
        {
            //SpVoice objSpeech = new SpVoice();
            //objSpeech.Speak(this.textResults.Text, SpeechVoiceSpeakFlags.SVSFlagsAsync);
          //  objSpeech.WaitUntilDone(100000); 

        }
    }
}