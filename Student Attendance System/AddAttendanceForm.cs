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
    public partial class AddAttendanceForm : Form
    {
        private DateTimePicker dtpAbsenceDate;
        private TextBox txtClassPeriod;
        private TextBox txtCourseName;
        private TextBox txtStudentId;
        private TextBox txtStudentName; // 新增学生姓名文本框
        private ComboBox cmbAbsenceType;
        private Button btnOK;
        private Button btnCancel;
        private string connectionString;

        public AddAttendanceForm(string connectionString)
        {
            this.connectionString = connectionString;
            InitializeComponents();
            this.BackgroundImageLayout = ImageLayout.Stretch;
            // 设置窗体为可缩放
            this.FormBorderStyle = FormBorderStyle.Sizable;
        }

        private void InitializeComponents()
        {
            dtpAbsenceDate = new DateTimePicker();
            txtClassPeriod = new TextBox();
            txtCourseName = new TextBox();
            txtStudentId = new TextBox();
            txtStudentName = new TextBox(); // 新增学生姓名文本框
            cmbAbsenceType = new ComboBox();
            btnOK = new Button();
            btnCancel = new Button();

            dtpAbsenceDate.Location = new System.Drawing.Point(120, 20);
            txtClassPeriod.Location = new System.Drawing.Point(120, 60);
            txtCourseName.Location = new System.Drawing.Point(120, 100);
            txtStudentId.Location = new System.Drawing.Point(120, 140);
            txtStudentName.Location = new System.Drawing.Point(120, 180); // 新增学生姓名文本框位置
            cmbAbsenceType.Location = new System.Drawing.Point(120, 220);

            cmbAbsenceType.Items.Add("迟到");
            cmbAbsenceType.Items.Add("早退");
            cmbAbsenceType.Items.Add("旷课");
            cmbAbsenceType.SelectedIndex = 0;

            btnOK.Location = new System.Drawing.Point(80, 260);
            btnOK.Text = "确定";
            btnOK.Click += btnOK_Click;

            btnCancel.Location = new System.Drawing.Point(180, 260);
            btnCancel.Text = "取消";
            btnCancel.Click += (sender, e) => { this.Close(); };

            Label lblAbsenceDate = new Label { Text = "缺勤日期:", Location = new System.Drawing.Point(20, 20) };
            Label lblClassPeriod = new Label { Text = "课程节次:", Location = new System.Drawing.Point(20, 60) };
            Label lblCourseName = new Label { Text = "课程名称:", Location = new System.Drawing.Point(20, 100) };
            Label lblStudentId = new Label { Text = "学生 ID:", Location = new System.Drawing.Point(20, 140) };
            Label lblStudentName = new Label { Text = "学生姓名:", Location = new System.Drawing.Point(20, 180) }; // 新增学生姓名标签
            Label lblAbsenceType = new Label { Text = "缺勤类型:", Location = new System.Drawing.Point(20, 220) };

            this.Controls.Add(lblAbsenceDate);
            this.Controls.Add(lblClassPeriod);
            this.Controls.Add(lblCourseName);
            this.Controls.Add(lblStudentId);
            this.Controls.Add(lblStudentName); // 新增学生姓名标签到控件集合
            this.Controls.Add(lblAbsenceType);
            this.Controls.Add(dtpAbsenceDate);
            this.Controls.Add(txtClassPeriod);
            this.Controls.Add(txtCourseName);
            this.Controls.Add(txtStudentId);
            this.Controls.Add(txtStudentName); // 新增学生姓名文本框到控件集合
            this.Controls.Add(cmbAbsenceType);
            this.Controls.Add(btnOK);
            this.Controls.Add(btnCancel);

            this.Size = new System.Drawing.Size(300, 350);
            this.Text = "添加考勤记录";
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime absenceDate = dtpAbsenceDate.Value;
                int classPeriod = int.Parse(txtClassPeriod.Text);
                string courseName = txtCourseName.Text;
                string studentId = txtStudentId.Text;
                string studentName = txtStudentName.Text; // 获取学生姓名
                string absenceType = cmbAbsenceType.SelectedItem.ToString();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO AbsenceRecords (AbsenceDate, ClassPeriod, CourseName, StudentId, StudentName, AbsenceType) " +
                                   "VALUES (@AbsenceDate, @ClassPeriod, @CourseName, @StudentId, @StudentName, @AbsenceType)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@AbsenceDate", absenceDate);
                    command.Parameters.AddWithValue("@ClassPeriod", classPeriod);
                    command.Parameters.AddWithValue("@CourseName", courseName);
                    command.Parameters.AddWithValue("@StudentId", studentId);
                    command.Parameters.AddWithValue("@StudentName", studentName); // 添加学生姓名参数
                    command.Parameters.AddWithValue("@AbsenceType", absenceType);
                    command.ExecuteNonQuery();
                }

                MessageBox.Show("考勤记录添加成功", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"添加考勤记录时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
