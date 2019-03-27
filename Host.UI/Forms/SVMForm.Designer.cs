namespace Host.UI.SettingForm
{
    partial class SVMFrom
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
            this.featurelayer_comboBox = new System.Windows.Forms.ComboBox();
            this.featurel_layer_label = new System.Windows.Forms.Label();
            this.open_button = new System.Windows.Forms.Button();
            this.ok_other_button = new System.Windows.Forms.Button();
            this.sample_file_textBox = new System.Windows.Forms.TextBox();
            this.sample_label = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.button3 = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.open_save_button = new System.Windows.Forms.Button();
            this.open_wait_file_button = new System.Windows.Forms.Button();
            this.save_file_textBox = new System.Windows.Forms.TextBox();
            this.wait_file_textBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // featurelayer_comboBox
            // 
            this.featurelayer_comboBox.FormattingEnabled = true;
            this.featurelayer_comboBox.Location = new System.Drawing.Point(158, 86);
            this.featurelayer_comboBox.Margin = new System.Windows.Forms.Padding(2);
            this.featurelayer_comboBox.Name = "featurelayer_comboBox";
            this.featurelayer_comboBox.Size = new System.Drawing.Size(171, 20);
            this.featurelayer_comboBox.TabIndex = 14;
            this.featurelayer_comboBox.SelectedIndexChanged += new System.EventHandler(this.featurelayer_comboBox_SelectedIndexChanged);
            // 
            // featurel_layer_label
            // 
            this.featurel_layer_label.AutoSize = true;
            this.featurel_layer_label.Location = new System.Drawing.Point(68, 89);
            this.featurel_layer_label.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.featurel_layer_label.Name = "featurel_layer_label";
            this.featurel_layer_label.Size = new System.Drawing.Size(59, 12);
            this.featurel_layer_label.TabIndex = 13;
            this.featurel_layer_label.Text = "待分类图:";
            // 
            // open_button
            // 
            this.open_button.Location = new System.Drawing.Point(351, 40);
            this.open_button.Margin = new System.Windows.Forms.Padding(2);
            this.open_button.Name = "open_button";
            this.open_button.Size = new System.Drawing.Size(56, 24);
            this.open_button.TabIndex = 12;
            this.open_button.Text = "打开";
            this.open_button.UseVisualStyleBackColor = true;
            this.open_button.Click += new System.EventHandler(this.open_sample_button_Click);
            // 
            // ok_other_button
            // 
            this.ok_other_button.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ok_other_button.Location = new System.Drawing.Point(347, 133);
            this.ok_other_button.Margin = new System.Windows.Forms.Padding(2);
            this.ok_other_button.Name = "ok_other_button";
            this.ok_other_button.Size = new System.Drawing.Size(56, 24);
            this.ok_other_button.TabIndex = 11;
            this.ok_other_button.Text = "确定";
            this.ok_other_button.UseVisualStyleBackColor = true;
            this.ok_other_button.Click += new System.EventHandler(this.ok_other_button_Click);
            // 
            // sample_file_textBox
            // 
            this.sample_file_textBox.Location = new System.Drawing.Point(158, 42);
            this.sample_file_textBox.Margin = new System.Windows.Forms.Padding(2);
            this.sample_file_textBox.Name = "sample_file_textBox";
            this.sample_file_textBox.Size = new System.Drawing.Size(171, 21);
            this.sample_file_textBox.TabIndex = 10;
            // 
            // sample_label
            // 
            this.sample_label.AutoSize = true;
            this.sample_label.Location = new System.Drawing.Point(62, 45);
            this.sample_label.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.sample_label.Name = "sample_label";
            this.sample_label.Size = new System.Drawing.Size(65, 12);
            this.sample_label.TabIndex = 9;
            this.sample_label.Text = "样本文件：";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(472, 190);
            this.tabControl1.TabIndex = 15;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.button3);
            this.tabPage1.Controls.Add(this.featurelayer_comboBox);
            this.tabPage1.Controls.Add(this.featurel_layer_label);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(464, 164);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "图像分类";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button3.Location = new System.Drawing.Point(347, 133);
            this.button3.Margin = new System.Windows.Forms.Padding(2);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(56, 24);
            this.button3.TabIndex = 15;
            this.button3.Text = "确定";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.OK_Image_button_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.open_save_button);
            this.tabPage2.Controls.Add(this.open_wait_file_button);
            this.tabPage2.Controls.Add(this.save_file_textBox);
            this.tabPage2.Controls.Add(this.wait_file_textBox);
            this.tabPage2.Controls.Add(this.ok_other_button);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(464, 164);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "其他分类";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // open_save_button
            // 
            this.open_save_button.Location = new System.Drawing.Point(347, 98);
            this.open_save_button.Margin = new System.Windows.Forms.Padding(2);
            this.open_save_button.Name = "open_save_button";
            this.open_save_button.Size = new System.Drawing.Size(56, 24);
            this.open_save_button.TabIndex = 15;
            this.open_save_button.Text = "打开";
            this.open_save_button.UseVisualStyleBackColor = true;
            this.open_save_button.Click += new System.EventHandler(this.open_save_button_Click);
            // 
            // open_wait_file_button
            // 
            this.open_wait_file_button.Location = new System.Drawing.Point(347, 56);
            this.open_wait_file_button.Margin = new System.Windows.Forms.Padding(2);
            this.open_wait_file_button.Name = "open_wait_file_button";
            this.open_wait_file_button.Size = new System.Drawing.Size(56, 24);
            this.open_wait_file_button.TabIndex = 14;
            this.open_wait_file_button.Text = "打开";
            this.open_wait_file_button.UseVisualStyleBackColor = true;
            this.open_wait_file_button.Click += new System.EventHandler(this.open_wait_file_button_Click);
            // 
            // save_file_textBox
            // 
            this.save_file_textBox.Location = new System.Drawing.Point(154, 101);
            this.save_file_textBox.Margin = new System.Windows.Forms.Padding(2);
            this.save_file_textBox.Name = "save_file_textBox";
            this.save_file_textBox.Size = new System.Drawing.Size(171, 21);
            this.save_file_textBox.TabIndex = 13;
            // 
            // wait_file_textBox
            // 
            this.wait_file_textBox.Location = new System.Drawing.Point(154, 59);
            this.wait_file_textBox.Margin = new System.Windows.Forms.Padding(2);
            this.wait_file_textBox.Name = "wait_file_textBox";
            this.wait_file_textBox.Size = new System.Drawing.Size(171, 21);
            this.wait_file_textBox.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(60, 105);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "保存地址：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(47, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "待分类文件：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(60, 122);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            // 
            // SVMFrom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(472, 190);
            this.Controls.Add(this.open_button);
            this.Controls.Add(this.sample_file_textBox);
            this.Controls.Add(this.sample_label);
            this.Controls.Add(this.tabControl1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "SVMFrom";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SVMForm";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox featurelayer_comboBox;
        private System.Windows.Forms.Label featurel_layer_label;
        private System.Windows.Forms.Button open_button;
        private System.Windows.Forms.Button ok_other_button;
        private System.Windows.Forms.TextBox sample_file_textBox;
        private System.Windows.Forms.Label sample_label;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button open_save_button;
        private System.Windows.Forms.Button open_wait_file_button;
        private System.Windows.Forms.TextBox save_file_textBox;
        private System.Windows.Forms.TextBox wait_file_textBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
    }
}