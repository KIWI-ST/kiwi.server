namespace Host.Image.UI.SettingForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Source_comboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Label_comboBox = new System.Windows.Forms.ComboBox();
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
            ((System.ComponentModel.ISupportInitialize)(this.Epochs_numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Width_numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Height_numericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(96, 103);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "数据源：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(96, 163);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "标注图：";
            // 
            // Source_comboBox
            // 
            this.Source_comboBox.FormattingEnabled = true;
            this.Source_comboBox.Location = new System.Drawing.Point(190, 98);
            this.Source_comboBox.Name = "Source_comboBox";
            this.Source_comboBox.Size = new System.Drawing.Size(342, 23);
            this.Source_comboBox.TabIndex = 2;
            this.Source_comboBox.SelectedIndexChanged += new System.EventHandler(this.Source_comboBox_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(51, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 15);
            this.label3.TabIndex = 3;
            this.label3.Text = "数据采集方法：";
            // 
            // Label_comboBox
            // 
            this.Label_comboBox.FormattingEnabled = true;
            this.Label_comboBox.Location = new System.Drawing.Point(190, 159);
            this.Label_comboBox.Name = "Label_comboBox";
            this.Label_comboBox.Size = new System.Drawing.Size(342, 23);
            this.Label_comboBox.TabIndex = 4;
            this.Label_comboBox.SelectedIndexChanged += new System.EventHandler(this.Label_comboBox_SelectedIndexChanged);
            // 
            // FeaturePicked_comboBox
            // 
            this.FeaturePicked_comboBox.FormattingEnabled = true;
            this.FeaturePicked_comboBox.Location = new System.Drawing.Point(190, 39);
            this.FeaturePicked_comboBox.Name = "FeaturePicked_comboBox";
            this.FeaturePicked_comboBox.Size = new System.Drawing.Size(342, 23);
            this.FeaturePicked_comboBox.TabIndex = 5;
            this.FeaturePicked_comboBox.SelectedIndexChanged += new System.EventHandler(this.FeaturePicked_comboBox_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(81, 273);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 15);
            this.label4.TabIndex = 6;
            this.label4.Text = "训练轮次：";
            // 
            // Epochs_numericUpDown
            // 
            this.Epochs_numericUpDown.Location = new System.Drawing.Point(412, 271);
            this.Epochs_numericUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.Epochs_numericUpDown.Name = "Epochs_numericUpDown";
            this.Epochs_numericUpDown.Size = new System.Drawing.Size(120, 25);
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
            this.label5.Location = new System.Drawing.Point(57, 332);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(106, 15);
            this.label5.TabIndex = 8;
            this.label5.Text = "GPU加速状态：";
            // 
            // GPU_Status_label
            // 
            this.GPU_Status_label.AutoSize = true;
            this.GPU_Status_label.Location = new System.Drawing.Point(476, 332);
            this.GPU_Status_label.Name = "GPU_Status_label";
            this.GPU_Status_label.Size = new System.Drawing.Size(22, 15);
            this.GPU_Status_label.TabIndex = 9;
            this.GPU_Status_label.Text = "否";
            // 
            // OK_button
            // 
            this.OK_button.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OK_button.Location = new System.Drawing.Point(465, 379);
            this.OK_button.Name = "OK_button";
            this.OK_button.Size = new System.Drawing.Size(118, 36);
            this.OK_button.TabIndex = 10;
            this.OK_button.Text = "确定";
            this.OK_button.UseVisualStyleBackColor = true;
            this.OK_button.Click += new System.EventHandler(this.OK_button_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(81, 220);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(82, 15);
            this.label6.TabIndex = 11;
            this.label6.Text = "输入宽度：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(324, 220);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(82, 15);
            this.label7.TabIndex = 12;
            this.label7.Text = "输入高度：";
            // 
            // Width_numericUpDown
            // 
            this.Width_numericUpDown.Location = new System.Drawing.Point(190, 216);
            this.Width_numericUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.Width_numericUpDown.Name = "Width_numericUpDown";
            this.Width_numericUpDown.Size = new System.Drawing.Size(120, 25);
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
            this.Height_numericUpDown.Location = new System.Drawing.Point(412, 216);
            this.Height_numericUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.Height_numericUpDown.Name = "Height_numericUpDown";
            this.Height_numericUpDown.Size = new System.Drawing.Size(120, 25);
            this.Height_numericUpDown.TabIndex = 14;
            this.Height_numericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Height_numericUpDown.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.Height_numericUpDown.ValueChanged += new System.EventHandler(this.Height_numericUpDown_ValueChanged);
            // 
            // CNNForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(608, 437);
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
            this.Controls.Add(this.Label_comboBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Source_comboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "CNNForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "CNNForm";
            ((System.ComponentModel.ISupportInitialize)(this.Epochs_numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Width_numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Height_numericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox Source_comboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox Label_comboBox;
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
    }
}