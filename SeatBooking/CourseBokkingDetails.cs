using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SeatBooking
{
    public partial class CourseBokkingDetails : Form
    {
        private readonly List<Courses> _selectedCourseOrderedByDateList;
        private Font printFont;
        private StreamReader streamToPrint;

        public Form courseMenu { get; set; }

        public CourseBokkingDetails()
        {
            InitializeComponent();
        }

        public CourseBokkingDetails(String selectedCourse)
            : this()
        {
            _selectedCourseOrderedByDateList = Common.Courses.FindAll(f => f.CourseName == selectedCourse).OrderBy(c => c.CourseDate).ToList();
            DisplayBookingDetailsOfSelectedCourse();
        }

        private void OnBooking_Button_Click(object sender, EventArgs e)
        {
            var button = sender as Button;

            if (button != null && button.Text == "B")
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

        private void SaveAndClose_ButtonClick(object sender, EventArgs e)
        {
            string bookingDetails = null;
            int stratIndexOfCourseBookingDetails = 0;
            bool isBookingDetailsChanged = false;

            foreach (Control control in Controls)
            {
                if (control is Button && control.Name.StartsWith("booking"))
                {
                    bookingDetails += control.Text;
                }
            }

            foreach (var c in _selectedCourseOrderedByDateList)
            {
                string currentCourseBookingDetails = bookingDetails.Substring(stratIndexOfCourseBookingDetails, c.CourseCapacityDetails.Length);

                if (c.CourseCapacityDetails != currentCourseBookingDetails)
                {
                    c.CourseCapacityDetails = currentCourseBookingDetails;
                    Common.CourseDeatils = AddNewBookingDetails(Common.CourseDeatils, currentCourseBookingDetails, c);
                    isBookingDetailsChanged = true;
                }

                stratIndexOfCourseBookingDetails += c.CourseCapacityDetails.Length;
            }

            if (isBookingDetailsChanged)
            {
                WriteCourseDetailsToFile(Common.CourseDeatils);
                MessageBox.Show("Booking Changes Saved sucessfully");
            }
            else
            {
                MessageBox.Show("No New Changes to Save");
            }

            Close();
            courseMenu.Show();
        }

        private void WriteCourseDetailsToFile(IEnumerable<string> txtLines)
        {
            using (File.Create(Common.FileName)) { }

            foreach (string str in txtLines)
            {
                File.AppendAllText(Common.FileName, str + Environment.NewLine);
            }
        }

        private List<String> AddNewBookingDetails(List<string> courseDetails, String currentCourseBookingDetails, Courses c)
        {
            string newBookingDetails = currentCourseBookingDetails;
            String courseName = c.CourseName;
            string courseDate = c.CourseDate.ToShortDateString();

            var courseNameIndex = courseDetails.IndexOf(courseName);

            while (courseNameIndex >= 0)
            {
                if (courseDetails[courseNameIndex + 1] == courseDate)
                {
                    break;
                }

                courseNameIndex = courseDetails.IndexOf(courseName, courseNameIndex + 1);
            }

            var replaceLocation = courseNameIndex + Constants.CourseDeatilsArray.Length - 1;
            courseDetails.RemoveAt(replaceLocation);
            courseDetails.Insert(replaceLocation, newBookingDetails);

            return courseDetails;
        }

        private void DisplayBookingDetailsOfSelectedCourse()
        {
            int left = 100;
            int top = 100;
            int j = 1;

            var label = AddLabel(600, top - 60, "Course Name : " + _selectedCourseOrderedByDateList.First().CourseName);
            label.Font = new Font(label.Font.FontFamily, 15);
            label.ForeColor = Color.DarkBlue;
            label = AddLabel(1180, top - 30, "Course Date");
            label.Font = new Font(label.Font.FontFamily, 8);
            label.ForeColor = Color.DarkBlue;
            label = AddLabel(1180 + 90, top - 30, "Course Per Person");
            label.Font = new Font(label.Font.FontFamily, 8);
            label.ForeColor = Color.DarkBlue;

            foreach (var c in _selectedCourseOrderedByDateList)
            {
                for (int i = 0; i < c.CourseCapacityDetails.Length; i++)
                {
                    var button = new Button
                    {
                        Name = "booking",
                        Text = c.CourseCapacityDetails[i].ToString(),
                        Left = left,
                        Tag = j + "_" + i,
                        Top = top,
                        BackColor = c.CourseCapacityDetails[i].ToString() == "F" ? Color.White : Color.Green
                    };

                    left += 90;
                    Controls.Add(button);
                    button.Click += OnBooking_Button_Click;
                }

                AddLabel(left, top, c.CourseDate.ToShortDateString());
                AddLabel(left + 90, top, "£" + c.CourseFess);
                left = 100;
                top += 40;
                j++;
            }

            var saveButton = AddButton(left + 1000, top, "Save And Close Form");
            saveButton.Click += SaveAndClose_ButtonClick;
            var printButton = AddButton(left + 800, top, "Print");
            printButton.Click += Print_ButtonClick;
        }

        private Control AddLabel(int left, int top, string text)
        {
            var label = new Label
            {
                AutoSize = true,
                Left = left,
                Top = top,
                Name = "courselabel",
                TabIndex = 0,
                Text = text
            };
            Controls.Add(label);

            return label;
        }

        private Control AddButton(int left, int top, string name)
        {
            var button = new Button
            {
                Name = name,
                Text = name,
                Left = left / 2,
                Top = top + 50,
                AutoSize = true
            };
            Controls.Add(button);
            return button;
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            var g = e.Graphics;
            var message = Environment.UserName;
            var messageFont = new Font("Arial",
                     24, GraphicsUnit.Point);
            g.DrawString(Common.CourseDeatils.First(), messageFont, Brushes.Black, 100, 100);

            try
            {
                streamToPrint = new StreamReader(Common.FileName);
                try
                {
                    printFont = new Font("Arial", 10);
                    var pd = new PrintDocument();
                    pd.PrintPage += PrintPage;
                    pd.Print();
                }
                finally
                {
                    streamToPrint.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void PrintPage(object sender, PrintPageEventArgs ev)
        {
            float linesPerPage = 0;
            float yPos = 0;
            int count = 0;
            float leftMargin = ev.MarginBounds.Left;
            float topMargin = ev.MarginBounds.Top;
            string line = null;

            // Calculate the number of lines per page.
            linesPerPage = ev.MarginBounds.Height /
               printFont.GetHeight(ev.Graphics);

            // Print each line of the file.
            while (count < linesPerPage &&
               ((line = streamToPrint.ReadLine()) != null))
            {
                yPos = topMargin + (count *
                   printFont.GetHeight(ev.Graphics));
                ev.Graphics.DrawString(line, printFont, Brushes.Black,
                   leftMargin, yPos, new StringFormat());
                count++;
            }

            // If more lines exist, print another page.
            if (line != null)
                ev.HasMorePages = true;
            else
                ev.HasMorePages = false;
        }

        private void Print_ButtonClick(object sender, EventArgs e)
        {
            printPreviewDialog1.ShowDialog();
        }
    }
}