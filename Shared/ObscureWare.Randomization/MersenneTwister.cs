// Required ?!?! - but seems to not work if not set
#define x64

using System.Collections.Generic;
using System.Linq;
using System.Text;
using ObscureWare.Randomization;

/* 
   A C-program for MT19937, with initialization improved 2002/1/26.
   Coded by Takuji Nishimura and Makoto Matsumoto.

   Before using, initialize the state by using init_genrand(seed)  
   or init_by_array(init_key, key_length).

   Copyright (C) 1997 - 2002, Makoto Matsumoto and Takuji Nishimura,
   All rights reserved.                          

   Redistribution and use in source and binary forms, with or without
   modification, are permitted provided that the following conditions
   are met:

     1. Redistributions of source code must retain the above copyright
        notice, this list of conditions and the following disclaimer.

     2. Redistributions in binary form must reproduce the above copyright
        notice, this list of conditions and the following disclaimer in the
        documentation and/or other materials provided with the distribution.

     3. The names of its contributors may not be used to endorse or promote 
        products derived from this software without specific prior written 
        permission.

   THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
   "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
   LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
   A PARTICULAR PURPOSE ARE DISCLAIMED.  IN NO EVENT SHALL THE COPYRIGHT OWNER OR
   CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
   EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
   PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
   PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
   LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
   NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
   SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.


   Any feedback is very welcome.
   http://www.math.sci.hiroshima-u.ac.jp/~m-mat/MT/emt.html
   email: m-mat @ math.sci.hiroshima-u.ac.jp (remove space)
 
 
    C# Version by Sebastian Gruchacz
            August 2008, Sharp Devs Development
  
 
*/


namespace SharpDevs.Randomization
{
    public class MersenneTwister : IRandomizer
    {
        const int N = 624;
        const ulong M = 397;
        const ulong MATRIX_A = 0x9908b0dfUL; /* constant vector a */
        const ulong UPPER_MASK = 0x80000000UL; /* most significant w-r bits */
        const ulong LOWER_MASK = 0x7fffffffUL; /* least significant r bits */

        static ulong[] mt = new ulong[N]; /* the array for the state vector  */
        static int mti = N + 1; /* mti==N+1 means mt[N] is not initialized */

        /// <summary>
        /// initializes mt[N] with a seed 
        /// </summary>
        /// <param name="seed"></param>
        public MersenneTwister(int seed)
        {
            this.init_genrand((ulong)seed);
        }

        private void init_genrand(ulong seed)
        {
            mt[0] = seed & 0xffffffffUL;
            for (mti = 1; mti < N; mti++)
            {
                mt[mti] = (1812433253UL * (mt[mti - 1] ^ (mt[mti - 1] >> 30)) + (ulong)mti);
                /* See Knuth TAOCP Vol2. 3rd Ed. P.106 for multiplier. */
                /* In the previous versions, MSBs of the seed affect   */
                /* only MSBs of the array mt[].                        */
                /* 2002/01/09 modified by Makoto Matsumoto             */
#if x64                
                mt[mti] &= 0xffffffffUL;                /* for >32 bit machines */
#endif
            }
        }


        /* initialize by an array */
        /* init_key is the array for initializing keys */
        /* slight change for C++, 2004/2/26 */
        public void InitByArray(ulong[] init_key)
        {
            int i, j, k;
            this.init_genrand(19650218UL);
            i = 1; j = 0;
            k = (N > init_key.Length ? N : init_key.Length);
            for (; k > 0; k--)
            {
                mt[i] = (mt[i] ^ ((mt[i - 1] ^ (mt[i - 1] >> 30)) * 1664525UL)) + init_key[j] + (ulong)j; /* non linear */
#if x64  
                mt[i] &= 0xffffffffUL; /* for WORDSIZE > 32 machines */
#endif
                i++; j++;
                if (i >= N) { mt[0] = mt[N - 1]; i = 1; }
                if (j >= init_key.Length) j = 0;
            }
            for (k = N - 1; k > 0; k--)
            {
                mt[i] = (mt[i] ^ ((mt[i - 1] ^ (mt[i - 1] >> 30)) * 1566083941UL)) - (ulong)i; /* non linear */
#if x64  
                mt[i] &= 0xffffffffUL; /* for WORDSIZE > 32 machines */
#endif
                i++;
                if (i >= N) { mt[0] = mt[N - 1]; i = 1; }
            }

            mt[0] = 0x80000000UL; /* MSB is 1; assuring non-zero initial array */
        }

        static ulong[] mag01 = new ulong[] { 0x0UL, MATRIX_A };
        /* generates a random number on [0,0xffffffff]-interval */
        private ulong genrand_int32()
        {
            ulong y;
            /* mag01[x] = x * MATRIX_A  for x=0,1 */

            if (mti >= N)
            { /* generate N words at one time */
                ulong kk;

                if (mti == N + 1)   /* if init_genrand() has not been called, */
                    this.init_genrand(5489UL); /* a default initial seed is used */

                for (kk = 0; kk < N - M; kk++)
                {
                    y = (mt[kk] & UPPER_MASK) | (mt[kk + 1] & LOWER_MASK);
                    mt[kk] = mt[kk + M] ^ (y >> 1) ^ mag01[y & 0x1UL];
                }
                for (; kk < N - 1; kk++)
                {
                    y = (mt[kk] & UPPER_MASK) | (mt[kk + 1] & LOWER_MASK);
                    mt[kk] = mt[kk + M - N] ^ (y >> 1) ^ mag01[y & 0x1UL];
                }
                y = (mt[N - 1] & UPPER_MASK) | (mt[0] & LOWER_MASK);
                mt[N - 1] = mt[M - 1] ^ (y >> 1) ^ mag01[y & 0x1UL];

                mti = 0;
            }

            y = mt[mti++];

            /* Tempering */
            y ^= (y >> 11);
            y ^= (y << 7) & 0x9d2c5680UL;
            y ^= (y << 15) & 0xefc60000UL;
            y ^= (y >> 18);

            return y;
        }


        #region IRandomizer Members

        public ulong GetNextULong()
        {
            return this.genrand_int32();
        }

        public int GetNext()
        {
            return (int) this.genrand_int32();
        }

        public int GetNext(int max)
        {
            return (int)(this.GetNextDouble() * (double)max);
        }

        public int GetNext(int min, int max)
        {
            return (int)(this.GetNextDouble() * ((double)max - (double)min) + (double)min);

            //return (int)((long)(GetNextDouble() * ((double)max - (double)min) + (double)min) & (long)int.MaxValue);
        }

        public double GetNextDouble()
        {
            // 53 bit
            ulong a = this.genrand_int32() >> 5;
            ulong b = this.genrand_int32() >> 6;

            return (a * 67108864.0 + b) * (1.0 / 9007199254740992.0);
        }

        #endregion
    }
}
