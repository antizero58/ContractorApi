using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractorApi
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string str)
        {
            return String.IsNullOrEmpty(str);
        }
    }
}
