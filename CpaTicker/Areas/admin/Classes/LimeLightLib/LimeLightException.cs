using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CpaTicker.Areas.admin.Classes.LimeLightLib
{
    public class LimeLightException : Exception
    {
        public LimeLightException()
            : base() { }

        public LimeLightException(string message)
            : base(message) { }

        public LimeLightException(string format, params object[] args)
            : base(string.Format(format, args)) { }

        public LimeLightException(string message, Exception innerException)
            : base(message, innerException) { }

        public LimeLightException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException) { }
    }
}