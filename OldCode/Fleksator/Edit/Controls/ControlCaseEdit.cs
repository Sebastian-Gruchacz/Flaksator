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
    public partial class ControlCaseEdit : UserControl
    {
        public ControlCaseEdit()
        {
            this.InitializeComponent();
        }

        private EnumTranslator translator = EnumTranslator.GetTranslator();

        private InflectionCase aCase;
        public InflectionCase Case
        {
            get { return this.aCase; }
            set
            {
                this.aCase = value;

                if (this.translator != null)
                    this.lCaseName.Text = this.translator.TranslateWordCase(this.aCase);
                else
                    this.lCaseName.Text = this.aCase.ToString();
            }
        }

        public string Value
        {
            get { return this.eText.Text; }
            set { this.eText.Text = value; }
        }
    }
}
