using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace home2
{
    public partial class gmailaccount : Form
    {
        public gmailaccount()
        {
            InitializeComponent();
        }

        private void gmailaccount_Load(object sender, EventArgs e)
        {
            textBox1.Focus();


            
        }

        private void textBox1_Enter(object sender, System.EventArgs e)
        {
            // If the TextBox contains text, change its foreground and background colors. 
            if (textBox1.Text != String.Empty)
            {
                textBox1.ForeColor = Color.Red;
                textBox1.BackColor = Color.Black;
                // Move the selection pointer to the end of the text of the control.
                textBox1.Select(textBox1.Text.Length, 0);
            }
            else
            {
                MessageBox.Show("Please enter username");
                textBox1.Focus();
            }
        }


        private void textBox1_Leave(object sender, System.EventArgs e)
        {
            if (textBox1.Text != String.Empty && textBox1.Text.Length < 10)
            {
                textBox1.ForeColor = Color.Red;
                textBox1.BackColor = Color.Black;
                // Move the selection pointer to the end of the text of the control.
                textBox1.Select(textBox1.Text.Length, 0);
            }
            else
            {
                MessageBox.Show("Please enter username");
                textBox1.Focus();
            }
        }

        private void button1_Click(object sender, EventArgs e){
     
            var u = textBox1.Text;
            var p = textBox2.Text;

            this.Close();
        }
    }
}
