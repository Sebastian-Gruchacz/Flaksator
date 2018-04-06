using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SharpDevs.Helpers.Presentation
{    
    /// <summary>
    /// 
    /// </summary>
    public partial class StandardTextBox : TextBox
    {
        public StandardTextBox()
        {
            this.InitializeComponent();
        }

        private bool ctrlPressed = false;
        Keys leftControl = Keys.LButton | Keys.Control | Keys.ShiftKey;

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if ((keyData & this.leftControl) == this.leftControl)
            {
                this.ctrlPressed = (msg.Msg == 0x100); // WM_KEYDOWN
            }
            if (this.ctrlPressed)
            {
                if ((keyData & Keys.A) == Keys.A)
                {
                    this.SelectAll();
                    return true;
                }

            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

    }
}
