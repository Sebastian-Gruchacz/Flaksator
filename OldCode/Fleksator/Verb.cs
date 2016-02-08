using System;
using System.Collections.Generic;
using System.Text;

namespace SharpDevs.Fleksator
{
    public class Verb : ConiugativeWord
    {
        public Verb()
        {
            
        }

        /// <summary>
        /// Forma bezokolicznika
        /// </summary>
        public string Infinitive
        {
            get { return base.root; }
            set { root = value; }
        }
        
        private int conjugationNumber;
        /// <summary>
        /// Numer koniugacji
        /// </summary>
        public int ConjugationNumber
        {
            get { return conjugationNumber; }
            set { conjugationNumber = value; }
        }

        private bool canBeActive;
        /// <summary>
        /// Moze wystêpowaæ w stronie czynnej
        /// </summary>
        public bool CanBeActive
        {
            get { return canBeActive; }
            set { canBeActive = value; }
        }

        private bool canBePassive;
        /// <summary>
        /// Moze wystêpowaæ w stronie biernej
        /// </summary>
        public bool CanBePassive
        {
            get { return canBePassive; }
            set { canBePassive = value; }
        }


    }
}
