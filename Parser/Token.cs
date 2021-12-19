﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiny_Parser
{
    class Token
    {
        public string Tokenvalue { get; set; }
        public string Tokentype { get; set; }
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