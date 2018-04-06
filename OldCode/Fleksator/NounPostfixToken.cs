using System;
using System.Collections.Generic;
using System.Text;
using SharpDevs.Fleksator.Grammar;

namespace SharpDevs.Fleksator
{
    public class NounPostfixToken
    {
        private DecliantionNumber amount;

        public DecliantionNumber DecliantionNumber
        {
            get { return this.amount; }
            set { this.amount = value; }
        }

        private InflectionCase inflectionCase;

        public InflectionCase InflectionCase
        {
            get { return this.inflectionCase; }
            set { this.inflectionCase = value; }
        }

        private string[] postfixes;

        public string[] Postfixes
        {
            get { return this.postfixes; }
            set { this.postfixes = value; }
        }

        private string nounDeclination;

        public string Declination
        {
            get { return this.nounDeclination; }
            set { this.nounDeclination = value; }
        }

        private GrammaticalGender genre;

        public GrammaticalGender Genre
        {
            get { return this.genre; }
            set { this.genre = value; }
        }

        public NounPostfixToken()
        {

        }

        public NounPostfixToken(GrammaticalGender grammaticalGender, InflectionCase inflectionCase, DecliantionNumber decliantionNumber,
            string wordDeclination, params string[] wordPostfixes)
        {
            this.genre = grammaticalGender;
            this.inflectionCase = inflectionCase;
            this.amount = decliantionNumber;
            this.nounDeclination = wordDeclination;
            this.postfixes = wordPostfixes;

        }
    }
}
