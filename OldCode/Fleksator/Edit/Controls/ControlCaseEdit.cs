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
            InitializeComponent();
        }

        private EnumTranslator translator = EnumTranslator.GetTranslator();

        private InflectionCase aCase;
        public InflectionCase Case
        {
            get { return aCase; }
            set
            {
                aCase = value;

                if (translator != null)
                    lCaseName.Text = translator.TranslateWordCase(aCase);
                else
                    lCaseName.Text = aCase.ToString();
            }
        }

        public string Value
        {
            get { return eText.Text; }
            set { eText.Text = value; }
        }
    }
}
