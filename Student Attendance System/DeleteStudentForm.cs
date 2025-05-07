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
    public partial class DeleteStudentForm: Form
    {
        private string connectionString;
        private string studentID;
        private Label lblConfirm;
        private Button btnYes;
        private Button btnNo;
        public DeleteStudentForm(string connectionString, string studentID)
        {
            this.connectionString = connectionString;
            this.studentID = studentID;
            InitializeComponents();
            this.BackgroundImageLayout = ImageLayout.Stretch;
            // 设置窗体为可缩放
            this.FormBorderStyle = FormBorderStyle.Sizable;
        }
        private void InitializeComponents()
        {
            lblConfirm = new Label { Text = $"确定要删除学号为 {studentID} 的学生记录吗？", Location = new System.Drawing.Point(20, 20) };
            btnYes = new Button { Text = "是", Location = new System.Drawing.Point(80, 60) };
            btnYes.Click += btnYes_Click;
            btnNo = new Button { Text = "否", Location = new System.Drawing.Point(180, 60) };
            btnNo.Click += (sender, e) => { this.Close(); };

            this.Controls.Add(lblConfirm);
            this.Controls.Add(btnYes);
            this.Controls.Add(btnNo);

            this.Size = new System.Drawing.Size(300, 150);
            this.Text = "确认删除";
        }
        private void DeleteStudentForm_Load(object sender, EventArgs e)
        {

        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string deleteQuery = "DELETE FROM Students WHERE StudentID = @StudentID";
                    SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection);
                    deleteCommand.Parameters.AddWithValue("@StudentID", studentID);
                    deleteCommand.ExecuteNonQuery();
                    MessageBox.Show("学生记录删除成功！");
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("删除学生记录时出错：" + ex.Message);
                }
            }
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
