using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.HTTP.Common
{
    public class CoreValidator
    {
        /*Holds two methods for checking for null or empty values*/
        public static void ThrowIfNull(object obj, string name)
        {
            if (obj == null) throw new ArgumentException(name);
        }

        public static void ThrowIfNullOrEmpty(string text, string name)
        {
            if (string.IsNullOrEmpty(text)) throw new ArgumentException(message: $"Cannot be null or empty.", name);
        }

        internal static void ThrowIfNullOrEmpty()
        {
            throw new NotImplementedException();
        }
    }   
}
