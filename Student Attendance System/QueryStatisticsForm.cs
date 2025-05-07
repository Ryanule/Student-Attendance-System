using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Student_Attendance_System
{
    public partial class QueryStatisticsForm: Form
    {
        private string connectionString;
        private TextBox studentIdTextBox;
        private TextBox studentNameTextBox;
        private TextBox courseNameTextBox;
        private Button searchStudentsButton;
        private Button searchAbsenceRecordsButton;
        private Button statisticsByCourseButton;
        private Button statisticsByNameButton;
        private DataGridView resultGridView;
        public QueryStatisticsForm()
        {
            InitializeComponents();
            connectionString = MainForm.connectionString;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            // 设置窗体为可缩放
            this.FormBorderStyle = FormBorderStyle.Sizable;
        }
        private void InitializeComponents()
        {
            studentIdTextBox = new TextBox();
            studentNameTextBox = new TextBox();
            courseNameTextBox = new TextBox();
            searchStudentsButton = new Button();
            searchAbsenceRecordsButton = new Button();
            statisticsByCourseButton = new Button();
            statisticsByNameButton = new Button();
            resultGridView = new DataGridView();

            studentIdTextBox.Location = new System.Drawing.Point(12, 12);
            studentIdTextBox.Size = new System.Drawing.Size(150, 20);
            SetPlaceholderText(studentIdTextBox, "输入学号");

            studentNameTextBox.Location = new System.Drawing.Point(170, 12);
            studentNameTextBox.Size = new System.Drawing.Size(150, 20);
            SetPlaceholderText(studentNameTextBox, "输入姓名");

            courseNameTextBox.Location = new System.Drawing.Point(328, 12);
            courseNameTextBox.Size = new System.Drawing.Size(150, 20);
            SetPlaceholderText(courseNameTextBox, "输入课程名");

            // 调整按钮尺寸
            searchStudentsButton.Location = new System.Drawing.Point(12, 40);
            searchStudentsButton.Size = new System.Drawing.Size(130, 30); // 增大宽度和高度
            searchStudentsButton.Text = "查询学生记录";
            searchStudentsButton.Click += SearchStudentsButton_Click;

            searchAbsenceRecordsButton.Location = new System.Drawing.Point(150, 40);
            searchAbsenceRecordsButton.Size = new System.Drawing.Size(130, 30); // 增大宽度和高度
            searchAbsenceRecordsButton.Text = "查询缺课记录";
            searchAbsenceRecordsButton.Click += SearchAbsenceRecordsButton_Click;

            statisticsByCourseButton.Location = new System.Drawing.Point(288, 40);
            statisticsByCourseButton.Size = new System.Drawing.Size(130, 30); // 增大宽度和高度
            statisticsByCourseButton.Text = "按课程统计旷课";
            statisticsByCourseButton.Click += StatisticsByCourseButton_Click;

            statisticsByNameButton.Location = new System.Drawing.Point(426, 40);
            statisticsByNameButton.Size = new System.Drawing.Size(130, 30); // 增大宽度和高度
            statisticsByNameButton.Text = "按姓名统计旷课";
            statisticsByNameButton.Click += StatisticsByNameButton_Click;

            resultGridView.Location = new System.Drawing.Point(12, 80);
            resultGridView.Size = new System.Drawing.Size(600, 300);


            this.Controls.Add(studentIdTextBox);
            this.Controls.Add(studentNameTextBox);
            this.Controls.Add(courseNameTextBox);
            this.Controls.Add(searchStudentsButton);
            this.Controls.Add(searchAbsenceRecordsButton);
            this.Controls.Add(statisticsByCourseButton);
            this.Controls.Add(statisticsByNameButton);
            this.Controls.Add(resultGridView);

            this.ClientSize = new System.Drawing.Size(624, 392);
            this.Text = "查询统计窗口";
        }
        private void SetPlaceholderText(TextBox textBox, string placeholder)
        {
            textBox.Text = placeholder;
            textBox.ForeColor = Color.Gray;
            textBox.GotFocus += (sender, e) =>
            {
                if (textBox.Text == placeholder)
                {
                    textBox.Text = "";
                    textBox.ForeColor = SystemColors.WindowText;
                }
            };
            textBox.LostFocus += (sender, e) =>
            {
                if (string.IsNullOrEmpty(textBox.Text))
                {
                    textBox.Text = placeholder;
                    textBox.ForeColor = Color.Gray;
                }
            };
        }

        private void SearchStudentsButton_Click(object sender, EventArgs e)
        {
            string studentId = studentIdTextBox.Text.Trim();
            if (studentId == "输入学号")
            {
                studentId = "";
            }
            string studentName = studentNameTextBox.Text.Trim();
            if (studentName == "输入姓名")
            {
                studentName = "";
            }

            if (string.IsNullOrEmpty(studentId) && string.IsNullOrEmpty(studentName))
            {
                MessageBox.Show("请输入学号或姓名进行查询", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT * FROM Students WHERE 1 = 1";
                    if (!string.IsNullOrEmpty(studentId))
                    {
                        query += " AND StudentId LIKE @StudentId";
                    }
                    if (!string.IsNullOrEmpty(studentName))
                    {
                        query += " AND StudentName LIKE @StudentName";
                    }
                    query += " ORDER BY StudentId";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    if (!string.IsNullOrEmpty(studentId))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@StudentId", $"%{studentId}%");
                    }
                    if (!string.IsNullOrEmpty(studentName))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@StudentName", $"%{studentName}%");
                    }

                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    if (dataTable.Rows.Count == 0)
                    {
                        MessageBox.Show("未找到符合条件的学生记录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    resultGridView.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"查询学生记录时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SearchAbsenceRecordsButton_Click(object sender, EventArgs e)
        {
            string studentName = studentNameTextBox.Text.Trim();
            if (studentName == "输入姓名")
            {
                studentName = "";
            }
            string courseName = courseNameTextBox.Text.Trim();
            if (courseName == "输入课程名")
            {
                courseName = "";
            }

            if (string.IsNullOrEmpty(studentName) && string.IsNullOrEmpty(courseName))
            {
                MessageBox.Show("请输入学生姓名或课程名进行查询", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT * FROM AbsenceRecords WHERE 1 = 1";
                    if (!string.IsNullOrEmpty(studentName))
                    {
                        query += " AND StudentName LIKE @StudentName";
                    }
                    if (!string.IsNullOrEmpty(courseName))
                    {
                        query += " AND CourseName LIKE @CourseName";
                    }
                    query += " ORDER BY CourseName, StudentName";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    if (!string.IsNullOrEmpty(studentName))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@StudentName", $"%{studentName}%");
                    }
                    if (!string.IsNullOrEmpty(courseName))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@CourseName", $"%{courseName}%");
                    }

                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    if (dataTable.Rows.Count == 0)
                    {
                        MessageBox.Show("未找到符合条件的缺课记录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    resultGridView.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"查询缺课记录时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void StatisticsByCourseButton_Click(object sender, EventArgs e)
        {
            string courseName = courseNameTextBox.Text.Trim();
            if (courseName == "输入课程名")
            {
                courseName = "";
            }

            if (string.IsNullOrEmpty(courseName))
            {
                MessageBox.Show("请输入课程名进行统计", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT StudentName, COUNT(*) AS AbsenceCount " +
                                   "FROM AbsenceRecords " +
                                   "WHERE CourseName LIKE @CourseName AND AbsenceType = '旷课' " +
                                   "GROUP BY StudentName " +
                                   "ORDER BY AbsenceCount DESC";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    adapter.SelectCommand.Parameters.AddWithValue("@CourseName", $"%{courseName}%");

                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    if (dataTable.Rows.Count == 0)
                    {
                        MessageBox.Show("未找到该课程的旷课记录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    resultGridView.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"按课程统计旷课信息时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void StatisticsByNameButton_Click(object sender, EventArgs e)
        {
            string studentName = studentNameTextBox.Text.Trim();
            if (studentName == "输入姓名")
            {
                studentName = "";
            }

            if (string.IsNullOrEmpty(studentName))
            {
                MessageBox.Show("请输入学生姓名进行统计", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT CourseName, COUNT(*) AS AbsenceCount " +
                                   "FROM AbsenceRecords " +
                                   "WHERE StudentName LIKE @StudentName AND AbsenceType = '旷课' " +
                                   "GROUP BY CourseName " +
                                   "ORDER BY AbsenceCount DESC";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    adapter.SelectCommand.Parameters.AddWithValue("@StudentName", $"%{studentName}%");

                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    if (dataTable.Rows.Count == 0)
                    {
                        MessageBox.Show("未找到该学生的旷课记录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    resultGridView.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"按姓名统计旷课信息时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
