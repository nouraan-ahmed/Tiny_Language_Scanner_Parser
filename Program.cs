using System;
using System.Windows.Forms;
using WindowsFormsApp1;

namespace ConsoleApp
{
    class Program
    {
        
       
            // Console.WriteLine("Hello World!");



            [STAThread]
            static void Main()
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
        }
    }
    

