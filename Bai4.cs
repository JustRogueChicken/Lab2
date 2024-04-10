using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace Lab2
{
    public partial class Bai4 : Form
    {
        private const string inputFileName = "input4.txt";
        private const string outputFileName = "output4.txt";
        private Student[] students;
        private int currentPage = 0;
        private const int pageSize = 10;

        public Bai4()
        {
            InitializeComponent();
            LoadStudents();
            ShowStudents();
        }

        private void LoadStudents()
        {
            if (File.Exists(inputFileName))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream fs = new FileStream(inputFileName, FileMode.Open))
                {
                    students = (Student[])formatter.Deserialize(fs);
                }
            }
            else
            {
                students = new Student[0];
            }
        }

        private void SaveStudents()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream(inputFileName, FileMode.Create))
            {
                formatter.Serialize(fs, students);
            }
        }

        private void ShowStudents()
        {
            richTextBox1.Clear();
            int startIndex = currentPage * pageSize;
            int endIndex = Math.Min((currentPage + 1) * pageSize, students.Length);

            for (int i = startIndex; i < endIndex; i++)
            {
                richTextBox1.AppendText($"Name: {students[i].Name}\n");
                richTextBox1.AppendText($"MSSV: {students[i].MSSV}\n");
                richTextBox1.AppendText($"Phone: {students[i].Phone}\n");
                richTextBox1.AppendText($"Course 1: {students[i].Course1}\n");
                richTextBox1.AppendText($"Course 2: {students[i].Course2}\n");
                richTextBox1.AppendText($"Course 3: {students[i].Course3}\n");
                richTextBox1.AppendText($"Average: {students[i].CalculateAverage()}\n\n");
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            SaveStudents();
            MessageBox.Show("Students' information saved to input4.txt.");
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            LoadStudents();
            CalculateAndSaveAverage();
            ShowStudents();
            MessageBox.Show("Student information loaded from input4.txt. Averages calculated and saved to output4.txt.");

            // Hiển thị thông tin học sinh đầu tiên (nếu có)
            if (students.Length > 0)
            {
                var firstStudent = students[0];
                textBox14.Text = firstStudent.Name;
                textBox13.Text = firstStudent.MSSV.ToString();
                textBox12.Text = firstStudent.Phone;
                textBox11.Text = firstStudent.Course1.ToString();
                textBox10.Text = firstStudent.Course2.ToString();
                textBox9.Text = firstStudent.Course3.ToString();
                textBox8.Text = firstStudent.CalculateAverage().ToString();
            }
        }

        private void CalculateAndSaveAverage()
        {
            if (students == null || students.Length == 0)
            {
                MessageBox.Show("No student data to process.");
                return;
            }

            try
            {
                using (StreamWriter writer = new StreamWriter(outputFileName))
                {
                    foreach (var student in students)
                    {
                        float average = student.CalculateAverage();
                        writer.WriteLine($"Name: {student.Name}");
                        writer.WriteLine($"MSSV: {student.MSSV}");
                        writer.WriteLine($"Phone: {student.Phone}");
                        writer.WriteLine($"Course 1: {student.Course1}");
                        writer.WriteLine($"Course 2: {student.Course2}");
                        writer.WriteLine($"Course 3: {student.Course3}");
                        writer.WriteLine($"Average: {average}\n");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving data to output file: {ex.Message}");
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            // Add a new student
            if (textBox1.TextLength == 0 || textBox2.TextLength == 0 || textBox3.TextLength == 0 ||
                textBox4.TextLength == 0 || textBox5.TextLength == 0 || textBox6.TextLength == 0 ||
                textBox7.TextLength == 0)
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            // Validate phone number
            if (textBox3.Text.Length != 10 || !textBox3.Text.StartsWith("0") || !long.TryParse(textBox3.Text, out _))
            {
                MessageBox.Show("Invalid phone number. Phone number must start with 0 and have 10 digits.");
                return;
            }

            // Validate MSSV
            if (textBox2.Text.Length != 8 || !int.TryParse(textBox2.Text, out _))
            {
                MessageBox.Show("Invalid MSSV. MSSV must have 8 digits.");
                return;
            }

            // Validate course scores
            float course1, course2, course3;
            if (!float.TryParse(textBox4.Text, out course1) || !float.TryParse(textBox5.Text, out course2) || !float.TryParse(textBox6.Text, out course3))
            {
                MessageBox.Show("Invalid course scores. Please enter numeric values.");
                return;
            }
            if (course1 < 0 || course1 > 10 || course2 < 0 || course2 > 10 || course3 < 0 || course3 > 10)
            {
                MessageBox.Show("Invalid course scores. Course scores must be between 0 and 10.");
                return;
            }

            // Add the new student
            Array.Resize(ref students, students.Length + 1);
            students[students.Length - 1] = new Student
            {
                Name = textBox1.Text,
                MSSV = int.Parse(textBox2.Text),
                Phone = textBox3.Text,
                Course1 = float.Parse(textBox4.Text),
                Course2 = float.Parse(textBox5.Text),
                Course3 = float.Parse(textBox6.Text)
            };

            // Show the updated list of students
            ShowStudents();

            // Clear input fields
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            // Navigate to previous page of students
            if (currentPage > 0)
            {
                currentPage--;
                ShowStudents();
            }
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            // Navigate to next page of students
            if ((currentPage + 1) * pageSize < students.Length)
            {
                currentPage++;
                ShowStudents();
            }
        }
    }

    [Serializable]
    public class Student
    {
        public string Name { get; set; }
        public int MSSV { get; set; }
        public string Phone { get; set; }
        public float Course1 { get; set; }
        public float Course2 { get; set; }
        public float Course3 { get; set; }

        public float CalculateAverage()
        {
            return (Course1 + Course2 + Course3) / 3;
        }
    }
}
