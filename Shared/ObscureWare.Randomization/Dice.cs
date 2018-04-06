using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDevs.Randomization;

namespace ObscureWare.Randomization
{
    /// <summary>
    /// Defines available Dices
    /// </summary>
    public enum Dices { d4 = 4, d6 = 6, d8 = 8, d10 = 10, d12 = 12, d20 = 20, d100 = 100 };

    public class Dice
    {
        // TODO: make it singleton and IDebugable

        public int Roll(int count, int diceSides, IRandomizer rnd)
        {
            int result = rnd.GetNext(count, diceSides * count + 1);

            //DoDebugLog(string.Format("Dice roll: {0}d{1} = {2}", count, diceSides, result));

            return result;
        }

        public int Roll(int count, Dices dice, IRandomizer rnd)
        {
            return this.Roll(count, (int)dice, rnd);
        }

        public int Roll(string rollDesc, IRandomizer rnd)
        {
            return -1; // TODO: implement parser
        }

        /// <summary>
        /// Complete random roll of random count of random dice type
        /// </summary>
        /// <returns></returns>
        /// <remarks>There will be 1 to 9 rolled dices</remarks>
        public int Roll(IRandomizer rnd)
        {
            int dicesCount = rnd.GetNext(1, 10);
            string[] numDiceTypes = Enum.GetNames(typeof(Dices));
            int selectedDiceIndex = rnd.GetNext(numDiceTypes.Length);
            Dices selectedDice = (Dices)Enum.Parse(typeof(Dices), numDiceTypes[selectedDiceIndex]);

            return this.Roll(dicesCount, selectedDice, rnd);
        }

        //private void DoDebugLog(string message)
        //{
        //    SharpDevs.Debugging.DebugLogsSink.Instance.Log(message,
        //        SharpDevs.Debugging.ErrorSeverity.Debug);
        //}
    }
}
