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
                button.Text = button.Tag.ToString().Split(':').First().Split('_').Last();
                button.BackColor = Color.White;
            }
            else
            {
                button.Text = "B";
                button.BackColor = Color.Green;
            } 
        
        }

        private void New_Click(object sender, EventArgs e)
        {
            RemoveButtons();
            
            int left = 100;
            int top = 100;
            for (int j = 1; j <= 10; j++)
            {
                for (int i = 1; i <= 12; i++)
                {
                    var button = new Button();
                    button.Name = "booking";
                    button.Text = i.ToString();
                    button.Left = left;
                    button.Tag = j + "_" + i;
                    button.Top = top;
                    left += 90;
                    this.Controls.Add(button);
                    button.Click += new EventHandler(this.Btn_Click);
                }

                left = 100;
                top += 40;
            }        
        }
        
        private void Save_Click(object sender, EventArgs e)
        {
            using (StreamWriter s = new StreamWriter(@"C:\Users\alekhya\Documents\log.txt"))
            {
               foreach (Control control in this.Controls)
               {
                   if (control.GetType() == typeof(Button) && control.Name.StartsWith("booking"))
                   {
                       Button b = control as Button;
                       s.Write(b.Tag);
                       s.WriteLine(":"+b.Text);
                   }                
               }  
            }
        }




        private void Open_Click(object sender, EventArgs e)
        {
            RemoveButtons();
                
                using (StreamReader sr = new StreamReader(@"C:\Users\alekhya\Documents\log.txt"))
                {
                    int left = 100;
                    int top = 100;

                    String line;
                    String row="1";
                    // Read and display lines from the file until the end of
                    // the file is reached.
                    while ((line = sr.ReadLine()) != null)
                   {
                       if (row != line.Split('_').First())
                       {
                           row = line.Split('_').First();
                           left = 100;
                           top += 40;
                       }
                            var button = new Button();
                            button.Left = left;
                            button.Top = top;
                            button.Name = "booking";
                            button.Tag = line.Split(':').First();
                            button.Text = line.Split(':').Last();
                            if (button.Text == "B")
                            {
                               button.BackColor = Color.Green;
                            }
                            left += 90;
                            this.Controls.Add(button);
                            button.Click += new EventHandler(this.Btn_Click);
                   }   
                }
            
        }
    }
}


