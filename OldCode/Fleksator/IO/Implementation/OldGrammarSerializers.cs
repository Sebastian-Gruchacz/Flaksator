using System;
using System.Collections.Generic;
using System.Text;
using SharpDevs.Fleksator.Grammar;

namespace SharpDevs.Fleksator.IO.Implementation
{
    internal class OldGrammarSerializers : IGrammarSerializers
    {
        private readonly IGrammaticalWordSerializer<Noun> _nounSerializer = new OldNounSerializer();

        public IGrammaticalWordSerializer<Noun> NounSerializer
        {
            get { return this._nounSerializer; }
        }
    }
}
