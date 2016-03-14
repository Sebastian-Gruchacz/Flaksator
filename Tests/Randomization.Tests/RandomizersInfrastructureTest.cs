using System;
using System.Diagnostics;
using NUnit.Framework;
using ObscureWare.Randomization;

namespace Randomization.Tests
{
    [TestFixture]
    public class RandomizersInfrastructureTest
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

        // Times compare(ms): 117(PURE), 378(FAKE), 356(SYS) - single run
        // Times compare(ms): 1057(PURE), 2089(FAKE), 1793(SYS) - profiling
    }
}
