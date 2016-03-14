namespace ObscureWare.Randomization
{
    public interface IRandomizerBuilder
    {
        IRandomizer ConstructDeafultRandomizer();

        IRandomizer ConstructRandomizer(int seed);
    }
}
