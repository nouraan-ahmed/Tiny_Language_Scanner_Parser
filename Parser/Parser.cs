using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tiny_Parser
{
    public class Parser
    {
        Stack<Token> stack = new Stack<Token>();
        Tree tree = new Tree();
        Scanner scanner = new Scanner();
        string inputCode;
        Token g_token = new Token();
        public Parser(string input, Tree tree)
        {
            inputCode = input;
            this.tree = tree;
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
        * we fill its value with the value of the token (i.e. pass by address)*/
        public Boolean match(Token expectedToken)
        {
            if (g_token.Tokentype == expectedToken.Tokentype)
            {
                expectedToken.Tokenvalue = g_token.Tokenvalue;
                scanner.getToken(inputCode, g_token);
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean program(Node parent)
        {
            scanner.getToken(inputCode, g_token);
            return stmt_sequence(parent);
        }

        public Boolean stmt_sequence(Node parent)
        {
            Boolean result_match = statement(parent);
            if (!result_match)
            {
                return false;
            }
            Token value = new Token(null, "SEMICOLON");

            Boolean result = match(value);
            if (result)
            {
                result = statement(parent);
                return result;
            }

            return result_match;

        }

        public Boolean statement(Node parent)
        {

            switch (g_token.Tokentype)
            {
                case "IF":
                    return if_stmt(parent);

                case "READ":
                    return read_stmt(parent);

                case "IDENTIFIER":
                    return assign_stmt(parent);

                case "REPEAT":
                    return repeat_stmt(parent);

                case "WRITE":
                    return write_stmt(parent);
            }
            return false;
        }

        public Boolean if_stmt(Node parent)
        {
            Token value = new Token(null, "IF");
            Boolean result_match = true;
            result_match = match(value);

            if (!result_match)
            {
                return false;
            }

            Node if_v = new Node(value);
            result_match = exp(if_v);

            if (!result_match)
            {
                return false;
            }
            tree.appendChild(parent, if_v);
            Token value1 = new Token(null, "THEN");
            result_match = match(value1);

            if (!result_match)
            {
                return false;
            }

            result_match = stmt_sequence(if_v);

            if (!result_match)
            {
                return false;
            }

            Token value2 = new Token(null, "ELSE");

            result_match = match(value2);
            //for GUI
            value2.isElsePart = result_match;

            if (result_match)
            {
                Node else_v = new Node(value2);
                result_match = stmt_sequence(else_v);
                tree.appendChild(if_v, else_v);
            }

            Token value3 = new Token(null, "END");
            result_match = match(value3);

            if (!result_match)
            {
                return false;
            }
            return true;
        }

        public Boolean repeat_stmt(Node parent)
        {
            Token value = new Token(null, "REPEAT");

            Boolean result_match = true;
            result_match = match(value);
            if (!result_match)
            {
                return false;
            }
            Node repeat = new Node(value);
            result_match = stmt_sequence(repeat);
            if (!result_match)
            {
                return false;
            }
            Token value1 = new Token(null, "UNTIL");

            result_match = match(value1);
            if (!result_match)
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

            Boolean result_match = true;
            result_match = match(value);
            if (!result_match)
            {
                return false;
            }
            Node ifbody_read = new Node(value);
            Token value1 = new Token(null, "IDENTIFIER");
            result_match = match(value1);
            if (!result_match)
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

            Boolean result_match = true;
            result_match = match(value);
            if (!result_match)
            {
                return false;
            }
            Node identifier = new Node(value);
            Token value1 = new Token(null, "ASSIGN");
            result_match = match(value1);
            if (!result_match)
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

            Boolean result_match = true;
            result_match = match(value);
            if (!result_match)
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

            if (g_token.Tokenvalue == "=" || g_token.Tokenvalue == "<")
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
                parent.getChildren().RemoveAt(parent.getChildrenCount() - 1);
                tree.appendChild(parent, compare);
                tree.appendChild(compare, n);
                result = simple_exp(compare);
                if (!result)
                {
                    return false;
                }
            }
            return true;
        }
        public Boolean comparison_op(Node compare)
        {
            Token value = new Token(null, "EQUAL");
            Boolean result_match = true;

            result_match = match(value);

            if (!result_match)
            {
                Token value1 = new Token(null, "LESSTHAN");
                result_match = match(value1);
                if (!result_match)
                {
                    return false;
                }
                compare.setToken(value1);
                return true;
            }
            compare.setToken(value);
            return true;
        }
        //term → factor {mulop factor}
        public Boolean term(Node parent)
        {
            Boolean firstMulOp = true;
            Node temp = new Node();
            Node newtemp = new Node();

            /* first temp is the left child */
            temp.setToken(g_token);

            Boolean isFactor = factor(parent);
            if (!isFactor)
            {
                return false;
            }

            /* now g_temp is the mulop if it exists */

            Token MulOpToken = new Token();

            while ((g_token.Tokentype == "MULT") || (g_token.Tokentype == "DIV"))
            {
                MulOpToken.Tokentype = g_token.Tokentype;
                if (firstMulOp)
                {
                    Token lastTreeChildToken = new Token();
                    lastTreeChildToken.Tokenvalue = parent.getChildren().Last().getToken().Tokenvalue;
                    lastTreeChildToken.Tokentype = parent.getChildren().Last().getToken().Tokentype;
                    /* Put the first factor in this node*/
                    Node lastTreeChildNode = new Node(lastTreeChildToken);
                    /* delete the first factor node form being the child of the root parent*/
                    parent.getChildren().RemoveAt(parent.getChildrenCount() - 1);
                    firstMulOp = false;
                }
                /* Make new temp as the new head MulOp*/
                newtemp.setToken(g_token);
                //match the expected token which is MulOp token to the g_token
                if (mulop(MulOpToken))
                {
                    tree.appendChild(newtemp, temp);
                    factor(newtemp);
                }
                else
                    return false;
            }
            return true;
        }
        public Boolean addop(Node parent)
        {
            if ((g_token.Tokentype == "PLUS") || (g_token.Tokentype == "MINUS"))
            {
                Token op = new Token();
                op.Tokenvalue = null;
                op.Tokentype = g_token.Tokentype;
                match(op);

                Node op_node = new Node(op);
                parent = op_node;

                return true;
            }
            else
                return false;
        }

        public Boolean mulop(Node parent)
        {
            if ((g_token.Tokentype == "MULT") || (g_token.Tokentype == "DIV"))
            {
                Token op = new Token();
                op.Tokenvalue = g_token.Tokenvalue;
                op.Tokentype = g_token.Tokentype;
                match(op);
                parent.setToken(op);
                return true;
            }
            else
                return false;
        }
        //term {addop term}
        // simple-exp → simple-exp  addop  term | term
        public Boolean simple_exp(Node parent)
        {
            Boolean firstAddOp = true;
            Node temp = new Node();
            Node newtemp = new Node();

            /* first temp is the left child */
            temp.setToken(g_token);

            Boolean isTerm = term(parent);
            if (!isTerm)
            {
                return false;
            }

            /* now g_temp is the addop if it exists */

            Token AddOpToken = new Token();

            while ((g_token.Tokentype == "PLUS") || (g_token.Tokentype == "MINUS"))
            {
                AddOpToken.Tokentype = g_token.Tokentype;
                if (firstAddOp)
                {
                    Token lastTreeChildToken = new Token();
                    lastTreeChildToken.Tokenvalue = parent.getChildren().Last().getToken().Tokenvalue;
                    lastTreeChildToken.Tokentype = parent.getChildren().Last().getToken().Tokentype;
                    /* Put the first factor in this node*/
                    Node lastTreeChildNode = new Node(lastTreeChildToken);
                    /* delete the first factor node form being the child of the root parent*/
                    parent.getChildren().RemoveAt(parent.getChildrenCount() - 1);
                    firstAddOp = false;
                }
                /* Make new temp as the new head AddOp*/
                newtemp.setToken(g_token);
                //match the expected token which is AddOp token to the g_token
                if (addop(AddOpToken))
                {
                    tree.appendChild(newtemp, temp);
                    term(newtemp);
                }
                else
                    return false;
            }
            return true;

        }

        public Boolean factor(Node parent)
        {
            Token fact = new Token();
            fact.Tokenvalue = g_token.Tokenvalue;
            fact.Tokentype = g_token.Tokentype;

            switch (g_token.Tokentype)
            {
                case "OPENBRACKET":
                    Token t = new Token("(", "OPENBRACKET");
                    if (match(t))
                    {
                        if (exp(parent))
                        {
                            t.Tokenvalue = ")";
                            t.Tokentype = "CLOSEDBRACKET";
                            if (match(t))
                            {
                                return true;
                            }
                        }
                    }
                    break;

                case "NUMBER":
                    Token t1 = new Token(null, "NUMBER");
                    if (match(t1))
                    {
                        Node fact_node1 = new Node(fact);
                        tree.appendChild(parent, fact_node1);
                        return true;
                    }
                    break;

                case "IDENTIFIER":
                    Token t2 = new Token(null, "IDENTIFIER");
                    if (match(t2))
                    {
                        Node fact_node2 = new Node(fact);
                        tree.appendChild(parent, fact_node2);
                        return true;
                    }
                    break;
            }
            return false;
        }

    }
}
