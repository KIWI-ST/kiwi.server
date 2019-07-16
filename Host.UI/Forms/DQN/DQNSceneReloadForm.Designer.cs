namespace Host.UI.Forms.DQN
{
    partial class DQNSceneReloadForm
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
            this.OK_button = new System.Windows.Forms.Button();
            this.Apply_button = new System.Windows.Forms.Button();
            this.Apply_Directory_textBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SampleBatches_button = new System.Windows.Forms.Button();
            this.Sample_Batches_Directory_textBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.DQN_button = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.DQN_Directory_textBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.Epochs_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.Switch_Epoch_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Epochs_numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Switch_Epoch_numericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // OK_button
            // 
            this.OK_button.Location = new System.Drawing.Point(557, 378);
            this.OK_button.Name = "OK_button";
            this.OK_button.Size = new System.Drawing.Size(83, 32);
            this.OK_button.TabIndex = 15;
            this.OK_button.Text = "确定";
            this.OK_button.UseVisualStyleBackColor = true;
            this.OK_button.Click += new System.EventHandler(this.OK_button_Click);
            // 
            // Apply_button
            // 
            this.Apply_button.Location = new System.Drawing.Point(557, 108);
            this.Apply_button.Name = "Apply_button";
            this.Apply_button.Size = new System.Drawing.Size(83, 32);
            this.Apply_button.TabIndex = 14;
            this.Apply_button.Text = "设置";
            this.Apply_button.UseVisualStyleBackColor = true;
            this.Apply_button.Click += new System.EventHandler(this.Apply_button_Click);
            // 
            // Apply_Directory_textBox
            // 
            this.Apply_Directory_textBox.Location = new System.Drawing.Point(249, 110);
            this.Apply_Directory_textBox.Name = "Apply_Directory_textBox";
            this.Apply_Directory_textBox.Size = new System.Drawing.Size(302, 28);
            this.Apply_Directory_textBox.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(32, 115);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(134, 18);
            this.label3.TabIndex = 12;
            this.label3.Text = "待分类图片数据";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(164, 115);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 18);
            this.label2.TabIndex = 11;
            this.label2.Text = "[可选]:";
            // 
            // SampleBatches_button
            // 
            this.SampleBatches_button.Location = new System.Drawing.Point(557, 43);
            this.SampleBatches_button.Name = "SampleBatches_button";
            this.SampleBatches_button.Size = new System.Drawing.Size(83, 32);
            this.SampleBatches_button.TabIndex = 10;
            this.SampleBatches_button.Text = "设置";
            this.SampleBatches_button.UseVisualStyleBackColor = true;
            this.SampleBatches_button.Click += new System.EventHandler(this.SampleBatches_button_Click);
            // 
            // Sample_Batches_Directory_textBox
            // 
            this.Sample_Batches_Directory_textBox.Location = new System.Drawing.Point(249, 45);
            this.Sample_Batches_Directory_textBox.Name = "Sample_Batches_Directory_textBox";
            this.Sample_Batches_Directory_textBox.Size = new System.Drawing.Size(302, 28);
            this.Sample_Batches_Directory_textBox.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(127, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 18);
            this.label1.TabIndex = 8;
            this.label1.Text = "样本根目录：";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.SampleBatches_button);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.Apply_button);
            this.groupBox1.Controls.Add(this.Sample_Batches_Directory_textBox);
            this.groupBox1.Controls.Add(this.Apply_Directory_textBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(706, 172);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "样本设置";
            // 
            // DQN_button
            // 
            this.DQN_button.Location = new System.Drawing.Point(557, 317);
            this.DQN_button.Name = "DQN_button";
            this.DQN_button.Size = new System.Drawing.Size(83, 32);
            this.DQN_button.TabIndex = 19;
            this.DQN_button.Text = "设置";
            this.DQN_button.UseVisualStyleBackColor = true;
            this.DQN_button.Click += new System.EventHandler(this.DQN_button_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(108, 324);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(134, 18);
            this.label4.TabIndex = 17;
            this.label4.Text = "载入模型目录：";
            // 
            // DQN_Directory_textBox
            // 
            this.DQN_Directory_textBox.Location = new System.Drawing.Point(249, 319);
            this.DQN_Directory_textBox.Name = "DQN_Directory_textBox";
            this.DQN_Directory_textBox.Size = new System.Drawing.Size(302, 28);
            this.DQN_Directory_textBox.TabIndex = 18;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(146, 208);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(98, 18);
            this.label5.TabIndex = 20;
            this.label5.Text = "训练轮次：";
            // 
            // Epochs_numericUpDown
            // 
            this.Epochs_numericUpDown.Location = new System.Drawing.Point(250, 203);
            this.Epochs_numericUpDown.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.Epochs_numericUpDown.Name = "Epochs_numericUpDown";
            this.Epochs_numericUpDown.Size = new System.Drawing.Size(120, 28);
            this.Epochs_numericUpDown.TabIndex = 21;
            this.Epochs_numericUpDown.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // Switch_Epoch_numericUpDown
            // 
            this.Switch_Epoch_numericUpDown.Location = new System.Drawing.Point(249, 259);
            this.Switch_Epoch_numericUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.Switch_Epoch_numericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.Switch_Epoch_numericUpDown.Name = "Switch_Epoch_numericUpDown";
            this.Switch_Epoch_numericUpDown.Size = new System.Drawing.Size(120, 28);
            this.Switch_Epoch_numericUpDown.TabIndex = 23;
            this.Switch_Epoch_numericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(110, 264);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(134, 18);
            this.label6.TabIndex = 22;
            this.label6.Text = "环境切换轮次：";
            // 
            // DQNSceneReloadForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(706, 440);
            this.Controls.Add(this.Switch_Epoch_numericUpDown);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.Epochs_numericUpDown);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.DQN_button);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.DQN_Directory_textBox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.OK_button);
            this.Name = "DQNSceneReloadForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "DQNSceneReloadForm";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Epochs_numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Switch_Epoch_numericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OK_button;
        private System.Windows.Forms.Button Apply_button;
        private System.Windows.Forms.TextBox Apply_Directory_textBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button SampleBatches_button;
        private System.Windows.Forms.TextBox Sample_Batches_Directory_textBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button DQN_button;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox DQN_Directory_textBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown Epochs_numericUpDown;
        private System.Windows.Forms.NumericUpDown Switch_Epoch_numericUpDown;
        private System.Windows.Forms.Label label6;
    }
}