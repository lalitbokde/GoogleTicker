using System;

namespace CpaTicker.Core
{
    /// <summary>
    /// additional documentation for model property's at api help page
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    sealed public class AdditionalInfoAttribute : Attribute
    {
        readonly string _info;

        public string Info
        {
            get
            {
                return _info;
            }
        }

        public AdditionalInfoAttribute(string info)
        {
            _info = info;
        }
    }
}