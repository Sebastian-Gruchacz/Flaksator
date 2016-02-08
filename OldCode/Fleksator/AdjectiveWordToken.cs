using System;
using System.Collections.Generic;
using System.Text;
using SharpDevs.Fleksator.Grammar;

namespace SharpDevs.Fleksator
{
    public class AdjectiveWordToken : WordToken
    {
        private AdjectiveLevel level;

        public AdjectiveLevel Level
        {
            get { return level; }
            set { level = value; }
        }


        private GrammaticalGender genre;

        public GrammaticalGender GrammaticalGender
        {
            get { return genre; }
            set { genre = value; }
        }

        public AdjectiveWordToken()
        {

        }

        public AdjectiveWordToken(string text, InflectionCase inflectionCase,
            GrammaticalGender grammaticalGender, DecliantionNumber decliantionNumber, AdjectiveLevel level)
            : base(text, inflectionCase, decliantionNumber)
        {
            this.level = level;
            this.genre = grammaticalGender;
        }

        public bool Is(AdjectiveWordToken token2)
        {
            return (base.Is(token2) && this.level == token2.Level && this.genre == token2.genre);
        }
    }
}
