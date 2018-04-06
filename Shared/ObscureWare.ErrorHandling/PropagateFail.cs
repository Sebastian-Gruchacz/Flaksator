namespace ObscureWare.ErrorHandling
{
    using System;

    public static class Propagate
    {
        //public static OperationResult<T> Fail<T>(OperationResult<object> innerFail)
        //{
        //    if (innerFail.Success)
        //    {
        //        throw new InvalidOperationException(@"Only failing OperationResult objects can be passed further.");
        //    }

        //    return innerFail._fail;
        //}

        public static Fail Fail(OperationResult<object> innerFail)
        {
            if (innerFail.Success)
            {
                throw new InvalidOperationException(@"Only failing OperationResult objects can be passed further.");
            }

            return innerFail.Fail; // will be converted into valid result later-on
        }
    }
}
