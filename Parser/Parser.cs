using System;
using System.Collections.Generic;
using System.Text;

namespace Tiny_Parser
{
    internal class Parser
    {
        Stack<Token> stack = new Stack<Token>();
        public Stack<Token> pushTokensToStack(List<Token>tokensList)
        {
            for(int i= tokensList.Count; i>0;i--)
            {
                stack.Push(tokensList[i]);
            }
            return stack;
        }
    }
}
