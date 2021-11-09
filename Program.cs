using System;

namespace ConsoleApp
{
    class Program
    {
        /******************** Types Declarations******************/
       enum states { START, COMMENT, NUM, IDINTIFIER, ASSIGN, DONE, ERROR };

        /******************** Global Variables******************/
        states state = states.START;

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
        public void getToken (string input)
        {
            string token;
            while (state!=states.DONE)
            {
                int counter = 0;
                switch (state)
                {
                    case states.START:
                        if (isSpace(input[counter]))
                        {
                            /* if scanned character is white space then ignore it and go to scan the next character
                             as long as the input has not finished,
                            But if this white space is the last character in the input string then go state DONE*/
                            counter++;
                            if (counter == input.Length)
                                state = states.DONE;

                        }
                        else if (isDigit(input[counter]))
                        {
                            /*if scanned character is digit then go to state NUM*/
                            state = states.NUM;
                        }
                        else if (isLetter(input[counter]))
                        {
                            /*if scanned character is letter then go to state IDENTIFIER*/
                            state = states.IDINTIFIER;
                        }
                        else if (isSymbol(input[counter]))
                        {
                            /*if scanned character is symbol then print it as a string value of 
                             * a token of token type Special Symbols*/

                            // N7OTAHA LMA N&OT DA 3AL GUI

                            /*if this symbol is the last one in the input string then go to state DONE,
                             if not then go to scan the next character*/
                            counter++;
                            if (counter == input.Length)
                                state = states.DONE;
                            

                        }
                        /* increment the counter to scan the next character in the input string*/
                        else if (input[counter]==':')
                        {
                            counter++;
                            /*if the scanned character ':'  is the last character in the input string then go to state ERROR*/
                            if (counter == input.Length)
                                state = states.ERROR;

                            /*if the scanned character ':'  is not the last character then go to state ASSIGN*/
                            else 
                                state = states.ASSIGN;
                            
                        }
                        else if (input[counter]=='{')
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
                        if(input[counter]=='=')
                        {
                            state = states.DONE;
                        }
                        else
                        {
                            state = states.ERROR;
                        }
                        break;


                    
                }
            }
            

        }
        static void Main(string[] args)
        {
           // Console.WriteLine("Hello World!");
           
        


        }
    }
}
