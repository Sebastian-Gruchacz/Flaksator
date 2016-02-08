namespace SharpDevs.Fleksator.Edit.Controls
{
    partial class ControlAdjectiveCaseEdit
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
            this.bEqual = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // eText
            // 
            this.eText.Size = new System.Drawing.Size(249, 20);
            // 
            // bEqual
            // 
            this.bEqual.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bEqual.AutoSize = true;
            this.bEqual.Location = new System.Drawing.Point(251, 22);
            this.bEqual.Margin = new System.Windows.Forms.Padding(0);
            this.bEqual.Name = "bEqual";
            this.bEqual.Size = new System.Drawing.Size(24, 23);
            this.bEqual.TabIndex = 2;
            this.bEqual.Text = "=";
            this.bEqual.UseVisualStyleBackColor = true;
            this.bEqual.Click += new System.EventHandler(this.button1_Click);
            // 
            // ControlNounCaseEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Controls.Add(this.bEqual);
            this.Name = "ControlNounCaseEdit";
            this.SizeChanged += new System.EventHandler(this.ControlNounCaseEdit_SizeChanged);
            this.Controls.SetChildIndex(this.bEqual, 0);
            this.Controls.SetChildIndex(this.lCaseName, 0);
            this.Controls.SetChildIndex(this.eText, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bEqual;
    }
}
