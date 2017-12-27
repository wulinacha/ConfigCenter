using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigCenter.Common.Extensions
{
    public static class ObjectExtensions
    {
        public static bool IsNull(this object obj) {
            if (obj == null)
                return true;
            else
                return false;
        }

        public static bool IsNullOrSpeac(this object obj)
        {
            if (obj == null || obj.ToString() == "")
                return true;
            else
                return false;
        }
    }
}
