﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace CountLinesOfCode
{
    public partial class formTool : Form
    {
        public formTool()
        {
            InitializeComponent();
        }

        //Select File
        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            string fileName = openFileDialog1.FileName;
            txtLinkFile.Text = fileName.ToString();
        }

        //Check xem từng dòng có phải là dòng chứa function
        //name hay không
        public bool IsFunction(string line)
        {
            if (line.Contains("//") || line.Contains("/*") || line.Contains("*/"))
                return false;

            var lines = line.Split(' ');
            var types = new List<string>
            {
                "void", "int", "long", "float", "short", "char", "double", "bool",
                "public", "private", "static", "internal", "protected", "protected internal"
            };

            if (lines.Any(l => types.Contains(l)) && line.EndsWith(")"))
            {
                if (!line.Contains(";"))
                {
                    return true;
                }
            }

            return false;
        }

        //kiểm tra xem số lượng dòng code của mỗi function.
        public int CountLineOfCodeFunction(string functionName, string[] data)
        {
            int count = 0;
            int start = 0, end = 0;

            foreach (var d in data)
            {
                if (d == functionName)
                {
                    count++;
                    continue;
                }

                if (count > 0)
                {

                    if (d.Contains("{"))
                    {
                        start++;
                        count++;
                        continue;
                    }

                    if (d.Contains("}"))
                    {
                        end++;
                        count++;
                        continue;
                    }

                    if (start == end)
                        break;

                    if (d != null)
                        count++;
                }


            }

            return count;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var path = txtLinkFile.Text; // Get đường dẫn file từ text box 1

            int numberOfFunction = 0; // Đếm số lượng function trong file
            int lineOfCode = 0; // Đếm số dòng code trong file

            var data = File.ReadAllLines(path); // Đọc toàn bộ dòng trong file, trả về string[]
            Dictionary<string, int> result = new Dictionary<string, int>(); // Lưu cặp giá trị: string (line function name), int (số lines code của function)

            foreach (var line in data)
            {
                if (IsFunction(line)) // == true: đây là line function name
                {
                    numberOfFunction++; // Tăng biến đếm lên 1.
                    int lineCode = CountLineOfCodeFunction(line, data); // Đếm số dòng code của function
                    result.Add(line, lineCode); // Add cặp giá trị: functionName, lineCode
                }

                lineOfCode++; // Tăng biến đếm tổng line code lên 1
            }

            txt_NumberOfFunction.Text = numberOfFunction.ToString(); // Hiển thị số lượng function ra text box 3
            txt_LineOfCode.Text = lineOfCode.ToString(); // Hiển thị tổng số line code ra text box 2

            // In dữ liệu ra list view với 2 columns
            listView1.View = View.Details;
            listView1.Columns.Add("Function Name", 500); // Add column 1 với name và độ rộng
            listView1.Columns.Add("Line Of Code", 100); // Add column 2 với name và độ rộng

            // Chèn dữ liệu vào list view
            foreach (var item in result)
            {
                // Add từng mảng giá trị tương ứng
                listView1.Items.Add(new ListViewItem(new string[] { item.Key, item.Value.ToString() }));
            }

            listView1.GridLines = true;
        }

        private void btn_Thoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btn_Reset_Click(object sender, EventArgs e)
        {
            txtLinkFile.Text = "";
            listView1.Items.Clear();
        }

    }
}
