using System.Collections.Generic;

namespace SharpDevs.Fleksator.Grammar
{
    public class Noun : DeflectionableGrammaticalWord
    {
        /// <summary>
        /// Genre of word
        /// </summary>
        public GrammaticalGender Genre { get; set; } = GrammaticalGender._Unknown;

        public bool HasIrregularGenre { get; set; } = false;

        public GrammaticalGender IrregularGenre { get; set; } = GrammaticalGender._Unknown;

        /// <summary>
        /// Root for other cases then Nominative
        /// </summary>
        public string RootOther { get; set; }

        /// <summary>
        /// Each declination type shall be choosed for this noun as default
        /// </summary>
        public string DeclinationType { get; set; } = "";

        public bool CanBeSingular { get; set; } = true;

        public bool CanBePlural { get; set; } = true;

        public List<int> Categories { get; } = new List<int>();

        #region Post Fix Indexing

        public Dictionary<InflectionCase, int> SingularPostfixSelector { get; set; } =
            new Dictionary<InflectionCase, int>();


        public Dictionary<InflectionCase, int> PluralPostfixSelector { get; set; } =
            new Dictionary<InflectionCase, int>();

        #endregion

        /// <summary>
        /// Irregular forms of this Noun
        /// </summary>
        public List<WordToken> Irregulars { get; internal set; } = new List<WordToken>();

        private void Clear()
        {
            this.Irregulars.Clear();
            this.Categories.Clear();
            this.SingularPostfixSelector.Clear();
            this.PluralPostfixSelector.Clear();
        }

        public void AnalyzeLine(object writeNoun)
        {
            //throw new System.NotImplementedException("Missing code!");
        }

        public object WriteNoun()
        {
            return this;
            throw new System.NotImplementedException("Missing code!");
        }
    }
}
