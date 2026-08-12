using System;
using System.Collections.Generic;
using System.Text;
using SharpDevs.Fleksator.Grammar;

namespace SharpDevs.Fleksator.IO.Implementation
{
    internal class OldGrammarSerializers : IGrammarSerializers
    {
        private readonly IGrammaticalWordSerializer<Noun> _nounSerializer = new OldNounSerializer();
        private readonly IGrammaticalWordSerializer<Verb> _verbSerializer = new OldVerbSerializer();
        private readonly IGrammaticalWordSerializer<Pronoun> _pronounSerializer = new OldPronounSerializer();

        public IGrammaticalWordSerializer<Noun> NounSerializer
        {
            get { return this._nounSerializer; }
        }

        public IGrammaticalWordSerializer<Verb> VerbSerializer
        {
            get { return this._verbSerializer; }
        }

        public IGrammaticalWordSerializer<Pronoun> PronounSerializer
        {
            get { return this._pronounSerializer; }
        }
    }
}
