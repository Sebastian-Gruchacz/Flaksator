using System;
using NUnit.Framework;

namespace ObscureWare.ErrorHandling.Tests
{
    public class StructOperationResultTests
    {
        readonly BusinessLogicRepository _testedInstance = new BusinessLogicRepository();

        [Test]
        public void ProperlyExecutedMethodShallReturnValidResult()
        {
            var result = _testedInstance.SomeSuccessfullReadOperation();
            Assert.NotNull(result);
            Assert.IsTrue(result.Success);
            Assert.IsFalse(result.Failed);
            Assert.AreEqual(ErrorCodes.Success, result.ErrorCode);
            Assert.AreEqual(BusinessLogicRepository.SAMPLE_TEXT, result.Value);
        }

        [Test]
        public void ResultOfProperlyExecutedMethodShallThrowExceptionIfTryingToReadErrorMessage()
        {
            var result = _testedInstance.SomeSuccessfullReadOperation();
            Assert.Throws<InvalidOperationException>(() => Console.WriteLine(result.ErrorMessage));
        }

        [Test]
        public void ResultOfProperlyExecutedMethodShallThrowExceptionIfTryingToReadStackTrace()
        {
            var result = _testedInstance.SomeSuccessfullReadOperation();
            Assert.Throws<InvalidOperationException>(() => Console.WriteLine(result.StackTrace));
        }

        [Test]
        public void ResultOfProperlyExecutedMethodShallThrowExceptionIfTryingToReadException()
        {
            var result = _testedInstance.SomeSuccessfullReadOperation();
            Assert.Throws<InvalidOperationException>(() => Console.WriteLine(result.Exception));
        }

        [Test]
        public void SimplyFailingMethodShallReturnFailedResultWithProperMessage()
        {
            var result = _testedInstance.SomeFailingReadOperation();
            Assert.NotNull(result);
            Assert.IsTrue(result.Failed);
            Assert.IsFalse(result.Success);
            Assert.IsNull(result.Exception);
            Assert.AreNotEqual(ErrorCodes.Success, result.ErrorCode);
            Assert.AreEqual(BusinessLogicRepository.ERROR_MESSAGE, result.ErrorMessage);
        }

        [Test]
        public void SimplyFailingMethodShallThrowExceptionWhenTryingToAccessValue()
        {
            var result = _testedInstance.SomeFailingReadOperation();
            Assert.Throws<InvalidOperationException>(() => Console.WriteLine(result.Value));
        }

        [Test]
        public void ExceptionallyFailingMethodShallReturnFailedResultWithProperMessage()
        {
            var result = _testedInstance.SomeExceptionalReadOperation();
            Assert.NotNull(result);
            Assert.IsTrue(result.Failed);
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
            Assert.AreEqual(ErrorCodes.GenericFailure, result.ErrorCode);
            Assert.AreEqual(BusinessLogicRepository.EXCEPTION_MESSAGE, result.ErrorMessage);
            Assert.AreEqual(BusinessLogicRepository.EXCEPTION_MESSAGE, result.Exception.Message);
        }

        [Test]
        public void ExceptionallyFailingMethodShallThrowExceptionWhenTryingToAccessValue()
        {
            var result = _testedInstance.SomeExceptionalReadOperation();
            Assert.Throws<InvalidOperationException>(() => Console.WriteLine(result.Value));
        }


        [Test]
        public void ProperSuccessfullHandling()
        {
            var result = _testedInstance.SomeSuccessfullReadOperation();
            if (result.Success)
            {
                Console.WriteLine(result.Value);
            }
        }


        [Test]
        public void ProperFailureHandling()
        {
            var result = _testedInstance.SomeFailingReadOperation();
            if (result.Failed)
            {
                Console.WriteLine(result.ErrorMessage);
            }
        }
    }
}
