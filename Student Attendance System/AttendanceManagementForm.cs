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
    public partial class AttendanceManagementForm: Form
    {
        private string connectionString;
        public AttendanceManagementForm()
        {
            InitializeComponent();
            connectionString = MainForm.connectionString;
            LoadAttendanceData();
            this.BackgroundImageLayout = ImageLayout.Stretch;
            // 设置窗体为可缩放
            this.FormBorderStyle = FormBorderStyle.Sizable;
        }
        private void LoadAttendanceData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM AbsenceRecords";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载考勤记录时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void AttendanceManagementForm_Load(object sender, EventArgs e)
        {

        }

        private void btnAddAttendance_Click_1(object sender, EventArgs e)
        {
            AddAttendanceForm addForm = new AddAttendanceForm(connectionString);
            addForm.ShowDialog();
            LoadAttendanceData();
        }

        private void btnDeleteRecord_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int recordId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["RecordID"].Value);
                if (MessageBox.Show("确定要删除这条考勤记录吗？", "确认删除", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            string query = "DELETE FROM AbsenceRecords WHERE RecordID = @RecordID";
                            SqlCommand command = new SqlCommand(query, connection);
                            command.Parameters.AddWithValue("@RecordID", recordId);
                            command.ExecuteNonQuery();
                        }
                        MessageBox.Show("考勤记录删除成功", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadAttendanceData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"删除考勤记录时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("请选择要删除的考勤记录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
