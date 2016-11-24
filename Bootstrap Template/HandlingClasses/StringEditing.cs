using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Musoftware
{
    public class StringEditing
    {
        public static string StringToJs(string e)
        {
            e = e.Replace("\"", "\\\"");
            e = e.Replace("\n", "\\n");
            e = e.Replace("\t", "\\t");
            e = e.Replace("\r", "\\r");
            e = e.Replace("\0", "\\0");

            return e;
        }
    }
}
