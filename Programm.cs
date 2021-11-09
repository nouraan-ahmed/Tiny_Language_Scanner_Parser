using System;

namespace ConsoleApp
{
    class Program
    {
        /******************** Types Declarations******************/
        enum states { START, COMMENT, NUM, IDINTIFIER, ASSIGN, DONE };

        /******************** Global Variables ******************/

        states state = states.NUM;

        /********************Function Definitions****************/

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
        bool isSpace(char s) { return (s == ' ' || s == '\t' || s == '\n'); }

        /*This function represents the scanner, it scans the input string character by character, and classifies the 
         * tokens according to each token type and token value */
        public void getToken(string input)
        {
            string token = "";
            int i = 0;
            while (state != states.DONE)
            {
                switch (state)
                {
                    //IN CASE INPUT IS BETWEEN { }
                    //COMMENT CASE

                    case states.COMMENT:

                        if (state == states.COMMENT)
                        {
                            while (true)
                            {
                                i++;
                                if (input[i] == '}')
                                {
                                    break;
                                }
                            }
                            if (i + 1 == input.Length)
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

                        while (isDigit(input[i]))
                        {
                            token += input[i];
                            i++;
                        }

                        //label1.Text += mytoken += " , IS NUMBER \n";
                        token = "";
                        if (i == input.Length)
                        {
                            /* go to done state */
                            state = states.DONE;
                        }
                        else
                        {
                            /* return to start state */
                            state = states.START;
                        }
                        break;
                }
            }
        }
        static void Main(string[] args)
        {
            Program p = new Program();
            p.getToken(Console.ReadLine());
            // Console.WriteLine("Hello World!");


        }
    }
}