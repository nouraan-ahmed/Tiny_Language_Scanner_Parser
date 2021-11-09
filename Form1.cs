﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        enum states { START, COMMENT, NUM, IDINTIFIER, ERROR,ASSIGN, DONE };

        /********************Globel variable ****************/
        states state = states.START;
        int j = 0;
        string after;
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
       
        public Form1()
        {
            InitializeComponent();
        }
        OpenFileDialog ofd = new OpenFileDialog();
        private void button1_Click(object sender, EventArgs e)
        {
           
            
            ofd.ShowDialog();
            string x = ofd.FileName;
            string before=null;
            int n = 0;
             string lines;
            string[] lines1 = System.IO.File.ReadAllLines(@x);
            
            for (int i = 0; i < lines1.Length; i++)
            {
                before += lines1[i] + Environment.NewLine;
                after += lines1[i];
            }
            richTextBox1.Text = before;
            /* StreamReader file = new StreamReader(@x);
            string text;

             while ((lines = file.ReadLine()) != null)
             {
                 text += lines;
                 n++; 
             }*/
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
           

        }
        public string getToken(string input )
        {
            state = states.START;
            // s Reserve;
            bool flag = false;
            string[] Reserve = new string[] { "if", "then", "else", "end", "repeat", "until", "read", "write" };
            string token = null;
            string token1 = null;
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
                            /*if scanned character is digit then go to state NUM*/
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
                                    token1 = input[j] + " , Symbol \n";
                                }
                            }
                            else
                            {
                                token1 = input[j] + " , Symbol \n";
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
                            token1 = token + ",Assign\n";
                        }
                        else
                        {
                            state = states.ERROR;
                        }
                        break;
                    case states.IDINTIFIER:

                        {

                            while ( (j != input.Length) && (isLetter(input[j])))
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
                                token1 =
                                   token + " , reserved word \n";
                                flag = true;
                                break;
                            }
                            
                        }
                        if(flag==false)
                        {
                            token1 =
                            token + " ,  identifier \n";
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
                            if (j  == input.Length)
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
                        }

                        //label1.Text += mytoken += " , IS NUMBER \n";
                        token1 = token + " , NUMBER \n";
                        //token = "";
                        
                            /* go to done state */
                            state = states.DONE;
                       
                        
                           
                        
                        break;
                    case states.DONE:
                        break;
                }
            }
            return token1;
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //state = states.IDINTIFIER;
            //string x = "{ Sample program in TINY language – computes factorial } read x;{ input an integer }if  0 < x   then     { don’t compute if x <= 0 }fact:= 1; repeat fact  := fact * x  x:= x - 1 until x = 0; write fact { output factorial of x }end";
            string str=null;
            
            while ( j!=after.Length) 
            {
                str+=getToken(after) ; 
                
            }
            richTextBox2.Text = str;

        }
    }
}
