using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using SharpDevs.Fleksator;
using SharpDevs.Fleksator.Edit;

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
            FormEditNouns frm = new FormEditNouns();
            frm.Show();
        }

        private void adjectivesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormEditAdjectives frm = new FormEditAdjectives();
            frm.Show();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            // Load dictionaries...
            WordCategories.Categories.LoadFromFile(".\\DataFiles\\Categories.ini");
            AdjectiveCollection.Collection.LoadFromFile(".\\DataFiles\\Adjectives.txt");
            NounCollection.Collection.LoadFromFile(".\\DataFiles\\Nouns.txt");
            NounDecliner.Decliner.LoadPostfixes(".\\DataFiles\\NounPostfixes.txt");


            VersesCreator.Creator.LoadFromFile(".\\DataFiles\\Verses.txt");
            TitleCreator.Creator.LoadFromFile(".\\DataFiles\\_Title.txt");
            SongCreator.Creator.LoadFromFile(".\\DataFiles\\_Title.txt");
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            NounCollection.Collection.SaveToFile(".\\DataFiles\\Nouns.txt");
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