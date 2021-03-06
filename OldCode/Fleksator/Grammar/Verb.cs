namespace SharpDevs.Fleksator.Grammar
{
    public class Verb : ConjugativeGrammaticalWord
    {
        public Verb()
        {
            
        }

        /// <summary>
        /// Forma bezokolicznika
        /// </summary>
        public string Infinitive
        {
            get { return this.Root; }
            set { this.Root = value; }
        }

        /// <summary>
        /// Numer koniugacji
        /// </summary>
        public int ConjugationNumber { get; set; }

        /// <summary>
        /// Moze występować w stronie czynnej
        /// </summary>
        public bool CanBeActive { get; set; }

        /// <summary>
        /// Moze występować w stronie biernej
        /// </summary>
        public bool CanBePassive { get; set; }
    }
}
