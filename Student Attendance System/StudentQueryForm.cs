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
    public partial class StudentQueryForm: Form
    {
        private string connectionString;
        private string username;

        // 控件定义
        private DataGridView dataGridView1;
        private Label lblTitle;
        private Button btnRefresh;
        private Button btnClose;
        public StudentQueryForm(string username)
        {
            InitializeComponents();
            this.username = username;
            connectionString = MainForm.connectionString;
            LoadAbsenceRecords();

            // 设置窗口样式
            this.Text = $"学生缺课记录查询 - {username}";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ClientSize = new Size(800, 479);
            this.BackColor = Color.White;
        }
        private void InitializeComponents()
        {
            this.dataGridView1 = new DataGridView();
            this.lblTitle = new Label();
            this.btnRefresh = new Button();
            this.btnClose = new Button();

            // 数据表格设置
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new Point(12, 45);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new Size(776, 393);
            this.dataGridView1.TabIndex = 0;

            // 标题标签设置
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new Font("微软雅黑", 12F, FontStyle.Bold, GraphicsUnit.Point, 134);
            this.lblTitle.Location = new Point(327, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new Size(131, 22);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "学生缺课记录查询";

            // 刷新按钮设置
            this.btnRefresh.Location = new Point(632, 444);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new Size(75, 23);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.Text = "刷新";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new EventHandler(this.btnRefresh_Click);

            // 关闭按钮设置
            this.btnClose.Location = new Point(713, 444);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new Size(75, 23);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new EventHandler(this.btnClose_Click);

            // 窗体设置
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.dataGridView1);
            this.Name = "StudentQueryForm";
        }
        private void LoadAbsenceRecords()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // 查询当前学生的ID
                    string studentIdQuery = "SELECT StudentID FROM Users WHERE Username = @Username AND Role = 'Student'";
                    SqlCommand studentIdCommand = new SqlCommand(studentIdQuery, connection);
                    studentIdCommand.Parameters.AddWithValue("@Username", username);

                    connection.Open();
                    object studentIdObj = studentIdCommand.ExecuteScalar();

                    if (studentIdObj == null || studentIdObj == DBNull.Value)
                    {
                        MessageBox.Show("未找到当前学生的ID信息", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string studentId = studentIdObj.ToString();

                    // 使用学生ID查询缺课记录
                    string query = @"
                        SELECT 
                            a.CourseName AS 课程名称,
                            a.AbsenceDate AS 缺课日期,
                            a.AbsenceType AS 缺课原因
                        FROM 
                            AbsenceRecords a
                        WHERE 
                            a.StudentID = @StudentID
                        ORDER BY 
                            a.AbsenceDate DESC";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    adapter.SelectCommand.Parameters.AddWithValue("@StudentID", studentId);

                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    if (dataTable.Rows.Count == 0)
                    {
                        MessageBox.Show("未找到您的缺课记录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    dataGridView1.DataSource = dataTable;

                    // 设置列样式
                    if (dataGridView1.Columns.Count > 0)
                    {
                        foreach (DataGridViewColumn column in dataGridView1.Columns)
                        {
                            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        }

                        // 设置日期列格式
                        if (dataGridView1.Columns.Contains("缺课日期"))
                        {
                            dataGridView1.Columns["缺课日期"].DefaultCellStyle.Format = "yyyy-MM-dd";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"查询缺课记录时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadAbsenceRecords();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
