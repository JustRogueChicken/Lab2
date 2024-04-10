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

namespace Lab2
{
    public partial class Bai2 : Form
    {
        public Bai2()
        {
            InitializeComponent();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    try
                    {
                        string fileName = Path.GetFileName(filePath);
                        FileInfo fileInfo = new FileInfo(filePath);
                        long fileSize = fileInfo.Length;

                        string fileContent;
                        using (StreamReader reader = new StreamReader(filePath))
                        {
                            fileContent = reader.ReadToEnd();
                        }

                        int lineCount = File.ReadAllLines(filePath).Length;
                        int wordCount = fileContent.Split(new char[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length;
                        int charCount = fileContent.Length;

                        textBox1.Text = fileName;
                        textBox2.Text = fileSize.ToString() + " bytes";
                        textBox3.Text = filePath;
                        textBox4.Text = lineCount.ToString();
                        textBox5.Text = wordCount.ToString();
                        textBox6.Text = charCount.ToString();

                        richTextBox1.Text = fileContent;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Đã xảy ra lỗi khi đọc file: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close(); ;
        }

        
    }
} 
    

