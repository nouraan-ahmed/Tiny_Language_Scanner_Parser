using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Tiny_Parser
{
    public partial class MainWindow : Window
    {
        int page_number = 0;
        string sourceDirectory = null;
        string inputFileName = null;
        Boolean isDrawn = false;

        public MainWindow()
        {
            InitializeComponent();
            //hide buttons
            Hide_Page1();
            Hide_Page2();

            //Token t1 = new Token();
            //t1.Tokentype = "bb";
            //t1.Tokenvalue = "cc";

            //Token t2 = new Token();
            //t2 = t1;
            //t2.Tokentype = "aa";
            //List<Token> tt = new List<Token>();
            //tt.Add(t1);
            //tt.Add(t2);

            //tt.Remove(t2);
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            //success_msg.Visibility = Visibility.Hidden;
            //error_msg.Visibility = Visibility.Hidden;
            //num_errors.Visibility = Visibility.Hidden;
            OpenFileDialog input_file = new OpenFileDialog();

            input_file.Filter = "txt files (*.txt)|*.txt";
            input_file.FilterIndex = 2;
            input_file.RestoreDirectory = true;

            if (input_file.ShowDialog() == true)
            {
                syntax_tree.Visibility = Visibility.Hidden;

                //remove any text writen before in output textbox when browsing new file
                output_text.Text = null;
                input_text.Text = null;

                //Get the path of specified file
                var filePath = input_file.FileName;
                inputFileName = System.IO.Path.GetFileNameWithoutExtension(filePath);
                
                //check if the chosen file is empty
                long fileLen = new FileInfo(filePath).Length;
                if (fileLen == 0 || (fileLen == 3 && File.ReadAllBytes("file").SequenceEqual(new byte[] { 239, 187, 191 })))
                {
                    /* Is empty */
                    MessageBox.Show("An empty file can not be scanned.\nPlease, choose a non empty file", "Browse Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                else
                {
                    isDrawn = false;
                    //get the directory name the file is in
                    sourceDirectory = System.IO.Path.GetDirectoryName(filePath);

                    //Read the contents of the file into a stream
                    var fileStream = input_file.OpenFile();
                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        var fileContent = reader.ReadToEnd();
                        input_text.Text = fileContent;

                        //read from the begginig of the file
                        reader.DiscardBufferedData();
                        reader.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);
                        reader.Close();
                    }
                    scan.Visibility = Visibility.Visible;
                }
                //syntax_tree.Visibility = Visibility.Hidden;
            }
        }

        private void Scaner_Click(object sender, RoutedEventArgs e)
        {
            syntax_tree.Visibility = Visibility.Visible;
            List<Token> tokens = new List<Token>();
            Scanner s = new Scanner();
            s.getTokenList(input_text.Text, tokens);

            //remove any text writen before in output textbox
            string destFileName = System.IO.Path.Combine(sourceDirectory, inputFileName + "_Scanner_Output.txt");
            output_text.Text = null;
            StreamWriter sw = new StreamWriter(destFileName, false);
            for(int i = 0; i < tokens.Count(); i++)
            {
                sw.WriteLine(tokens[i].Tokenvalue + ", " + tokens[i].Tokentype);
            }
            sw.Close();

            using (StreamReader reader = new StreamReader(destFileName))
            {
                var fileContent = reader.ReadToEnd();
                output_text.Text = fileContent;
                reader.Close();
            }

            scan.Visibility = Visibility.Hidden;
        }

        private void Syntax_Tree_Click(object sender, RoutedEventArgs e)
        {
            syntax_tree_canvas.Children.Clear();
            isDrawn = true;
            page_number = 2;
            Hide_Page1();
            Show_Page2();

            Tree tree = new Tree();
            /*Token token1 = new Token("read", "READ");
            Token token2 = new Token("X", "IDENTIFIER");
            Node root = tree.getTreeRoot();
            Node n1 = new Node(token1);
            Node n2 = new Node(token2);
            tree.appendChild(root, n1);
            tree.appendChild(n1, n2);

            Token token3 = new Token("if", "IF");
            Token token4 = new Token("<", "LESSTHAN");
            Token token5 = new Token("0", "NUMBER");
            Token token6 = new Token("X", "IDENTIFIER");
            Node n3 = new Node(token3);
            Node n4 = new Node(token4);
            Node n5 = new Node(token5);
            Node n6 = new Node(token6);
            tree.appendChild(root, n3);
            tree.appendChild(n3, n4);
            tree.appendChild(n4, n5);
            tree.appendChild(n4, n6);

            Token token7 = new Token("assign", "ASSIGN");
            Token token8 = new Token("fact", "IDENTIFIER");
            Token token9 = new Token("1", "NUMBER");
            Node n7 = new Node(token7);
            Node n8 = new Node(token8);
            Node n9 = new Node(token9);
            tree.appendChild(n3, n7);
            tree.appendChild(n7, n8);
            tree.appendChild(n7, n9);

            Token token10 = new Token("repeat", "REPEAT");

            Token token11 = new Token("assign", "ASSIGN");
            Token token12 = new Token("fact", "IDENTIFIER");
            Token token13 = new Token("*", "MULT");
            Token token14 = new Token("fact", "IDENTIFIER");
            Token token15 = new Token("x", "IDENTIFIER");

            Token token16 = new Token("assign", "ASSIGN");
            Token token17 = new Token("x", "IDENTIFIER");
            Token token18 = new Token("-", "MINUS");
            Token token19 = new Token("x", "IDENTIFIER");
            Token token20 = new Token("1", "NUMBER");

            Token token21 = new Token("=", "EQUAL");
            Token token22 = new Token("x", "IDENTIFIER");
            Token token23 = new Token("0", "NUMBER");

            Node n10 = new Node(token10);
            Node n11 = new Node(token11);
            Node n12 = new Node(token12);
            Node n13 = new Node(token13);
            Node n14 = new Node(token14);
            Node n15 = new Node(token15);
            Node n16 = new Node(token16);
            Node n17 = new Node(token17);
            Node n18 = new Node(token18);
            Node n19 = new Node(token19);
            Node n20 = new Node(token20);
            Node n21 = new Node(token21);
            Node n22 = new Node(token22);
            Node n23 = new Node(token23);
            tree.appendChild(n3, n10);
            tree.appendChild(n10, n11);
            tree.appendChild(n10, n16);
            tree.appendChild(n10, n21);

            tree.appendChild(n11, n12);
            tree.appendChild(n11, n13);
            tree.appendChild(n13, n14);
            tree.appendChild(n13, n15);
            tree.appendChild(n16, n17);
            tree.appendChild(n16, n18);
            tree.appendChild(n18, n19);
            tree.appendChild(n18, n20);

            tree.appendChild(n21, n22);
            tree.appendChild(n21, n23);

            Token token24 = new Token("write", "WRITE");
            Token token25 = new Token("fact", "IDENTIFIER");
            Node n24 = new Node(token24);
            Node n25 = new Node(token25);
            tree.appendChild(n3, n24);
            tree.appendChild(n24, n25);*/
            Parser parse = new Parser(input_text.Text, tree);
            Boolean result = parse.program(tree.getTreeRoot());
            if(result == true)
            {
                DrawSyntaxTree draw = new DrawSyntaxTree(syntax_tree_canvas);
                tree.traverseTree(draw.Draw_Child_Node);
            }
            else
            {
                Hide_Page2();
                Show_Page1();
                MessageBox.Show("There is an error in your code, please revise your code according to the rules of Tiny Language", "Parsing Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        // GUI Styling Functions // 
        private void Parser_Click(object sender, RoutedEventArgs e)
        {
            page_number = 1;
            //hide the items of main page
            Hide_Main_Page();

            //show items of input page
            Show_Page1();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (page_number == 1)
            {
                page_number--;
                //go back to the main page
                Hide_Page1();
                Show_Main_Page();
            }

            else if (page_number == 2)
            {
                page_number--;
                Hide_Page2();
                Show_Page1();
            }
        }

        // Multi-page manipulation functions
        private void Show_Main_Page()
        {
            logo.Visibility = Visibility.Visible;
            header.Visibility = Visibility.Visible;
            parser.Visibility = Visibility.Visible;
        }

        private void Hide_Main_Page()
        {
            logo.Visibility = Visibility.Hidden;
            header.Visibility = Visibility.Hidden;
            parser.Visibility = Visibility.Hidden;
        }

        private void Show_Page1()
        {
            back.Visibility = Visibility.Visible;
            //syntax_tree.Visibility = Visibility.Visible;
            if (isDrawn)
            {
                syntax_tree.Visibility = Visibility.Visible;
            }

            header2.Visibility = Visibility.Visible;
            browse.Visibility = Visibility.Visible;
            input.Visibility = Visibility.Visible;
            output.Visibility = Visibility.Visible;
            input_text.Visibility = Visibility.Visible;
            output_text.Visibility = Visibility.Visible;
        }

        private void Hide_Page1()
        {
            scan.Visibility = Visibility.Hidden;

            back.Visibility = Visibility.Hidden;
            syntax_tree.Visibility = Visibility.Hidden;

            header2.Visibility = Visibility.Hidden;
            browse.Visibility = Visibility.Hidden;
            input.Visibility = Visibility.Hidden;
            output.Visibility = Visibility.Hidden;
            input_text.Visibility = Visibility.Hidden;
            output_text.Visibility = Visibility.Hidden;
        }

        private void Show_Page2()
        {
            syntax_tree.Visibility = Visibility.Hidden;
            back.Visibility = Visibility.Visible;
            header3.Visibility = Visibility.Visible;
            syntax_tree_canvas.Visibility = Visibility.Visible;
            scroll.Visibility = Visibility.Visible;
        }

        private void Hide_Page2()
        {
            header3.Visibility = Visibility.Hidden;
            syntax_tree_canvas.Visibility = Visibility.Hidden;
            scroll.Visibility = Visibility.Hidden;
        }

        // To overcome the default effect of WPF when hovering over a button
        private void Button_Hover(object sender, MouseEventArgs e)
        {
            (sender as Button).Foreground = Brushes.Black;
        }

        private void Mouse_Leave_Button(object sender, MouseEventArgs e)
        {
            (sender as Button).Foreground = Brushes.White;
        }

    }
}
