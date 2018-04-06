namespace Randomization.Tests
{
    using System;
    using System.Diagnostics;

    using NUnit.Framework;

    using ObscureWare.Randomization;

    [TestFixture]
    public class RandomizersInfrastructureTests
    {
        [Test]
        public void CompareCleanPerformanceVsInfrastructure()
        {
            Random pureImpl = new Random(0);

            FakeRandomizerBuilder fakeImplBuilder = new FakeRandomizerBuilder(0); // short
            var fakeimpl = fakeImplBuilder.ConstructRandomizer(0);

            SystemRandomizerBuilder sysBuilder = new SystemRandomizerBuilder();
            var sysImpl = sysBuilder.ConstructRandomizer(0);

            int testQuantity = 10000000;

            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < testQuantity; i++)
            {
                int r = pureImpl.Next();
            }
            sw.Stop();
            long pureMs = sw.ElapsedMilliseconds;

            sw.Restart();
            for (int i = 0; i < testQuantity; i++)
            {
                int r = fakeimpl.GetNext();
            }
            sw.Stop();
            long fakemMs = sw.ElapsedMilliseconds;

            sw.Restart();
            for (int i = 0; i < testQuantity; i++)
            {
                int r = sysImpl.GetNext();
            }
            sw.Stop();
            long sysMs = sw.ElapsedMilliseconds;

            Console.WriteLine("Times compare(ms): {0}(PURE), {1}(FAKE), {2}(SYS)",
                pureMs, fakemMs, sysMs);
        }

        // Times compare(ms): 117(PURE), 378(FAKE), 356(SYS) - single run on laptop
        // Times compare(ms): 1057(PURE), 2089(FAKE), 1793(SYS) - profiling on laptop
        // Times compare(ms): 80(PURE), 105(FAKE), 114(SYS) - monster (release)
        // Times compare(ms): 94(PURE), 377(FAKE), 357(SYS) - monster (debug)
        // Times compare(ms): 928(PURE), 1900(FAKE), 2106(SYS) - monster, debug, profiling
    }
}
