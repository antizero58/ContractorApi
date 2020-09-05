using System;
using System.Linq;

namespace ContractorApi
{
    public static class Extensions
    {
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool InList<T>(this T source, params T[] list)
        {
            return list.Contains(source);
        }
    }
}
