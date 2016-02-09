using System;

namespace ObscureWare.ErrorHandling
{
    public struct OperationResult<T>
    {
        private readonly T _value;
        private readonly Fail _fail;

        public OperationResult(T value)
        {
            _value = value;
            _fail = null;
        }

        private OperationResult(TrackedFail fail)
        {
            _value = default(T);
            _fail = fail;
        }

        private OperationResult(Fail fail)
        {
            _value = default(T);
            _fail = fail;
        }

        public string ErrorMessage
        {
            get
            {
                if (this.Success)
                {
                    throw new InvalidOperationException(@"Trying to read ErrorMessage from successfull OperationResult object.");
                }

                return _fail.ErrorMessage;  
            }
        }

        public string StackTrace
        {
            get
            {
                if (this.Success)
                {
                    throw new InvalidOperationException(@"Trying to read StackTrace from successfull OperationResult object.");
                }

                return _fail.StackTrace ?? @"# NO STACK TRACE STORED #";
            }
        }

        public Exception Exception
        {
            get
            {
                if (this.Success)
                {
                    throw new InvalidOperationException(@"Trying to read Exception from successfull OperationResult object.");
                }

                return _fail.Exception;
            }
        }

        public bool Failed
        {
            get { return _fail != null; }
        }

        public bool Success
        {
            get { return _fail == null; }
        }

        public ErrorCodes ErrorCode
        {
            get { return _fail?.ErrorCode ?? ErrorCodes.Success; }
        }

        public T Value
        {
            get
            {
                if (!this.Success)
                {
                    throw new InvalidOperationException("Trying to read value from failed OperationResult object");
                }

                return _value;
            }
        }

        public static implicit operator OperationResult<T>(T value)
        {
            return new OperationResult<T>(value);
        }

        public static implicit operator OperationResult<T>(Exception exception)
        {
            return new OperationResult<T>(new Fail(exception));
        }

        public static implicit operator OperationResult<T>(TrackedFail fail)
        {
            return new OperationResult<T>(fail);
        }

        public static implicit operator OperationResult<T>(Fail fail)
        {
            return new OperationResult<T>(fail);
        }
    }
}
