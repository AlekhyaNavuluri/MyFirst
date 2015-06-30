using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SeatBooking
{
    public partial class CourseMenu : Form
    {
        private readonly List<string> _newCourseDeatils;

        public CourseMenu()
        {
            InitializeComponent();
            _newCourseDeatils = new List<string>();
        }

        private void NewMenuItem_Click(object sender, EventArgs e)
        {
            CreateNewCourseForm();
            comboBox1.Hide();
            Common.Courses = new List<Courses>();
        }

        private void AddCourseToList_Click(object sender, EventArgs e)
        {
            var courseName = Controls.Find(Constants.CourseDeatilsArray[0], false).First().Text.Trim();
            var courseDateTextBox = Controls.Find(Constants.CourseDeatilsArray[1], false).First();

            if (CheckCourseIsValid(courseName, courseDateTextBox))
            {
                AddCourseDetails(courseName, courseDateTextBox.Text);
            }
        }

        private void SaveCourses(object sender, EventArgs e)
        {
            if (listBox1.Items.Count <= 0)
            {
                MessageBox.Show("Please create new course to add");
                return;
            }

            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "txt files (*.txt)|*.txt";
            DialogResult result = saveFileDialog.ShowDialog();

            if (result == DialogResult.Cancel)
            {
                return;
            }

            if (result == DialogResult.OK && saveFileDialog.FileName != null)
            {
                Common.FileName = saveFileDialog.FileName;
            }
            else
            {
                MessageBox.Show("Please select file");
                saveFileDialog.ShowDialog();
            }

            File.AppendAllLines(Common.FileName, _newCourseDeatils.ToArray());
            MessageBox.Show("Course Added Sucesfully to " + Common.FileName);

            foreach (Control control in Controls)
            {
                if (!(control is ComboBox || control is MenuStrip || control is Button))
                {
                    control.Text = "";
                }
            }
        }

        private void OpenCourse_Click(object sender, EventArgs e)
        {
            var openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "txt files (*.txt)|*.txt";
            DialogResult result = openFileDialog1.ShowDialog();

            if (result == DialogResult.Cancel)
            {
                return;
            }

            if (result == DialogResult.OK && openFileDialog1.FileName != null)
            {
                Common.FileName = openFileDialog1.FileName;
                Common.Courses = new List<Courses>();
            }
            else
            {
                MessageBox.Show("Please select file");
                openFileDialog1.ShowDialog();
            }

            try
            {
                ReadCourseDeatailsFromFile();
            }
            catch (CannotReadFileException ex)
            {
                MessageBox.Show(ex.InnerException.ToString());
                return;
            }

            LoadComboListWithCourses();
            comboBox1.SelectedItem = "Select Course";

            if (!Common.Courses.Any())
            {
                MessageBox.Show("No Courses available Please add using New from File menu");
                return;
            }

            foreach (Control control in Controls)
            {
                if (!(control is ComboBox || control is MenuStrip))
                {
                    control.Hide();
                }
            }

            comboBox1.Show();
        }

        private bool CheckCourseIsValid(string courseName, Control courseDateTextBox)
        {
            if (!IsCourseDetailsValid())
            {
                return false;
            }

            if (CourseExceededMaximumAllowedDates(courseName))
            {
                MessageBox.Show("Maximum course booking plans for a specific course is more than " + Constants.MaximunCourseDatesAvailable);
                return false;
            }

            if (Common.Courses.Any(c => c.CourseName == courseName && c.CourseDate == DateTime.Parse(courseDateTextBox.Text)))
            {
                MessageBox.Show("Course with same date already exists");
                return false;
            }
            return true;
        }

        private void AddCourseDetails(string courseName, String courseDate)
        {
            var courseFees = Controls.Find(Constants.CourseDeatilsArray[2], false).First().Text.Trim();

            string courseBokkingDeatils = null;

            for (int i = 0; i < 12; i++)
            {
                courseBokkingDeatils += "F";
            }

            _newCourseDeatils.Add(courseName);
            _newCourseDeatils.Add(courseDate);
            _newCourseDeatils.Add(courseFees);
            _newCourseDeatils.Add(courseBokkingDeatils);

            listBox1.Items.Add(courseName);

            Common.Courses.Add(new Courses
            {
                CourseName = courseName,
                CourseDate = DateTime.Parse(courseDate),
                CourseFess = Decimal.Parse(courseFees),
                CourseCapacityDetails = courseBokkingDeatils
            });
        }

        private bool CourseExceededMaximumAllowedDates(string courseName)
        {
            return Common.Courses.Count(c => c.CourseName == courseName) >= Constants.MaximunCourseDatesAvailable;
        }

        private void ExitToolStripMenuItemClick(object sender, EventArgs e)
        {
            Close();
        }

        private bool IsCourseDetailsValid()
        {
            const int stringLength = 30;
            DateTime dateTime;
            Decimal courseFees;

            var courseNameTextBox = Controls.Find(Constants.CourseDeatilsArray[0], false).First();
            var courseDateTextBox = Controls.Find(Constants.CourseDeatilsArray[1], false).First();
            var courseFeesTextBox = Controls.Find(Constants.CourseDeatilsArray[2], false).First();

            if (IsCourseNameValid(courseNameTextBox.Text.Length))
            {
                DisplayErrorMessageForTextBox(courseNameTextBox as TextBox, "Please Enter name more than 0 and less than " + stringLength + " charcters");
                return false;
            }
            if (!IsCourseDateValid(courseDateTextBox.Text, out dateTime))
            {
                DisplayErrorMessageForTextBox(courseDateTextBox as TextBox, "Please Enter Valid Date");
                return false;
            }

            if (!IsCourseFeesValid(courseFeesTextBox.Text, out courseFees))
            {
                DisplayErrorMessageForTextBox(courseFeesTextBox as TextBox, "Please Enter Valid Fees");
                return false;
            }

            return true;
        }

        private bool IsCourseDateValid(string courseDate, out DateTime dateTime)
        {
            return DateTime.TryParse(courseDate, out dateTime);
        }

        private bool IsCourseFeesValid(string courseFees, out Decimal fees)
        {
            return Decimal.TryParse(courseFees, out fees);
        }

        private bool IsCourseNameValid(int courseName)
        {
            return courseName <= 0 || courseName > 30;
        }

        private void ComboBoxOnCourse_SelectionChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
                return;
            Hide();
            var courseBokkingDetails = new CourseBokkingDetails(comboBox1.SelectedItem.ToString());
            courseBokkingDetails.courseMenu = this;
            courseBokkingDetails.Show();
        }

        private List<String> ReadCourseDetails()
        {
            if (File.Exists(Common.FileName))
            {
                try
                {
                    return File.ReadAllLines(Common.FileName).ToList();
                }
                catch (Exception e)
                {
                    throw new CannotReadFileException(e);
                }
            }

            return new List<string>();
        }

        private void DisplayErrorMessageForTextBox(TextBox textBox, String message)
        {
            MessageBox.Show(message);
            textBox.Clear();
            textBox.Focus();
        }

        private void ReadCourseDeatailsFromFile()
        {
            var index = 0;

            Common.CourseDeatils = ReadCourseDetails();

            while (index < Common.CourseDeatils.Count)
            {
                var courseName = Common.CourseDeatils[index++];
                DateTime courseDate;
                decimal courseFees;

                if (IsCourseNameValid(courseName.Length))
                {
                    throw new CannotReadFileException(new Exception("Invalid Course Name"));
                }
                if (!IsCourseDateValid(Common.CourseDeatils[index++], out courseDate))
                {
                    throw new CannotReadFileException(new Exception("Invalid Course Date"));
                }

                if (!IsCourseFeesValid(Common.CourseDeatils[index++], out courseFees))
                {
                    throw new CannotReadFileException(new Exception("Invalid Course Date"));
                }

                var courseCapacityDetails = Common.CourseDeatils[index++];

                if (courseCapacityDetails.Length != Constants.CourseSeats ||
                    courseCapacityDetails.Any(c => c != 'B' && c != 'F'))
                {
                    throw new CannotReadFileException(new Exception("Invalid Course Booking details"));
                }

                Common.Courses.Add(new Courses
                {
                    CourseName = courseName,
                    CourseDate = courseDate,
                    CourseFess = courseFees,
                    CourseCapacityDetails = courseCapacityDetails
                });
            }
        }

        private void LoadComboListWithCourses()
        {
            comboBox1.Items.Clear();
            comboBox1.Items.Add("Select Course");
            comboBox1.SelectedIndex = 0;

            foreach (var c in Common.Courses)
            {
                if (!comboBox1.Items.Contains(c.CourseName))
                {
                    comboBox1.Items.Add(c.CourseName);
                }
            }
        }
    }
}