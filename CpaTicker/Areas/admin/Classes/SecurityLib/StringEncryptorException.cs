using System;

namespace CpaTicker.Areas.admin.Classes.SecurityLib
{
    public class StringEncryptorException : Exception
    {
        public StringEncryptorException(string message)
            : base(message)
        {
        }
    }
}