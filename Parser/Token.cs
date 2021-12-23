using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiny_Parser
{
    public class Token
    {
        public string Tokenvalue { get; set; }
        public string Tokentype { get; set; }
        public Boolean isElsePart = false;
        public Token(string tokenVal, string tokenType)
        {
            Tokenvalue = tokenVal;
            Tokentype = tokenType;
        }

        public Token()
        {
            Tokenvalue = null;
            Tokentype = null;
        }
    }
}
