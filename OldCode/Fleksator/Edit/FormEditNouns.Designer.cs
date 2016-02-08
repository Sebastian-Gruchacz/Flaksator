namespace SharpDevs.Fleksator.Edit
{
    partial class FormEditNouns
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.groupFilters = new System.Windows.Forms.GroupBox();
            this.panelFilters = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.chlistCategories = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.bDelete = new System.Windows.Forms.Button();
            this.bNew = new System.Windows.Forms.Button();
            this.bUndo = new System.Windows.Forms.Button();
            this.listNouns = new System.Windows.Forms.ListBox();
            this.controlEditNoun = new SharpDevs.Fleksator.Edit.Controls.ControlEditNoun();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupFilters.SuspendLayout();
            this.panelFilters.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.controlEditNoun);
            this.splitContainer1.Size = new System.Drawing.Size(884, 548);
            this.splitContainer1.SplitterDistance = 294;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.groupFilters);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.bDelete);
            this.splitContainer2.Panel2.Controls.Add(this.bNew);
            this.splitContainer2.Panel2.Controls.Add(this.bUndo);
            this.splitContainer2.Panel2.Controls.Add(this.listNouns);
            this.splitContainer2.Size = new System.Drawing.Size(294, 548);
            this.splitContainer2.SplitterDistance = 255;
            this.splitContainer2.TabIndex = 0;
            // 
            // groupFilters
            // 
            this.groupFilters.Controls.Add(this.panelFilters);
            this.groupFilters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupFilters.Location = new System.Drawing.Point(0, 0);
            this.groupFilters.Name = "groupFilters";
            this.groupFilters.Size = new System.Drawing.Size(294, 255);
            this.groupFilters.TabIndex = 1;
            this.groupFilters.TabStop = false;
            this.groupFilters.Text = "Filtry:";
            // 
            // panelFilters
            // 
            this.panelFilters.AutoScroll = true;
            this.panelFilters.Controls.Add(this.label2);
            this.panelFilters.Controls.Add(this.textBox1);
            this.panelFilters.Controls.Add(this.chlistCategories);
            this.panelFilters.Controls.Add(this.label1);
            this.panelFilters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelFilters.Location = new System.Drawing.Point(3, 16);
            this.panelFilters.Name = "panelFilters";
            this.panelFilters.Size = new System.Drawing.Size(288, 236);
            this.panelFilters.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 216);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Szukanie:";
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(63, 213);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(222, 20);
            this.textBox1.TabIndex = 3;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
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
            this.chlistCategories.Size = new System.Drawing.Size(279, 188);
            this.chlistCategories.Sorted = true;
            this.chlistCategories.TabIndex = 1;
            this.chlistCategories.SelectedIndexChanged += new System.EventHandler(this.chlistCategories_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Kategorie:";
            // 
            // bDelete
            // 
            this.bDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bDelete.Location = new System.Drawing.Point(51, 260);
            this.bDelete.Name = "bDelete";
            this.bDelete.Size = new System.Drawing.Size(75, 23);
            this.bDelete.TabIndex = 4;
            this.bDelete.Text = "Usuñ";
            this.bDelete.UseVisualStyleBackColor = true;
            this.bDelete.Click += new System.EventHandler(this.bDelete_Click);
            // 
            // bNew
            // 
            this.bNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bNew.Location = new System.Drawing.Point(132, 260);
            this.bNew.Name = "bNew";
            this.bNew.Size = new System.Drawing.Size(75, 23);
            this.bNew.TabIndex = 3;
            this.bNew.Text = "Nowy";
            this.bNew.UseVisualStyleBackColor = true;
            this.bNew.Click += new System.EventHandler(this.button2_Click);
            // 
            // bUndo
            // 
            this.bUndo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bUndo.Location = new System.Drawing.Point(213, 260);
            this.bUndo.Name = "bUndo";
            this.bUndo.Size = new System.Drawing.Size(75, 23);
            this.bUndo.TabIndex = 2;
            this.bUndo.Text = "Anuluj Zmiany";
            this.bUndo.UseVisualStyleBackColor = true;
            this.bUndo.Click += new System.EventHandler(this.button1_Click);
            // 
            // listNouns
            // 
            this.listNouns.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listNouns.FormattingEnabled = true;
            this.listNouns.IntegralHeight = false;
            this.listNouns.Location = new System.Drawing.Point(0, 0);
            this.listNouns.Name = "listNouns";
            this.listNouns.Size = new System.Drawing.Size(294, 254);
            this.listNouns.Sorted = true;
            this.listNouns.TabIndex = 1;
            this.listNouns.SelectedIndexChanged += new System.EventHandler(this.listNouns_SelectedIndexChanged);
            // 
            // controlEditNoun
            // 
            this.controlEditNoun.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlEditNoun.Location = new System.Drawing.Point(0, 0);
            this.controlEditNoun.MinimumSize = new System.Drawing.Size(575, 550);
            this.controlEditNoun.Name = "controlEditNoun";
            this.controlEditNoun.Size = new System.Drawing.Size(586, 550);
            this.controlEditNoun.TabIndex = 0;
            // 
            // FormEditNouns
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 548);
            this.Controls.Add(this.splitContainer1);
            this.DoubleBuffered = true;
            this.Name = "FormEditNouns";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Edycja Rzeczowników";
            this.Load += new System.EventHandler(this.FormEditNouns_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.groupFilters.ResumeLayout(false);
            this.panelFilters.ResumeLayout(false);
            this.panelFilters.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private SharpDevs.Fleksator.Edit.Controls.ControlEditNoun controlEditNoun;
        private System.Windows.Forms.ListBox listNouns;
        private System.Windows.Forms.GroupBox groupFilters;
        private System.Windows.Forms.Panel panelFilters;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.CheckedListBox chlistCategories;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bNew;
        private System.Windows.Forms.Button bUndo;
        private System.Windows.Forms.Button bDelete;

    }
}