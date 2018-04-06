namespace ObscureWare.Randomization
{
    public abstract class Rnd<T> : IRandomizerEventSource, IRandomizer
    {
        private readonly T _randomEngine;

        protected Rnd(T randomEngine)
        {
            this._randomEngine = randomEngine;
        }

        public event NextIntEventHandler OnNextInt;
        public event NextMaxIntEventHandler OnNextMaxInt;
        public event NextIntRangeEventHandler OnNextIntRange;
        public event NextDoubleEventHandler OnNextDouble;

        public int GetNext()
        {
            int value = this.InnerGetNext(this._randomEngine);
            this.OnNextInt?.Invoke(this, value);
            return value;
        }

        public int GetNext(int max)
        {
            int value = this.InnerGetNext(this._randomEngine, max);
            this.OnNextMaxInt?.Invoke(this, new NextMaxIntEventHandlerArgs(max, value));
            return value;
        }

        public int GetNext(int min, int max)
        {
            int value = this.InnerGetNext(this._randomEngine, min, max);
            this.OnNextIntRange?.Invoke(this, new NextIntRangeEventHandlerArgs(min, max, value));
            return value;
        }

        public double GetNextDouble()
        {
            double value = this.InnerGetDouble(this._randomEngine);
            this.OnNextDouble?.Invoke(this, value);
            return value;
        }
        
        protected abstract int InnerGetNext(T engine);

        protected abstract int InnerGetNext(T engine, int max);

        protected abstract int InnerGetNext(T engine, int min, int max);

        protected abstract double InnerGetDouble(T engine);
    }
}
