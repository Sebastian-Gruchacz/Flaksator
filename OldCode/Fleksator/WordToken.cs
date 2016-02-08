using System;
using System.Collections.Generic;
using System.Text;
using SharpDevs.Fleksator.Grammar;

namespace SharpDevs.Fleksator
{
    public class WordToken
    {
        private DecliantionNumber amount;
        /// <summary>
        /// Liczba (pojedyncza lub mnoga)
        /// </summary>
        public DecliantionNumber DecliantionNumber
        {
            get { return amount; }
            set { amount = value; }
        }

        private InflectionCase inflectionCase;
        /// <summary>
        /// Przypadek
        /// </summary>
        public InflectionCase InflectionCase
        {
            get { return inflectionCase; }
            set { inflectionCase = value; }
        }

        private string root;

        public string Text
        {
            get { return root; }
            set { root = value; }
        }

        public WordToken()
        {

        }

        public WordToken(string text, InflectionCase inflectionCase, DecliantionNumber decliantionNumber)
        {
            this.root = text;
            this.inflectionCase = inflectionCase;
            this.amount = decliantionNumber;
        }

        public virtual bool Is(WordToken token2)
        {
            return (this.amount == token2.amount &&
                this.inflectionCase == token2.inflectionCase);
        }
    }
}
