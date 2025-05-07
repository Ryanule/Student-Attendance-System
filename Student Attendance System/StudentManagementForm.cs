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
    public partial class StudentManagementForm: Form
    {
        private string connectionString;
        public StudentManagementForm()
        {
            InitializeComponent();
            // 从主程序中获取数据库连接字符串
            connectionString = MainForm.connectionString;
            // 调用加载学生数据的方法
            LoadStudentData();
            this.BackgroundImageLayout = ImageLayout.Stretch;
            // 设置窗体为可缩放
            this.FormBorderStyle = FormBorderStyle.Sizable;
        }
        private void LoadStudentData()
        {
            try
            {
                // 使用 using 语句确保 SqlConnection 对象在使用完毕后正确释放资源
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // 打开数据库连接
                    connection.Open();
                    // 定义 SQL 查询语句，用于查询所有学生记录
                    string query = "SELECT * FROM Students";
                    // 创建 SqlDataAdapter 对象，用于执行 SQL 查询并填充 DataTable
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    // 创建 DataTable 对象，用于存储查询结果
                    DataTable dataTable = new DataTable();
                    // 使用 SqlDataAdapter 填充 DataTable
                    adapter.Fill(dataTable);
                    // 将 DataTable 中的数据绑定到 DataGridView 控件上，显示学生记录
                    dataGridView1.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                // 若加载学生数据时出现异常，弹出消息框显示错误信息
                MessageBox.Show("加载学生数据时出错：" + ex.Message);
            }
        }

        private void StudentManagementForm_Load(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddStudentForm addForm = new AddStudentForm(connectionString);
            addForm.ShowDialog();
            LoadStudentData();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string studentID = dataGridView1.SelectedRows[0].Cells["StudentID"].Value.ToString();
                EditStudentForm editForm = new EditStudentForm(connectionString, studentID);
                editForm.ShowDialog();
                LoadStudentData();
            }
            else
            {
                MessageBox.Show("请选择要修改的学生记录！");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string studentID = dataGridView1.SelectedRows[0].Cells["StudentID"].Value.ToString();
                DeleteStudentForm deleteForm = new DeleteStudentForm(connectionString, studentID);
                deleteForm.ShowDialog();
                LoadStudentData();
            }
            else
            {
                MessageBox.Show("请选择要删除的学生记录！");
            }
        }
    }
}