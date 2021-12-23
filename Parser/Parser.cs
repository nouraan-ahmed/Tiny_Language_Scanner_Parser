using System;
using System.Collections.Generic;
using System.Text;

namespace Tiny_Parser
{
    internal class Parser
    {
        Stack<Token> stack = new Stack<Token>();
        Tree tree = new Tree();
        Scanner scanner = new Scanner();
        string inputCode;
        Token g_token;
        Parser(string input)
        {
            inputCode= input;
        }
        Stack<Token> pushTokensToStack(List<Token>tokensList)
        {
            for(int i= tokensList.Count; i>0;i--)
            {
                stack.Push(tokensList[i]);
            }
            return stack;
        }
        public Stack<Token> getTokensStack (string inputCode)
        {
            Scanner s = new Scanner();
            List<Token> tokens = new List<Token>();
            s.getTokenList(inputCode,tokens);
            stack=pushTokensToStack(tokens);   
            return stack;
        }
        public void settree(Tree Tree)
        {
            tree = Tree;
        }
        public Tree gettree()
        {
            return tree;
        }
        public Boolean matchTokenByStack (Token expectedToken)
        {
            if (String.Equals(stack.Peek().Tokentype,expectedToken.Tokentype))
            {
                expectedToken.Tokenvalue = stack.Peek().Tokenvalue;
                stack.Pop();
                return true;
            }
            else
            {
                return false;
            }
        }
        /* expected token is an input of value null, if its type matches the type of the token so 
         * we fill its value with the value of the token (i.e. pass by address)*/
        public Boolean match (Token expectedToken)
        {
            Token currentToken=null;
            /* get the next token in current token which is passed by address to be filled by the scanner*/
            scanner.getToken(inputCode, currentToken);
            if (currentToken.Tokentype == expectedToken.Tokentype)
            {
                expectedToken.Tokenvalue=currentToken.Tokenvalue;
                return true;
            }
            else
            {
                return false;
            }
        }
        

    }
}
