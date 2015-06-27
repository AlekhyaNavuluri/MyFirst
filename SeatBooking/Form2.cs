using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace SeatBooking
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            CreateNewCourseForm();
            ReadCourseDeatailsFromFile();
            this.Text = "adslkhaslfhasldflj";
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveCourseDetailsFromForm();
            this.saveToolStripMenuItem.Enabled = false;
            foreach (var textBoxName in courseDeatilsArray)
            {
                var courseName = this.Controls.Find(textBoxName, false).First();  
                courseName.Show();
            }

            this.Controls.Find(courseDeatilsArray[0], false).First().Focus();
            foreach (var label in this.Controls.Find("Enter", false))
            {
                label.Show();
            }
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(622, 235);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Add Course";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.AddCourse_Click);
            this.Controls.Add(this.button1);
            button1.Show();
            comboBox1.Hide();
        }

        private void AddCourse_Click(object sender, EventArgs e)
        {
            if (!IsCourseDetailsValid())
            {
                return;
            }

            if (courses.Find(c => c.CourseName == this.Controls.Find(courseDeatilsArray[0], false).First().Text && 
                c.CourseDate == DateTime.Parse(this.Controls.Find(courseDeatilsArray[1], false).First().Text)) != null)
            {
                MessageBox.Show( "Course Exists for Specified Date");
                return;
            }

            if (courses.Count(c => c.CourseName == this.Controls.Find(courseDeatilsArray[0], false).First().Text) >= MaximunCourseDatesAvailable)
            {
                MessageBox.Show("Maximum course booking plans for a specific course " + MaximunCourseDatesAvailable.ToString()+"is reached");
                return;
            }

            Courses newCourse = null;
            string courseBokkingDeatils = null;

            using (StreamWriter s = new System.IO.StreamWriter(@"C:\Users\alekhya\Documents\log.txt",true))
            {
                foreach (var courseDetail in courseDeatilsArray)
                {
                    var courseName = this.Controls.Find(courseDetail, false).First();
                    var name = courseName.Text;

                    var t = Array.IndexOf(courseDeatilsArray, courseDetail);

                    switch (t)
                    {
                        case 0:
                            newCourse = new Courses();
                            newCourse.CourseName = name;
                            break;
                        case 1:
                            newCourse.CourseDate = DateTime.Parse(name.Trim());
                            break;
                        case 2:
                            newCourse.CourseFess = Decimal.Parse(name.Trim());
                            break;
                        case 3:                            
                            for (int i = 0; i < int.Parse(name.Trim()); i++)
                            {
                                courseBokkingDeatils += "F";
                            }
                            newCourse.CourseCapacityDetails = courseBokkingDeatils;
                            courses.Add(newCourse);
                            break;
                        default:
                            throw new Exception("Can't read file");
                    }

                    if (courseDetail == courseDeatilsArray[courseDeatilsArray.Length - 1])
                    {
                        s.Write(courseDetail + ":" + courseBokkingDeatils);
                        s.WriteLine();
                    }
                    else
                    {
                        s.WriteLine(courseDetail + ":" + name);
                    }
                    courseName.Text = "";
                }
                MessageBox.Show("Course Added Sucesfully");
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.button1.Click -= this.AddCourse_Click;
            LoadComboListWithCourses();
            this.saveToolStripMenuItem.Enabled = false;
            comboBox1.SelectedItem = "Select Course";

            if (courses.Count() == 0)
            {
                MessageBox.Show("No Courses available Please add using New from File menu");
                return;
            }

            foreach (var textBoxName in courseDeatilsArray)
            {
                var courseName = this.Controls.Find(textBoxName, false).First();
                courseName.Hide();
            }

            foreach (var label in this.Controls.Find("Enter", false))
            {
                label.Hide();
            }
            button1.Hide();
            comboBox1.Show();
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool IsCourseDetailsValid()
        {
            int stringLength = 30;
            DateTime dateTime = DateTime.MinValue;
            Decimal d; int i;

            if (this.Controls.Find(courseDeatilsArray[0], false).First().Text.Length <= 0 || this.Controls.Find(courseDeatilsArray[0], false).First().Text.Length > stringLength)
            {
                DisplayErrorMessageForTextBox(this.Controls.Find(courseDeatilsArray[0], false).First() as TextBox, "Please Enter name more than 0 and less than " + stringLength.ToString() + " charcters");
                return false;
            }
            else if (!DateTime.TryParse(this.Controls.Find(courseDeatilsArray[1], false).First().Text, out dateTime))
            {
                DisplayErrorMessageForTextBox(this.Controls.Find(courseDeatilsArray[1], false).First() as TextBox, "Please Enter Valid Date");
                return false;
            }
            else if (!Decimal.TryParse(this.Controls.Find(courseDeatilsArray[2], false).First().Text, out d))
            {
                DisplayErrorMessageForTextBox(this.Controls.Find(courseDeatilsArray[2], false).First() as TextBox, "Please Enter Valid Fees");
                return false;
            }
            else if (!int.TryParse(this.Controls.Find(courseDeatilsArray[3], false).First().Text, out i) && i > 12)
            {
                DisplayErrorMessageForTextBox(this.Controls.Find(courseDeatilsArray[3], false).First() as TextBox, "Please Enter Valid course places");
                return false;
            }
            return true;
        }


        private void save_Click(object sender, EventArgs e)
        {
            string bookingDetails = null;
            string courseName = comboBox1.SelectedItem.ToString();
            int stratIndexOfCourseBookingDetails = 0;
            bool isBookingDetailsChanged = false;
            string currentCourseBookingDetails;

            var courseList = GetCourseByNameOrderByDate(courseName);            

            foreach (Control control in this.Controls)
            {
                if (control.GetType() == typeof(Button) && control.Name.StartsWith("booking"))
                {
                    bookingDetails += control.Text;
                }
            }

            List<String> courseBookingDetails = readCourseDetails();
            
            foreach (var c in courseList)
            {
                currentCourseBookingDetails = bookingDetails.Substring(stratIndexOfCourseBookingDetails, c.CourseCapacityDetails.Length);

                if (c.CourseCapacityDetails != currentCourseBookingDetails)
                {
                    c.CourseCapacityDetails = currentCourseBookingDetails;
                    courseBookingDetails = AddNewBookingDetails(courseBookingDetails, currentCourseBookingDetails, c);
                    isBookingDetailsChanged = true;
                }
                
                stratIndexOfCourseBookingDetails += c.CourseCapacityDetails.Length;
            }

            if (isBookingDetailsChanged)
            {
                writeCourseDetailsToFile(courseBookingDetails);
                MessageBox.Show("Booking Changes Saved sucessfully");
            }
            else
            {
                MessageBox.Show("No New Changes to Save");
            }

            comboBox1.SelectedItem = courseName;
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
                return;
            RemoveCourseDetailsFromForm();
            this.saveToolStripMenuItem.Enabled = true;
            string name = comboBox1.SelectedItem.ToString();

            var courseList = GetCourseByNameOrderByDate(name);

            int left = 100;
            int top = 100;
            int j = 1;

            AddLabel(1180, top-30, "Course Date");
            AddLabel(1180 + 90, top - 30, "Course Per Person");

            foreach (var c in courseList)
            {
                for (int i = 0; i < c.CourseCapacityDetails.Length; i++)
                {
                    var button = new Button();
                    button.Name = "booking";
                    button.Text = c.CourseCapacityDetails[i].ToString();
                    SetButtonColour(button);
                    button.Left = left;
                    button.Tag = j +"_" + i;
                    button.Top = top;
                    left += 90;
                    this.Controls.Add(button);
                    button.Click += new EventHandler(this.Btn_Click);
                }

                AddLabel(left, top, c.CourseDate.ToShortDateString());
                AddLabel(left+90, top, "£"+c.CourseFess.ToString());

                left = 100;
                top += 40;
                j++;
            }
        }

        private List<Courses> GetCourseByNameOrderByDate(string name)
        {
            return courses.FindAll(f => f.CourseName == name).OrderBy(c => c.CourseDate).ToList();
        }

        private void AddLabel(int left,int top,string text)
        {
            var label = new Label();
            label.AutoSize = true;
            label.Left = left;
            label.Top = top;
            label.Name = "courselabel";
            label.TabIndex = 0;
            label.AutoSize = true;
            label.Text = text;
            this.Controls.Add(label);
        }

        private void RemoveCourseDetailsFromForm()
        {
            for (int i = 0; i < this.Controls.Count; i++)
            {
                if (Controls[i] is Button || (Controls[i] is Label && Controls[i].Name != "Enter"))
                {
                    Controls.Remove(Controls[i]);
                    i--;
                }
            }
        }

        private void SetButtonColour(Button button)
        {
            if (button.Text == "F")
            {
                button.BackColor = Color.White;
            }
            else
            {
                button.BackColor = Color.Green;
            }
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            SetButtonText(button);
        }

        private void SetButtonText(Button button)
        {
            if (button.Text == "B")
            {
                button.Text = "F";
                button.BackColor = Color.White;
            }
            else
            {
                button.Text = "B";
                button.BackColor = Color.Green;
            }
        }

        private List<String> readCourseDetails()
        {
            List<string> txtLines = new List<string>();

            foreach (string str in File.ReadAllLines(fileName))
            {
                txtLines.Add(str);
            }

            return txtLines;
        }

        private void writeCourseDetailsToFile(List<String> txtLines)
        {
            using (File.Create(fileName)) { }

            foreach (string str in txtLines)
            {
                File.AppendAllText(fileName, str + Environment.NewLine);
            }
        }

        private List<String> AddNewBookingDetails(List<string> txtLines,String currentCourseBookingDetails, Courses c)
        {
            string newBookingDetails = courseDeatilsArray[3] + ":" + currentCourseBookingDetails;
            String courseName = courseDeatilsArray[0] + ":" + c.CourseName;
            string courseDate = courseDeatilsArray[1] + ":" + c.CourseDate.ToShortDateString();
            bool foundCourseToReplace = false;

            var courseNameIndex = txtLines.IndexOf(courseName);

            while (!foundCourseToReplace && courseNameIndex >= 0)
            {
                if (txtLines[courseNameIndex + 1] == courseDate)
                {
                    foundCourseToReplace = true;
                    break;
                }

                courseNameIndex = txtLines.IndexOf(courseName, courseNameIndex+1);
            }

            //Insert the line you want to add last under the tag 'item1'.
            var replaceLocation = courseNameIndex + courseDeatilsArray.Length - 1;
            txtLines.RemoveAt(replaceLocation);
            txtLines.Insert(replaceLocation, newBookingDetails);

            return txtLines;
        }



        
        private void DisplayErrorMessageForTextBox(TextBox textBox,String message)
        {
            MessageBox.Show(message);
            textBox.Clear();
            textBox.Focus();
        }

        private void ReadCourseDeatailsFromFile()
        {
                Dictionary<string, int> courseName = new Dictionary<string, int>();
                courses = new List<Courses>();
                Courses c = null;

                var courseDeatils = readCourseDetails();

                // Read and display lines from the file until the end of
                // the file is reached.
                foreach (var line in courseDeatils)
                {
                    var name = line.Split(':').First();

                    var t = Array.IndexOf(courseDeatilsArray, name);

                    switch (t)
                    {
                        case 0:
                            c = new Courses();
                            c.CourseName = line.Split(':').Last();
                            break;
                        case 1:
                            c.CourseDate = DateTime.Parse(line.Split(':').Last());
                            break;
                        case 2:
                            c.CourseFess = Decimal.Parse(line.Split(':').Last());
                            break;
                        case 3:
                            c.CourseCapacityDetails = line.Split(':').Last();
                            courses.Add(c);
                            break;
                        default:
                            throw new Exception("Can't read file");
                    }
                }
        }

        private void LoadComboListWithCourses()
        {
            comboBox1.Items.Clear();
            comboBox1.Items.Add("Select Course");
            comboBox1.SelectedIndex = 0;
            
            foreach(var c in courses)
            {                   
                if (!comboBox1.Items.Contains(c.CourseName))
                {
                    comboBox1.Items.Add(c.CourseName);
                }
            }            
        }
    }
}
