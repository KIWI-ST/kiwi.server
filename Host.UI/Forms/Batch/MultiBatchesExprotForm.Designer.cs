namespace Host.UI.Forms
{
    partial class MultiBatchesExprotForm
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
            this.Root_Directory_textBox = new System.Windows.Forms.TextBox();
            this.Setting_Root_button = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.Pick_Method_comboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Each_Class_Size_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.Repeat_Pick_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.Start_button = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.Pick_Band_Count_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.Output_Samples_button = new System.Windows.Forms.Button();
            this.Output_Samples_textBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Each_Class_Size_numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Repeat_Pick_numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pick_Band_Count_numericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(70, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "样本根目录：";
            // 
            // Root_Directory_textBox
            // 
            this.Root_Directory_textBox.Location = new System.Drawing.Point(192, 39);
            this.Root_Directory_textBox.Name = "Root_Directory_textBox";
            this.Root_Directory_textBox.Size = new System.Drawing.Size(228, 28);
            this.Root_Directory_textBox.TabIndex = 1;
            // 
            // Setting_Root_button
            // 
            this.Setting_Root_button.Location = new System.Drawing.Point(436, 37);
            this.Setting_Root_button.Name = "Setting_Root_button";
            this.Setting_Root_button.Size = new System.Drawing.Size(82, 32);
            this.Setting_Root_button.TabIndex = 2;
            this.Setting_Root_button.Text = "设置";
            this.Setting_Root_button.UseVisualStyleBackColor = true;
            this.Setting_Root_button.Click += new System.EventHandler(this.Setting_Root_button_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(89, 92);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 18);
            this.label2.TabIndex = 3;
            this.label2.Text = "采集方式：";
            // 
            // Pick_Method_comboBox
            // 
            this.Pick_Method_comboBox.FormattingEnabled = true;
            this.Pick_Method_comboBox.Location = new System.Drawing.Point(192, 89);
            this.Pick_Method_comboBox.Name = "Pick_Method_comboBox";
            this.Pick_Method_comboBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Pick_Method_comboBox.Size = new System.Drawing.Size(228, 26);
            this.Pick_Method_comboBox.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(78, 207);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(116, 18);
            this.label3.TabIndex = 5;
            this.label3.Text = "每类样本数：";
            // 
            // Each_Class_Size_numericUpDown
            // 
            this.Each_Class_Size_numericUpDown.Location = new System.Drawing.Point(204, 204);
            this.Each_Class_Size_numericUpDown.Name = "Each_Class_Size_numericUpDown";
            this.Each_Class_Size_numericUpDown.Size = new System.Drawing.Size(120, 28);
            this.Each_Class_Size_numericUpDown.TabIndex = 6;
            this.Each_Class_Size_numericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Each_Class_Size_numericUpDown.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(61, 267);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(134, 18);
            this.label4.TabIndex = 7;
            this.label4.Text = "重复采集次数：";
            // 
            // Repeat_Pick_numericUpDown
            // 
            this.Repeat_Pick_numericUpDown.Location = new System.Drawing.Point(204, 262);
            this.Repeat_Pick_numericUpDown.Name = "Repeat_Pick_numericUpDown";
            this.Repeat_Pick_numericUpDown.Size = new System.Drawing.Size(120, 28);
            this.Repeat_Pick_numericUpDown.TabIndex = 8;
            this.Repeat_Pick_numericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Repeat_Pick_numericUpDown.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // Start_button
            // 
            this.Start_button.Location = new System.Drawing.Point(436, 366);
            this.Start_button.Name = "Start_button";
            this.Start_button.Size = new System.Drawing.Size(82, 32);
            this.Start_button.TabIndex = 9;
            this.Start_button.Text = "开始";
            this.Start_button.UseVisualStyleBackColor = true;
            this.Start_button.Click += new System.EventHandler(this.Start_button_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(77, 152);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(116, 18);
            this.label5.TabIndex = 11;
            this.label5.Text = "采集波段数：";
            // 
            // Pick_Band_Count_numericUpDown
            // 
            this.Pick_Band_Count_numericUpDown.Enabled = false;
            this.Pick_Band_Count_numericUpDown.Location = new System.Drawing.Point(204, 147);
            this.Pick_Band_Count_numericUpDown.Name = "Pick_Band_Count_numericUpDown";
            this.Pick_Band_Count_numericUpDown.Size = new System.Drawing.Size(120, 28);
            this.Pick_Band_Count_numericUpDown.TabIndex = 12;
            this.Pick_Band_Count_numericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Pick_Band_Count_numericUpDown.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // Output_Samples_button
            // 
            this.Output_Samples_button.Location = new System.Drawing.Point(436, 311);
            this.Output_Samples_button.Name = "Output_Samples_button";
            this.Output_Samples_button.Size = new System.Drawing.Size(82, 32);
            this.Output_Samples_button.TabIndex = 15;
            this.Output_Samples_button.Text = "设置";
            this.Output_Samples_button.UseVisualStyleBackColor = true;
            this.Output_Samples_button.Click += new System.EventHandler(this.Output_Samples_button_Click);
            // 
            // Output_Samples_textBox
            // 
            this.Output_Samples_textBox.Location = new System.Drawing.Point(202, 313);
            this.Output_Samples_textBox.Name = "Output_Samples_textBox";
            this.Output_Samples_textBox.Size = new System.Drawing.Size(228, 28);
            this.Output_Samples_textBox.TabIndex = 14;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(64, 318);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(134, 18);
            this.label6.TabIndex = 13;
            this.label6.Text = "存放样本目录：";
            // 
            // MultiBatchesExprotForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(590, 435);
            this.Controls.Add(this.Output_Samples_button);
            this.Controls.Add(this.Output_Samples_textBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.Pick_Band_Count_numericUpDown);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Start_button);
            this.Controls.Add(this.Repeat_Pick_numericUpDown);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.Each_Class_Size_numericUpDown);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Pick_Method_comboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Setting_Root_button);
            this.Controls.Add(this.Root_Directory_textBox);
            this.Controls.Add(this.label1);
            this.Name = "MultiBatchesExprotForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "MultiBatchExprotForm";
            ((System.ComponentModel.ISupportInitialize)(this.Each_Class_Size_numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Repeat_Pick_numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pick_Band_Count_numericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Root_Directory_textBox;
        private System.Windows.Forms.Button Setting_Root_button;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox Pick_Method_comboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown Each_Class_Size_numericUpDown;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown Repeat_Pick_numericUpDown;
        private System.Windows.Forms.Button Start_button;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown Pick_Band_Count_numericUpDown;
        private System.Windows.Forms.Button Output_Samples_button;
        private System.Windows.Forms.TextBox Output_Samples_textBox;
        private System.Windows.Forms.Label label6;
    }
}