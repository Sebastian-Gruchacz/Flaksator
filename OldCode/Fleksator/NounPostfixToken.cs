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
            get { return amount; }
            set { amount = value; }
        }

        private InflectionCase inflectionCase;

        public InflectionCase InflectionCase
        {
            get { return inflectionCase; }
            set { inflectionCase = value; }
        }

        private string[] postfixes;

        public string[] Postfixes
        {
            get { return postfixes; }
            set { postfixes = value; }
        }

        private string nounDeclination;

        public string Declination
        {
            get { return nounDeclination; }
            set { nounDeclination = value; }
        }

        private GrammaticalGender genre;

        public GrammaticalGender Genre
        {
            get { return genre; }
            set { genre = value; }
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
