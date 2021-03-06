using System;
using System.Collections.Generic;
using System.Text;

namespace SharpDevs.Fleksator
{
    /// <summary>
    /// Bazowa klasa dla słów
    /// </summary>
    public abstract class Word
    {
        /// <summary>
        /// This is used to display word on list (in most cases equal to root, but sometimes shall be different)
        /// </summary>
        public string DisplayName { get; set; }

        protected string root;
        /// <summary>
        /// Rdzeń słowa (lub pełna forma jeśli nie odmienny)
        /// </summary>
        /// <remarks>Dla przymiotników - formą podstawową jest mianownik l.p. rodzaju żeńskiego</remarks>
        public string Root
        {
            get { return root; }
            set { root = value; }
        }

        protected bool isException = false;
        /// <summary>
        /// Słowo odmienia się nieregularnie
        /// </summary>
        public bool IsException
        {
            get { return isException; }
            set { isException = value; }
        }

        protected bool isConstant = false;
        /// <summary>
        /// Słowo się nie odmienia
        /// </summary>
        public bool IsConstant
        {
            get { return isConstant; }
            set { isConstant = value; }
        }

        public override string ToString()
        {
            return DisplayName;
        }
    }
}
