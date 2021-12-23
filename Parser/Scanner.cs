using System;
using System.Collections.Generic;
using System.Text;

namespace Tiny_Parser
{
    //public class Token
    //{
    //    public string Tokenvalue;
    //    public string Tokentype;
    //}
    ////enum states { START, COMMENT, NUM, IDINTIFIER, ERROR, ASSIGN, DONE };
    public class Scanner
    {
        public Token token;
        public List<Token> tokens;

        /******************** Globel variables ****************/
        enum states { START, COMMENT, NUM, IDINTIFIER, ERROR, ASSIGN, DONE };
        states state = states.START;
        int j = 0;
        /******************** Function Definitions ****************/

        /* if the input is withen the range of digits (0 to 9) this function returns true,
         * and returns false if not */
        bool isDigit(char d) { return (d >= '0' && d <= '9'); }

        /* if the input is withen the range of alphapetic letters (lowercase or uppercase) this function returns true,
         * and returns false if not */
        bool isLetter(char l) { return (l >= 'a' && l <= 'z' || l >= 'A' && l <= 'Z'); }

        /* if the input is one of those symbols: '+' , '-' , '*' , '/','*' =, '<' , '(' , ')' ,';' this function returns true,
         * and returns false if not */
        bool isSymbol(char c)
        {
            return (c == '+' || c == '-' || c == '*' || c == '/' || c == '=' || c == '<' || c == '>' ||
                c == '(' || c == ')' || c == ';');
        }
        /* if the input is either space, tab, or new line, it is considered white space and this function returns true,
         * and returns false if not */
        bool isSpace(char s) { return (s == ' ' || s == '\t' || s == '\n' || s =='\r'); }
        
        /* function returns the next token in the string input */
        void getToken(string input, Token t)
        {
            state = states.START;

            // s Reserve;
            bool flag = false;
            string[] Reserve = new string[] { "if", "then", "else", "end", "repeat", "until", "read", "write" };
            string token = null;
            //string token1 = null;
            //Token t=new Token();

            while (state != states.DONE && state != states.ERROR)
            {
                switch (state)
                {
                    case states.START:
                        if (isSpace(input[j]))
                        {
                            /* if scanned character is white space then ignore it and go to scan the next character
                             as long as the input has not finished,
                            But if this white space is the last character in the input string then go state DONE*/
                            j++;
                            if (j == input.Length)
                            {
                                state = states.DONE;
                            }
                            else
                            {
                                state = states.START;
                            }

                        }
                        else if (isDigit(input[j]))
                        {
                            /*if scanned character is digit then go to state INNUM*/
                            state = states.NUM;
                        }
                        else if (isLetter(input[j]))
                        {
                            /*if scanned character is letter then go to state IDENTIFIER*/
                            state = states.IDINTIFIER;
                        }
                        else if (isSymbol(input[j]))
                        {
                            /*if scanned character is symbol then print it as a string value of 
                             * a token of token type Special Symbols*/

                            if (input[j] == '=')
                            {
                                if (input[j - 1] != ':')
                                {
                                    //token1 = ;
                                    t.Tokenvalue = input[j].ToString();
                                    t.Tokentype = "EQUAL";

                                }
                            }
                            else if (input[j] == ';')
                            {
                                //token1 = input[j] + " Symbol \n";
                                t.Tokenvalue = input[j].ToString();
                                t.Tokentype = "SEMICOLON";
                            }
                            else if (input[j] == '<')
                            {
                                //token1 = input[j] + " Symbol \n";
                                t.Tokenvalue = input[j].ToString();
                                t.Tokentype = "LESSTHAN";
                            }
                            else if (input[j] == '>')
                            {
                                //token1 = input[j] + " Symbol \n";
                                t.Tokenvalue = input[j].ToString();
                                t.Tokentype = "GREATERTHAN";
                            }
                            else if (input[j] == '+')
                            {
                                //token1 = input[j] + " Symbol \n";
                                t.Tokenvalue = input[j].ToString();
                                t.Tokentype = "PLUS";
                            }
                            else if (input[j] == '-')
                            {
                                //token1 = input[j] + " Symbol \n";
                                t.Tokenvalue = input[j].ToString();
                                t.Tokentype = "MINUS";
                            }
                            else if (input[j] == '*')
                            {
                                //token1 = input[j] + " Symbol \n";
                                t.Tokenvalue = input[j].ToString();
                                t.Tokentype = "MULT";
                            }
                            else if (input[j] == '/')
                            {
                                //token1 = input[j] + " Symbol \n";
                                t.Tokenvalue = input[j].ToString();
                                t.Tokentype = "DIV";
                            }
                            else if (input[j] == '(')
                            {
                                //token1 = input[j] + " Symbol \n";
                                t.Tokenvalue = input[j].ToString();
                                t.Tokentype = "OPENBRACKET";
                            }
                            else if (input[j] == ')')
                            {
                                //token1 = input[j] + " Symbol \n";
                                t.Tokenvalue = input[j].ToString();
                                t.Tokentype = "CLOSEDBRACKET";
                            }

                            /*if this symbol is the last one in the input string then go to state DONE,
                             if not then go to scan the next character*/
                            j++;
                            //if (j == input.Length)
                            state = states.DONE;


                        }
                        /* increment the j to scan the next character in the input string*/
                        else if (input[j] == ':')
                        {
                            j++;
                            /*if the scanned character ':'  is the last character in the input string then go to state ERROR*/
                            if (j == input.Length)
                                state = states.ERROR;

                            /*if the scanned character ':'  is not the last character then go to state ASSIGN*/
                            else
                                state = states.ASSIGN;

                        }
                        else if (input[j] == '{')
                        {
                            /*if scanned character is ':' then go to state COMMENT*/
                            state = states.COMMENT;
                        }
                        else
                        {
                            /*if the scanned character is [other] then go to state DONE */
                            state = states.DONE;
                        }
                        break;

                    case states.ASSIGN:
                        if (input[j] == '=')
                        {
                            state = states.DONE;
                            token = ":=";
                            t.Tokenvalue = token;
                            t.Tokentype = "ASSIGN";
                            //t.Tokentype = ", reserved word \n";
                        }
                        else
                        {
                            state = states.ERROR;
                        }
                        break;
                    case states.IDINTIFIER:

                        {

                            while ((j != input.Length) && (isLetter(input[j])))
                            {
                                token += input[j];
                                j++;
                                /*if (j == input.Length)
                                {
                                    state = states.DONE;
                                    break;
                                }*/
                            }

                            //else state = states.DONE;
                        }
                        for (int i = 0; i < 8; i++)
                        {
                            if (token == Reserve[i])
                            {
                                // token1 =
                                //  token + " , reserved word \n";
                                t.Tokenvalue = token;
                                switch (i)
                                {
                                    case 0:
                                        t.Tokentype = "IF";
                                        break;
                                    case 1:
                                        t.Tokentype = "THEN";
                                        break;
                                    case 2:
                                        t.Tokentype = "ELSE";
                                        break;
                                    case 3:
                                        t.Tokentype = "END";
                                        break;
                                    case 4:
                                        t.Tokentype = "REPEAT";
                                        break;
                                    case 5:
                                        t.Tokentype = "UNTIL";
                                        break;
                                    case 6:
                                        t.Tokentype = "READ";
                                        break;
                                    case 7:
                                        t.Tokentype = "WRITE";
                                        break;



                                }
                                //t.Tokentype=", "+ Reserve[i]+"\n";
                                flag = true;
                                break;
                            }

                        }
                        if (flag == false)
                        {
                            //token1 =
                            //token + " ,  identifier \n";
                            t.Tokenvalue = token;
                            t.Tokentype = "IDENTIFIER";
                        }

                        state = states.DONE;
                        //else state = states.START;

                        break;
                    //IN CASE INPUT IS BETWEEN { }
                    //COMMENT CASE

                    case states.COMMENT:

                        if (state == states.COMMENT)
                        {
                            while (true)
                            {
                                j++;
                                if (input[j] == '}')
                                {
                                    j++;
                                    break;
                                }
                            }
                            if (j == input.Length)
                            {
                                /* go to done state */
                                state = states.DONE;
                            }
                            else
                            {
                                /* return to start state */
                                state = states.START;
                            }

                        }
                        break;

                    //IN CASE INPUT IS DIGIT
                    //NUM CASE

                    case states.NUM:

                        while ((j != input.Length) && isDigit(input[j]))
                        {
                            token += input[j];
                            j++;
                            t.Tokenvalue = token;

                        }

                        //label1.Text += mytoken += " , IS NUMBER \n";
                        // token1 = token + ;
                        t.Tokentype = "NUMBER";
                        //token = "";

                        /* go to done state */
                        state = states.DONE;




                        break;
                    case states.DONE:
                        break;
                }
            }
            // return t;
        }
        //int counter = 0;
        //Token[] list = new Token;
        /* function returns all the list of tokens in a Tiny language code*/
        public void getTokenList(string after,List<Token> tokens)
        {
            while (j != after.Length)
            {
                Token t1 = new Token();
                getToken(after, t1);


                if ((t1.Tokenvalue != null) && (t1.Tokentype != null))
                {
                    //str += t1.Tokenvalue + "   " + t1.Tokentype;
                    tokens.Add(t1);
                    //list[counter] = t1;
                    //counter++;
                }
            }

        }
    }
}
