using System;

namespace SharpDevs.Randomization
{
    /// <summary>
    /// Summary description for RandomMT.
    /// </summary>
    public class RandomMT
    {
        private const int N = 624;
        private const int M = 397;
        private const uint K = 0x9908B0DFU;
        private const uint DEFAULT_SEED = 4357;

        private ulong[] state = new ulong[N + 1];
        private int next = 0;
        private ulong seedValue;


        public RandomMT()
        {
            this.SeedMT(DEFAULT_SEED);
        }
        public RandomMT(ulong _seed)
        {
            this.seedValue = _seed;
            this.SeedMT(this.seedValue);
        }

        public ulong RandomInt()
        {
            ulong y;

            if ((this.next + 1) > N)
                return (this.ReloadMT());

            y = this.state[this.next++];
            y ^= (y >> 11);
            y ^= (y << 7) & 0x9D2C5680U;
            y ^= (y << 15) & 0xEFC60000U;
            return (y ^ (y >> 18));
        }

        private void SeedMT(ulong _seed)
        {
            ulong x = (_seed | 1U) & 0xFFFFFFFFU;
            int j = N;

            for (j = N; j >= 0; j--)
            {
                this.state[j] = (x *= 69069U) & 0xFFFFFFFFU;
            }
            this.next = 0;
        }

        public int RandomRange(int lo, int hi)
        {
            return (Math.Abs((int) this.RandomInt() % (hi - lo + 1)) + lo);
        }

        public int RollDice(int face, int number_of_dice)
        {
            int roll = 0;
            for (int loop = 0; loop < number_of_dice; loop++)
            {
                roll += (this.RandomRange(1, face));
            }
            return roll;
        }

        public int HeadsOrTails() { return ((int)(this.RandomInt()) % 2); }

        public int D6(int die_count) { return this.RollDice(6, die_count); }
        public int D8(int die_count) { return this.RollDice(8, die_count); }
        public int D10(int die_count) { return this.RollDice(10, die_count); }
        public int D12(int die_count) { return this.RollDice(12, die_count); }
        public int D20(int die_count) { return this.RollDice(20, die_count); }
        public int D25(int die_count) { return this.RollDice(25, die_count); }


        private ulong ReloadMT()
        {
            ulong[] p0 = this.state;
            int p0pos = 0;
            ulong[] p2 = this.state;
            int p2pos = 2;
            ulong[] pM = this.state;
            int pMpos = M;
            ulong s0;
            ulong s1;

            int j;

            if ((this.next + 1) > N)
                this.SeedMT(this.seedValue);

            for (s0 = this.state[0], s1 = this.state[1], j = N - M + 1; --j > 0; s0 = s1, s1 = p2[p2pos++])
                p0[p0pos++] = pM[pMpos++] ^ (this.mixBits(s0, s1) >> 1) ^ (this.loBit(s1) != 0 ? K : 0U);


            for (pM[0] = this.state[0], pMpos = 0, j = M; --j > 0; s0 = s1, s1 = p2[p2pos++])
                p0[p0pos++] = pM[pMpos++] ^ (this.mixBits(s0, s1) >> 1) ^ (this.loBit(s1) != 0 ? K : 0U);


            s1 = this.state[0];
            p0[p0pos] = pM[pMpos] ^ (this.mixBits(s0, s1) >> 1) ^ (this.loBit(s1) != 0 ? K : 0U);
            s1 ^= (s1 >> 11);
            s1 ^= (s1 << 7) & 0x9D2C5680U;
            s1 ^= (s1 << 15) & 0xEFC60000U;
            return (s1 ^ (s1 >> 18));
        }

        private ulong hiBit(ulong _u)
        {
            return ((_u) & 0x80000000U);
        }
        private ulong loBit(ulong _u)
        {
            return ((_u) & 0x00000001U);
        }
        private ulong loBits(ulong _u)
        {
            return ((_u) & 0x7FFFFFFFU);
        }
        private ulong mixBits(ulong _u, ulong _v)
        {
            return (this.hiBit(_u) | this.loBits(_v));
        }
    }
}