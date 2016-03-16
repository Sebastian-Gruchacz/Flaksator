namespace ObscureWare.Randomization
{
    public abstract class Rnd<T> : IRandomizerEventSource, IRandomizer
    {
        private readonly T _randomEngine;

        protected Rnd(T randomEngine)
        {
            _randomEngine = randomEngine;
        }

        public event NextIntEventHandler OnNextInt;
        public event NextMaxIntEventHandler OnNextMaxInt;
        public event NextIntRangeEventHandler OnNextIntRange;
        public event NextDoubleEventHandler OnNextDouble;

        public int GetNext()
        {
            int value = InnerGetNext(_randomEngine);
            OnNextInt?.Invoke(this, value);
            return value;
        }

        public int GetNext(int max)
        {
            int value = InnerGetNext(_randomEngine, max);
            OnNextMaxInt?.Invoke(this, new NextMaxIntEventHandlerArgs(max, value));
            return value;
        }

        public int GetNext(int min, int max)
        {
            int value = InnerGetNext(_randomEngine, min, max);
            OnNextIntRange?.Invoke(this, new NextIntRangeEventHandlerArgs(min, max, value));
            return value;
        }

        public double GetNextDouble()
        {
            double value = InnerGetDouble(_randomEngine);
            OnNextDouble?.Invoke(this, value);
            return value;
        }
        
        protected abstract int InnerGetNext(T engine);

        protected abstract int InnerGetNext(T engine, int max);

        protected abstract int InnerGetNext(T engine, int min, int max);

        protected abstract double InnerGetDouble(T engine);
    }
}
