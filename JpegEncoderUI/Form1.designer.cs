namespace EncoderUI
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.menuStrip2 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.encodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.writeJpgToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.writeCurrentToJpgToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.Cb_Label = new System.Windows.Forms.Label();
            this.Y_Label = new System.Windows.Forms.Label();
            this.Cr_Label = new System.Windows.Forms.Label();
            this.YChPictureBox = new System.Windows.Forms.PictureBox();
            this.CbChPictureBox = new System.Windows.Forms.PictureBox();
            this.CrChPictureBox = new System.Windows.Forms.PictureBox();
            this.DisplayGroupBox = new System.Windows.Forms.GroupBox();
            this.DrawInBWCheckBox = new System.Windows.Forms.CheckBox();
            this.YCbCrDisplayTypeRadioButton = new System.Windows.Forms.RadioButton();
            this.RGBDisplayTypeRadioButton = new System.Windows.Forms.RadioButton();
            this.SettingsGroupBox = new System.Windows.Forms.GroupBox();
            this.writeCurrentImageButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.MainProgressBar = new System.Windows.Forms.ProgressBar();
            this.quantizerQualityComboBox = new System.Windows.Forms.ComboBox();
            this.WriteJpgButton = new System.Windows.Forms.Button();
            this.mainPictureBox = new System.Windows.Forms.PictureBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Chromiance_Data_RichTextBox = new System.Windows.Forms.RichTextBox();
            this.Luminance_Data_RichTextBox = new System.Windows.Forms.RichTextBox();
            this.ProgressTimer = new System.Windows.Forms.Timer(this.components);
            this.EncodeBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.ImageLoadBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.menuStrip2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.YChPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CbChPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CrChPictureBox)).BeginInit();
            this.DisplayGroupBox.SuspendLayout();
            this.SettingsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainPictureBox)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip2
            // 
            this.menuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem1,
            this.encodeToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip2.Location = new System.Drawing.Point(0, 0);
            this.menuStrip2.Name = "menuStrip2";
            this.menuStrip2.Size = new System.Drawing.Size(584, 24);
            this.menuStrip2.TabIndex = 1;
            this.menuStrip2.Text = "menuStrip2";
            // 
            // fileToolStripMenuItem1
            // 
            this.fileToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.exitToolStripMenuItem1});
            this.fileToolStripMenuItem1.Name = "fileToolStripMenuItem1";
            this.fileToolStripMenuItem1.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem1.Text = "&File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.open_Click);
            // 
            // exitToolStripMenuItem1
            // 
            this.exitToolStripMenuItem1.Name = "exitToolStripMenuItem1";
            this.exitToolStripMenuItem1.Size = new System.Drawing.Size(111, 22);
            this.exitToolStripMenuItem1.Text = "Exit";
            this.exitToolStripMenuItem1.Click += new System.EventHandler(this.exitToolStripMenuItem1_Click);
            // 
            // encodeToolStripMenuItem
            // 
            this.encodeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.writeJpgToolStripMenuItem,
            this.writeCurrentToJpgToolStripMenuItem});
            this.encodeToolStripMenuItem.Name = "encodeToolStripMenuItem";
            this.encodeToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.encodeToolStripMenuItem.Text = "Encode";
            // 
            // writeJpgToolStripMenuItem
            // 
            this.writeJpgToolStripMenuItem.Enabled = false;
            this.writeJpgToolStripMenuItem.Name = "writeJpgToolStripMenuItem";
            this.writeJpgToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.writeJpgToolStripMenuItem.Text = "Write Original To Jpg";
            this.writeJpgToolStripMenuItem.Click += new System.EventHandler(this.WriteJpgButton_Click);
            // 
            // writeCurrentToJpgToolStripMenuItem
            // 
            this.writeCurrentToJpgToolStripMenuItem.Enabled = false;
            this.writeCurrentToJpgToolStripMenuItem.Name = "writeCurrentToJpgToolStripMenuItem";
            this.writeCurrentToJpgToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.writeCurrentToJpgToolStripMenuItem.Text = "Write Current To Jpg";
            this.writeCurrentToJpgToolStripMenuItem.Click += new System.EventHandler(this.writeCurrentImageButton_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem1,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // helpToolStripMenuItem1
            // 
            this.helpToolStripMenuItem1.Name = "helpToolStripMenuItem1";
            this.helpToolStripMenuItem1.Size = new System.Drawing.Size(114, 22);
            this.helpToolStripMenuItem1.Text = "Help";
            this.helpToolStripMenuItem1.Click += new System.EventHandler(this.helpToolStripMenuItem1_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 76.16747F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.83253F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.DisplayGroupBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.SettingsGroupBox, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.mainPictureBox, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 91F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(570, 423);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.Cb_Label, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.Y_Label, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.Cr_Label, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.YChPictureBox, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.CbChPictureBox, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.CrChPictureBox, 0, 5);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(437, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 6;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 13F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 13F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 13F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(130, 326);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // Cb_Label
            // 
            this.Cb_Label.AutoSize = true;
            this.Cb_Label.Location = new System.Drawing.Point(3, 108);
            this.Cb_Label.Name = "Cb_Label";
            this.Cb_Label.Size = new System.Drawing.Size(78, 13);
            this.Cb_Label.TabIndex = 4;
            this.Cb_Label.Text = "Green Channel";
            // 
            // Y_Label
            // 
            this.Y_Label.AutoSize = true;
            this.Y_Label.Location = new System.Drawing.Point(3, 0);
            this.Y_Label.Name = "Y_Label";
            this.Y_Label.Size = new System.Drawing.Size(69, 13);
            this.Y_Label.TabIndex = 0;
            this.Y_Label.Text = "Red Channel";
            // 
            // Cr_Label
            // 
            this.Cr_Label.AutoSize = true;
            this.Cr_Label.Location = new System.Drawing.Point(3, 216);
            this.Cr_Label.Name = "Cr_Label";
            this.Cr_Label.Size = new System.Drawing.Size(70, 13);
            this.Cr_Label.TabIndex = 5;
            this.Cr_Label.Text = "Blue Channel";
            // 
            // YChPictureBox
            // 
            this.YChPictureBox.BackColor = System.Drawing.Color.Gainsboro;
            this.YChPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.YChPictureBox.Location = new System.Drawing.Point(3, 16);
            this.YChPictureBox.Name = "YChPictureBox";
            this.YChPictureBox.Size = new System.Drawing.Size(124, 89);
            this.YChPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.YChPictureBox.TabIndex = 6;
            this.YChPictureBox.TabStop = false;
            this.YChPictureBox.Click += new System.EventHandler(this.YChPictureBox_Click);
            // 
            // CbChPictureBox
            // 
            this.CbChPictureBox.BackColor = System.Drawing.Color.Gainsboro;
            this.CbChPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CbChPictureBox.Location = new System.Drawing.Point(3, 124);
            this.CbChPictureBox.Name = "CbChPictureBox";
            this.CbChPictureBox.Size = new System.Drawing.Size(124, 89);
            this.CbChPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.CbChPictureBox.TabIndex = 7;
            this.CbChPictureBox.TabStop = false;
            this.CbChPictureBox.Click += new System.EventHandler(this.CbChPictureBox_Click);
            // 
            // CrChPictureBox
            // 
            this.CrChPictureBox.BackColor = System.Drawing.Color.Gainsboro;
            this.CrChPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CrChPictureBox.Location = new System.Drawing.Point(3, 232);
            this.CrChPictureBox.Name = "CrChPictureBox";
            this.CrChPictureBox.Size = new System.Drawing.Size(124, 91);
            this.CrChPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.CrChPictureBox.TabIndex = 8;
            this.CrChPictureBox.TabStop = false;
            this.CrChPictureBox.Click += new System.EventHandler(this.CrChPictureBox_Click);
            // 
            // DisplayGroupBox
            // 
            this.DisplayGroupBox.Controls.Add(this.DrawInBWCheckBox);
            this.DisplayGroupBox.Controls.Add(this.YCbCrDisplayTypeRadioButton);
            this.DisplayGroupBox.Controls.Add(this.RGBDisplayTypeRadioButton);
            this.DisplayGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DisplayGroupBox.Enabled = false;
            this.DisplayGroupBox.Location = new System.Drawing.Point(437, 335);
            this.DisplayGroupBox.Name = "DisplayGroupBox";
            this.DisplayGroupBox.Size = new System.Drawing.Size(130, 85);
            this.DisplayGroupBox.TabIndex = 4;
            this.DisplayGroupBox.TabStop = false;
            this.DisplayGroupBox.Text = "Display";
            // 
            // DrawInBWCheckBox
            // 
            this.DrawInBWCheckBox.AutoSize = true;
            this.DrawInBWCheckBox.Location = new System.Drawing.Point(16, 58);
            this.DrawInBWCheckBox.Name = "DrawInBWCheckBox";
            this.DrawInBWCheckBox.Size = new System.Drawing.Size(115, 17);
            this.DrawInBWCheckBox.TabIndex = 2;
            this.DrawInBWCheckBox.Text = "Draw RGB In B/W";
            this.DrawInBWCheckBox.UseVisualStyleBackColor = true;
            this.DrawInBWCheckBox.CheckedChanged += new System.EventHandler(this.DrawInBWCheckBox_CheckedChanged);
            // 
            // YCbCrDisplayTypeRadioButton
            // 
            this.YCbCrDisplayTypeRadioButton.AutoSize = true;
            this.YCbCrDisplayTypeRadioButton.Location = new System.Drawing.Point(16, 18);
            this.YCbCrDisplayTypeRadioButton.Name = "YCbCrDisplayTypeRadioButton";
            this.YCbCrDisplayTypeRadioButton.Size = new System.Drawing.Size(67, 17);
            this.YCbCrDisplayTypeRadioButton.TabIndex = 0;
            this.YCbCrDisplayTypeRadioButton.Text = "Y, Cb, Cr";
            this.YCbCrDisplayTypeRadioButton.UseVisualStyleBackColor = true;
            this.YCbCrDisplayTypeRadioButton.CheckedChanged += new System.EventHandler(this.YCbCrDisplayTypeRadioButton_CheckedChanged);
            // 
            // RGBDisplayTypeRadioButton
            // 
            this.RGBDisplayTypeRadioButton.AutoSize = true;
            this.RGBDisplayTypeRadioButton.Checked = true;
            this.RGBDisplayTypeRadioButton.Location = new System.Drawing.Point(16, 35);
            this.RGBDisplayTypeRadioButton.Name = "RGBDisplayTypeRadioButton";
            this.RGBDisplayTypeRadioButton.Size = new System.Drawing.Size(60, 17);
            this.RGBDisplayTypeRadioButton.TabIndex = 1;
            this.RGBDisplayTypeRadioButton.TabStop = true;
            this.RGBDisplayTypeRadioButton.Text = "R, G, B";
            this.RGBDisplayTypeRadioButton.UseVisualStyleBackColor = true;
            // 
            // SettingsGroupBox
            // 
            this.SettingsGroupBox.Controls.Add(this.writeCurrentImageButton);
            this.SettingsGroupBox.Controls.Add(this.label1);
            this.SettingsGroupBox.Controls.Add(this.MainProgressBar);
            this.SettingsGroupBox.Controls.Add(this.quantizerQualityComboBox);
            this.SettingsGroupBox.Controls.Add(this.WriteJpgButton);
            this.SettingsGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SettingsGroupBox.Enabled = false;
            this.SettingsGroupBox.Location = new System.Drawing.Point(3, 335);
            this.SettingsGroupBox.Name = "SettingsGroupBox";
            this.SettingsGroupBox.Size = new System.Drawing.Size(428, 85);
            this.SettingsGroupBox.TabIndex = 5;
            this.SettingsGroupBox.TabStop = false;
            this.SettingsGroupBox.Text = "Settings";
            // 
            // writeCurrentImageButton
            // 
            this.writeCurrentImageButton.Location = new System.Drawing.Point(324, 19);
            this.writeCurrentImageButton.Name = "writeCurrentImageButton";
            this.writeCurrentImageButton.Size = new System.Drawing.Size(88, 27);
            this.writeCurrentImageButton.TabIndex = 4;
            this.writeCurrentImageButton.Text = "Write Current";
            this.writeCurrentImageButton.UseVisualStyleBackColor = true;
            this.writeCurrentImageButton.Click += new System.EventHandler(this.writeCurrentImageButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Quantizer Quality:";
            // 
            // MainProgressBar
            // 
            this.MainProgressBar.Location = new System.Drawing.Point(17, 58);
            this.MainProgressBar.Name = "MainProgressBar";
            this.MainProgressBar.Size = new System.Drawing.Size(395, 15);
            this.MainProgressBar.TabIndex = 2;
            // 
            // quantizerQualityComboBox
            // 
            this.quantizerQualityComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.quantizerQualityComboBox.FormattingEnabled = true;
            this.quantizerQualityComboBox.Items.AddRange(new object[] {
            "1",
            "10",
            "20",
            "30",
            "40",
            "50",
            "60",
            "70",
            "80",
            "90",
            "100",
            "150",
            "300",
            "600",
            "900"});
            this.quantizerQualityComboBox.Location = new System.Drawing.Point(126, 22);
            this.quantizerQualityComboBox.Name = "quantizerQualityComboBox";
            this.quantizerQualityComboBox.Size = new System.Drawing.Size(90, 21);
            this.quantizerQualityComboBox.TabIndex = 1;
            // 
            // WriteJpgButton
            // 
            this.WriteJpgButton.Location = new System.Drawing.Point(234, 19);
            this.WriteJpgButton.Name = "WriteJpgButton";
            this.WriteJpgButton.Size = new System.Drawing.Size(84, 27);
            this.WriteJpgButton.TabIndex = 0;
            this.WriteJpgButton.Text = "Write Original";
            this.WriteJpgButton.UseVisualStyleBackColor = true;
            this.WriteJpgButton.Click += new System.EventHandler(this.WriteJpgButton_Click);
            // 
            // mainPictureBox
            // 
            this.mainPictureBox.BackColor = System.Drawing.Color.Gainsboro;
            this.mainPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPictureBox.Location = new System.Drawing.Point(3, 3);
            this.mainPictureBox.Name = "mainPictureBox";
            this.mainPictureBox.Size = new System.Drawing.Size(428, 326);
            this.mainPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.mainPictureBox.TabIndex = 6;
            this.mainPictureBox.TabStop = false;
            this.mainPictureBox.Click += new System.EventHandler(this.mainPictureBox_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 479);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(584, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // StatusLabel
            // 
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(38, 17);
            this.StatusLabel.Text = "Ready";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tabControl1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(584, 455);
            this.panel1.TabIndex = 4;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(584, 455);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tableLayoutPanel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(576, 429);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Image";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(628, 448);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Tables";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.Chromiance_Data_RichTextBox);
            this.groupBox1.Controls.Add(this.Luminance_Data_RichTextBox);
            this.groupBox1.Location = new System.Drawing.Point(11, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(600, 422);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Quantization Tables";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(32, 232);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(210, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Chromiance Quantization Table";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(32, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(203, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Luminance Quantization Table";
            // 
            // Chromiance_Data_RichTextBox
            // 
            this.Chromiance_Data_RichTextBox.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Chromiance_Data_RichTextBox.Location = new System.Drawing.Point(35, 248);
            this.Chromiance_Data_RichTextBox.Name = "Chromiance_Data_RichTextBox";
            this.Chromiance_Data_RichTextBox.Size = new System.Drawing.Size(322, 135);
            this.Chromiance_Data_RichTextBox.TabIndex = 1;
            this.Chromiance_Data_RichTextBox.Text = resources.GetString("Chromiance_Data_RichTextBox.Text");
            // 
            // Luminance_Data_RichTextBox
            // 
            this.Luminance_Data_RichTextBox.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Luminance_Data_RichTextBox.Location = new System.Drawing.Point(35, 48);
            this.Luminance_Data_RichTextBox.Name = "Luminance_Data_RichTextBox";
            this.Luminance_Data_RichTextBox.Size = new System.Drawing.Size(322, 146);
            this.Luminance_Data_RichTextBox.TabIndex = 0;
            this.Luminance_Data_RichTextBox.Text = resources.GetString("Luminance_Data_RichTextBox.Text");
            // 
            // ProgressTimer
            // 
            this.ProgressTimer.Enabled = true;
            this.ProgressTimer.Tick += new System.EventHandler(this.ProgressTimer_Tick);
            // 
            // EncodeBackgroundWorker
            // 
            this.EncodeBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.EncodeBackgroundWorker_DoWork);
            this.EncodeBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.EncodeBackgroundWorker_RunWorkerCompleted);
            // 
            // ImageLoadBackgroundWorker
            // 
            this.ImageLoadBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.ImageLoadBackgroundWorker_DoWork);
            this.ImageLoadBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.ImageLoadBackgroundWorker_RunWorkerCompleted);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 501);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip2);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "JPEG Encoder v1.5 - ArpanTECH - 2010";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip2.ResumeLayout(false);
            this.menuStrip2.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.YChPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CbChPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CrChPictureBox)).EndInit();
            this.DisplayGroupBox.ResumeLayout(false);
            this.DisplayGroupBox.PerformLayout();
            this.SettingsGroupBox.ResumeLayout(false);
            this.SettingsGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainPictureBox)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip2;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem encodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem writeJpgToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label Cb_Label;
        private System.Windows.Forms.Label Y_Label;
        private System.Windows.Forms.Label Cr_Label;
        private System.Windows.Forms.PictureBox YChPictureBox;
        private System.Windows.Forms.PictureBox CbChPictureBox;
        private System.Windows.Forms.PictureBox CrChPictureBox;
        private System.Windows.Forms.GroupBox DisplayGroupBox;
        private System.Windows.Forms.RadioButton YCbCrDisplayTypeRadioButton;
        private System.Windows.Forms.RadioButton RGBDisplayTypeRadioButton;
        private System.Windows.Forms.GroupBox SettingsGroupBox;
        private System.Windows.Forms.ComboBox quantizerQualityComboBox;
        private System.Windows.Forms.Button WriteJpgButton;
        private System.Windows.Forms.PictureBox mainPictureBox;
        private System.Windows.Forms.ProgressBar MainProgressBar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox DrawInBWCheckBox;
        private System.Windows.Forms.Timer ProgressTimer;
        private System.ComponentModel.BackgroundWorker EncodeBackgroundWorker;
        private System.ComponentModel.BackgroundWorker ImageLoadBackgroundWorker;
        private System.Windows.Forms.Button writeCurrentImageButton;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox Chromiance_Data_RichTextBox;
        private System.Windows.Forms.RichTextBox Luminance_Data_RichTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolStripMenuItem writeCurrentToJpgToolStripMenuItem;

    }
}

