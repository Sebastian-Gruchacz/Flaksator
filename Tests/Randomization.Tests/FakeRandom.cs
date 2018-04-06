namespace Randomization.Tests
{
    using System;

    public class FakeRandom// : Random
    {
        private readonly int[] _fakeSequence;
        private int _index;

        public FakeRandom(int[] fakeSequence)
        {
            if (fakeSequence == null) throw new ArgumentNullException(nameof(fakeSequence));
            if (fakeSequence.Length == 0)
                throw new ArgumentException(@"Argument is empty collection", nameof(fakeSequence));

            this._fakeSequence = fakeSequence;
            this._index = 0;
        }

        public int GetNext()
        {
            try
            {
                return this._fakeSequence[this._index];
            }
            finally
            {
                this.CheckIndex();
            }
        }

        internal int GetNext(int max)
        {
            try
            {
                int value = this._fakeSequence[this._index];
                if (value >= max)
                {
                    throw new TestException(@"Value in FakeRandom sequence is greater than expected GetNext(max) boundary.");
                }
                return value;
            }
            finally
            {
                this.CheckIndex();
            }
        }

        internal int GetNext(int min, int max)
        {
            try
            {
                int value = this._fakeSequence[this._index];
                if (value < min)
                {
                    throw new TestException(@"Value in FakeRandom sequence is lower than expected GetNext(min, max) boundary.");
                }
                if (value >= max)
                {
                    throw new TestException(@"Value in FakeRandom sequence is greater than expected GetNext(min, max) boundary.");
                }
                return value;
            }
            finally
            {
                this.CheckIndex();
            }
        }

        public double GetNextDouble()
        {
            try
            {
                return (double) this._fakeSequence[this._index];
            }
            finally
            {
                this.CheckIndex();
            }
        }

        private void CheckIndex()
        {
            this._index++;
            if (this._index >= this._fakeSequence.Length)
            {
                this._index = 0;
            }
        }
    }
}