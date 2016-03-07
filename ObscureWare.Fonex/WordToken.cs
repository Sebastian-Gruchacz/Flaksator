using System;

namespace ObscureWare.Flex
{
    /// <summary>
    /// TODO: rediscover purpose of this type...
    /// </summary>
    [Serializable]
    public class WordToken
    {
        /// <summary>
        /// Liczba (pojedyncza lub mnoga)
        /// </summary>
        public DecliantionNumber DecliantionNumber { get; set; }

        /// <summary>
        /// Przypadek
        /// </summary>
        public InflectionCase InflectionCase { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Text { get; set; }

        public WordToken()
        {

        }

        public WordToken(string text, InflectionCase inflectionCase, DecliantionNumber decliantionNumber)
        {
            this.Text = text;
            this.InflectionCase = inflectionCase;
            this.DecliantionNumber = decliantionNumber;
        }

        public virtual bool Is(WordToken token2)
        {
            return (this.DecliantionNumber == token2.DecliantionNumber &&
                    this.InflectionCase == token2.InflectionCase);
        }
    }
}