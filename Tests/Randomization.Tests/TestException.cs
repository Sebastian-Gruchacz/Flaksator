using System;

namespace Randomization.Tests
{
    [Serializable]
    internal class TestException : Exception
    {
        public TestException(string message) : base(message)
        {
            
        }
    }
}