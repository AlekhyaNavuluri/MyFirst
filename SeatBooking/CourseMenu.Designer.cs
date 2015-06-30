using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
namespace SeatBooking
{
    partial class CourseMenu
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fIleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.AddCourseToList = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fIleToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(733, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fIleToolStripMenuItem
            // 
            this.fIleToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fIleToolStripMenuItem.Name = "fIleToolStripMenuItem";
            this.fIleToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fIleToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.NewMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.OpenCourse_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItemClick);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(96, 27);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 1;
            this.comboBox1.Hide();
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.ComboBoxOnCourse_SelectionChanged);
            // 
            // AddCourseToList
            // 
            this.AddCourseToList.Location = new System.Drawing.Point(0, 0);
            this.AddCourseToList.Name = "AddCourseToList";
            this.AddCourseToList.Size = new System.Drawing.Size(75, 23);
            this.AddCourseToList.TabIndex = 0;
           
            // 
            // CourseMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(733, 394);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "CourseMenu";
            this.Text = "Course Menu";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void CreateNewCourseForm()
        {
            int top = 90;
            int left = 96;
            int textBoxTop = 110;
            int textBoxLeft = 96;

            for (int i = 0; i < Constants.CourseDeatilsArray.Length-1; i++)
            {
                var label = new System.Windows.Forms.Label();
                label.AutoSize = true;
                label.Location = new System.Drawing.Point(left, top);
                label.Name = "Enter";
                label.Size = new System.Drawing.Size(100, 20);
                label.TabIndex = 2;
                label.Text = "Enter " + Constants.CourseDeatilsArray[i];
                left += 150;
                this.Controls.Add(label);

                var textBox1 = new System.Windows.Forms.TextBox();
                textBox1.Location = new System.Drawing.Point(textBoxLeft, textBoxTop);
                textBox1.Name = Constants.CourseDeatilsArray[i];
                textBox1.Size = new System.Drawing.Size(100, 20);
                textBox1.TabIndex = 5;
                textBoxLeft += 150;

                this.Controls.Add(textBox1);
            }

            AddCourseToList.Location = new Point(textBoxLeft-10, textBoxTop-5);
            AddCourseToList.Name = "AddCourseToList";
            AddCourseToList.Size = new Size(75, 23);
            AddCourseToList.TabIndex = 2;
            AddCourseToList.Text = "Add Course To List";
            AddCourseToList.UseVisualStyleBackColor = true;
            AddCourseToList.Click += AddCourseToList_Click;
            Controls.Add(AddCourseToList);

            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(100, textBoxTop+30);
            this.listBox1.Name = "AddAllCourses";
            this.listBox1.AutoSize = true;
            this.listBox1.TabIndex = 2;
            this.listBox1.Show();
            Controls.Add(listBox1);

            AddAllCourses = new Button();
            AddAllCourses.Location = new Point(250, textBoxTop + 100);
            AddAllCourses.Name = "AddCourseToList";
            AddAllCourses.AutoSize = true;
            AddAllCourses.TabIndex = 2;
            AddAllCourses.Text = "Save Course List";
            AddAllCourses.UseVisualStyleBackColor = true;
            AddAllCourses.Click += SaveCourses;
            Controls.Add(AddAllCourses);

            AddCourseToList.Show();
        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fIleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button AddCourseToList;
        private System.Windows.Forms.Button AddAllCourses;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ListBox listBox1;
    }
}