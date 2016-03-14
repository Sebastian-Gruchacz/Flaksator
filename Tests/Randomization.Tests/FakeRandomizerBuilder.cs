using ObscureWare.Randomization;

namespace Randomization.Tests
{
    public class FakeRandomizerBuilder : IRandomizerBuilder
    {
        private readonly int[] _fakeSequence;

        public FakeRandomizerBuilder(params int[] fakeSequence)
        {
            _fakeSequence = fakeSequence;
        }

        public IRandomizer ConstructDeafultRandomizer()
        {
            return new FakeRnd(new FakeRandom(_fakeSequence));
        }

        public IRandomizer ConstructRandomizer(int seed)
        {
            return new FakeRnd(new FakeRandom(_fakeSequence));
        }
    }
}
