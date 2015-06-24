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

namespace SeatBooking
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        private void Btn_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender ;
            if (button.Text == "B")
            {
                button.Text = button.Name;
                button.BackColor = Color.White;
            }
            else
            {
                button.Text = "B";
                button.BackColor = Color.Green;
            }

            using (StreamWriter w = File.AppendText(@"C:\Temp\log.txt"))
            {
                w.WriteLine(button.Text);
                w.WriteLine(button.Name);
            }
        }
    }
}


