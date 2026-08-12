using System;
using System.Collections.Generic;
using System.Text;
using SharpDevs.Fleksator.Grammar;

namespace SharpDevs.Fleksator.IO
{
    public interface IGrammarSerializers
    {
        IGrammaticalWordSerializer<Noun> NounSerializer { get; }
        IGrammaticalWordSerializer<Verb> VerbSerializer { get; }
        IGrammaticalWordSerializer<Pronoun> PronounSerializer { get; }
    }
}
