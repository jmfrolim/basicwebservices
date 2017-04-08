using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace One_BasicConcepts
{
   public static class ExtensionMethods
    {
        public static string LeftOf(this String src, string s)
        {
            string ret = src;
            int idx = src.IndexOf(s);
            if (idx != -1)
            {
                ret = src.Substring(0,idx);
            }
            return ret;
        }

        public static string RightOf(this String src,string s)
        {
            string ret = String.Empty;
            int idx = src.IndexOf(s);

            if (idx != -1)
            {
                ret = src.Substring(idx + s.Length);
            }
            return ret;
        }
    }
}
