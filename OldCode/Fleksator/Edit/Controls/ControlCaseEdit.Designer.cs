namespace SharpDevs.Fleksator.Edit.Controls
{
    partial class ControlCaseEdit
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
            this.lCaseName = new System.Windows.Forms.Label();
            this.eText = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lCaseName
            // 
            this.lCaseName.AutoSize = true;
            this.lCaseName.Location = new System.Drawing.Point(3, 6);
            this.lCaseName.Name = "lCaseName";
            this.lCaseName.Size = new System.Drawing.Size(35, 13);
            this.lCaseName.TabIndex = 0;
            this.lCaseName.Text = "label1";
            // 
            // eText
            // 
            this.eText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.eText.Location = new System.Drawing.Point(3, 23);
            this.eText.Name = "eText";
            this.eText.Size = new System.Drawing.Size(270, 20);
            this.eText.TabIndex = 1;
            // 
            // ControlCaseEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.eText);
            this.Controls.Add(this.lCaseName);
            this.DoubleBuffered = true;
            this.Name = "ControlCaseEdit";
            this.Size = new System.Drawing.Size(276, 45);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected System.Windows.Forms.Label lCaseName;
        protected System.Windows.Forms.TextBox eText;
    }
}
