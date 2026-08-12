using System.Collections.Generic;

namespace SharpDevs.Fleksator.Grammar
{
    public sealed class Pronoun : GrammaticalWord
    {
        private readonly List<PronounForm> forms = new List<PronounForm>();

        public List<PronounForm> Forms
        {
            get { return this.forms; }
        }
    }
}
