using System;

namespace ObscureWare.Randomization
{
    internal class SystemRnd : Rnd<Random>
    {
        public SystemRnd(Random randomEngine) : base(randomEngine)
        {
        }

        protected override int InnerGetNext()
        {
            return _randomEngine.Next();
        }

        protected override int InnerGetNext(int max)
        {
            return _randomEngine.Next(max);
        }

        protected override int InnerGetNext(int min, int max)
        {
            return _randomEngine.Next(min, max);
        }

        protected override double InnerGetDouble()
        {
            return _randomEngine.NextDouble();
        }
    }
}