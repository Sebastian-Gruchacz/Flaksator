namespace ObscureWare.ErrorHandling
{
    using System;

    public struct OperationResult<T>
    {
        private readonly T _value;
        internal readonly Fail Fail;

        public OperationResult(T value)
        {
            this._value = value;
            this.Fail = null;
        }

        private OperationResult(TrackedFail fail)
        {
            this._value = default(T);
            this.Fail = fail;
        }

        private OperationResult(Fail fail)
        {
            this._value = default(T);
            this.Fail = fail;
        }

        public string ErrorMessage
        {
            get
            {
                if (this.Success)
                {
                    throw new InvalidOperationException(@"Trying to read ErrorMessage from successful OperationResult object.");
                }

                return this.Fail.ErrorMessage;  
            }
        }

        public string StackTrace
        {
            get
            {
                if (this.Success)
                {
                    throw new InvalidOperationException(@"Trying to read StackTrace from successful OperationResult object.");
                }

                return this.Fail.StackTrace ?? @"# NO STACK TRACE STORED #";
            }
        }

        public Exception Exception
        {
            get
            {
                if (this.Success)
                {
                    throw new InvalidOperationException(@"Trying to read Exception from successful OperationResult object.");
                }

                return this.Fail.Exception;
            }
        }

        public bool Failed => this.Fail != null;

        public bool Success => this.Fail == null;

        public ErrorCodes ErrorCode => this.Fail?.ErrorCode ?? ErrorCodes.Success;

        public T Value
        {
            get
            {
                if (!this.Success)
                {
                    throw new InvalidOperationException("Trying to read value from failed OperationResult object");
                }

                return this._value;
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

        // SMG: This does not work unfortunately... :anger:
        public static implicit operator Fail(OperationResult<T> value)
        {
            if (value.Success)
            {
                throw new InvalidOperationException(@"Only failing OperationResult objects can be passed further.");
            }

            return value.Fail;
        }
    }
}
