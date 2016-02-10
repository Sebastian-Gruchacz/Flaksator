using System;
using System.Runtime.InteropServices;

namespace ObscureWare.ErrorHandling.Tests
{
    /// <summary>
    /// Test helper class
    /// </summary>
    class BusinessLogicRepository
    {
        public const string SAMPLE_TEXT = @"Proper response!";
        public const string ERROR_MESSAGE = @"Operation has failed.";
        public const string EXCEPTION_MESSAGE = @"The Operation has failed.";

        public OperationResult<string> SomeSuccessfullReadOperation()
        {
            return SAMPLE_TEXT;
        }

        public OperationResult<string> SomeFailingReadOperation()
        {
            return new Fail(ERROR_MESSAGE);
        }

        public OperationResult<string> SomeExceptionalReadOperation()
        {
            try
            {
                throw new InvalidOperationException(EXCEPTION_MESSAGE);
            }
            catch (Exception exception)
            {
                return exception;
            }
        }
    }
}