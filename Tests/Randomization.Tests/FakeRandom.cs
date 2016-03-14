using System;

namespace Randomization.Tests
{
    public class FakeRandom// : Random
    {
        private readonly int[] _fakeSequence;
        private int _index;

        public FakeRandom(int[] fakeSequence)
        {
            if (fakeSequence == null) throw new ArgumentNullException(nameof(fakeSequence));
            if (fakeSequence.Length == 0)
                throw new ArgumentException("Argument is empty collection", nameof(fakeSequence));

            _fakeSequence = fakeSequence;
            _index = 0;
        }

        public int GetNext()
        {
            try
            {
                return _fakeSequence[_index];
            }
            finally
            {
                CheckIndex();
            }
        }

        internal int GetNext(int max)
        {
            try
            {
                return _fakeSequence[_index];
            }
            finally
            {
                CheckIndex();
            }
        }

        internal int GetNext(int min, int max)
        {
            try
            {
                return _fakeSequence[_index];
            }
            finally
            {
                CheckIndex();
            }
        }

        public double GetNextDouble()
        {
            try
            {
                return (double)_fakeSequence[_index];
            }
            finally
            {
                CheckIndex();
            }
        }

        private void CheckIndex()
        {
            _index++;
            if (_index >= _fakeSequence.Length)
            {
                _index = 0;
            }
        }
    }
}