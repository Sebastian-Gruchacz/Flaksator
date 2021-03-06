﻿using System;

namespace ObscureWare.ErrorHandling
{
    public class Fail
    {
        public Fail(string errorMessage, ErrorCodes errorCode = ErrorCodes.GenericFailure)
        {
            this.ErrorCode = errorCode;
            this.ErrorMessage = errorMessage;
        }

        public Fail(ErrorCodes errorCode, string formatString, params object[] args)
        {
            if (formatString == null) throw new ArgumentNullException(nameof(formatString));

            this.ErrorCode = errorCode;
            this.ErrorMessage = string.Format(formatString, (object[])args);
        }

        public Fail(string message, Exception exception, ErrorCodes errorCode = ErrorCodes.GenericFailure) : this(message)
        {
            this.ErrorCode = errorCode;
            this.Exception = exception;
            this.StackTrace = exception.StackTrace;
        }

        public Fail(Exception exception, ErrorCodes errorCode = ErrorCodes.GenericFailure) : this(exception.Message)
        {
            this.ErrorCode = errorCode;
            this.Exception = exception;
            this.StackTrace = exception.StackTrace;
        }

        public string ErrorMessage { get; private set; }

        public string StackTrace { get; protected set; }

        public Exception Exception { get; private set; }

        public ErrorCodes ErrorCode { get; private set; }

        public static implicit operator Fail(Exception exception)
        {
            return new Fail(exception);
        }
    }
}
