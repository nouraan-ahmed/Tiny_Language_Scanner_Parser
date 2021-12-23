using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tiny_Parser
{
    internal class Parser
    {
        Stack<Token> stack = new Stack<Token>();
        Tree tree = new Tree();
        Scanner scanner = new Scanner();
        string inputCode;
        Parser(string input)
        {
            inputCode= input;
        }
        Stack<Token> pushTokensToStack(List<Token>tokensList)
        {
            for (int i = tokensList.Count; i > 0; i--)
            {
                stack.Push(tokensList[i]);
            }
            return stack;
        }
        public Stack<Token> getTokensStack(string inputCode)
        {
            Scanner s = new Scanner();
            List<Token> tokens = new List<Token>();
            s.getTokenList(inputCode, tokens);
            stack = pushTokensToStack(tokens);
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
        public Boolean matchTokenByStack(Token expectedToken)
        {
            if (String.Equals(stack.Peek().Tokentype, expectedToken.Tokentype))
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
        public Boolean read_stmt(Node parent)
        {
            Token value = new Token(null, "READ");

            Boolean result_matchTokenByStack = true;
            result_matchTokenByStack = matchTokenByStack(value);
            if (!result_matchTokenByStack)
            {
                return false;
            }
            Node ifbody_read = new Node(value);
            Token value1 = new Token(null, "IDENTIFIER");
            result_matchTokenByStack = matchTokenByStack(value1);
            if (!result_matchTokenByStack)
            {
                return false;
            }
            Node readid = new Node(value1);
            tree.appendChild(parent, ifbody_read);
            tree.appendChild(ifbody_read, readid);
            return true;

        }
        public Boolean assign_stmt(Node parent)
        {
            Token value = new Token(null, "IDENTIFIER");
        /* expected token is an input of value null, if its type matches the type of the token so 
         * we fill its value with the value of the token */
        public Boolean match (Token expectedToken)
        {
            Token currentToken=null;
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
        

            Boolean result_matchTokenByStack = true;
            result_matchTokenByStack = matchTokenByStack(value);
            if (!result_matchTokenByStack)
            {
                return false;
            }
            Node identifier = new Node(value);
            Token value1 = new Token(null, "ASSIGN");
            result_matchTokenByStack = matchTokenByStack(value1);
            if (!result_matchTokenByStack)
            {
                return false;
            }
            Node assign = new Node(value1);
            tree.appendChild(parent, assign);
            tree.appendChild(assign, identifier);
            Boolean result = exp(assign);
            return result;

        }
        public Boolean write_stmt(Node parent)
        {
            Token value = new Token(null, "WRITE");

            Boolean result_matchTokenByStack = true;
            result_matchTokenByStack = matchTokenByStack(value);
            if (!result_matchTokenByStack)
            {
                return false;
            }
            Node write = new Node(value);

            tree.appendChild(parent, write);
            Boolean result = exp(write);
            return result;

        }
        public Boolean exp(Node parent)
        {
            Boolean result = simple_exp(parent);
            if (!result)
            {
                return false;
            }

            if (stack.Peek().Tokenvalue == "=" || stack.Peek().Tokenvalue == "<")
            {
                Token t = new Token();
                t.Tokenvalue = parent.getChildren().Last().getToken().Tokenvalue;
                t.Tokentype = parent.getChildren().Last().getToken().Tokentype;
                Node n = new Node();
                n.setToken(t);
                Node compare = new Node();
                result = comparison_op(compare);
                if (!result)
                {
                    return false;

                }

                result = simple_exp(compare);
                if (!result)
                {
                    return false;
                }
                parent.getChildren().RemoveAt(parent.getChildrenCount() - 1);
                tree.appendChild(parent, compare);
                tree.appendChild(compare, n);


            }
            return true;
        }
        public Boolean comparison_op(Node compare)
        {
            Token value = new Token(null, "EQUAL");
            Boolean result_matchTokenByStack = true;

            result_matchTokenByStack = matchTokenByStack(value);

            if (!result_matchTokenByStack)
            {
                Token value1 = new Token(null, "LESSTHAN");
                result_matchTokenByStack = matchTokenByStack(value1);
                if (!result_matchTokenByStack)
                {
                    return false;
                }
                compare.setToken(value1);
            }
            compare.setToken(value);
            return true;
        }
    }
}
