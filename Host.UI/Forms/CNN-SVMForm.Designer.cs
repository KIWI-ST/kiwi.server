namespace Host.UI.SettingForm
{
    partial class CNNSVMFrom
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
            this.source_image_comboBox = new System.Windows.Forms.ComboBox();
            this.soruce_image_label = new System.Windows.Forms.Label();
            this.open_button = new System.Windows.Forms.Button();
            this.open_sample_textBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Height_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.Width_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.OK_button = new System.Windows.Forms.Button();
            this.GPU_Status_label = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.Epochs_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.FeaturePicked_comboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Height_numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Width_numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Epochs_numericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // source_image_comboBox
            // 
            this.source_image_comboBox.FormattingEnabled = true;
            this.source_image_comboBox.Location = new System.Drawing.Point(171, 79);
            this.source_image_comboBox.Margin = new System.Windows.Forms.Padding(4);
            this.source_image_comboBox.Name = "source_image_comboBox";
            this.source_image_comboBox.Size = new System.Drawing.Size(343, 23);
            this.source_image_comboBox.TabIndex = 35;
            this.source_image_comboBox.SelectedIndexChanged += new System.EventHandler(this.Source_comboBox_SelectedIndexChanged);
            // 
            // soruce_image_label
            // 
            this.soruce_image_label.AutoSize = true;
            this.soruce_image_label.Location = new System.Drawing.Point(50, 83);
            this.soruce_image_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.soruce_image_label.Name = "soruce_image_label";
            this.soruce_image_label.Size = new System.Drawing.Size(97, 15);
            this.soruce_image_label.TabIndex = 34;
            this.soruce_image_label.Text = "待分类影像：";
            // 
            // open_button
            // 
            this.open_button.Location = new System.Drawing.Point(420, 128);
            this.open_button.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.open_button.Name = "open_button";
            this.open_button.Size = new System.Drawing.Size(93, 30);
            this.open_button.TabIndex = 33;
            this.open_button.Text = "打开";
            this.open_button.UseVisualStyleBackColor = true;
            this.open_button.Click += new System.EventHandler(this.open_sample_button_Click);
            // 
            // open_sample_textBox
            // 
            this.open_sample_textBox.Location = new System.Drawing.Point(171, 129);
            this.open_sample_textBox.Margin = new System.Windows.Forms.Padding(4);
            this.open_sample_textBox.Name = "open_sample_textBox";
            this.open_sample_textBox.Size = new System.Drawing.Size(241, 25);
            this.open_sample_textBox.TabIndex = 32;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(63, 133);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 15);
            this.label1.TabIndex = 31;
            this.label1.Text = "样本文件：";
            // 
            // Height_numericUpDown
            // 
            this.Height_numericUpDown.Location = new System.Drawing.Point(394, 184);
            this.Height_numericUpDown.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Height_numericUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.Height_numericUpDown.Name = "Height_numericUpDown";
            this.Height_numericUpDown.Size = new System.Drawing.Size(120, 25);
            this.Height_numericUpDown.TabIndex = 30;
            this.Height_numericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Height_numericUpDown.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.Height_numericUpDown.ValueChanged += new System.EventHandler(this.Width_numericUpDown_ValueChanged);
            // 
            // Width_numericUpDown
            // 
            this.Width_numericUpDown.Location = new System.Drawing.Point(171, 184);
            this.Width_numericUpDown.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Width_numericUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.Width_numericUpDown.Name = "Width_numericUpDown";
            this.Width_numericUpDown.Size = new System.Drawing.Size(120, 25);
            this.Width_numericUpDown.TabIndex = 29;
            this.Width_numericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Width_numericUpDown.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.Width_numericUpDown.ValueChanged += new System.EventHandler(this.Height_numericUpDown_ValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(302, 192);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(82, 15);
            this.label7.TabIndex = 28;
            this.label7.Text = "输入高度：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(63, 188);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(82, 15);
            this.label6.TabIndex = 27;
            this.label6.Text = "输入宽度：";
            // 
            // OK_button
            // 
            this.OK_button.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OK_button.Location = new System.Drawing.Point(422, 322);
            this.OK_button.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.OK_button.Name = "OK_button";
            this.OK_button.Size = new System.Drawing.Size(93, 30);
            this.OK_button.TabIndex = 26;
            this.OK_button.Text = "确定";
            this.OK_button.UseVisualStyleBackColor = true;
            this.OK_button.Click += new System.EventHandler(this.OK_button_Click);
            // 
            // GPU_Status_label
            // 
            this.GPU_Status_label.AutoSize = true;
            this.GPU_Status_label.Location = new System.Drawing.Point(468, 289);
            this.GPU_Status_label.Name = "GPU_Status_label";
            this.GPU_Status_label.Size = new System.Drawing.Size(22, 15);
            this.GPU_Status_label.TabIndex = 25;
            this.GPU_Status_label.Text = "否";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(40, 289);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(106, 15);
            this.label5.TabIndex = 24;
            this.label5.Text = "GPU加速状态：";
            // 
            // Epochs_numericUpDown
            // 
            this.Epochs_numericUpDown.Location = new System.Drawing.Point(394, 234);
            this.Epochs_numericUpDown.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Epochs_numericUpDown.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.Epochs_numericUpDown.Name = "Epochs_numericUpDown";
            this.Epochs_numericUpDown.Size = new System.Drawing.Size(120, 25);
            this.Epochs_numericUpDown.TabIndex = 23;
            this.Epochs_numericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Epochs_numericUpDown.Value = new decimal(new int[] {
            30000,
            0,
            0,
            0});
            this.Epochs_numericUpDown.ValueChanged += new System.EventHandler(this.Epochs_numericUpDown_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(63, 236);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 15);
            this.label4.TabIndex = 22;
            this.label4.Text = "训练轮次：";
            // 
            // FeaturePicked_comboBox
            // 
            this.FeaturePicked_comboBox.FormattingEnabled = true;
            this.FeaturePicked_comboBox.Location = new System.Drawing.Point(171, 29);
            this.FeaturePicked_comboBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.FeaturePicked_comboBox.Name = "FeaturePicked_comboBox";
            this.FeaturePicked_comboBox.Size = new System.Drawing.Size(343, 23);
            this.FeaturePicked_comboBox.TabIndex = 21;
            this.FeaturePicked_comboBox.SelectedIndexChanged += new System.EventHandler(this.FeaturePicked_comboBox_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(34, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 15);
            this.label3.TabIndex = 20;
            this.label3.Text = "数据采集方法：";
            // 
            // CNN_SVMFrom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(558, 372);
            this.Controls.Add(this.source_image_comboBox);
            this.Controls.Add(this.soruce_image_label);
            this.Controls.Add(this.open_button);
            this.Controls.Add(this.open_sample_textBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Height_numericUpDown);
            this.Controls.Add(this.Width_numericUpDown);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.OK_button);
            this.Controls.Add(this.GPU_Status_label);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Epochs_numericUpDown);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.FeaturePicked_comboBox);
            this.Controls.Add(this.label3);
            this.Name = "CNN_SVMFrom";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "CNN_SVMFrom";
            ((System.ComponentModel.ISupportInitialize)(this.Height_numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Width_numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Epochs_numericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox source_image_comboBox;
        private System.Windows.Forms.Label soruce_image_label;
        private System.Windows.Forms.Button open_button;
        private System.Windows.Forms.TextBox open_sample_textBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown Height_numericUpDown;
        private System.Windows.Forms.NumericUpDown Width_numericUpDown;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button OK_button;
        private System.Windows.Forms.Label GPU_Status_label;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown Epochs_numericUpDown;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox FeaturePicked_comboBox;
        private System.Windows.Forms.Label label3;
    }
}