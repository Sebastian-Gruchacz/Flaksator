using System;
using System.Diagnostics;
using System.Linq;
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

        [Test]
        public void CompareDistributionOfMersennes()
        {
            Random rnd = new Random();
            int baseSeed = rnd.Next();

            var m1 = new MT19937();
            m1.init_genrand((ulong)baseSeed);
            var m2 = new RandomMT((ulong)baseSeed);
            var m3 = new SharpDevs.Randomization.MersenneTwister(baseSeed);
            var m4 = new Random(baseSeed);
            var maxRange = 1000;
            var iterations = 10000000;

            var dist1 = CalculateDistribution(() => m1.RandomRange(0, maxRange - 1), maxRange, iterations);
            var dist2 = CalculateDistribution(() => m2.RandomRange(0, maxRange - 1), maxRange, iterations);
            var dist3 = CalculateDistribution(() => m3.GetNext(0, maxRange), maxRange, iterations);
            var dist4 = CalculateDistribution(() => m4.Next(0, maxRange), maxRange, iterations);

            var distribution1 = AnalyzeDistribution(dist1, iterations);
            var distribution2 = AnalyzeDistribution(dist2, iterations);
            var distribution3 = AnalyzeDistribution(dist3, iterations);
            var distribution4 = AnalyzeDistribution(dist4, iterations);

            Console.WriteLine("{0}\t{1}\t{2}\t{3}",
                distribution1, distribution2, distribution3, distribution4);
            var expectedFill = (decimal)iterations / maxRange;
            Console.WriteLine("Przy oczekiwanym napełnieniu: " + expectedFill);
            Console.WriteLine("Co daje procentowo:");
            Console.WriteLine("{0}\t{1}\t{2}\t{3}",
                distribution1 / expectedFill * 100,
                distribution2 / expectedFill * 100,
                distribution3 / expectedFill * 100,
                distribution4 / expectedFill * 100);
        }

        private decimal AnalyzeDistribution(int[] distribution, int iterations)
        {
            int expectedScores = distribution.Length;
            decimal expectedScore = (decimal)iterations/ expectedScores;
            var deviation = distribution.Average(i => Math.Abs(i - expectedScore));
            return deviation;
        }

        private int[] CalculateDistribution(Func<int> generator, int maxRange, int iterations)
        {
            Stopwatch sw = Stopwatch.StartNew();

            int[] resultScore = new int[maxRange];

            for (int i = 0; i < iterations; i++)
            {
                int score = generator.Invoke();
                resultScore[score]++;
            }

            sw.Stop();
            Console.WriteLine(@"Czas wykonania: " + sw.ElapsedMilliseconds + @"ms");

            return resultScore;
        }
    }
    
}
