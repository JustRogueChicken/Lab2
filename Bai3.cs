using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Lab2
{
    public partial class Bai3 : Form
    {
        public Bai3()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string inputFilePath = openFileDialog.FileName;

                    if (!File.Exists(inputFilePath))
                    {
                        MessageBox.Show("File không tồn tại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    richTextBox1.Text = File.ReadAllText(inputFilePath);
                    MessageBox.Show("Đã đọc nội dung thành công!", "Thông báo", MessageBoxButtons.OK);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string outputFilePath = saveFileDialog.FileName;
                    string[] lines = richTextBox1.Lines;
                    StringBuilder resultBuilder = new StringBuilder();

                    foreach (string line in lines)
                    {
                        try
                        {
                            double expressionResult = Solve(line);
                            resultBuilder.AppendLine($"{line} = {expressionResult}");
                        }
                        catch (FormatException ex)
                        {
                            MessageBox.Show($"Biểu thức không hợp lệ: {line}\n{ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    string result = resultBuilder.ToString();
                    File.WriteAllText(outputFilePath, result);
                    richTextBox1.Text = result;
                    MessageBox.Show("Đã ghi kết quả xuống file output3.txt.", "Thông báo", MessageBoxButtons.OK);
                }
            }
        }

        private string RemoveExtraSpaces(string input)
        {
            return Regex.Replace(input, @"\s+", "");
        }

        private bool IsValidExpression(string input)
        {
            return Regex.IsMatch(input, @"^[0-9\.+\-*\/]+$") && !Regex.IsMatch(input, @"\.\.+");
        }

        private double Solve(string expression)
        {
            // Kiểm tra tính hợp lệ của biểu thức
            if (!IsValidExpression(expression))
            {
                throw new FormatException("Biểu thức không hợp lệ.");
            }

            // Xóa các khoảng trắng thừa
            expression = RemoveExtraSpaces(expression);

            // Tách các phần tử trong biểu thức
            string[] elements = Regex.Split(expression, @"([+\-*\/])");
            double result = double.Parse(elements[0]);

            // Thực hiện các phép toán
            for (int i = 1; i < elements.Length; i += 2)
            {
                string operation = elements[i];
                double operand = double.Parse(elements[i + 1]);

                switch (operation)
                {
                    case "+":
                        result += operand;
                        break;
                    case "-":
                        result -= operand;
                        break;
                    case "*":
                        result *= operand;
                        break;
                    case "/":
                        if (operand == 0)
                        {
                            throw new DivideByZeroException("Không thể chia cho 0.");
                        }
                        result /= operand;
                        break;
                }
            }
            return result;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}