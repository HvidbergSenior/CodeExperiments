using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insight.BuildingBlocks.Infrastructure
{
    public static class HashHelpers
    {
        public static string GetHashCode(params object[] values)
        {
            return string.Join(':', values);
        }

        public static int GetNullSafeHashCode<T>(this T value) where T : class
        {
            return value == null ? 1 : value.GetHashCode();
        }
    }
}
