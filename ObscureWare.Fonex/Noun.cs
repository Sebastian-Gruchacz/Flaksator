using System;
using System.Collections.Generic;

namespace ObscureWare.Flex
{
    /// <summary>
    /// Rzeczownik
    /// </summary>
    [Serializable]
    public class Noun : DeflectionableGrammaticalWord
    {
        /// <summary>
        /// Genre of word
        /// </summary>
        public GrammaticalGender Genre { get; set; } = GrammaticalGender._Unknown;

        public bool HasIrregularGenre => this.IrregularGenre != GrammaticalGender._Unknown;

        public GrammaticalGender IrregularGenre { get; set; } = GrammaticalGender._Unknown;

        /// <summary>
        /// Root for other cases then Nominative
        /// </summary>
        public string RootOther { get; set; }

        /// <summary>
        /// Each declination type shall be chosen for this noun as default
        /// </summary>
        public string DeclinationType { get; set; } = "";

        public bool CanBeSingular => (this.Countability & NounCountability.SingularAllowed) == NounCountability.SingularAllowed;

        public bool CanBePlural => (this.Countability & NounCountability.PluralAllowed) == NounCountability.PluralAllowed;

        public NounCountability Countability { get; set; } = NounCountability.PluralAllowed | NounCountability.SingularAllowed;

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
            throw new NotImplementedException("Missing code!");
        }

        public object WriteNoun()
        {
            throw new NotImplementedException("Missing code!");
        }
    }

    [Flags]
    public enum NounCountability
    {
        SingularAllowed = 1,
        PluralAllowed = 2
    }
}