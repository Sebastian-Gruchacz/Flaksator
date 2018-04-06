namespace Randomization.Tests
{
    using ObscureWare.Randomization;

    public class FakeRandomizerBuilder : IRandomizerBuilder
    {
        private readonly int[] _fakeSequence;

        public FakeRandomizerBuilder(params int[] fakeSequence)
        {
            this._fakeSequence = fakeSequence;
        }

        public IRandomizer ConstructDeafultRandomizer()
        {
            return new FakeRnd(new FakeRandom(this._fakeSequence));
        }

        public IRandomizer ConstructRandomizer(int seed)
        {
            return new FakeRnd(new FakeRandom(this._fakeSequence));
        }
    }
}
