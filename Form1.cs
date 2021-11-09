using System;
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
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            string x = ofd.FileName;

             int n = 0;
             string lines;
            string[] lines1 = System.IO.File.ReadAllLines(@x);
            string before=null;
            for (int i = 0; i < lines1.Length; i++)
            {
                before += lines1[i] + Environment.NewLine;
               
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
    }
}
