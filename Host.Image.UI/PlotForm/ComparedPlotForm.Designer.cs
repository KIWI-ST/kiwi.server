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
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.position_textbox = new System.Windows.Forms.TextBox();
            this.value_textbox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.plot_name_textbox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.series_name_textbox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.y_min = new System.Windows.Forms.TextBox();
            this.y_max = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.x_min)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.x_max)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(138, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "X轴数值范围:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(498, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(22, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "到";
            // 
            // x_min
            // 
            this.x_min.Location = new System.Drawing.Point(322, 37);
            this.x_min.Name = "x_min";
            this.x_min.Size = new System.Drawing.Size(120, 25);
            this.x_min.TabIndex = 2;
            // 
            // x_max
            // 
            this.x_max.Location = new System.Drawing.Point(568, 37);
            this.x_max.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.x_max.Name = "x_max";
            this.x_max.Size = new System.Drawing.Size(120, 25);
            this.x_max.TabIndex = 3;
            this.x_max.Value = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(161, 168);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "点位信息:";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 15;
            this.listBox1.Location = new System.Drawing.Point(322, 168);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(366, 214);
            this.listBox1.TabIndex = 5;
            // 
            // position_textbox
            // 
            this.position_textbox.Location = new System.Drawing.Point(374, 416);
            this.position_textbox.Name = "position_textbox";
            this.position_textbox.Size = new System.Drawing.Size(100, 25);
            this.position_textbox.TabIndex = 6;
            // 
            // value_textbox
            // 
            this.value_textbox.Location = new System.Drawing.Point(588, 416);
            this.value_textbox.Name = "value_textbox";
            this.value_textbox.Size = new System.Drawing.Size(100, 25);
            this.value_textbox.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(292, 419);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 15);
            this.label4.TabIndex = 8;
            this.label4.Text = "位置：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(549, 419);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(22, 15);
            this.label5.TabIndex = 9;
            this.label5.Text = "值";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(737, 417);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "添加点";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(737, 539);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 11;
            this.button2.Text = "添加折线";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(864, 539);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 12;
            this.button3.Text = "绘制折线";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // plot_name_textbox
            // 
            this.plot_name_textbox.Location = new System.Drawing.Point(322, 123);
            this.plot_name_textbox.Name = "plot_name_textbox";
            this.plot_name_textbox.Size = new System.Drawing.Size(100, 25);
            this.plot_name_textbox.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(180, 123);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(37, 15);
            this.label6.TabIndex = 14;
            this.label6.Text = "图名";
            // 
            // series_name_textbox
            // 
            this.series_name_textbox.Location = new System.Drawing.Point(588, 537);
            this.series_name_textbox.Name = "series_name_textbox";
            this.series_name_textbox.Size = new System.Drawing.Size(100, 25);
            this.series_name_textbox.TabIndex = 15;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(498, 88);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(22, 15);
            this.label7.TabIndex = 17;
            this.label7.Text = "到";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(138, 88);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(98, 15);
            this.label8.TabIndex = 16;
            this.label8.Text = "Y轴数值范围:";
            // 
            // y_min
            // 
            this.y_min.Location = new System.Drawing.Point(322, 85);
            this.y_min.Name = "y_min";
            this.y_min.Size = new System.Drawing.Size(120, 25);
            this.y_min.TabIndex = 18;
            this.y_min.Text = "0";
            // 
            // y_max
            // 
            this.y_max.Location = new System.Drawing.Point(568, 85);
            this.y_max.Name = "y_max";
            this.y_max.Size = new System.Drawing.Size(118, 25);
            this.y_max.TabIndex = 19;
            this.y_max.Text = "1";
            // 
            // ComparedPlotForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1018, 615);
            this.Controls.Add(this.y_max);
            this.Controls.Add(this.y_min);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.series_name_textbox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.plot_name_textbox);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.value_textbox);
            this.Controls.Add(this.position_textbox);
            this.Controls.Add(this.listBox1);
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
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown x_min;
        private System.Windows.Forms.NumericUpDown x_max;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.TextBox position_textbox;
        private System.Windows.Forms.TextBox value_textbox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox plot_name_textbox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox series_name_textbox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox y_min;
        private System.Windows.Forms.TextBox y_max;
    }
}