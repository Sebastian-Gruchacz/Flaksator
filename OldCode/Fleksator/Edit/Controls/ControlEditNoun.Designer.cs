namespace SharpDevs.Fleksator.Edit.Controls
{
    partial class ControlEditNoun
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.eRootIrregular = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.eRoot = new System.Windows.Forms.TextBox();
            this.checkNoSingular = new System.Windows.Forms.CheckBox();
            this.checkConstant = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupPlural = new System.Windows.Forms.GroupBox();
            this.panelPlural = new System.Windows.Forms.Panel();
            this.groupSingular = new System.Windows.Forms.GroupBox();
            this.panelSingular = new System.Windows.Forms.Panel();
            this.bNewCategory = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chlistCategories = new System.Windows.Forms.CheckedListBox();
            this.checkNoPlural = new System.Windows.Forms.CheckBox();
            this.cboGenres = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cboGenresIrregular = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.bAdjectiveTest = new System.Windows.Forms.Button();
            this.cboPostfixes = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupPlural.SuspendLayout();
            this.groupSingular.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // eRootIrregular
            // 
            this.eRootIrregular.Location = new System.Drawing.Point(109, 36);
            this.eRootIrregular.Name = "eRootIrregular";
            this.eRootIrregular.Size = new System.Drawing.Size(199, 20);
            this.eRootIrregular.TabIndex = 18;
            this.eRootIrregular.TextChanged += new System.EventHandler(this.eRootIrregular_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "Cecha oboczna:";
            // 
            // eRoot
            // 
            this.eRoot.Location = new System.Drawing.Point(109, 10);
            this.eRoot.Name = "eRoot";
            this.eRoot.Size = new System.Drawing.Size(199, 20);
            this.eRoot.TabIndex = 16;
            this.eRoot.TextChanged += new System.EventHandler(this.eRoot_TextChanged);
            // 
            // checkNoSingular
            // 
            this.checkNoSingular.Location = new System.Drawing.Point(6, 84);
            this.checkNoSingular.Name = "checkNoSingular";
            this.checkNoSingular.Size = new System.Drawing.Size(185, 23);
            this.checkNoSingular.TabIndex = 14;
            this.checkNoSingular.Text = "Brak Liczby Pojedynczej";
            this.checkNoSingular.UseVisualStyleBackColor = true;
            // 
            // checkConstant
            // 
            this.checkConstant.AutoSize = true;
            this.checkConstant.Location = new System.Drawing.Point(6, 62);
            this.checkConstant.Name = "checkConstant";
            this.checkConstant.Size = new System.Drawing.Size(165, 17);
            this.checkConstant.TabIndex = 13;
            this.checkConstant.Text = "Rzeczownik jest nieodmienny\r\n";
            this.checkConstant.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Cecha główna:";
            // 
            // groupPlural
            // 
            this.groupPlural.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.groupPlural.Controls.Add(this.panelPlural);
            this.groupPlural.Location = new System.Drawing.Point(289, 237);
            this.groupPlural.Name = "groupPlural";
            this.groupPlural.Size = new System.Drawing.Size(282, 335);
            this.groupPlural.TabIndex = 21;
            this.groupPlural.TabStop = false;
            this.groupPlural.Text = "Liczba Mnoga";
            // 
            // panelPlural
            // 
            this.panelPlural.AutoScroll = true;
            this.panelPlural.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPlural.Location = new System.Drawing.Point(3, 16);
            this.panelPlural.Name = "panelPlural";
            this.panelPlural.Size = new System.Drawing.Size(276, 316);
            this.panelPlural.TabIndex = 1;
            // 
            // groupSingular
            // 
            this.groupSingular.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupSingular.Controls.Add(this.panelSingular);
            this.groupSingular.Location = new System.Drawing.Point(3, 237);
            this.groupSingular.Name = "groupSingular";
            this.groupSingular.Size = new System.Drawing.Size(280, 335);
            this.groupSingular.TabIndex = 20;
            this.groupSingular.TabStop = false;
            this.groupSingular.Text = "Liczba Pojedyncza";
            // 
            // panelSingular
            // 
            this.panelSingular.AutoScroll = true;
            this.panelSingular.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSingular.Location = new System.Drawing.Point(3, 16);
            this.panelSingular.Name = "panelSingular";
            this.panelSingular.Size = new System.Drawing.Size(274, 316);
            this.panelSingular.TabIndex = 0;
            // 
            // bNewCategory
            // 
            this.bNewCategory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bNewCategory.Location = new System.Drawing.Point(152, 116);
            this.bNewCategory.Name = "bNewCategory";
            this.bNewCategory.Size = new System.Drawing.Size(102, 23);
            this.bNewCategory.TabIndex = 1;
            this.bNewCategory.Text = "Nowa Kategoria...";
            this.bNewCategory.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.bNewCategory);
            this.groupBox1.Controls.Add(this.chlistCategories);
            this.groupBox1.Location = new System.Drawing.Point(314, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(257, 147);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Kategorie:";
            // 
            // chlistCategories
            // 
            this.chlistCategories.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.chlistCategories.CheckOnClick = true;
            this.chlistCategories.FormattingEnabled = true;
            this.chlistCategories.IntegralHeight = false;
            this.chlistCategories.Location = new System.Drawing.Point(6, 19);
            this.chlistCategories.Name = "chlistCategories";
            this.chlistCategories.Size = new System.Drawing.Size(248, 91);
            this.chlistCategories.Sorted = true;
            this.chlistCategories.TabIndex = 0;
            this.chlistCategories.Leave += new System.EventHandler(this.chlistCategories_Leave);
            this.chlistCategories.Click += new System.EventHandler(this.chlistCategories_Click);
            // 
            // checkNoPlural
            // 
            this.checkNoPlural.Location = new System.Drawing.Point(6, 108);
            this.checkNoPlural.Name = "checkNoPlural";
            this.checkNoPlural.Size = new System.Drawing.Size(227, 23);
            this.checkNoPlural.TabIndex = 22;
            this.checkNoPlural.Text = "Brak Liczby Mnogiej";
            this.checkNoPlural.UseVisualStyleBackColor = true;
            // 
            // cboGenres
            // 
            this.cboGenres.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cboGenres.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGenres.FormattingEnabled = true;
            this.cboGenres.Location = new System.Drawing.Point(317, 156);
            this.cboGenres.Name = "cboGenres";
            this.cboGenres.Size = new System.Drawing.Size(254, 21);
            this.cboGenres.TabIndex = 24;
            this.cboGenres.SelectedIndexChanged += new System.EventHandler(this.cboGenres_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 159);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(85, 13);
            this.label5.TabIndex = 23;
            this.label5.Text = "Rodzaj odmiany:";
            // 
            // cboGenresIrregular
            // 
            this.cboGenresIrregular.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cboGenresIrregular.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGenresIrregular.FormattingEnabled = true;
            this.cboGenresIrregular.Location = new System.Drawing.Point(317, 183);
            this.cboGenresIrregular.Name = "cboGenresIrregular";
            this.cboGenresIrregular.Size = new System.Drawing.Size(254, 21);
            this.cboGenresIrregular.TabIndex = 26;
            this.cboGenresIrregular.SelectedIndexChanged += new System.EventHandler(this.cboGenresIrregular_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 186);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(188, 13);
            this.label2.TabIndex = 25;
            this.label2.Text = "Nieregularna odmiana przymiotnikowa:";
            // 
            // bAdjectiveTest
            // 
            this.bAdjectiveTest.Location = new System.Drawing.Point(188, 106);
            this.bAdjectiveTest.Name = "bAdjectiveTest";
            this.bAdjectiveTest.Size = new System.Drawing.Size(120, 23);
            this.bAdjectiveTest.TabIndex = 27;
            this.bAdjectiveTest.Text = "Testuj Przymiotniki...";
            this.bAdjectiveTest.UseVisualStyleBackColor = true;
            this.bAdjectiveTest.Click += new System.EventHandler(this.bAdjectiveTest_Click);
            // 
            // cboPostfixes
            // 
            this.cboPostfixes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cboPostfixes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPostfixes.FormattingEnabled = true;
            this.cboPostfixes.Items.AddRange(new object[] {
            "M1",
            "M2",
            "M3",
            "M4",
            "M5",
            "F1",
            "F2",
            "F3",
            "F4",
            "F5",
            "F6",
            "N1",
            "N2",
            "N3",
            "N4",
            "N5",
            "N6",
            ".1",
            ".2"});
            this.cboPostfixes.Location = new System.Drawing.Point(317, 210);
            this.cboPostfixes.Name = "cboPostfixes";
            this.cboPostfixes.Size = new System.Drawing.Size(254, 21);
            this.cboPostfixes.TabIndex = 29;
            this.cboPostfixes.SelectedIndexChanged += new System.EventHandler(this.cboPostfixes_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 213);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(142, 13);
            this.label4.TabIndex = 28;
            this.label4.Text = "Bazowy zestaw przyrostków:";
            // 
            // ControlEditNoun
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cboPostfixes);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.bAdjectiveTest);
            this.Controls.Add(this.cboGenresIrregular);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cboGenres);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.checkNoPlural);
            this.Controls.Add(this.eRootIrregular);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.eRoot);
            this.Controls.Add(this.checkNoSingular);
            this.Controls.Add(this.checkConstant);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupPlural);
            this.Controls.Add(this.groupSingular);
            this.Controls.Add(this.groupBox1);
            this.DoubleBuffered = true;
            this.MinimumSize = new System.Drawing.Size(575, 550);
            this.Name = "ControlEditNoun";
            this.Size = new System.Drawing.Size(575, 579);
            this.Load += new System.EventHandler(this.ControlEditNoun_Load);
            this.groupPlural.ResumeLayout(false);
            this.groupSingular.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox eRootIrregular;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox eRoot;
        private System.Windows.Forms.CheckBox checkNoSingular;
        private System.Windows.Forms.CheckBox checkConstant;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupPlural;
        private System.Windows.Forms.Panel panelPlural;
        private System.Windows.Forms.GroupBox groupSingular;
        private System.Windows.Forms.Panel panelSingular;
        private System.Windows.Forms.Button bNewCategory;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckedListBox chlistCategories;
        private System.Windows.Forms.CheckBox checkNoPlural;
        private System.Windows.Forms.ComboBox cboGenres;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cboGenresIrregular;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button bAdjectiveTest;
        private System.Windows.Forms.ComboBox cboPostfixes;
        private System.Windows.Forms.Label label4;
    }
}
