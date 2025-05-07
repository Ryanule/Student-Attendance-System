using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Student_Attendance_System
{

    public partial class DataMaintenanceForm : Form
    {
        private string connectionString;
        private Button saveToFileButton;
        private Button loadFromFileButton;
        private ComboBox recordTypeComboBox;
        public DataMaintenanceForm()
        {
            InitializeComponents();
            connectionString = MainForm.connectionString;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            // 设置窗体为可缩放
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.BackgroundImage = Properties.Resources.background1;
        }
        private void InitializeComponents()
        {
            saveToFileButton = new Button();
            loadFromFileButton = new Button();
            recordTypeComboBox = new ComboBox();

            recordTypeComboBox.Location = new System.Drawing.Point(20, 20);
            recordTypeComboBox.Size = new System.Drawing.Size(150, 21);
            recordTypeComboBox.Items.Add("学生信息");
            recordTypeComboBox.Items.Add("考勤信息");
            recordTypeComboBox.SelectedIndex = 0;

            saveToFileButton.Location = new System.Drawing.Point(200, 20);
            saveToFileButton.Size = new System.Drawing.Size(150, 30);
            saveToFileButton.Text = "保存记录到文件";
            saveToFileButton.Click += SaveToFileButton_Click;

            loadFromFileButton.Location = new System.Drawing.Point(200, 60);
            loadFromFileButton.Size = new System.Drawing.Size(150, 30);
            loadFromFileButton.Text = "从文件加载记录";
            loadFromFileButton.Click += LoadFromFileButton_Click;

            this.Controls.Add(recordTypeComboBox);
            this.Controls.Add(saveToFileButton);
            this.Controls.Add(loadFromFileButton);

            this.ClientSize = new System.Drawing.Size(380, 120);
            this.Text = "数据维护窗口";
        }
        private void SaveToFileButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV 文件 (*.csv)|*.csv";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string tableName = GetSelectedTableName();
                    string recordTypeName = recordTypeComboBox.SelectedItem.ToString();

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = $"SELECT * FROM {tableName}";
                        SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName, false, Encoding.UTF8))
                        {
                            // 写入列名
                            for (int i = 0; i < dataTable.Columns.Count; i++)
                            {
                                writer.Write(dataTable.Columns[i].ColumnName);
                                if (i < dataTable.Columns.Count - 1)
                                {
                                    writer.Write(",");
                                }
                            }
                            writer.WriteLine();

                            // 写入数据行
                            foreach (DataRow row in dataTable.Rows)
                            {
                                for (int i = 0; i < dataTable.Columns.Count; i++)
                                {
                                    writer.Write(row[i].ToString());
                                    if (i < dataTable.Columns.Count - 1)
                                    {
                                        writer.Write(",");
                                    }
                                }
                                writer.WriteLine();
                            }
                        }

                        MessageBox.Show($"{recordTypeName} 已成功保存到文件。", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"保存 {recordTypeComboBox.SelectedItem.ToString()} 到文件时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LoadFromFileButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV 文件 (*.csv)|*.csv";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string tableName = GetSelectedTableName();
                    string recordTypeName = recordTypeComboBox.SelectedItem.ToString();

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        DataTable dataTable = new DataTable();
                        using (StreamReader reader = new StreamReader(openFileDialog.FileName, Encoding.UTF8))
                        {
                            string[] headers = reader.ReadLine().Split(',');
                            foreach (string header in headers)
                            {
                                dataTable.Columns.Add(header);
                            }

                            while (!reader.EndOfStream)
                            {
                                string[] values = reader.ReadLine().Split(',');
                                DataRow row = dataTable.NewRow();
                                for (int i = 0; i < headers.Length; i++)
                                {
                                    row[i] = values[i];
                                }
                                dataTable.Rows.Add(row);
                            }
                        }

                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                        {
                            bulkCopy.DestinationTableName = tableName;
                            bulkCopy.WriteToServer(dataTable);
                        }

                        MessageBox.Show($"{recordTypeName} 已成功从文件加载到数据库。", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"从文件加载 {recordTypeComboBox.SelectedItem.ToString()} 到数据库时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private string GetSelectedTableName()
        {
            if (recordTypeComboBox.SelectedItem.ToString() == "学生信息")
            {
                return "Students";
            }
            else
            {
                return "AbsenceRecords";
            }
        }
    }
}
