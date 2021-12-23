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
        Token g_token;
        Parser(string input)
        {
            inputCode = input;
        }
        Stack<Token> pushTokensToStack(List<Token> tokensList)
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
        /* expected token is an input of value null, if its type matches the type of the token so 
        * we fill its value with the value of the token */
        public Boolean match(Token expectedToken)
        {
            Token currentToken = null;
            scanner.getToken(inputCode, currentToken);
            if (currentToken.Tokentype == expectedToken.Tokentype)
            {
                expectedToken.Tokenvalue = currentToken.Tokenvalue;
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean program(Node parent)
        {
            return stmt_sequence(parent);
        }

        public Boolean stmt_sequence(Node parent)
        {
            Boolean result_matchTokenByStack = statement(parent);
            if (!result_matchTokenByStack)
            {
                return false;
            }
            Token value = new Token(null, "SEMICOLON");

            Boolean result = matchTokenByStack(value);
            if (result)
            {
                result = statement(parent);
                return result;
            }

            return result_matchTokenByStack;

        }

        public Boolean statement(Node parent)
        {
            Token value_if = new Token(null, "IF");
            Token value_read = new Token(null, "READ");
            Token value_assign = new Token(null, "ASSIGN");
            Token value_repeat = new Token(null, "REPEAT");
            Token value_write = new Token(null, "WRITE");

            Boolean result_matchTokenByStack = matchTokenByStack(value_if);
            if (result_matchTokenByStack)
            {
                if_stmt(parent);
            }
            result_matchTokenByStack = matchTokenByStack(value_read);
            if (result_matchTokenByStack)
            {
                read_stmt(parent);
            }
            result_matchTokenByStack = matchTokenByStack(value_assign);
            if (result_matchTokenByStack)
            {
                assign_stmt(parent);
            }
            result_matchTokenByStack = matchTokenByStack(value_repeat);
            if (result_matchTokenByStack)
            {
                repeat_stmt(parent);
            }
            result_matchTokenByStack = matchTokenByStack(value_write);
            if (result_matchTokenByStack)
            {
                write_stmt(parent);
            }
            return false;
        }

        public Boolean if_stmt(Node parent)
        {
            Token value = new Token(null, "IF");
            Boolean result_matchTokenByStack = true;
            result_matchTokenByStack = matchTokenByStack(value);

            if (!result_matchTokenByStack)
            {
                return false;
            }

            Node if_v = new Node(value);
            result_matchTokenByStack = exp(if_v);

            if (!result_matchTokenByStack)
            {
                return false;
            }

            Token value1 = new Token(null, "THEN");
            result_matchTokenByStack = matchTokenByStack(value1);

            if (!result_matchTokenByStack)
            {
                return false;
            }

            result_matchTokenByStack = stmt_sequence(if_v);

            if (!result_matchTokenByStack)
            {
                return false;
            }

            Token value2 = new Token(null, "ELSE");
            result_matchTokenByStack = matchTokenByStack(value2);

            if (result_matchTokenByStack)
            {
                result_matchTokenByStack = stmt_sequence(if_v);
            }

            Token value3 = new Token(null, "END");
            result_matchTokenByStack = matchTokenByStack(value3);

            if (!result_matchTokenByStack)
            {
                return false;
            }
            tree.appendChild(parent, if_v);
            return true;
        }

        public Boolean repeat_stmt(Node parent)
        {
            Token value = new Token(null, "REPEAT");

            Boolean result_matchTokenByStack = true;
            result_matchTokenByStack = matchTokenByStack(value);
            if (!result_matchTokenByStack)
            {
                return false;
            }
            Node repeat = new Node(value);
            result_matchTokenByStack = stmt_sequence(repeat);
            if (!result_matchTokenByStack)
            {
                return false;
            }
            Token value1 = new Token(null, "UNTIL");

            result_matchTokenByStack = matchTokenByStack(value1);
            if (!result_matchTokenByStack)
            {
                return false;
            }
            Node until = new Node(value1);
            tree.appendChild(parent, repeat);
            tree.appendChild(repeat, until);
            Boolean result = exp(until);
            return result;

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
                return true;
            }
            compare.setToken(value);
            return true;
        }

    }
}
