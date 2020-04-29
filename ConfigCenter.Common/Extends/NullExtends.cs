using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigCenter.Common.Extends
{
    public static class NullExtends
    {
        public static bool IsNull(this object obj)
        {
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
