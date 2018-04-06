using System;

namespace ObscureWare.Flex
{
    /// <summary>
    /// Bazowa klasa dla słów
    /// </summary>
    [Serializable]
    public abstract class GrammaticalWord
    {
        /// <summary>
        /// This is used to display word on list (in most cases equal to root, but sometimes shall be different)
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Rdzeń słowa (lub pełna forma jeśli nieodmienny)
        /// </summary>
        /// <remarks>Dla przymiotników - formą podstawową jest mianownik l.p. rodzaju żeńskiego</remarks>
        public string Root { get; set; }

        /// <summary>
        /// Słowo odmienia się nieregularnie
        /// </summary>
        public bool IsException { get; set; } = false;

        /// <summary>
        /// Słowo się nie odmienia
        /// </summary>
        public bool IsConstant { get; set; } = false;

        /// <summary>
        /// Mostly for Debug / Edition
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.DisplayName;
        }
    }
}
