using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractorApi
{
    public static class Extensions
    {
        public static bool IsNullOrEmpty(this string str)
        {
            return String.IsNullOrEmpty(str);
        }

        public static bool InList<T>(this T source, params T[] list)
        {
            return list.Contains(source);
        }
    }
}
