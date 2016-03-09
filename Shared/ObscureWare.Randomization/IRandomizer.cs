namespace ObscureWare.Randomization
{
    //public interface IRandomizer
    //{
    //    int NextInt(int minIncludedValue, int maxExcludedValue);
    //}

    /// <summary>
    /// Interfece, that every RND engine shall provide
    /// </summary>
    public interface IRandomizer
    {
        /// <summary>
        /// Randomize an integer value
        /// </summary>
        /// <returns></returns>
        int GetNext();

        /// <summary>
        /// Randomize integer value not greater than <paramref name="max"/>
        /// </summary>
        /// <param name="max"></param>
        /// <returns></returns>
        int GetNext(int max);

        /// <summary>
        /// Randomize integer value not greater than <paramref name="max"/> and greater than <paramref name="min"/>
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        int GetNext(int min, int max);

        /// <summary>
        /// Randomize new real value
        /// </summary>
        /// <returns></returns>
        double GetNextDouble();
    }
}