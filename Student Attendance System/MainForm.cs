using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Student_Attendance_System
{
    public partial class MainForm: Form
    {
        public static string connectionString = "Data Source=CHINAMI-0UB312L;Initial Catalog=StudentAttendanceDB;Integrated Security=True;";
        public MainForm()
        {
            InitializeComponent();
            this.BackgroundImageLayout = ImageLayout.Stretch;
            // 设置窗体为可缩放
            this.FormBorderStyle = FormBorderStyle.Sizable;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            new StudentManagementForm().Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new AttendanceManagementForm().Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new QueryStatisticsForm().Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            new DataMaintenanceForm().Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            new SystemSettingsForm().Show();
        }
    }

}
