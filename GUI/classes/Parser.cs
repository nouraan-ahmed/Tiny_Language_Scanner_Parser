using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tiny_Parser
{
    class Parser
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
                expectedToken.isElsePart = g_token.isElsePart;
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

            while (g_token.Tokentype == "SEMICOLON")
            {
                result_match = match(value);

                if (!result_match)
                {
                    return false;
                }
                if (result_match)
                {
                    result_match = statement(parent);
                }
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
            Boolean first_stmt_else = true;
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

            if (g_token.Tokentype == "ELSE") {

                match(value2);
                //for GUI
                g_token.isElsePart = first_stmt_else;
                first_stmt_else = false;
                result_match = stmt_sequence(if_v);
                g_token.isElsePart = first_stmt_else;
                if (!result_match)
                {
                    return false;
                }
            }

            Token value3 = new Token(null, "END");
            if (g_token.Tokentype == "END")
            {
                result_match = match(value3);

                if (!result_match)
                {
                    return false;
                }
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
            tree.appendChild(parent, repeat);
            Boolean result = exp(repeat);
            return result;

        }
        public Boolean read_stmt(Node parent)
        {
            Token value = new Token(null, "READ");

            Boolean result_match = match(value);
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

            Boolean result_match = match(value);
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
            Boolean result_match = match(value);
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

            Node temp = new Node();

            Boolean isFactor = factor(parent);
            if (!isFactor)
            {
                return false;
            }

            /* now g_temp is the mulop if it exists */

            Token MulOpToken = new Token();
            Node Mulop_Node = new Node(MulOpToken);


            while ((g_token.Tokentype == "MULT") || (g_token.Tokentype == "DIV"))
            {
                Token new_g_token = new Token();
                Node newtemp = new Node();
                /* Make new temp as the new head MulOp*/
                new_g_token.Tokentype = g_token.Tokentype;
                new_g_token.Tokenvalue = g_token.Tokenvalue;

                MulOpToken.Tokentype = g_token.Tokentype;
                // assign the last child of the tree to temp node 
                temp = parent.getChildren().Last();
                /* delete the first factor node form being the child of the root parent*/
                parent.getChildren().RemoveAt(parent.getChildrenCount() - 1);
                // assign the mulop node to newtemp node
                newtemp.setToken(new_g_token);
                tree.appendChild(parent, newtemp);

                // consume the mulop token
                if (mulop(Mulop_Node))
                {
                    tree.appendChild(newtemp, temp);
                    isFactor = factor(newtemp);
                    if (!isFactor)
                    {
                        return false;
                    }
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
                if (!match(op))
                    return false;

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
                
                if (!match(op))
                    return false;

                parent.setToken(op);
                return true;
            }
            else
                return false;
        }

        // simple-exp → simple-exp  addop  term | term
        public Boolean simple_exp(Node parent)
        {

            Node temp = new Node();

            Boolean isTerm = term(parent);

            if (!isTerm)
            {
                return false;
            }

            /* now g_temp is the addop if it exists */
            Token AddOpToken = new Token();
            Node Addop_Node = new Node(AddOpToken);


            while ((g_token.Tokentype == "PLUS") || (g_token.Tokentype == "MINUS"))
            {
                Token new_g_token = new Token();
                Node newtemp = new Node();
                /* Make new temp as the new head AddOp*/
                //newtemp.setToken(g_token);
                new_g_token.Tokentype = g_token.Tokentype;
                new_g_token.Tokenvalue = g_token.Tokenvalue;

                AddOpToken.Tokentype = g_token.Tokentype;
                // assign the last child of the tree to temp node
                temp = parent.getChildren().Last();
                /* delete the last addop node form being the child of the root parent*/
                parent.getChildren().RemoveAt(parent.getChildrenCount() - 1);
                // assign the new addop node to newtemp node 
                newtemp.setToken(new_g_token);
                tree.appendChild(parent, newtemp);

                // consume the AddOp token
                if (addop(Addop_Node))
                {
                    tree.appendChild(newtemp, temp);
                    isTerm = term(newtemp);
                    if (!isTerm)
                    {
                        return false;
                    }
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
                        Node fact_node1 = new Node(t1);
                        tree.appendChild(parent, fact_node1);
                        return true;
                    }
                    break;

                case "IDENTIFIER":
                    Token t2 = new Token(null, "IDENTIFIER");
                    if (match(t2))
                    {
                        Node fact_node2 = new Node(t2);
                        tree.appendChild(parent, fact_node2);
                        return true;
                    }
                    break;
            }
            return false;
        }

    }
}
