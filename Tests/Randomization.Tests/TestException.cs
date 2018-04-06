namespace Randomization.Tests
{
    using System;

    [Serializable]
    internal class TestException : Exception
    {
        public TestException(string message) : base(message)
        {
            
        }
    }
}