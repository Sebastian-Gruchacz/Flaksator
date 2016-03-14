namespace ObscureWare.Randomization
{
    public abstract class Rnd<T> : IRandomizerEventSource, IRandomizer
    {
        protected readonly T _randomEngine;

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
            int value = InnerGetNext();
            OnNextInt?.Invoke(this, value);
            return value;
        }

        public int GetNext(int max)
        {
            int value = InnerGetNext(max);
            OnNextMaxInt?.Invoke(this, new NextMaxIntEventHandlerArgs(max, value));
            return value;
        }

        public int GetNext(int min, int max)
        {
            int value = InnerGetNext(min, max);
            OnNextIntRange?.Invoke(this, new NextIntRangeEventHandlerArgs(min, max, value));
            return value;
        }

        public double GetNextDouble()
        {
            double value = InnerGetDouble();
            OnNextDouble?.Invoke(this, value);
            return value;
        }
        
        protected abstract int InnerGetNext();

        protected abstract int InnerGetNext(int max);

        protected abstract int InnerGetNext(int min, int max);

        protected abstract double InnerGetDouble();
    }
}
