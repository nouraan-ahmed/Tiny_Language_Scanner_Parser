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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int page_number = 0;
        string sourceDirectory = null;
        string inputFileName = null;
        string outputFile = null;

        public MainWindow()
        {
            InitializeComponent();

            //hide buttons
            Hide_Page1();
        }

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
            if(page_number == 1)
            {
                page_number--;
                //go back to the main page
                Hide_Page1();

                Show_Main_Page();

            }
            else if(page_number == 2)
            {
                page_number--;
                Hide_Page2();
                Show_Page1();
            }
        }

        private void Syntax_Tree_Click(object sender, RoutedEventArgs e)
        {
            page_number = 2;
            Hide_Page1();
            Show_Page2();
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
                //remove any text writen before in output textbox when browsing new file
                output_text.Text = null;

                //Get the path of specified file
                var filePath = input_file.FileName;
                inputFileName = System.IO.Path.GetFileNameWithoutExtension(filePath);

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
        }

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
        }

        private void Hide_Page2()
        {
            header3.Visibility = Visibility.Hidden;
        }

        private void Scaner_Click(object sender, RoutedEventArgs e)
        {
            syntax_tree.Visibility = Visibility.Visible;

        }

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
