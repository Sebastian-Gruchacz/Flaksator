namespace ObscureWare.ErrorHandling
{
    using System;
    using System.Diagnostics;
    using System.Linq;

    public class TrackedFail : Fail
    {
        public TrackedFail(string errorMessage, ErrorCodes errorCode = ErrorCodes.GenericFailure) : base(errorMessage, errorCode)
        {
            this.GrabStackTrace();
        }

        public TrackedFail(ErrorCodes errorCode, string formatString, params object[] args) : base(errorCode, formatString, args)
        {
            if (formatString == null) throw new ArgumentNullException(nameof(formatString));

            this.GrabStackTrace();
        }

        private void GrabStackTrace()
        {
            StackTrace st = new StackTrace();
            this.StackTrace = string.Join(Environment.NewLine, st.GetFrames().Skip(2).Select(frame => frame.ToString())); // omit this method and ctor
        }
    }
}