using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using SharpDevs.Fleksator.Grammar;

namespace SharpDevs.Fleksator.Edit.Controls
{
    public partial class ControlNounCaseEdit : ControlCaseEdit
    {
        public ControlNounCaseEdit()
        {
            InitializeComponent();
        }

        private string[] postFixes;

        public string[] PostFixes
        {
            get { return postFixes; }
            set
            {
                postFixes = value;

                // remove buttons
                Control[] ctrls = new Control[this.Controls.Count];
                this.Controls.CopyTo(ctrls, 0);
                foreach (Control ctrl in ctrls)
                {
                    if (!(ctrl is Button) || ctrl.Name == "bEqual")
                        continue;
                    this.Controls.Remove(ctrl);
                }

                if (postFixes.Length > 1)
                {
                    int pos = 1;
                    foreach (string postFix in postFixes)
                    {
                        Button posButton = new Button();
                        posButton.FlatStyle = FlatStyle.Popup;
                        posButton.Margin = new Padding(0, 0, 0, 0);
                        posButton.AutoSize = true;
                        posButton.Height = 14;
                        posButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                        posButton.Font = new Font(posButton.Font.FontFamily, (float)8);
                        posButton.Text = postFix;
                        posButton.Click += new EventHandler(posButton_Click);
                        this.Controls.Add(posButton);
                        posButton.Top = 0; // CaseName.Top;
                        posButton.Left = this.Width - (pos + posButton.Width);
                        pos += 3 + posButton.Width;
                    }
                }
            }
        }

        void posButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.postFixes.Length; i++)
                if (this.postFixes[i] == ((Button)sender).Text)
                {
                    this.PostFixIndex = i;
                    break;
                }

            if (PostfixButtonPressed != null)
                PostfixButtonPressed(this, EventArgs.Empty);
        }

        private int selectedPostfixIndex;

        public int PostFixIndex
        {
            get { return selectedPostfixIndex; }
            set
            {
                selectedPostfixIndex = value;
                foreach (Control ctrl in this.Controls)
                {
                    if (!(ctrl is Button) || ctrl.Name == "bEqual")
                        continue;

                    Button b = (Button)ctrl;

                    if (selectedPostfixIndex < this.postFixes.Length &&
                        b.Text == this.postFixes[selectedPostfixIndex])
                        b.FlatStyle = FlatStyle.Standard;
                    else
                        b.FlatStyle = FlatStyle.Popup;
                }
            }
        }

        public event EventHandler PostfixButtonPressed;
        public event EventHandler IrregularSet;

        private DecliantionNumber amount;
        /// <summary>
        /// For reference
        /// </summary>
        public DecliantionNumber DecliantionNumber
        {
            get { return amount; }
            set { amount = value; }
        }

        private InflectionCase aCase;
        /// <summary>
        /// For reference
        /// </summary>
        public InflectionCase InflectionCase
        {
            get { return aCase; }
            set { aCase = value; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.IrregularSet != null)
                this.IrregularSet(this, EventArgs.Empty);
        }

        private void ControlNounCaseEdit_SizeChanged(object sender, EventArgs e)
        {
            this.bEqual.Left = this.ClientSize.Width - bEqual.Width - 1;
            this.eText.Width = this.ClientSize.Width - (this.eText.Left + bEqual.Width + 2);
        }


    }
}
