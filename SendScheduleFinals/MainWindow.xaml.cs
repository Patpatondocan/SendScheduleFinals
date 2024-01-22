using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Collections.Specialized.BitVector32;
using System.Net.NetworkInformation;
using System.IO.Packaging;
using System.Text.RegularExpressions;

namespace SendScheduleFinals
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DataClasses1DataContext dbase = new DataClasses1DataContext();
     

        public MainWindow()
        {
            InitializeComponent();
            dbase = new DataClasses1DataContext(Properties.Settings.Default.EnrollmentDatabaseConnectionString);
        }


        public static class EmailHelper
        {
            public static void SendEmail(string recipientEmail, string subject = "Class Schedule", string body = "This is your class Schedule")
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("slimjadeyyyy@gmail.com", "szzb btnq zowx gfad"),
                    EnableSsl = true,
                };

                try
                {
                    smtpClient.Send("slimjadeyyyy@gmail.com", recipientEmail, subject, body);
                    Console.WriteLine("Email sent successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending email: {ex.Message}");
                }
            }
        }




        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string enteredEmail;
            string inputText = tbx_email.Text.Trim(); // Trim to remove leading and trailing whitespaces

            if (string.IsNullOrEmpty(inputText))
            {
                // Ask the user first
                MessageBoxResult result = MessageBox.Show("The email input is blank. Do you want to send the schedule to all emails?", "Confirmation", MessageBoxButton.YesNo);




                if (result == MessageBoxResult.Yes)
                {




                    // Send schedule to all teachers
                    var teachers = dbase.tblEmployees.Where(s => s.classification.Equals("Teacher", StringComparison.OrdinalIgnoreCase)).ToList();
                    int teacherIndex = 0;

                    while (teacherIndex < teachers.Count)
                    {
                        enteredEmail = teachers[teacherIndex].employeeEmail;


                        //EmailHelper.SendEmail(enteredEmail);  UNCOMMENT THIS 

                        MessageBox.Show("Email sent successfully to: " + enteredEmail);
                        teacherIndex++;
                        System.Threading.Thread.Sleep(3000); // Pause for 3 seconds
                    }








                    // Send schedule to all students
                    var students = dbase.tblStudents.Where(s => s.studentStatus.Equals("Enrolled", StringComparison.OrdinalIgnoreCase)).ToList();
                    int studentIndex = 0;

                    while (studentIndex < students.Count)
                    {
                        enteredEmail = students[studentIndex].studentEmail;

                        //EmailHelper.SendEmail(enteredEmail);  UNCOMMENT THIS 

                        MessageBox.Show("Email sent successfully to: " + enteredEmail);
                        studentIndex++;
                        System.Threading.Thread.Sleep(3000); // Pause for 2 seconds
                    }



                    MessageBox.Show("Schedule has been sent to all Teacher's and Student's Emails");
                    return;
                }






                else
                {
                    // User chose not to send the schedule to all emails
                    return;
                }
            }





            // Check if the input is an email
            if (IsValidEmail(inputText))
            {
                
                var teacherByEmail = dbase.tblEmployees.FirstOrDefault(s => s.employeeEmail == inputText);
                var studentByEmail = dbase.tblStudents.FirstOrDefault(s => s.studentEmail == inputText);

                if (teacherByEmail != null)
                {
                    if(teacherByEmail.classification.Equals("Teacher", StringComparison.OrdinalIgnoreCase))
                    {
                        enteredEmail = teacherByEmail.employeeEmail;

                        //EmailHelper.SendEmail(enteredEmail);  UNCOMMENT THIS 

                        MessageBox.Show("Email sent successfully to: " + enteredEmail);
                        return;
                    }
                    else
                    {
                        MessageBox.Show("Email is not a teacher");
                        return;
                    }

                }

                if (studentByEmail != null)
                {
                    if (studentByEmail.studentStatus.Equals("Enrolled", StringComparison.OrdinalIgnoreCase))
                    {
                        enteredEmail = studentByEmail.studentEmail;

                        //EmailHelper.SendEmail(enteredEmail);  UNCOMMENT THIS 

                        MessageBox.Show("Email sent successfully to: " + enteredEmail);
                        return;
                    }
                    else
                    {
                        MessageBox.Show("Email is not a student who is ENROLLED");
                        return;
                    }

                }
            }







            // If not an email, try to parse as an integer for student or teacher ID
            if (int.TryParse(inputText, out int id))
            {


                var teacherByID = dbase.tblEmployees.FirstOrDefault(s => s.employeeID == id);
                var studentByID = dbase.tblStudents.FirstOrDefault(s => s.StudentID == id);
               
                if (teacherByID != null)
                {
                    if (teacherByID.classification.Equals("Teacher", StringComparison.OrdinalIgnoreCase))
                    {
                        enteredEmail = teacherByID.employeeEmail;

                        //EmailHelper.SendEmail(enteredEmail);  UNCOMMENT THIS 

                        MessageBox.Show("Email sent successfully to: " + enteredEmail);
                        return;
                    }

                    else
                    {
                        MessageBox.Show("Email is not a teacher");
                        return;
                    }
                }
              
                if (studentByID != null)
                {

                    if (studentByID.studentStatus.Equals("Enrolled", StringComparison.OrdinalIgnoreCase))
                    {
                        enteredEmail = studentByID.studentEmail;

                        //EmailHelper.SendEmail(enteredEmail);  UNCOMMENT THIS 

                        MessageBox.Show("Email sent successfully to: " + enteredEmail);
                        return;
                    }

                    else
                    {
                        MessageBox.Show("Email is not a student who is ENROLLED");
                        return;
                    }

                }

            }








            // Check if the input is a student name
            var studentByName = dbase.tblStudents.FirstOrDefault(s => s.studentName == inputText);

            if (studentByName != null)
            {
                if (studentByName.studentStatus.Equals("Enrolled", StringComparison.OrdinalIgnoreCase))
                {
                    enteredEmail = studentByName.studentEmail;

                    //EmailHelper.SendEmail(enteredEmail);  UNCOMMENT THIS 

                    MessageBox.Show("Email sent successfully to: " + enteredEmail);
                    return;
                }
                else
                {
                    MessageBox.Show("Email is not a student who is ENROLLED");
                    return;
                }
            }


            // Check if the input is a teacher name
            var teacherByName = dbase.tblEmployees.FirstOrDefault(s => s.employeeName == inputText);


            if (teacherByName != null)
            {
                if (teacherByName.classification.Equals("Teacher", StringComparison.OrdinalIgnoreCase))
                {
                    enteredEmail = teacherByName.employeeEmail;

                    //EmailHelper.SendEmail(enteredEmail);  UNCOMMENT THIS 

                    MessageBox.Show("Email sent successfully to: " + enteredEmail);
                    return;
                }
                else
                {
                    MessageBox.Show("Email is not a teacher");
                    return;
                }

            }






            // Check if the input is "Teachers"
            if (inputText == "Teachers")
            {
                var teachers = dbase.tblEmployees.Where(s => s.classification.Equals("Teacher", StringComparison.OrdinalIgnoreCase)).ToList();
                int index = 0;

                while (index < teachers.Count)
                {
                    enteredEmail = teachers[index].employeeEmail;

                    //EmailHelper.SendEmail(enteredEmail);  UNCOMMENT THIS 

                    MessageBox.Show("Email sent successfully to: " + enteredEmail);
                    index++;
                   System.Threading.Thread.Sleep(3000); // Pause for 3 second
                }

                MessageBox.Show("Schedule has been sent to all Teacher's Emails");
                return;
            }




            // Check if the input is "Students"
            if (inputText == "Students")
            {
                var students = dbase.tblStudents.Where(s => s.studentStatus.Equals("Enrolled", StringComparison.OrdinalIgnoreCase)).ToList();
                int index = 0;

                while (index < students.Count)
                {
                    enteredEmail = students[index].studentEmail;

                    //EmailHelper.SendEmail(enteredEmail);  UNCOMMENT THIS 

                    MessageBox.Show("Email sent successfully to: " + enteredEmail);
                    index++;
                   System.Threading.Thread.Sleep(3000); // Pause for 3 second
                }

                MessageBox.Show("Schedule has been sent to all Student's Emails");
                return;
            }








            // If neither email, student ID, teacher ID, student name, nor teacher name found
            MessageBox.Show("No matching email, student ID, teacher ID, student name, or teacher name found in the database.");
        }






        //validating email addresses
        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }
    }
}
