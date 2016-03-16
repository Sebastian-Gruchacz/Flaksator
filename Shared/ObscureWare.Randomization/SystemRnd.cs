using System;

namespace ObscureWare.Randomization
{
    internal class SystemRnd : Rnd<Random>
    {
        public SystemRnd(Random randomEngine) : base(randomEngine)
        {
        }

        protected override int InnerGetNext(Random engine)
        {
            return engine.Next();
        }

        protected override int InnerGetNext(Random engine, int max)
        {
            return engine.Next(max);
        }

        protected override int InnerGetNext(Random engine, int min, int max)
        {
            return engine.Next(min, max);
        }

        protected override double InnerGetDouble(Random engine)
        {
            return engine.NextDouble();
        }
    }
}