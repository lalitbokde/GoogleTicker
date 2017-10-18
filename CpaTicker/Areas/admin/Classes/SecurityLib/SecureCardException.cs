using System;

namespace CpaTicker.Areas.admin.Classes.SecurityLib
{
    public class SecureCardException : Exception
    {
        public SecureCardException(string message)
            : base(message)
        {
        }
    }
}