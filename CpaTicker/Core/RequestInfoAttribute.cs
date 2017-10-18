using System;

namespace CpaTicker.Core
{
    /// <summary>
    /// additional request documentation for api's at api help page
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    sealed public class RequestInfoAttribute : Attribute
    {
        readonly string _info;

        public string Info
        {
            get
            {
                return _info;
            }
        }

        public RequestInfoAttribute(string info)
        {
            _info = info;
        }
    }
}