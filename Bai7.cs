﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Lab2
{
    public partial class Bai7 : Form
    {
        public Bai7()
        {
            InitializeComponent();

            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo drive in drives)
            {
                TreeNode node = new TreeNode(drive.Name);
                node.Tag = drive.RootDirectory;
                treeView1.Nodes.Add(node);
                Load(node);
            }
        }

        private void Load(TreeNode parentNode)
        {
            DirectoryInfo[] directories = ((DirectoryInfo)parentNode.Tag).GetDirectories();
            AddNodes(parentNode, directories); 

            FileInfo[] files = ((DirectoryInfo)parentNode.Tag).GetFiles();
            AddNodes(parentNode, files); 
        }

        private void AddNodes(TreeNode parentNode, FileSystemInfo[] items)
        {
            foreach (var item in items)
            {
                TreeNode node;
                if (item is DirectoryInfo directory)
                {
                    node = new TreeNode(directory.Name);
                    node.Tag = directory;
                    node.Nodes.Add(""); 
                }
                else
                {
                    node = new TreeNode(item.Name);
                    node.Tag = item;
                }
                parentNode.Nodes.Add(node);
            }
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag is DirectoryInfo directory)
            {
                
                DirectoryInfo[] subDirectories = directory.GetDirectories();
                AddNodes(e.Node, subDirectories); 

                FileInfo[] files = directory.GetFiles();
                AddNodes(e.Node, files); 
            }
            else if (e.Node.Tag is FileInfo file)
            {
            
                if (IsImage(file.FullName))
                {
                    DisplayImageContent(file.FullName);
                }
                else
                {
                    DisplayTextContent(file.FullName);
                }
            }
        }
        private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Nodes.Count == 1 && e.Node.Nodes[0].Text == "")
            {
                e.Node.Nodes.Clear();
                Load(e.Node);
            }
        }
        private bool IsImage(string filePath)
        {
            string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
            string extension = Path.GetExtension(filePath);
            return imageExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase);
        }
        private void DisplayImageContent(string imagePath)
        {
            try
            {
                Image image = Image.FromFile(imagePath);
                pictureBox1.Image = image;
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            }
            catch
            {
                MessageBox.Show("Xảy ra lỗi!");
            }
        }

        private void DisplayTextContent(string filePath)
        {
            try
            {
                string content = File.ReadAllText(filePath);
                richTextBox1.Text = content;           
            }
            catch
            {
                MessageBox.Show("Xảy ra lỗi!");
            }
        }

         
    }
}