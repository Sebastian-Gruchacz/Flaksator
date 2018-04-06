namespace ObscureWare.Randomization
{
    public interface IRandomizerEventSource
    {
        event NextIntEventHandler OnNextInt;

        event NextMaxIntEventHandler OnNextMaxInt;

        event NextIntRangeEventHandler OnNextIntRange;

        event NextDoubleEventHandler OnNextDouble;
    }

    public delegate void NextIntRangeEventHandler(object sender, NextIntRangeEventHandlerArgs args);

    public class NextIntRangeEventHandlerArgs
    {
        public NextIntRangeEventHandlerArgs(int minRange, int maxRange, int generatedValue)
        {
            this.MaxRange = maxRange;
            this.GeneratedValue = generatedValue;
            this.MinRange = minRange;
        }

        public int MinRange { get; private set; }

        public int MaxRange { get; private set; }

        public int GeneratedValue { get; private set; }
    }

    public delegate void NextDoubleEventHandler(object sender, double args);

    public delegate void NextMaxIntEventHandler(object sender, NextMaxIntEventHandlerArgs args);

    public struct NextMaxIntEventHandlerArgs
    {
        public NextMaxIntEventHandlerArgs(int maxRange, int generatedValue)
        {
            this.MaxRange = maxRange;
            this.GeneratedValue = generatedValue;
        }

        public int MaxRange { get; private set; }

        public int GeneratedValue { get; private set; }
    }

    public delegate void NextIntEventHandler(object sender, int args);
}