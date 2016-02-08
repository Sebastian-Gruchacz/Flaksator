namespace Flaksator
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nounsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.adjectivesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verbsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bGenSong = new System.Windows.Forms.Button();
            this.textResults = new SharpDevs.Helpers.Presentation.StandardTextBox();
            this.bTitleGen = new System.Windows.Forms.Button();
            this.bClear = new System.Windows.Forms.Button();
            this.bSpeak = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(917, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nounsToolStripMenuItem,
            this.adjectivesToolStripMenuItem,
            this.verbsToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.editToolStripMenuItem.Text = "&Edit";
            // 
            // nounsToolStripMenuItem
            // 
            this.nounsToolStripMenuItem.Name = "nounsToolStripMenuItem";
            this.nounsToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.nounsToolStripMenuItem.Text = "&Nouns...";
            this.nounsToolStripMenuItem.Click += new System.EventHandler(this.nounsToolStripMenuItem_Click);
            // 
            // adjectivesToolStripMenuItem
            // 
            this.adjectivesToolStripMenuItem.Name = "adjectivesToolStripMenuItem";
            this.adjectivesToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.adjectivesToolStripMenuItem.Text = "&Adjectives...";
            this.adjectivesToolStripMenuItem.Click += new System.EventHandler(this.adjectivesToolStripMenuItem_Click);
            // 
            // verbsToolStripMenuItem
            // 
            this.verbsToolStripMenuItem.Name = "verbsToolStripMenuItem";
            this.verbsToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.verbsToolStripMenuItem.Text = "&Verbs...";
            // 
            // bGenSong
            // 
            this.bGenSong.Location = new System.Drawing.Point(12, 36);
            this.bGenSong.Name = "bGenSong";
            this.bGenSong.Size = new System.Drawing.Size(134, 23);
            this.bGenSong.TabIndex = 1;
            this.bGenSong.Text = "Generate Song";
            this.bGenSong.UseVisualStyleBackColor = true;
            this.bGenSong.Click += new System.EventHandler(this.bGenSong_Click);
            // 
            // textResults
            // 
            this.textResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textResults.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textResults.Location = new System.Drawing.Point(12, 65);
            this.textResults.Multiline = true;
            this.textResults.Name = "textResults";
            this.textResults.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textResults.Size = new System.Drawing.Size(893, 401);
            this.textResults.TabIndex = 2;
            // 
            // bTitleGen
            // 
            this.bTitleGen.Location = new System.Drawing.Point(152, 36);
            this.bTitleGen.Name = "bTitleGen";
            this.bTitleGen.Size = new System.Drawing.Size(136, 23);
            this.bTitleGen.TabIndex = 3;
            this.bTitleGen.Text = "GenerateTitle";
            this.bTitleGen.UseVisualStyleBackColor = true;
            this.bTitleGen.Click += new System.EventHandler(this.bTitleGen_Click);
            // 
            // bClear
            // 
            this.bClear.Location = new System.Drawing.Point(474, 36);
            this.bClear.Name = "bClear";
            this.bClear.Size = new System.Drawing.Size(136, 23);
            this.bClear.TabIndex = 4;
            this.bClear.Text = "Clear";
            this.bClear.UseVisualStyleBackColor = true;
            this.bClear.Click += new System.EventHandler(this.bClear_Click);
            // 
            // bSpeak
            // 
            this.bSpeak.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bSpeak.Location = new System.Drawing.Point(769, 36);
            this.bSpeak.Name = "bSpeak";
            this.bSpeak.Size = new System.Drawing.Size(136, 23);
            this.bSpeak.TabIndex = 5;
            this.bSpeak.Text = "Speak!";
            this.bSpeak.UseVisualStyleBackColor = true;
            this.bSpeak.Click += new System.EventHandler(this.bSpeak_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(917, 478);
            this.Controls.Add(this.bSpeak);
            this.Controls.Add(this.bClear);
            this.Controls.Add(this.bTitleGen);
            this.Controls.Add(this.textResults);
            this.Controls.Add(this.bGenSong);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nounsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem adjectivesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem verbsToolStripMenuItem;
        private System.Windows.Forms.Button bGenSong;
        private SharpDevs.Helpers.Presentation.StandardTextBox textResults;
        private System.Windows.Forms.Button bTitleGen;
        private System.Windows.Forms.Button bClear;
        private System.Windows.Forms.Button bSpeak;
    }
}

