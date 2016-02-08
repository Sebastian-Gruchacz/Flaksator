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
            get { return base.Root; }
            set { Root = value; }
        }

        /// <summary>
        /// Numer koniugacji
        /// </summary>
        public int ConjugationNumber { get; set; }

        /// <summary>
        /// Moze wystêpowaæ w stronie czynnej
        /// </summary>
        public bool CanBeActive { get; set; }

        /// <summary>
        /// Moze wystêpowaæ w stronie biernej
        /// </summary>
        public bool CanBePassive { get; set; }
    }
}
