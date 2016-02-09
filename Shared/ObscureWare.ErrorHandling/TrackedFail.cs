using System;
using System.Diagnostics;
using System.Linq;

namespace ObscureWare.ErrorHandling
{
    public class TrackedFail : Fail
    {
        public TrackedFail(string errorMessage, ErrorCodes errorCode = ErrorCodes.GenericFailure) : base(errorMessage, errorCode)
        {
            GrabStackTrace();
        }

        public TrackedFail(ErrorCodes errorCode, string formatString, params object[] args) : base(errorCode, formatString, args)
        {
            if (formatString == null) throw new ArgumentNullException(nameof(formatString));

            GrabStackTrace();
        }

        private void GrabStackTrace()
        {
            StackTrace st = new StackTrace();
            this.StackTrace = string.Join(Environment.NewLine, st.GetFrames().Skip(2).Select(frame => frame.ToString())); // ommit this method and ctor
        }
    }
}