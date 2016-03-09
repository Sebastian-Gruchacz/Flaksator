using System;

namespace ObscureWare.Randomization
{
    /// <summary>
    /// According to MSDN: The current implementation of the Random class is based on Donald E. Knuth's subtractive 
    /// random number generator algorithm. For more information, see D. E. Knuth.
    /// "The Art of Computer Programming, volume 2: Seminumerical Algorithms". Addison-Wesley, Reading, MA, second edition, 1981. 
    /// </summary>
    internal class SystemRandomWrapper : IRandomizer
    {
        private Random rnd = null;

        internal SystemRandomWrapper(int seed)
        {
            rnd = new Random(seed);
        }

        #region IRandomizer Members

        public int GetNext()
        {
            return rnd.Next();
        }

        public int GetNext(int min, int max)
        {
            return rnd.Next(min, max);
        }

        /// <summary>
        /// Returns a random number beetween 0.0 and 1.0
        /// </summary>
        /// <returns></returns>
        public double GetNextDouble()
        {
            return rnd.NextDouble();
        }

        /// <summary>
        /// returns a random number matching given range: &lt;0, max)
        /// </summary>
        /// <param name="max"></param>
        /// <returns></returns>
        public int GetNext(int max)
        {
            return rnd.Next(max);
        }

        #endregion
    }
}