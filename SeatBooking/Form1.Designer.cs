using System;
using System.Collections.Generic;
using System.Windows.Forms;
namespace SeatBooking
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            int left = 100;
            int top = 100;
            for (int j = 1; j <= 10; j++)
            {
                for (int i = 1; i <= 12; i++)
                {
                    var button = new Button();
                    button.Name = i.ToString();
                    button.Text = i.ToString();
                    button.Left = left;
                    button.Top = top;
                    left += 90;
                    this.Controls.Add(button);
                    button.Click += new EventHandler(this.Btn_Click);
                }

                left = 100;
                top += 40;
            }

            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            
            this.Name = "Form1";
            this.ResumeLayout(false);
            this.WindowState = FormWindowState.Maximized;

        }

        #endregion

        

    }
}

