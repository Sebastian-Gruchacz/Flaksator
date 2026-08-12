using System.Collections.Generic;

namespace SharpDevs.Fleksator.Grammar
{
    public partial class Verb
    {
        private readonly List<VerbForm> forms = new List<VerbForm>();

        public List<VerbForm> Forms
        {
            get { return this.forms; }
        }
    }
}
