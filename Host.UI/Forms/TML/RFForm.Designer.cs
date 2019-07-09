namespace Host.UI.SettingForm
{
    partial class RFForm
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
            this.tree_count_label = new System.Windows.Forms.Label();
            this.tree_count_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.sample_label = new System.Windows.Forms.Label();
            this.sample_file_textBox = new System.Windows.Forms.TextBox();
            this.open_button = new System.Windows.Forms.Button();
            this.base_groupBox = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.OK_Image_button = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.featurelayer_comboBox = new System.Windows.Forms.ComboBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.SAVE_FILE_textBox = new System.Windows.Forms.TextBox();
            this.WAIT_FILE_textBox = new System.Windows.Forms.TextBox();
            this.SAVE_FILE_button = new System.Windows.Forms.Button();
            this.WAIT_FILE_button = new System.Windows.Forms.Button();
            this.OK_Other_button = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.tree_count_numericUpDown)).BeginInit();
            this.base_groupBox.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tree_count_label
            // 
            this.tree_count_label.AutoSize = true;
            this.tree_count_label.Location = new System.Drawing.Point(36, 21);
            this.tree_count_label.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.tree_count_label.Name = "tree_count_label";
            this.tree_count_label.Size = new System.Drawing.Size(101, 12);
            this.tree_count_label.TabIndex = 0;
            this.tree_count_label.Text = "设置决策树个数：";
            // 
            // tree_count_numericUpDown
            // 
            this.tree_count_numericUpDown.Location = new System.Drawing.Point(141, 17);
            this.tree_count_numericUpDown.Margin = new System.Windows.Forms.Padding(2);
            this.tree_count_numericUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.tree_count_numericUpDown.Name = "tree_count_numericUpDown";
            this.tree_count_numericUpDown.Size = new System.Drawing.Size(176, 21);
            this.tree_count_numericUpDown.TabIndex = 1;
            this.tree_count_numericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tree_count_numericUpDown.Value = new decimal(new int[] {
            600,
            0,
            0,
            0});
            // 
            // sample_label
            // 
            this.sample_label.AutoSize = true;
            this.sample_label.Location = new System.Drawing.Point(72, 55);
            this.sample_label.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.sample_label.Name = "sample_label";
            this.sample_label.Size = new System.Drawing.Size(65, 12);
            this.sample_label.TabIndex = 2;
            this.sample_label.Text = "样本文件：";
            // 
            // sample_file_textBox
            // 
            this.sample_file_textBox.Location = new System.Drawing.Point(141, 51);
            this.sample_file_textBox.Margin = new System.Windows.Forms.Padding(2);
            this.sample_file_textBox.Name = "sample_file_textBox";
            this.sample_file_textBox.Size = new System.Drawing.Size(176, 21);
            this.sample_file_textBox.TabIndex = 3;
            // 
            // open_button
            // 
            this.open_button.Location = new System.Drawing.Point(338, 49);
            this.open_button.Margin = new System.Windows.Forms.Padding(2);
            this.open_button.Name = "open_button";
            this.open_button.Size = new System.Drawing.Size(56, 24);
            this.open_button.TabIndex = 6;
            this.open_button.Text = "打开";
            this.open_button.UseVisualStyleBackColor = true;
            this.open_button.Click += new System.EventHandler(this.open_button_Click);
            // 
            // base_groupBox
            // 
            this.base_groupBox.Controls.Add(this.open_button);
            this.base_groupBox.Controls.Add(this.tree_count_numericUpDown);
            this.base_groupBox.Controls.Add(this.tree_count_label);
            this.base_groupBox.Controls.Add(this.sample_label);
            this.base_groupBox.Controls.Add(this.sample_file_textBox);
            this.base_groupBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.base_groupBox.Location = new System.Drawing.Point(0, 0);
            this.base_groupBox.Name = "base_groupBox";
            this.base_groupBox.Size = new System.Drawing.Size(440, 84);
            this.base_groupBox.TabIndex = 9;
            this.base_groupBox.TabStop = false;
            this.base_groupBox.Text = "基本设置：";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 84);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(440, 128);
            this.tabControl1.TabIndex = 10;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.OK_Image_button);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.featurelayer_comboBox);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(432, 102);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "图像分类";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // OK_Image_button
            // 
            this.OK_Image_button.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OK_Image_button.Location = new System.Drawing.Point(325, 71);
            this.OK_Image_button.Name = "OK_Image_button";
            this.OK_Image_button.Size = new System.Drawing.Size(75, 23);
            this.OK_Image_button.TabIndex = 2;
            this.OK_Image_button.Text = "确定";
            this.OK_Image_button.UseVisualStyleBackColor = true;
            this.OK_Image_button.Click += new System.EventHandler(this.OK_Image_button_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "选择待分类图像：";
            // 
            // featurelayer_comboBox
            // 
            this.featurelayer_comboBox.FormattingEnabled = true;
            this.featurelayer_comboBox.Location = new System.Drawing.Point(137, 36);
            this.featurelayer_comboBox.Name = "featurelayer_comboBox";
            this.featurelayer_comboBox.Size = new System.Drawing.Size(176, 20);
            this.featurelayer_comboBox.TabIndex = 0;
            this.featurelayer_comboBox.SelectedIndexChanged += new System.EventHandler(this.featurelayer_comboBox_SelectedIndexChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.SAVE_FILE_textBox);
            this.tabPage2.Controls.Add(this.WAIT_FILE_textBox);
            this.tabPage2.Controls.Add(this.SAVE_FILE_button);
            this.tabPage2.Controls.Add(this.WAIT_FILE_button);
            this.tabPage2.Controls.Add(this.OK_Other_button);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(432, 102);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "其他分类";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // SAVE_FILE_textBox
            // 
            this.SAVE_FILE_textBox.Location = new System.Drawing.Point(137, 42);
            this.SAVE_FILE_textBox.Name = "SAVE_FILE_textBox";
            this.SAVE_FILE_textBox.Size = new System.Drawing.Size(176, 21);
            this.SAVE_FILE_textBox.TabIndex = 10;
            // 
            // WAIT_FILE_textBox
            // 
            this.WAIT_FILE_textBox.Location = new System.Drawing.Point(137, 11);
            this.WAIT_FILE_textBox.Name = "WAIT_FILE_textBox";
            this.WAIT_FILE_textBox.Size = new System.Drawing.Size(176, 21);
            this.WAIT_FILE_textBox.TabIndex = 9;
            // 
            // SAVE_FILE_button
            // 
            this.SAVE_FILE_button.Location = new System.Drawing.Point(334, 41);
            this.SAVE_FILE_button.Margin = new System.Windows.Forms.Padding(2);
            this.SAVE_FILE_button.Name = "SAVE_FILE_button";
            this.SAVE_FILE_button.Size = new System.Drawing.Size(56, 24);
            this.SAVE_FILE_button.TabIndex = 8;
            this.SAVE_FILE_button.Text = "打开";
            this.SAVE_FILE_button.UseVisualStyleBackColor = true;
            this.SAVE_FILE_button.Click += new System.EventHandler(this.SAVE_FILE_button_Click);
            // 
            // WAIT_FILE_button
            // 
            this.WAIT_FILE_button.Location = new System.Drawing.Point(334, 10);
            this.WAIT_FILE_button.Margin = new System.Windows.Forms.Padding(2);
            this.WAIT_FILE_button.Name = "WAIT_FILE_button";
            this.WAIT_FILE_button.Size = new System.Drawing.Size(56, 24);
            this.WAIT_FILE_button.TabIndex = 7;
            this.WAIT_FILE_button.Text = "打开";
            this.WAIT_FILE_button.UseVisualStyleBackColor = true;
            this.WAIT_FILE_button.Click += new System.EventHandler(this.WAIT_FILE_button_Click);
            // 
            // OK_Other_button
            // 
            this.OK_Other_button.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OK_Other_button.Location = new System.Drawing.Point(325, 71);
            this.OK_Other_button.Name = "OK_Other_button";
            this.OK_Other_button.Size = new System.Drawing.Size(75, 23);
            this.OK_Other_button.TabIndex = 2;
            this.OK_Other_button.Text = "确定";
            this.OK_Other_button.UseVisualStyleBackColor = true;
            this.OK_Other_button.Click += new System.EventHandler(this.OK_Other_button_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(44, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "结果保存地址：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(56, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "待分类文件：";
            // 
            // RFForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(440, 212);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.base_groupBox);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "RFForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "RandomForestForm";
            ((System.ComponentModel.ISupportInitialize)(this.tree_count_numericUpDown)).EndInit();
            this.base_groupBox.ResumeLayout(false);
            this.base_groupBox.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label tree_count_label;
        private System.Windows.Forms.NumericUpDown tree_count_numericUpDown;
        private System.Windows.Forms.Label sample_label;
        private System.Windows.Forms.TextBox sample_file_textBox;
        private System.Windows.Forms.Button open_button;
        private System.Windows.Forms.GroupBox base_groupBox;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button OK_Image_button;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox featurelayer_comboBox;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox SAVE_FILE_textBox;
        private System.Windows.Forms.TextBox WAIT_FILE_textBox;
        private System.Windows.Forms.Button SAVE_FILE_button;
        private System.Windows.Forms.Button WAIT_FILE_button;
        private System.Windows.Forms.Button OK_Other_button;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
    }
}