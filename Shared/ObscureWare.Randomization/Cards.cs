using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObscureWare.Randomization
{
    public enum CardColors { Clubs, Diamonds, Hearts, Spades };
    public enum CardFigures { Ace = 1, F2, F3, F4, F5, F6, F7, F8, F9, F10, Jack = 11, Queen = 12, King = 13, Joker = 14 };

    public class Card
    {
        private CardColors color = CardColors.Clubs;

        public CardColors Color
        {
            get { return color; }
        }

        private CardFigures figure = CardFigures.Ace;

        public CardFigures Figure
        {
            get { return figure; }
        }

        public Card(CardColors color, CardFigures figure)
        {
            this.color = color;
            this.figure = figure;
        }

        public override string ToString()
        {
            int figureVal = (int)this.figure;
            string figureName = this.figure.ToString();
            if (figureVal >= 2 && figureVal <= 10)
                figureName = figureVal.ToString();
            return string.Format("{0} of {1}", figureName, color.ToString());
        }

    }

    /// <summary>
    /// This class provides your game with cards shuffling and structures
    /// <remarks>Thsi class has not been sealed so you may someday extend it with your own functionalities</remarks>
    /// </summary>
    public static class CardSets
    {
        /// <summary>
        /// return (shuffled) set(s) of 
        /// </summary>
        /// <param name="setsCount"></param>
        /// <param name="includeJokers"></param>
        /// <param name="shuffled"></param>
        /// <returns></returns>
        public static Card[] GetSet52(int setsCount, bool includeJokers, bool shuffled)
        {
            // TODO: implement
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setsCount"></param>
        /// <param name="shuffled"></param>
        /// <returns></returns>
        /// <remarks>No jokers for that set</remarks>
        public static Card[] GetSet24(int setsCount, bool shuffled)
        {
            // TODO: implement
            return null;
        }
    }
}
