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
            get { return this.amount; }
            set { this.amount = value; }
        }

        private InflectionCase inflectionCase;
        /// <summary>
        /// Przypadek
        /// </summary>
        public InflectionCase InflectionCase
        {
            get { return this.inflectionCase; }
            set { this.inflectionCase = value; }
        }

        private string root;

        public string Text
        {
            get { return this.root; }
            set { this.root = value; }
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
