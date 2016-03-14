using System;

namespace ObscureWare.Randomization
{
    public class SystemRandomizerBuilder : IRandomizerBuilder
    {
        public IRandomizer ConstructDeafultRandomizer()
        {
            return new SystemRnd(new Random());
        }

        public IRandomizer ConstructRandomizer(int seed)
        {
            return new SystemRnd(new Random(seed));
        }
    }
}