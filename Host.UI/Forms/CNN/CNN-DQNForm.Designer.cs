namespace Host.UI.SettingForm
{
    partial class CNNDQNForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.FEATURE_LAYER_comboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.OPEN_SAMPLE_button = new System.Windows.Forms.Button();
            this.Depth_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.Height_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.Width_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.Epochs_CNN_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.Epochs_DQN_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.Depth_numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Height_numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Width_numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Epochs_CNN_numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Epochs_DQN_numericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(44, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "待分类影像：";
            // 
            // FEATURE_LAYER_comboBox
            // 
            this.FEATURE_LAYER_comboBox.FormattingEnabled = true;
            this.FEATURE_LAYER_comboBox.Location = new System.Drawing.Point(137, 25);
            this.FEATURE_LAYER_comboBox.Name = "FEATURE_LAYER_comboBox";
            this.FEATURE_LAYER_comboBox.Size = new System.Drawing.Size(247, 20);
            this.FEATURE_LAYER_comboBox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(56, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "样本文件：";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(137, 70);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(185, 21);
            this.textBox1.TabIndex = 3;
            // 
            // OPEN_SAMPLE_button
            // 
            this.OPEN_SAMPLE_button.Location = new System.Drawing.Point(328, 70);
            this.OPEN_SAMPLE_button.Name = "OPEN_SAMPLE_button";
            this.OPEN_SAMPLE_button.Size = new System.Drawing.Size(56, 20);
            this.OPEN_SAMPLE_button.TabIndex = 4;
            this.OPEN_SAMPLE_button.Text = "打开";
            this.OPEN_SAMPLE_button.UseVisualStyleBackColor = true;
            // 
            // Depth_numericUpDown
            // 
            this.Depth_numericUpDown.Location = new System.Drawing.Point(333, 117);
            this.Depth_numericUpDown.Margin = new System.Windows.Forms.Padding(2);
            this.Depth_numericUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.Depth_numericUpDown.Name = "Depth_numericUpDown";
            this.Depth_numericUpDown.Size = new System.Drawing.Size(49, 21);
            this.Depth_numericUpDown.TabIndex = 29;
            this.Depth_numericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Depth_numericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(288, 121);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 28;
            this.label3.Text = "深度：";
            // 
            // Height_numericUpDown
            // 
            this.Height_numericUpDown.Location = new System.Drawing.Point(235, 117);
            this.Height_numericUpDown.Margin = new System.Windows.Forms.Padding(2);
            this.Height_numericUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.Height_numericUpDown.Name = "Height_numericUpDown";
            this.Height_numericUpDown.Size = new System.Drawing.Size(49, 21);
            this.Height_numericUpDown.TabIndex = 27;
            this.Height_numericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Height_numericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // Width_numericUpDown
            // 
            this.Width_numericUpDown.Location = new System.Drawing.Point(137, 117);
            this.Width_numericUpDown.Margin = new System.Windows.Forms.Padding(2);
            this.Width_numericUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.Width_numericUpDown.Name = "Width_numericUpDown";
            this.Width_numericUpDown.Size = new System.Drawing.Size(49, 21);
            this.Width_numericUpDown.TabIndex = 26;
            this.Width_numericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Width_numericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(190, 121);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 12);
            this.label7.TabIndex = 25;
            this.label7.Text = "高度：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(79, 121);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 24;
            this.label6.Text = "宽度：";
            // 
            // Epochs_CNN_numericUpDown
            // 
            this.Epochs_CNN_numericUpDown.Location = new System.Drawing.Point(137, 165);
            this.Epochs_CNN_numericUpDown.Margin = new System.Windows.Forms.Padding(2);
            this.Epochs_CNN_numericUpDown.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.Epochs_CNN_numericUpDown.Name = "Epochs_CNN_numericUpDown";
            this.Epochs_CNN_numericUpDown.Size = new System.Drawing.Size(247, 21);
            this.Epochs_CNN_numericUpDown.TabIndex = 23;
            this.Epochs_CNN_numericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Epochs_CNN_numericUpDown.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(32, 169);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 12);
            this.label4.TabIndex = 22;
            this.label4.Text = "卷积训练轮次：";
            // 
            // Epochs_DQN_numericUpDown
            // 
            this.Epochs_DQN_numericUpDown.Location = new System.Drawing.Point(137, 216);
            this.Epochs_DQN_numericUpDown.Margin = new System.Windows.Forms.Padding(2);
            this.Epochs_DQN_numericUpDown.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.Epochs_DQN_numericUpDown.Name = "Epochs_DQN_numericUpDown";
            this.Epochs_DQN_numericUpDown.Size = new System.Drawing.Size(247, 21);
            this.Epochs_DQN_numericUpDown.TabIndex = 31;
            this.Epochs_DQN_numericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Epochs_DQN_numericUpDown.Value = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(32, 220);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 12);
            this.label5.TabIndex = 30;
            this.label5.Text = "强化训练轮次：";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(62, 265);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(59, 12);
            this.label8.TabIndex = 32;
            this.label8.Text = "GPU加速：";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(137, 265);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 12);
            this.label9.TabIndex = 33;
            this.label9.Text = "检查中";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(311, 260);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(73, 22);
            this.button1.TabIndex = 34;
            this.button1.Text = "开始";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // CNNDQNForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(444, 310);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.Epochs_DQN_numericUpDown);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Depth_numericUpDown);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Height_numericUpDown);
            this.Controls.Add(this.Width_numericUpDown);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.Epochs_CNN_numericUpDown);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.OPEN_SAMPLE_button);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.FEATURE_LAYER_comboBox);
            this.Controls.Add(this.label1);
            this.Name = "CNNDQNForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "CNN_DQNForm";
            ((System.ComponentModel.ISupportInitialize)(this.Depth_numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Height_numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Width_numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Epochs_CNN_numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Epochs_DQN_numericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox FEATURE_LAYER_comboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button OPEN_SAMPLE_button;
        private System.Windows.Forms.NumericUpDown Depth_numericUpDown;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown Height_numericUpDown;
        private System.Windows.Forms.NumericUpDown Width_numericUpDown;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown Epochs_CNN_numericUpDown;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown Epochs_DQN_numericUpDown;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button button1;
    }
}