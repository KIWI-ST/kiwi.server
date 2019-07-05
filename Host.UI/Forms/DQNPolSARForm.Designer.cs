namespace Host.UI.SettingForm
{
    partial class DQNPolSARForm
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
            this.epochs_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.epochs_label = new System.Windows.Forms.Label();
            this.ok_button = new System.Windows.Forms.Button();
            this.state_comboBox = new System.Windows.Forms.ComboBox();
            this.state_label = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SupportNet_comboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Device_comboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SampleFile_textBox = new System.Windows.Forms.TextBox();
            this.Open_button = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.epochs_numericUpDown)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // epochs_numericUpDown
            // 
            this.epochs_numericUpDown.Location = new System.Drawing.Point(186, 287);
            this.epochs_numericUpDown.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.epochs_numericUpDown.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.epochs_numericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.epochs_numericUpDown.Name = "epochs_numericUpDown";
            this.epochs_numericUpDown.Size = new System.Drawing.Size(96, 28);
            this.epochs_numericUpDown.TabIndex = 22;
            this.epochs_numericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.epochs_numericUpDown.Value = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            // 
            // epochs_label
            // 
            this.epochs_label.AutoSize = true;
            this.epochs_label.Location = new System.Drawing.Point(84, 292);
            this.epochs_label.Name = "epochs_label";
            this.epochs_label.Size = new System.Drawing.Size(62, 18);
            this.epochs_label.TabIndex = 21;
            this.epochs_label.Text = "轮次：";
            // 
            // ok_button
            // 
            this.ok_button.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ok_button.Location = new System.Drawing.Point(539, 329);
            this.ok_button.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ok_button.Name = "ok_button";
            this.ok_button.Size = new System.Drawing.Size(89, 30);
            this.ok_button.TabIndex = 20;
            this.ok_button.Text = "确定";
            this.ok_button.UseVisualStyleBackColor = true;
            this.ok_button.Click += new System.EventHandler(this.Ok_button_Click);
            // 
            // state_comboBox
            // 
            this.state_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.state_comboBox.FormattingEnabled = true;
            this.state_comboBox.Location = new System.Drawing.Point(186, 52);
            this.state_comboBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.state_comboBox.Name = "state_comboBox";
            this.state_comboBox.Size = new System.Drawing.Size(442, 26);
            this.state_comboBox.TabIndex = 19;
            this.state_comboBox.SelectedIndexChanged += new System.EventHandler(this.State_comboBox_SelectedIndexChanged);
            // 
            // state_label
            // 
            this.state_label.AutoSize = true;
            this.state_label.Location = new System.Drawing.Point(25, 55);
            this.state_label.Name = "state_label";
            this.state_label.Size = new System.Drawing.Size(116, 18);
            this.state_label.TabIndex = 16;
            this.state_label.Text = "待分类图层：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(48, 220);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 18);
            this.label2.TabIndex = 26;
            this.label2.Text = "选择网络:";
            // 
            // SupportNet_comboBox
            // 
            this.SupportNet_comboBox.FormattingEnabled = true;
            this.SupportNet_comboBox.Location = new System.Drawing.Point(186, 215);
            this.SupportNet_comboBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SupportNet_comboBox.Name = "SupportNet_comboBox";
            this.SupportNet_comboBox.Size = new System.Drawing.Size(320, 26);
            this.SupportNet_comboBox.TabIndex = 27;
            this.SupportNet_comboBox.SelectedIndexChanged += new System.EventHandler(this.SupportNet_comboBox_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(327, 294);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 18);
            this.label3.TabIndex = 28;
            this.label3.Text = "训练设备:";
            // 
            // Device_comboBox
            // 
            this.Device_comboBox.DropDownWidth = 85;
            this.Device_comboBox.FormattingEnabled = true;
            this.Device_comboBox.Location = new System.Drawing.Point(426, 289);
            this.Device_comboBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Device_comboBox.Name = "Device_comboBox";
            this.Device_comboBox.Size = new System.Drawing.Size(79, 26);
            this.Device_comboBox.TabIndex = 29;
            this.Device_comboBox.SelectedIndexChanged += new System.EventHandler(this.Device_comboBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(40, 121);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 18);
            this.label1.TabIndex = 23;
            this.label1.Text = "环境样本：";
            // 
            // SampleFile_textBox
            // 
            this.SampleFile_textBox.Location = new System.Drawing.Point(186, 116);
            this.SampleFile_textBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.SampleFile_textBox.Name = "SampleFile_textBox";
            this.SampleFile_textBox.Size = new System.Drawing.Size(320, 28);
            this.SampleFile_textBox.TabIndex = 24;
            // 
            // Open_button
            // 
            this.Open_button.Location = new System.Drawing.Point(539, 116);
            this.Open_button.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Open_button.Name = "Open_button";
            this.Open_button.Size = new System.Drawing.Size(89, 30);
            this.Open_button.TabIndex = 25;
            this.Open_button.Text = "打开";
            this.Open_button.UseVisualStyleBackColor = true;
            this.Open_button.Click += new System.EventHandler(this.Open_button_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.state_label);
            this.groupBox1.Controls.Add(this.state_comboBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.SampleFile_textBox);
            this.groupBox1.Controls.Add(this.Open_button);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(672, 184);
            this.groupBox1.TabIndex = 30;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "环境设置";
            // 
            // DQNPolSARForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(672, 383);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.Device_comboBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.SupportNet_comboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.epochs_numericUpDown);
            this.Controls.Add(this.epochs_label);
            this.Controls.Add(this.ok_button);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "DQNPolSARForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "DQNPolSARForm";
            ((System.ComponentModel.ISupportInitialize)(this.epochs_numericUpDown)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown epochs_numericUpDown;
        private System.Windows.Forms.Label epochs_label;
        private System.Windows.Forms.Button ok_button;
        private System.Windows.Forms.ComboBox state_comboBox;
        private System.Windows.Forms.Label state_label;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox SupportNet_comboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox Device_comboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox SampleFile_textBox;
        private System.Windows.Forms.Button Open_button;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}