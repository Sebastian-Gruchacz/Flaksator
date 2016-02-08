namespace SharpDevs.Fleksator.Edit
{
    partial class FormEditAdjectives
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
            this.checkOnlyLevelled = new System.Windows.Forms.CheckBox();
            this.chlistCategories = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.listAdjectives = new System.Windows.Forms.ListBox();
            this.controlEditAdjective1 = new SharpDevs.Fleksator.Edit.Controls.ControlEditAdjective();
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
            this.splitContainer1.Panel2.Controls.Add(this.controlEditAdjective1);
            this.splitContainer1.Size = new System.Drawing.Size(893, 559);
            this.splitContainer1.SplitterDistance = 297;
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
            this.splitContainer2.Panel2.Controls.Add(this.listAdjectives);
            this.splitContainer2.Size = new System.Drawing.Size(297, 559);
            this.splitContainer2.SplitterDistance = 238;
            this.splitContainer2.TabIndex = 0;
            // 
            // groupFilters
            // 
            this.groupFilters.Controls.Add(this.panelFilters);
            this.groupFilters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupFilters.Location = new System.Drawing.Point(0, 0);
            this.groupFilters.Name = "groupFilters";
            this.groupFilters.Size = new System.Drawing.Size(297, 238);
            this.groupFilters.TabIndex = 0;
            this.groupFilters.TabStop = false;
            this.groupFilters.Text = "Filtry:";
            // 
            // panelFilters
            // 
            this.panelFilters.AutoScroll = true;
            this.panelFilters.Controls.Add(this.label2);
            this.panelFilters.Controls.Add(this.textBox1);
            this.panelFilters.Controls.Add(this.checkOnlyLevelled);
            this.panelFilters.Controls.Add(this.chlistCategories);
            this.panelFilters.Controls.Add(this.label1);
            this.panelFilters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelFilters.Location = new System.Drawing.Point(3, 16);
            this.panelFilters.Name = "panelFilters";
            this.panelFilters.Size = new System.Drawing.Size(291, 219);
            this.panelFilters.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 199);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Szukanie:";
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(63, 196);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(225, 20);
            this.textBox1.TabIndex = 3;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // checkOnlyLevelled
            // 
            this.checkOnlyLevelled.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkOnlyLevelled.AutoSize = true;
            this.checkOnlyLevelled.Location = new System.Drawing.Point(3, 179);
            this.checkOnlyLevelled.Name = "checkOnlyLevelled";
            this.checkOnlyLevelled.Size = new System.Drawing.Size(119, 17);
            this.checkOnlyLevelled.TabIndex = 2;
            this.checkOnlyLevelled.Text = "Tylko Stopniowalne";
            this.checkOnlyLevelled.UseVisualStyleBackColor = true;
            this.checkOnlyLevelled.CheckedChanged += new System.EventHandler(this.checkOnlyLevelled_CheckedChanged);
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
            this.chlistCategories.Size = new System.Drawing.Size(282, 154);
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
            // listAdjectives
            // 
            this.listAdjectives.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listAdjectives.FormattingEnabled = true;
            this.listAdjectives.IntegralHeight = false;
            this.listAdjectives.Location = new System.Drawing.Point(0, 0);
            this.listAdjectives.Name = "listAdjectives";
            this.listAdjectives.Size = new System.Drawing.Size(297, 317);
            this.listAdjectives.Sorted = true;
            this.listAdjectives.TabIndex = 0;
            this.listAdjectives.SelectedIndexChanged += new System.EventHandler(this.listAdjectives_SelectedIndexChanged);
            // 
            // controlEditAdjective1
            // 
            this.controlEditAdjective1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlEditAdjective1.Location = new System.Drawing.Point(0, 0);
            this.controlEditAdjective1.Name = "controlEditAdjective1";
            this.controlEditAdjective1.Size = new System.Drawing.Size(592, 559);
            this.controlEditAdjective1.TabIndex = 0;
            // 
            // FormEditAdjectives
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(893, 559);
            this.Controls.Add(this.splitContainer1);
            this.DoubleBuffered = true;
            this.MinimumSize = new System.Drawing.Size(901, 586);
            this.Name = "FormEditAdjectives";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Edycja Przymiotników";
            this.Load += new System.EventHandler(this.FormEditAdjectives_Load);
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
        private System.Windows.Forms.GroupBox groupFilters;
        private System.Windows.Forms.ListBox listAdjectives;
        private SharpDevs.Fleksator.Edit.Controls.ControlEditAdjective controlEditAdjective1;
        private System.Windows.Forms.Panel panelFilters;
        private System.Windows.Forms.CheckedListBox chlistCategories;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkOnlyLevelled;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
    }
}