namespace ObscureWare.ErrorHandling.Tests
{
    using System;

    class ExternalBusinessLogicRepository
    {
        private readonly BusinessLogicRepository _dependencyLogic;

        public ExternalBusinessLogicRepository(BusinessLogicRepository dependencyLogic)
        {
            this._dependencyLogic = dependencyLogic;
        }

        public OperationResult<Tuple<string, DateTime>> PassSuccessfullMergedOperation()
        {
            var innerResult = this._dependencyLogic.SomeSuccessfullReadOperation();
            if (innerResult.Success)
            {
                return new Tuple<string, DateTime>(innerResult.Value, DateTime.Now);
            }
            else
            {
                return Propagate.Fail(innerResult); // 1st method, proffered
            }
        }

        public OperationResult<Tuple<string, DateTime>> PassFailedMergedOperation()
        {
            var innerResult = this._dependencyLogic.SomeFailingReadOperation();
            if (innerResult.Success)
            {
                return new Tuple<string, DateTime>(innerResult.Value, DateTime.Now);
            }
            else
            {
                return (Fail)innerResult; // 2nd method, alternative. Dislike the cast.
            }
        }
    }
}