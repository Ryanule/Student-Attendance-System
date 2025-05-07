using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Student_Attendance_System
{
    public partial class LoginForm: Form
    {
        private string connectionString = "Data Source=CHINAMI-0UB312L;Initial Catalog=StudentAttendanceDB;Integrated Security=True;";
        public LoginForm()
        {
            InitializeComponent();
            this.BackgroundImageLayout = ImageLayout.Stretch;
            // 设置窗体为可缩放
            this.FormBorderStyle = FormBorderStyle.Sizable;

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM Users WHERE Username = @Username AND Password = @Password";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);
                    int count = (int)command.ExecuteScalar();

                    if (count > 0)
                    {
                        MainForm mainForm = new MainForm();
                        mainForm.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("用户名或密码错误！");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("登录时出错：" + ex.Message);
                }
            }
        }

        private void 登录窗口_Load(object sender, EventArgs e)
        {

        }
    }
}
