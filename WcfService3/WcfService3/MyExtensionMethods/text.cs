using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyExtensionMethods
{
    public static class text
    {
        public static string[] SplitWithDelimiter(string text, char[] delimiterChars )
        {

            string[] words = text.Split(delimiterChars);

            return words;
        }
    }
}
