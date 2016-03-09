using System;
using MersenneTwister;
using NUnit.Framework;
using SharpDevs.Randomization;

namespace Randomization.Tests
{
    [TestFixture]
    public class RandomizersComparisonTests
    {
        [Test]
        public void CompareSequencesOfMersennes()
        {
            Random rnd = new Random();
            for (int i = 0; i < 10; i++)
            {
                int seed = rnd.Next();
                var m1 = new MT19937();
                m1.init_genrand((ulong)seed);
                var m2 = new RandomMT((ulong)seed);
                var m3 = new SharpDevs.Randomization.MersenneTwister(seed);
                var m4 = new Random(seed);

                for (int j = 0; j < 10; j++)
                {
                    var r1 = m1.genrand_int32();
                    var r2 = m2.RandomInt();
                    var r3 = m3.GetNextULong();
                    var r4 = m4.Next();

                    Console.WriteLine("{0}\t{1}\t{2}\t{3}", r1, r2, r3, r4);

                    //Assert.AreEqual(r1, r2);
                    //Assert.AreEqual(r2, r3);
                }

                Console.WriteLine();
            }

        }
    }
}
