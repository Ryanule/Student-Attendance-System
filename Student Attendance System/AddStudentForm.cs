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
    public partial class AddStudentForm: Form
    {
        private string connectionString;


        public AddStudentForm(string connectionString)
        {
            this.connectionString = connectionString;
            InitializeComponents();
            this.BackgroundImageLayout = ImageLayout.Stretch;
            // 设置窗体为可缩放
            this.FormBorderStyle = FormBorderStyle.Sizable;
        }
        private void InitializeComponents()
        {
            txtStudentID = new TextBox();
            txtName = new TextBox();
            txtGender = new TextBox();
            txtAge = new TextBox();
            txtClass = new TextBox();
            btnOK = new Button();
            btnCancel = new Button();

            txtStudentID.Location = new System.Drawing.Point(120, 20);
            txtName.Location = new System.Drawing.Point(120, 60);
            txtGender.Location = new System.Drawing.Point(120, 100);
            txtAge.Location = new System.Drawing.Point(120, 140);
            txtClass.Location = new System.Drawing.Point(120, 180);

            btnOK.Location = new System.Drawing.Point(80, 220);
            btnOK.Text = "确定";
            btnOK.Click += btnOK_Click;

            btnCancel.Location = new System.Drawing.Point(180, 220);
            btnCancel.Text = "取消";
            btnCancel.Click += (sender, e) => { this.Close(); };

            Label lblStudentID = new Label { Text = "学号:", Location = new System.Drawing.Point(20, 20) };
            Label lblName = new Label { Text = "姓名:", Location = new System.Drawing.Point(20, 60) };
            Label lblGender = new Label { Text = "性别:", Location = new System.Drawing.Point(20, 100) };
            Label lblAge = new Label { Text = "年龄:", Location = new System.Drawing.Point(20, 140) };
            Label lblClass = new Label { Text = "班级:", Location = new System.Drawing.Point(20, 180) };

            this.Controls.Add(lblStudentID);
            this.Controls.Add(lblName);
            this.Controls.Add(lblGender);
            this.Controls.Add(lblAge);
            this.Controls.Add(lblClass);
            this.Controls.Add(txtStudentID);
            this.Controls.Add(txtName);
            this.Controls.Add(txtGender);
            this.Controls.Add(txtAge);
            this.Controls.Add(txtClass);
            this.Controls.Add(btnOK);
            this.Controls.Add(btnCancel);

            this.Size = new System.Drawing.Size(300, 300);
            this.Text = "添加学生信息";
        }

        private void StudentInfoDialog_Load(object sender, EventArgs e)
        {

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string studentID = txtStudentID.Text;
            string name = txtName.Text;
            string gender = txtGender.Text;
            if (!int.TryParse(txtAge.Text, out int age) || age < 0)
            {
                MessageBox.Show("年龄必须为有效的正整数，请重新输入！");
                return;
            }
            string classInfo = txtClass.Text;

            if (string.IsNullOrEmpty(studentID) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(gender) || string.IsNullOrEmpty(classInfo))
            {
                MessageBox.Show("所有字段都不能为空，请重新输入！");
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string checkQuery = "SELECT COUNT(*) FROM Students WHERE StudentID = @StudentID";
                    SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
                    checkCommand.Parameters.AddWithValue("@StudentID", studentID);
                    int count = (int)checkCommand.ExecuteScalar();

                    if (count > 0)
                    {
                        MessageBox.Show("数据添加重复，取消添加！");
                        return;
                    }

                    string insertQuery = "INSERT INTO Students (StudentID, Name, Gender, Age, Class) VALUES (@StudentID, @Name, @Gender, @Age, @Class)";
                    SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                    insertCommand.Parameters.AddWithValue("@StudentID", studentID);
                    insertCommand.Parameters.AddWithValue("@Name", name);
                    insertCommand.Parameters.AddWithValue("@Gender", gender);
                    insertCommand.Parameters.AddWithValue("@Age", age);
                    insertCommand.Parameters.AddWithValue("@Class", classInfo);
                    insertCommand.ExecuteNonQuery();
                    MessageBox.Show("学生记录添加成功！");
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("添加学生记录时出错：" + ex.Message);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
