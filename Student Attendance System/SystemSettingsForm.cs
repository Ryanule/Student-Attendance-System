using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Student_Attendance_System
{
    public partial class SystemSettingsForm: Form
    {
        // 存储要控制的窗体列表
        private List<Form> controlledForms;

        // 字体大小设置
        private float fontSize;
        public float FontSize
        {
            get { return fontSize; }
            set
            {
                fontSize = value;
                ApplyFontSize();
            }
        }

        // 窗口透明度设置
        private float opacity;
        public float Opacity
        {
            get { return opacity; }
            set
            {
                opacity = value;
                ApplyOpacity();
            }
        }

        // 是否显示窗口边框设置
        private bool showBorder;
        public bool ShowBorder
        {
            get { return showBorder; }
            set
            {
                showBorder = value;
                ApplyShowBorder();
            }
        }

        // 窗口最小尺寸设置
        private Size minFormSize;
        public Size MinFormSize
        {
            get { return minFormSize; }
            set
            {
                minFormSize = value;
                ApplyMinFormSize();
            }
        }

        // 背景图片路径
        private string backgroundImagePath;
        public string BackgroundImagePath
        {
            get { return backgroundImagePath; }
            set
            {
                backgroundImagePath = value;
                ApplyBackgroundImage();
            }
        }

        private NumericUpDown numFontSize;
        private NumericUpDown numOpacity;
        private CheckBox chkShowBorder;
        private NumericUpDown numMinWidth;
        private NumericUpDown numMinHeight;
        private Button btnSelectImage;
        private Button btnSaveSettings;
        private Button btnApplySettings;
        private TableLayoutPanel tableLayoutPanel;
        public SystemSettingsForm(List<Form> forms)
        {
            if (forms == null)
            {
                throw new ArgumentNullException(nameof(forms), "传入的窗体列表不能为 null。");
            }
            InitializeComponent();
            controlledForms = forms;
            // 初始化设置
            fontSize = 12;
            opacity = 1.0f;
            showBorder = true;
            minFormSize = new Size(300, 200);
            backgroundImagePath = "";

            InitializeUI();
        }
        private void InitializeUI()
        {
            tableLayoutPanel = new TableLayoutPanel();
            tableLayoutPanel.Dock = DockStyle.Fill;
            tableLayoutPanel.ColumnCount = 2;
            tableLayoutPanel.RowCount = 6;
            tableLayoutPanel.Padding = new Padding(10);
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));

            // 字体大小设置
            Label fontSizeLabel = new Label();
            fontSizeLabel.Text = "字体大小：";
            tableLayoutPanel.Controls.Add(fontSizeLabel, 0, 0);

            numFontSize = new NumericUpDown();
            numFontSize.Minimum = 6;
            numFontSize.Maximum = 72;
            numFontSize.Value = (decimal)fontSize;
            numFontSize.ValueChanged += NumFontSize_ValueChanged;
            tableLayoutPanel.Controls.Add(numFontSize, 1, 0);

            // 窗口透明度设置
            Label opacityLabel = new Label();
            opacityLabel.Text = "窗口透明度：";
            tableLayoutPanel.Controls.Add(opacityLabel, 0, 1);

            numOpacity = new NumericUpDown();
            numOpacity.Minimum = 0;
            numOpacity.Maximum = 100;
            numOpacity.Value = (decimal)(opacity * 100);
            numOpacity.ValueChanged += NumOpacity_ValueChanged;
            tableLayoutPanel.Controls.Add(numOpacity, 1, 1);

            // 是否显示窗口边框设置
            Label showBorderLabel = new Label();
            showBorderLabel.Text = "显示窗口边框：";
            tableLayoutPanel.Controls.Add(showBorderLabel, 0, 2);

            chkShowBorder = new CheckBox();
            chkShowBorder.Checked = showBorder;
            chkShowBorder.CheckedChanged += ChkShowBorder_CheckedChanged;
            tableLayoutPanel.Controls.Add(chkShowBorder, 1, 2);

            // 窗口最小尺寸设置
            Label minWidthLabel = new Label();
            minWidthLabel.Text = "窗口最小宽度：";
            tableLayoutPanel.Controls.Add(minWidthLabel, 0, 3);

            numMinWidth = new NumericUpDown();
            numMinWidth.Minimum = 100;
            numMinWidth.Maximum = 1000;
            numMinWidth.Value = minFormSize.Width;
            numMinWidth.ValueChanged += NumMinWidth_ValueChanged;
            tableLayoutPanel.Controls.Add(numMinWidth, 1, 3);

            Label minHeightLabel = new Label();
            minHeightLabel.Text = "窗口最小高度：";
            tableLayoutPanel.Controls.Add(minHeightLabel, 0, 4);

            numMinHeight = new NumericUpDown();
            numMinHeight.Minimum = 100;
            numMinHeight.Maximum = 1000;
            numMinHeight.Value = minFormSize.Height;
            numMinHeight.ValueChanged += NumMinHeight_ValueChanged;
            tableLayoutPanel.Controls.Add(numMinHeight, 1, 4);

            // 背景图片设置
            Label backgroundImageLabel = new Label();
            backgroundImageLabel.Text = "背景图片：";
            tableLayoutPanel.Controls.Add(backgroundImageLabel, 0, 5);

            btnSelectImage = new Button();
            btnSelectImage.Text = "选择图片";
            btnSelectImage.Click += BtnSelectImage_Click;
            tableLayoutPanel.Controls.Add(btnSelectImage, 1, 5);

            // 保存和应用按钮
            Panel buttonPanel = new Panel();
            buttonPanel.Dock = DockStyle.Fill;

            btnSaveSettings = new Button();
            btnSaveSettings.Text = "保存设置";
            btnSaveSettings.Location = new Point(10, 10);
            btnSaveSettings.Click += btnSaveSettings_Click;
            buttonPanel.Controls.Add(btnSaveSettings);

            btnApplySettings = new Button();
            btnApplySettings.Text = "应用设置";
            btnApplySettings.Location = new Point(120, 10);
            btnApplySettings.Click += btnApplySettings_Click;
            buttonPanel.Controls.Add(btnApplySettings);

            tableLayoutPanel.Controls.Add(buttonPanel, 0, 6);
            tableLayoutPanel.SetColumnSpan(buttonPanel, 2);

            this.Controls.Add(tableLayoutPanel);
            this.ClientSize = new Size(350, 350);
            this.Text = "系统设置";
        }

        private void NumFontSize_ValueChanged(object sender, EventArgs e)
        {
            FontSize = (float)numFontSize.Value;
        }

        private void NumOpacity_ValueChanged(object sender, EventArgs e)
        {
            Opacity = (float)numOpacity.Value / 100;
        }

        private void ChkShowBorder_CheckedChanged(object sender, EventArgs e)
        {
            ShowBorder = chkShowBorder.Checked;
        }

        private void NumMinWidth_ValueChanged(object sender, EventArgs e)
        {
            minFormSize = new Size((int)numMinWidth.Value, minFormSize.Height);
            ApplyMinFormSize();
        }

        private void NumMinHeight_ValueChanged(object sender, EventArgs e)
        {
            minFormSize = new Size(minFormSize.Width, (int)numMinHeight.Value);
            ApplyMinFormSize();
        }

        private void BtnSelectImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "图片文件 (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                BackgroundImagePath = openFileDialog.FileName;
            }
        }

        // 应用字体大小到所有受控窗体
        private void ApplyFontSize()
        {
            foreach (Form form in controlledForms)
            {
                form.Font = new Font(form.Font.FontFamily, fontSize);
            }
        }

        // 应用窗口透明度到所有受控窗体
        private void ApplyOpacity()
        {
            foreach (Form form in controlledForms)
            {
                form.Opacity = opacity;
            }
        }

        // 应用是否显示窗口边框到所有受控窗体
        private void ApplyShowBorder()
        {
            foreach (Form form in controlledForms)
            {
                form.FormBorderStyle = showBorder ? FormBorderStyle.Sizable : FormBorderStyle.None;
            }
        }

        // 应用窗口最小尺寸到所有受控窗体
        private void ApplyMinFormSize()
        {
            foreach (Form form in controlledForms)
            {
                form.MinimumSize = minFormSize;
            }
        }

        // 应用背景图片到所有受控窗体
        private void ApplyBackgroundImage()
        {
            if (!string.IsNullOrEmpty(backgroundImagePath) && File.Exists(backgroundImagePath))
            {
                try
                {
                    Image backgroundImage = Image.FromFile(backgroundImagePath);
                    foreach (Form form in controlledForms)
                    {
                        form.BackgroundImage = backgroundImage;
                        form.BackgroundImageLayout = ImageLayout.Stretch;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"加载背景图片出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // 保存设置按钮点击事件
        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            // 这里可以添加保存设置到配置文件的逻辑
            MessageBox.Show("设置已保存");
        }

        // 应用设置按钮点击事件
        private void btnApplySettings_Click(object sender, EventArgs e)
        {
            ApplyFontSize();
            ApplyOpacity();
            ApplyShowBorder();
            ApplyMinFormSize();
            ApplyBackgroundImage();
            MessageBox.Show("设置已应用");
        }
    }
}
