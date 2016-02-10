using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObscureWare.ErrorHandling
{
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

            return innerFail._fail; // will be converted into valid result later-on
        }
    }
}
