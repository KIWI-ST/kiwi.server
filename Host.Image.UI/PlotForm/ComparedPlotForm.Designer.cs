namespace Host.Image.UI.PlotForm
{
    partial class ComparedPlotForm
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
            this.x_min = new System.Windows.Forms.NumericUpDown();
            this.x_max = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.addline_button = new System.Windows.Forms.Button();
            this.update_button = new System.Windows.Forms.Button();
            this.title_textbox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.y_min = new System.Windows.Forms.TextBox();
            this.y_max = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lengend_textBox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.y_open_button = new System.Windows.Forms.Button();
            this.x_open_button = new System.Windows.Forms.Button();
            this.y_textBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.x_textBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.x_min)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.x_max)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(53, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "X轴数值范围:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(413, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(22, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "到";
            // 
            // x_min
            // 
            this.x_min.Location = new System.Drawing.Point(237, 20);
            this.x_min.Name = "x_min";
            this.x_min.Size = new System.Drawing.Size(120, 25);
            this.x_min.TabIndex = 2;
            // 
            // x_max
            // 
            this.x_max.Location = new System.Drawing.Point(483, 20);
            this.x_max.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.x_max.Name = "x_max";
            this.x_max.Size = new System.Drawing.Size(120, 25);
            this.x_max.TabIndex = 3;
            this.x_max.Value = new decimal(new int[] {
            3500,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(76, 341);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "点位信息:";
            // 
            // addline_button
            // 
            this.addline_button.Location = new System.Drawing.Point(725, 218);
            this.addline_button.Name = "addline_button";
            this.addline_button.Size = new System.Drawing.Size(88, 35);
            this.addline_button.TabIndex = 11;
            this.addline_button.Text = "添加折线";
            this.addline_button.UseVisualStyleBackColor = true;
            this.addline_button.Click += new System.EventHandler(this.addline_button_Click);
            // 
            // update_button
            // 
            this.update_button.Location = new System.Drawing.Point(742, 445);
            this.update_button.Name = "update_button";
            this.update_button.Size = new System.Drawing.Size(88, 34);
            this.update_button.TabIndex = 12;
            this.update_button.Text = "更新绘制";
            this.update_button.UseVisualStyleBackColor = true;
            this.update_button.Click += new System.EventHandler(this.button3_Click);
            // 
            // title_textbox
            // 
            this.title_textbox.Location = new System.Drawing.Point(237, 113);
            this.title_textbox.Name = "title_textbox";
            this.title_textbox.Size = new System.Drawing.Size(366, 25);
            this.title_textbox.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(104, 117);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(52, 15);
            this.label6.TabIndex = 14;
            this.label6.Text = "图名：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(413, 71);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(22, 15);
            this.label7.TabIndex = 17;
            this.label7.Text = "到";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(53, 71);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(98, 15);
            this.label8.TabIndex = 16;
            this.label8.Text = "Y轴数值范围:";
            // 
            // y_min
            // 
            this.y_min.Location = new System.Drawing.Point(237, 68);
            this.y_min.Name = "y_min";
            this.y_min.Size = new System.Drawing.Size(120, 25);
            this.y_min.TabIndex = 18;
            this.y_min.Text = "0";
            // 
            // y_max
            // 
            this.y_max.Location = new System.Drawing.Point(483, 68);
            this.y_max.Name = "y_max";
            this.y_max.Size = new System.Drawing.Size(118, 25);
            this.y_max.TabIndex = 19;
            this.y_max.Text = "1";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.White;
            this.groupBox1.Controls.Add(this.lengend_textBox);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.y_open_button);
            this.groupBox1.Controls.Add(this.x_open_button);
            this.groupBox1.Controls.Add(this.y_textBox);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.x_textBox);
            this.groupBox1.Controls.Add(this.addline_button);
            this.groupBox1.Location = new System.Drawing.Point(17, 150);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(837, 274);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "折线区";
            // 
            // lengend_textBox
            // 
            this.lengend_textBox.Location = new System.Drawing.Point(220, 181);
            this.lengend_textBox.Name = "lengend_textBox";
            this.lengend_textBox.Size = new System.Drawing.Size(366, 25);
            this.lengend_textBox.TabIndex = 19;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(72, 184);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(67, 15);
            this.label9.TabIndex = 18;
            this.label9.Text = "图例名：";
            // 
            // y_open_button
            // 
            this.y_open_button.Location = new System.Drawing.Point(592, 116);
            this.y_open_button.Name = "y_open_button";
            this.y_open_button.Size = new System.Drawing.Size(75, 23);
            this.y_open_button.TabIndex = 17;
            this.y_open_button.Text = "打开";
            this.y_open_button.UseVisualStyleBackColor = true;
            this.y_open_button.Click += new System.EventHandler(this.y_open_button_Click);
            // 
            // x_open_button
            // 
            this.x_open_button.Location = new System.Drawing.Point(592, 53);
            this.x_open_button.Name = "x_open_button";
            this.x_open_button.Size = new System.Drawing.Size(75, 23);
            this.x_open_button.TabIndex = 16;
            this.x_open_button.Text = "打开";
            this.x_open_button.UseVisualStyleBackColor = true;
            this.x_open_button.Click += new System.EventHandler(this.x_open_button_Click);
            // 
            // y_textBox
            // 
            this.y_textBox.Location = new System.Drawing.Point(220, 116);
            this.y_textBox.Name = "y_textBox";
            this.y_textBox.Size = new System.Drawing.Size(366, 25);
            this.y_textBox.TabIndex = 15;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(94, 119);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(45, 15);
            this.label5.TabIndex = 14;
            this.label5.Text = "y轴：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(94, 54);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 15);
            this.label4.TabIndex = 13;
            this.label4.Text = "x轴：";
            // 
            // x_textBox
            // 
            this.x_textBox.Location = new System.Drawing.Point(220, 51);
            this.x_textBox.Name = "x_textBox";
            this.x_textBox.Size = new System.Drawing.Size(366, 25);
            this.x_textBox.TabIndex = 12;
            // 
            // ComparedPlotForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(867, 488);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.y_max);
            this.Controls.Add(this.y_min);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.title_textbox);
            this.Controls.Add(this.update_button);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.x_max);
            this.Controls.Add(this.x_min);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "ComparedPlotForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ComparisonPoltForm";
            ((System.ComponentModel.ISupportInitialize)(this.x_min)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.x_max)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown x_min;
        private System.Windows.Forms.NumericUpDown x_max;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button addline_button;
        private System.Windows.Forms.Button update_button;
        private System.Windows.Forms.TextBox title_textbox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox y_min;
        private System.Windows.Forms.TextBox y_max;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox y_textBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox x_textBox;
        private System.Windows.Forms.Button y_open_button;
        private System.Windows.Forms.Button x_open_button;
        private System.Windows.Forms.TextBox lengend_textBox;
        private System.Windows.Forms.Label label9;
    }
}