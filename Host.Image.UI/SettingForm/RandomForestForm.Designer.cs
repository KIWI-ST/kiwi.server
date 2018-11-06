namespace Host.UI.SettingForm
{
    partial class RandomForestForm
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
            this.ok_button = new System.Windows.Forms.Button();
            this.open_button = new System.Windows.Forms.Button();
            this.featurel_layer_label = new System.Windows.Forms.Label();
            this.featurelayer_comboBox = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.tree_count_numericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // tree_count_label
            // 
            this.tree_count_label.AutoSize = true;
            this.tree_count_label.Location = new System.Drawing.Point(51, 67);
            this.tree_count_label.Name = "tree_count_label";
            this.tree_count_label.Size = new System.Drawing.Size(97, 15);
            this.tree_count_label.TabIndex = 0;
            this.tree_count_label.Text = "设置树个数：";
            // 
            // tree_count_numericUpDown
            // 
            this.tree_count_numericUpDown.Location = new System.Drawing.Point(194, 60);
            this.tree_count_numericUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.tree_count_numericUpDown.Name = "tree_count_numericUpDown";
            this.tree_count_numericUpDown.Size = new System.Drawing.Size(227, 25);
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
            this.sample_label.Location = new System.Drawing.Point(66, 131);
            this.sample_label.Name = "sample_label";
            this.sample_label.Size = new System.Drawing.Size(82, 15);
            this.sample_label.TabIndex = 2;
            this.sample_label.Text = "样本文件：";
            // 
            // sample_file_textBox
            // 
            this.sample_file_textBox.Location = new System.Drawing.Point(194, 125);
            this.sample_file_textBox.Name = "sample_file_textBox";
            this.sample_file_textBox.Size = new System.Drawing.Size(227, 25);
            this.sample_file_textBox.TabIndex = 3;
            // 
            // ok_button
            // 
            this.ok_button.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ok_button.Location = new System.Drawing.Point(451, 214);
            this.ok_button.Name = "ok_button";
            this.ok_button.Size = new System.Drawing.Size(75, 30);
            this.ok_button.TabIndex = 5;
            this.ok_button.Text = "确定";
            this.ok_button.UseVisualStyleBackColor = true;
            this.ok_button.Click += new System.EventHandler(this.ok_button_Click);
            // 
            // open_button
            // 
            this.open_button.Location = new System.Drawing.Point(451, 123);
            this.open_button.Name = "open_button";
            this.open_button.Size = new System.Drawing.Size(75, 30);
            this.open_button.TabIndex = 6;
            this.open_button.Text = "打开";
            this.open_button.UseVisualStyleBackColor = true;
            this.open_button.Click += new System.EventHandler(this.open_button_Click);
            // 
            // featurel_layer_label
            // 
            this.featurel_layer_label.AutoSize = true;
            this.featurel_layer_label.Location = new System.Drawing.Point(73, 184);
            this.featurel_layer_label.Name = "featurel_layer_label";
            this.featurel_layer_label.Size = new System.Drawing.Size(75, 15);
            this.featurel_layer_label.TabIndex = 7;
            this.featurel_layer_label.Text = "待分类图:";
            // 
            // featurelayer_comboBox
            // 
            this.featurelayer_comboBox.FormattingEnabled = true;
            this.featurelayer_comboBox.Location = new System.Drawing.Point(194, 181);
            this.featurelayer_comboBox.Name = "featurelayer_comboBox";
            this.featurelayer_comboBox.Size = new System.Drawing.Size(227, 23);
            this.featurelayer_comboBox.TabIndex = 8;
            this.featurelayer_comboBox.SelectedIndexChanged += new System.EventHandler(this.featurelayer_comboBox_SelectedIndexChanged);
            // 
            // RandomForestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(587, 265);
            this.Controls.Add(this.featurelayer_comboBox);
            this.Controls.Add(this.featurel_layer_label);
            this.Controls.Add(this.open_button);
            this.Controls.Add(this.ok_button);
            this.Controls.Add(this.sample_file_textBox);
            this.Controls.Add(this.sample_label);
            this.Controls.Add(this.tree_count_numericUpDown);
            this.Controls.Add(this.tree_count_label);
            this.Name = "RandomForestForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "RandomForestForm";
            ((System.ComponentModel.ISupportInitialize)(this.tree_count_numericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label tree_count_label;
        private System.Windows.Forms.NumericUpDown tree_count_numericUpDown;
        private System.Windows.Forms.Label sample_label;
        private System.Windows.Forms.TextBox sample_file_textBox;
        private System.Windows.Forms.Button ok_button;
        private System.Windows.Forms.Button open_button;
        private System.Windows.Forms.Label featurel_layer_label;
        private System.Windows.Forms.ComboBox featurelayer_comboBox;
    }
}