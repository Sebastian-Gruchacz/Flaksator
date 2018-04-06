namespace Randomization.Tests
{
    using ObscureWare.Randomization;

    public class FakeRnd : Rnd<FakeRandom>
    {
        public FakeRnd(FakeRandom fakeRandom) : base(fakeRandom)
        {

        }

        protected override int InnerGetNext(FakeRandom engine)
        {
            return engine.GetNext();
        }

        protected override int InnerGetNext(FakeRandom engine, int max)
        {
            return engine.GetNext(max);
        }

        protected override int InnerGetNext(FakeRandom engine, int min, int max)
        {
            return engine.GetNext(min, max);
        }

        protected override double InnerGetDouble(FakeRandom engine)
        {
            return engine.GetNextDouble();
        }
    }
}