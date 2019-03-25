﻿namespace Host.UI.SettingForm
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
            this.label3 = new System.Windows.Forms.Label();
            this.FeaturePicked_comboBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.Epochs_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.GPU_Status_label = new System.Windows.Forms.Label();
            this.OK_button = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.Width_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.Height_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.open_sample_textBox = new System.Windows.Forms.TextBox();
            this.open_button = new System.Windows.Forms.Button();
            this.soruce_image_label = new System.Windows.Forms.Label();
            this.source_image_comboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.DEPTH_numericUpDown = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.Epochs_numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Width_numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Height_numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DEPTH_numericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(38, 39);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "数据采集方法：";
            // 
            // FeaturePicked_comboBox
            // 
            this.FeaturePicked_comboBox.FormattingEnabled = true;
            this.FeaturePicked_comboBox.Location = new System.Drawing.Point(141, 36);
            this.FeaturePicked_comboBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.FeaturePicked_comboBox.Name = "FeaturePicked_comboBox";
            this.FeaturePicked_comboBox.Size = new System.Drawing.Size(258, 20);
            this.FeaturePicked_comboBox.TabIndex = 5;
            this.FeaturePicked_comboBox.SelectedIndexChanged += new System.EventHandler(this.FeaturePicked_comboBox_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(58, 191);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "训练轮次：";
            // 
            // Epochs_numericUpDown
            // 
            this.Epochs_numericUpDown.Location = new System.Drawing.Point(305, 190);
            this.Epochs_numericUpDown.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Epochs_numericUpDown.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.Epochs_numericUpDown.Name = "Epochs_numericUpDown";
            this.Epochs_numericUpDown.Size = new System.Drawing.Size(90, 21);
            this.Epochs_numericUpDown.TabIndex = 7;
            this.Epochs_numericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Epochs_numericUpDown.Value = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.Epochs_numericUpDown.ValueChanged += new System.EventHandler(this.Epochs_numericUpDown_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(44, 233);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(83, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "GPU加速状态：";
            // 
            // GPU_Status_label
            // 
            this.GPU_Status_label.AutoSize = true;
            this.GPU_Status_label.Location = new System.Drawing.Point(382, 233);
            this.GPU_Status_label.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.GPU_Status_label.Name = "GPU_Status_label";
            this.GPU_Status_label.Size = new System.Drawing.Size(17, 12);
            this.GPU_Status_label.TabIndex = 9;
            this.GPU_Status_label.Text = "否";
            // 
            // OK_button
            // 
            this.OK_button.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OK_button.Location = new System.Drawing.Point(328, 266);
            this.OK_button.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.OK_button.Name = "OK_button";
            this.OK_button.Size = new System.Drawing.Size(70, 24);
            this.OK_button.TabIndex = 10;
            this.OK_button.Text = "确定";
            this.OK_button.UseVisualStyleBackColor = true;
            this.OK_button.Click += new System.EventHandler(this.OK_button_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(82, 159);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 11;
            this.label6.Text = "宽度：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(201, 160);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 12);
            this.label7.TabIndex = 12;
            this.label7.Text = "高度：";
            // 
            // Width_numericUpDown
            // 
            this.Width_numericUpDown.Location = new System.Drawing.Point(140, 156);
            this.Width_numericUpDown.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Width_numericUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.Width_numericUpDown.Name = "Width_numericUpDown";
            this.Width_numericUpDown.Size = new System.Drawing.Size(49, 21);
            this.Width_numericUpDown.TabIndex = 13;
            this.Width_numericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Width_numericUpDown.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.Width_numericUpDown.ValueChanged += new System.EventHandler(this.Width_numericUpDown_ValueChanged);
            // 
            // Height_numericUpDown
            // 
            this.Height_numericUpDown.Location = new System.Drawing.Point(242, 156);
            this.Height_numericUpDown.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Height_numericUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.Height_numericUpDown.Name = "Height_numericUpDown";
            this.Height_numericUpDown.Size = new System.Drawing.Size(49, 21);
            this.Height_numericUpDown.TabIndex = 14;
            this.Height_numericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Height_numericUpDown.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.Height_numericUpDown.ValueChanged += new System.EventHandler(this.Height_numericUpDown_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(60, 119);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 15;
            this.label1.Text = "样本文件：";
            // 
            // open_sample_textBox
            // 
            this.open_sample_textBox.Location = new System.Drawing.Point(141, 116);
            this.open_sample_textBox.Name = "open_sample_textBox";
            this.open_sample_textBox.Size = new System.Drawing.Size(182, 21);
            this.open_sample_textBox.TabIndex = 16;
            // 
            // open_button
            // 
            this.open_button.Location = new System.Drawing.Point(328, 115);
            this.open_button.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.open_button.Name = "open_button";
            this.open_button.Size = new System.Drawing.Size(70, 24);
            this.open_button.TabIndex = 17;
            this.open_button.Text = "打开";
            this.open_button.UseVisualStyleBackColor = true;
            this.open_button.Click += new System.EventHandler(this.open_sample_button_Click);
            // 
            // soruce_image_label
            // 
            this.soruce_image_label.AutoSize = true;
            this.soruce_image_label.Location = new System.Drawing.Point(50, 79);
            this.soruce_image_label.Name = "soruce_image_label";
            this.soruce_image_label.Size = new System.Drawing.Size(77, 12);
            this.soruce_image_label.TabIndex = 18;
            this.soruce_image_label.Text = "待分类影像：";
            // 
            // source_image_comboBox
            // 
            this.source_image_comboBox.FormattingEnabled = true;
            this.source_image_comboBox.Location = new System.Drawing.Point(141, 76);
            this.source_image_comboBox.Name = "source_image_comboBox";
            this.source_image_comboBox.Size = new System.Drawing.Size(258, 20);
            this.source_image_comboBox.TabIndex = 19;
            this.source_image_comboBox.SelectedIndexChanged += new System.EventHandler(this.Source_comboBox_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(305, 160);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 20;
            this.label2.Text = "深度：";
            // 
            // DEPTH_numericUpDown
            // 
            this.DEPTH_numericUpDown.Location = new System.Drawing.Point(346, 156);
            this.DEPTH_numericUpDown.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.DEPTH_numericUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.DEPTH_numericUpDown.Name = "DEPTH_numericUpDown";
            this.DEPTH_numericUpDown.Size = new System.Drawing.Size(49, 21);
            this.DEPTH_numericUpDown.TabIndex = 21;
            this.DEPTH_numericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.DEPTH_numericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.DEPTH_numericUpDown.ValueChanged += new System.EventHandler(this.DEPTH_numericUpDown_ValueChanged);
            // 
            // CNNForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(440, 320);
            this.Controls.Add(this.DEPTH_numericUpDown);
            this.Controls.Add(this.label2);
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
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "CNNForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "CNNForm";
            ((System.ComponentModel.ISupportInitialize)(this.Epochs_numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Width_numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Height_numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DEPTH_numericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox FeaturePicked_comboBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown Epochs_numericUpDown;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label GPU_Status_label;
        private System.Windows.Forms.Button OK_button;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown Width_numericUpDown;
        private System.Windows.Forms.NumericUpDown Height_numericUpDown;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox open_sample_textBox;
        private System.Windows.Forms.Button open_button;
        private System.Windows.Forms.Label soruce_image_label;
        private System.Windows.Forms.ComboBox source_image_comboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown DEPTH_numericUpDown;
    }
}