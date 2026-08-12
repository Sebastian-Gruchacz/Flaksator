using System.Collections.Generic;

namespace SharpDevs.Fleksator.Grammar
{
    public partial class Verb
    {
        private readonly List<VerbForm> forms = new List<VerbForm>();
        private readonly List<int> categories = new List<int>();

        public List<VerbForm> Forms
        {
            get { return this.forms; }
        }

        public bool IsReflexive { get; set; }

        public VerbAspect Aspect { get; set; }

        public List<int> Categories
        {
            get { return this.categories; }
        }
    }
}
