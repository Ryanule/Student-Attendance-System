using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace Student_Attendance_System
{
    public partial class UserManagementForm: Form
    {
        private string connectionString;
        private List<Form> controlledForms;

        // 控件定义
        private DataGridView dataGridView1;
        private Label lblTitle;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private ComboBox cmbRole;
        private Label lblUsername;
        private Label lblPassword;
        private Label lblRole;
        private Button btnAddUser;
        private Button btnDeleteUser;
        private Button btnClose;
        private ErrorProvider errorProvider1;
        private TextBox txtStudentID;  
        private Label lblStudentID;   
        public UserManagementForm(List<Form> forms)
        {
            InitializeComponents();
            this.controlledForms = forms;
            connectionString = MainForm.connectionString;
            LoadUsers();

            // 设置窗体样式
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ClientSize = new Size(800, 450);
            this.Text = "用户管理系统";
            this.BackColor = Color.White;

            // 初始化角色下拉框
            cmbRole.SelectedIndex = 0;
        }
        private void InitializeComponents()
        {
            this.components = new System.ComponentModel.Container();
            this.dataGridView1 = new DataGridView();
            this.lblTitle = new Label();
            this.txtUsername = new TextBox();
            this.txtPassword = new TextBox();
            this.cmbRole = new ComboBox();
            this.lblUsername = new Label();
            this.lblPassword = new Label();
            this.lblRole = new Label();
            this.btnAddUser = new Button();
            this.btnDeleteUser = new Button();
            this.btnClose = new Button();
            this.errorProvider1 = new ErrorProvider(this.components);
            this.txtStudentID = new TextBox();  // 新增：初始化学生ID文本框
            this.lblStudentID = new Label();    // 新增：初始化学生ID标签

            // 初始化控件
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();

            // dataGridView1 设置
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new Point(12, 102);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new Size(776, 286);
            this.dataGridView1.TabIndex = 0;

            // lblTitle 设置
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new Font("微软雅黑", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 134);
            this.lblTitle.Location = new Point(337, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new Size(125, 28);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "用户管理系统";

            // txtUsername 设置
            this.txtUsername.Location = new Point(100, 48);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new Size(160, 21);
            this.txtUsername.TabIndex = 2;

            // txtPassword 设置
            this.txtPassword.Location = new Point(332, 48);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new Size(160, 21);
            this.txtPassword.TabIndex = 3;
            this.txtPassword.UseSystemPasswordChar = true;

            // cmbRole 设置
            this.cmbRole.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbRole.FormattingEnabled = true;
            this.cmbRole.Items.AddRange(new object[] { "Admin", "Student" });
            this.cmbRole.Location = new Point(564, 48);
            this.cmbRole.Name = "cmbRole";
            this.cmbRole.Size = new Size(121, 20);
            this.cmbRole.TabIndex = 4;

            // lblUsername 设置
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new Point(44, 51);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new Size(53, 12);
            this.lblUsername.TabIndex = 5;
            this.lblUsername.Text = "用户名：";

            // lblPassword 设置
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new Point(276, 51);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new Size(53, 12);
            this.lblPassword.TabIndex = 6;
            this.lblPassword.Text = "密码：";

            // lblRole 设置
            this.lblRole.AutoSize = true;
            this.lblRole.Location = new Point(518, 51);
            this.lblRole.Name = "lblRole";
            this.lblRole.Size = new Size(53, 12);
            this.lblRole.TabIndex = 7;
            this.lblRole.Text = "角色：";

            // txtStudentID 设置  // 新增
            this.txtStudentID.Location = new Point(100, 75);
            this.txtStudentID.Name = "txtStudentID";
            this.txtStudentID.Size = new Size(160, 21);
            this.txtStudentID.TabIndex = 8;

            // lblStudentID 设置  // 新增
            this.lblStudentID.AutoSize = true;
            this.lblStudentID.Location = new Point(44, 78);
            this.lblStudentID.Name = "lblStudentID";
            this.lblStudentID.Size = new Size(53, 12);
            this.lblStudentID.TabIndex = 9;
            this.lblStudentID.Text = "学生ID：";

            // btnAddUser 设置
            this.btnAddUser.Location = new Point(12, 402);
            this.btnAddUser.Name = "btnAddUser";
            this.btnAddUser.Size = new Size(120, 30);
            this.btnAddUser.TabIndex = 10;
            this.btnAddUser.Text = "添加用户";
            this.btnAddUser.UseVisualStyleBackColor = true;
            this.btnAddUser.Click += new EventHandler(this.btnAddUser_Click);

            // btnDeleteUser 设置
            this.btnDeleteUser.Location = new Point(148, 402);
            this.btnDeleteUser.Name = "btnDeleteUser";
            this.btnDeleteUser.Size = new Size(120, 30);
            this.btnDeleteUser.TabIndex = 11;
            this.btnDeleteUser.Text = "删除用户";
            this.btnDeleteUser.UseVisualStyleBackColor = true;
            this.btnDeleteUser.Click += new EventHandler(this.btnDeleteUser_Click);

            // btnClose 设置
            this.btnClose.Location = new Point(668, 402);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new Size(120, 30);
            this.btnClose.TabIndex = 12;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new EventHandler(this.btnClose_Click);

            // errorProvider1 设置
            this.errorProvider1.ContainerControl = this;

            // UserManagementForm 设置
            this.Controls.Add(this.lblStudentID);
            this.Controls.Add(this.txtStudentID);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnDeleteUser);
            this.Controls.Add(this.btnAddUser);
            this.Controls.Add(this.lblRole);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.cmbRole);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.dataGridView1);
            this.Name = "UserManagementForm";

            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private void LoadUsers()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT UserID AS 用户ID, Username AS 用户名, Role AS 角色 FROM Users";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);

                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    dataGridView1.DataSource = dataTable;

                    // 设置表格样式
                    if (dataGridView1.Columns.Count > 0)
                    {
                        dataGridView1.Columns[0].Width = 80; // 用户ID列宽度
                        dataGridView1.Columns[1].Width = 150; // 用户名列宽度
                        dataGridView1.Columns[2].Width = 100; // 角色列宽度

                        // 设置列标题样式
                        dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("微软雅黑", 9F, FontStyle.Bold);
                        dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;

                        // 设置单元格样式
                        dataGridView1.DefaultCellStyle.Font = new Font("微软雅黑", 9F);
                        dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载用户信息时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            // 输入验证
            if (string.IsNullOrEmpty(txtUsername.Text))
            {
                MessageBox.Show("请输入用户名！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("请输入密码！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            if (cmbRole.SelectedIndex == -1)
            {
                MessageBox.Show("请选择用户角色！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbRole.Focus();
                return;
            }

            // 新增：学生ID验证
            if (cmbRole.SelectedItem.ToString() == "Student" && string.IsNullOrEmpty(txtStudentID.Text))
            {
                MessageBox.Show("添加学生用户时必须输入学生ID！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtStudentID.Focus();
                return;
            }

            string username = txtUsername.Text;
            string password = txtPassword.Text;
            string role = cmbRole.SelectedItem.ToString();
            string studentID = txtStudentID.Text;  // 新增

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // 检查用户名是否已存在
                string checkQuery = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
                SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
                checkCommand.Parameters.AddWithValue("@Username", username);

                try
                {
                    connection.Open();
                    int count = (int)checkCommand.ExecuteScalar();

                    if (count > 0)
                    {
                        MessageBox.Show("该用户名已存在，请选择其他用户名！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtUsername.Focus();
                        return;
                    }

                    // 新增：验证学生ID是否存在于学生表中
                    if (role == "Student")
                    {
                        string checkStudentQuery = "SELECT COUNT(*) FROM Students WHERE StudentID = @StudentID";
                        SqlCommand checkStudentCommand = new SqlCommand(checkStudentQuery, connection);
                        checkStudentCommand.Parameters.AddWithValue("@StudentID", studentID);

                        int studentCount = (int)checkStudentCommand.ExecuteScalar();
                        if (studentCount == 0)
                        {
                            MessageBox.Show("该学生ID不存在，请输入有效的学生ID！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtStudentID.Focus();
                            return;
                        }
                    }

                    // 插入新用户
                    string insertQuery = "INSERT INTO Users (Username, Password, Role, StudentID) VALUES (@Username, @Password, @Role, @StudentID)";
                    SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                    insertCommand.Parameters.AddWithValue("@Username", username);
                    insertCommand.Parameters.AddWithValue("@Password", password);
                    insertCommand.Parameters.AddWithValue("@Role", role);
                    object studentIdValue = role == "Student" ? (object)studentID : DBNull.Value;
                    insertCommand.Parameters.AddWithValue("@StudentID", studentIdValue);

                    insertCommand.ExecuteNonQuery();
                    MessageBox.Show("用户添加成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // 刷新用户列表
                    LoadUsers();

                    // 清空输入框
                    txtUsername.Clear();
                    txtPassword.Clear();
                    cmbRole.SelectedIndex = 0;
                    txtStudentID.Clear();  // 新增
                    txtUsername.Focus();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("添加用户时出错：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnDeleteUser_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要删除的用户！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 确认删除
            DialogResult result = MessageBox.Show("确定要删除选中的用户吗？", "确认删除",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
            {
                return;
            }

            try
            {
                int userID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["用户ID"].Value);
                string username = dataGridView1.SelectedRows[0].Cells["用户名"].Value.ToString();

                // 不允许删除当前登录用户
                if (username == MainForm.CurrentUsername)
                {
                    MessageBox.Show("不能删除当前登录的用户！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM Users WHERE UserID = @UserID";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@UserID", userID);

                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("用户删除成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadUsers();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("删除用户时出错：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
