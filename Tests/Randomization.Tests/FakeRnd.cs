using ObscureWare.Randomization;

namespace Randomization.Tests
{
    public class FakeRnd : Rnd<FakeRandom>
    {
        private readonly FakeRandom _fakeRandom;

        public FakeRnd(FakeRandom fakeRandom) : base(fakeRandom)
        {
            _fakeRandom = fakeRandom;
        }

        protected override int InnerGetNext()
        {
            return _fakeRandom.GetNext();
        }

        protected override int InnerGetNext(int max)
        {
            return _fakeRandom.GetNext(max);
        }

        protected override int InnerGetNext(int min, int max)
        {
            return _fakeRandom.GetNext(min, max);
        }

        protected override double InnerGetDouble()
        {
            return _fakeRandom.GetNextDouble();
        }
    }
}