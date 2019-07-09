namespace Host.UI.SettingForm
{
    partial class CNNForm
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
            this.label4 = new System.Windows.Forms.Label();
            this.Epochs_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.OK_button = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.Width_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.Height_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.open_sample_textBox = new System.Windows.Forms.TextBox();
            this.open_button = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.DEPTH_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.device_comboBox = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ConvType_comboBox = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.RasterLayer_comboBox = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.Epochs_numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Width_numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Height_numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DEPTH_numericUpDown)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(52, 283);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 15);
            this.label4.TabIndex = 6;
            this.label4.Text = "训练轮次：";
            // 
            // Epochs_numericUpDown
            // 
            this.Epochs_numericUpDown.Location = new System.Drawing.Point(147, 279);
            this.Epochs_numericUpDown.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Epochs_numericUpDown.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.Epochs_numericUpDown.Name = "Epochs_numericUpDown";
            this.Epochs_numericUpDown.Size = new System.Drawing.Size(85, 25);
            this.Epochs_numericUpDown.TabIndex = 7;
            this.Epochs_numericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Epochs_numericUpDown.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.Epochs_numericUpDown.ValueChanged += new System.EventHandler(this.Epochs_numericUpDown_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(264, 284);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 15);
            this.label5.TabIndex = 8;
            this.label5.Text = "训练设备：";
            // 
            // OK_button
            // 
            this.OK_button.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OK_button.Location = new System.Drawing.Point(467, 276);
            this.OK_button.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.OK_button.Name = "OK_button";
            this.OK_button.Size = new System.Drawing.Size(93, 30);
            this.OK_button.TabIndex = 10;
            this.OK_button.Text = "确定";
            this.OK_button.UseVisualStyleBackColor = true;
            this.OK_button.Click += new System.EventHandler(this.OK_button_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(80, 141);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(54, 20);
            this.label6.TabIndex = 11;
            this.label6.Text = "宽度：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(239, 142);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(54, 20);
            this.label7.TabIndex = 12;
            this.label7.Text = "高度：";
            // 
            // Width_numericUpDown
            // 
            this.Width_numericUpDown.Location = new System.Drawing.Point(147, 137);
            this.Width_numericUpDown.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Width_numericUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.Width_numericUpDown.Name = "Width_numericUpDown";
            this.Width_numericUpDown.Size = new System.Drawing.Size(76, 27);
            this.Width_numericUpDown.TabIndex = 13;
            this.Width_numericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Width_numericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.Width_numericUpDown.ValueChanged += new System.EventHandler(this.Width_numericUpDown_ValueChanged);
            // 
            // Height_numericUpDown
            // 
            this.Height_numericUpDown.Location = new System.Drawing.Point(283, 137);
            this.Height_numericUpDown.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Height_numericUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.Height_numericUpDown.Name = "Height_numericUpDown";
            this.Height_numericUpDown.Size = new System.Drawing.Size(76, 27);
            this.Height_numericUpDown.TabIndex = 14;
            this.Height_numericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Height_numericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.Height_numericUpDown.ValueChanged += new System.EventHandler(this.Height_numericUpDown_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(59, 91);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 20);
            this.label1.TabIndex = 15;
            this.label1.Text = "样本文件：";
            // 
            // open_sample_textBox
            // 
            this.open_sample_textBox.Location = new System.Drawing.Point(147, 88);
            this.open_sample_textBox.Margin = new System.Windows.Forms.Padding(4);
            this.open_sample_textBox.Name = "open_sample_textBox";
            this.open_sample_textBox.Size = new System.Drawing.Size(312, 27);
            this.open_sample_textBox.TabIndex = 16;
            // 
            // open_button
            // 
            this.open_button.Location = new System.Drawing.Point(467, 86);
            this.open_button.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.open_button.Name = "open_button";
            this.open_button.Size = new System.Drawing.Size(93, 30);
            this.open_button.TabIndex = 17;
            this.open_button.Text = "打开";
            this.open_button.UseVisualStyleBackColor = true;
            this.open_button.Click += new System.EventHandler(this.Open_button_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(378, 142);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 20);
            this.label2.TabIndex = 20;
            this.label2.Text = "深度：";
            // 
            // DEPTH_numericUpDown
            // 
            this.DEPTH_numericUpDown.Location = new System.Drawing.Point(421, 137);
            this.DEPTH_numericUpDown.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.DEPTH_numericUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.DEPTH_numericUpDown.Name = "DEPTH_numericUpDown";
            this.DEPTH_numericUpDown.Size = new System.Drawing.Size(76, 27);
            this.DEPTH_numericUpDown.TabIndex = 21;
            this.DEPTH_numericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.DEPTH_numericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.DEPTH_numericUpDown.ValueChanged += new System.EventHandler(this.DEPTH_numericUpDown_ValueChanged);
            // 
            // device_comboBox
            // 
            this.device_comboBox.FormattingEnabled = true;
            this.device_comboBox.Location = new System.Drawing.Point(352, 280);
            this.device_comboBox.Name = "device_comboBox";
            this.device_comboBox.Size = new System.Drawing.Size(107, 23);
            this.device_comboBox.TabIndex = 22;
            this.device_comboBox.SelectedIndexChanged += new System.EventHandler(this.Device_comboBox_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ConvType_comboBox);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.open_sample_textBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.DEPTH_numericUpDown);
            this.groupBox1.Controls.Add(this.open_button);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.Width_numericUpDown);
            this.groupBox1.Controls.Add(this.Height_numericUpDown);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(613, 186);
            this.groupBox1.TabIndex = 23;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "基础设置";
            // 
            // ConvType_comboBox
            // 
            this.ConvType_comboBox.FormattingEnabled = true;
            this.ConvType_comboBox.Location = new System.Drawing.Point(151, 37);
            this.ConvType_comboBox.Name = "ConvType_comboBox";
            this.ConvType_comboBox.Size = new System.Drawing.Size(308, 28);
            this.ConvType_comboBox.TabIndex = 23;
            this.ConvType_comboBox.SelectedIndexChanged += new System.EventHandler(this.ConvType_comboBox_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(59, 40);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(84, 20);
            this.label8.TabIndex = 22;
            this.label8.Text = "模型结构：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(37, 223);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 15);
            this.label3.TabIndex = 24;
            this.label3.Text = "待分类图像：";
            // 
            // RasterLayer_comboBox
            // 
            this.RasterLayer_comboBox.FormattingEnabled = true;
            this.RasterLayer_comboBox.Location = new System.Drawing.Point(147, 220);
            this.RasterLayer_comboBox.Name = "RasterLayer_comboBox";
            this.RasterLayer_comboBox.Size = new System.Drawing.Size(312, 23);
            this.RasterLayer_comboBox.TabIndex = 25;
            this.RasterLayer_comboBox.SelectedIndexChanged += new System.EventHandler(this.RasterLayer_comboBox_SelectedIndexChanged);
            // 
            // ConvForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(613, 335);
            this.Controls.Add(this.RasterLayer_comboBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.device_comboBox);
            this.Controls.Add(this.OK_button);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Epochs_numericUpDown);
            this.Controls.Add(this.label4);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "ConvForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ConvNetForm";
            ((System.ComponentModel.ISupportInitialize)(this.Epochs_numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Width_numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Height_numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DEPTH_numericUpDown)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown Epochs_numericUpDown;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button OK_button;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown Width_numericUpDown;
        private System.Windows.Forms.NumericUpDown Height_numericUpDown;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox open_sample_textBox;
        private System.Windows.Forms.Button open_button;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown DEPTH_numericUpDown;
        private System.Windows.Forms.ComboBox device_comboBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox ConvType_comboBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox RasterLayer_comboBox;
    }
}