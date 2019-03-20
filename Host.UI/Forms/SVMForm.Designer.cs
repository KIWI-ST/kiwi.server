namespace Host.UI.SettingForm
{
    partial class SVMForm
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
            this.ok_button = new System.Windows.Forms.Button();
            this.sample_file_textBox = new System.Windows.Forms.TextBox();
            this.sample_label = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // featurelayer_comboBox
            // 
            this.featurelayer_comboBox.FormattingEnabled = true;
            this.featurelayer_comboBox.Location = new System.Drawing.Point(211, 123);
            this.featurelayer_comboBox.Name = "featurelayer_comboBox";
            this.featurelayer_comboBox.Size = new System.Drawing.Size(227, 23);
            this.featurelayer_comboBox.TabIndex = 14;
            this.featurelayer_comboBox.SelectedIndexChanged += new System.EventHandler(this.featurelayer_comboBox_SelectedIndexChanged);
            // 
            // featurel_layer_label
            // 
            this.featurel_layer_label.AutoSize = true;
            this.featurel_layer_label.Location = new System.Drawing.Point(90, 126);
            this.featurel_layer_label.Name = "featurel_layer_label";
            this.featurel_layer_label.Size = new System.Drawing.Size(75, 15);
            this.featurel_layer_label.TabIndex = 13;
            this.featurel_layer_label.Text = "待分类图:";
            // 
            // open_button
            // 
            this.open_button.Location = new System.Drawing.Point(468, 65);
            this.open_button.Name = "open_button";
            this.open_button.Size = new System.Drawing.Size(75, 30);
            this.open_button.TabIndex = 12;
            this.open_button.Text = "打开";
            this.open_button.UseVisualStyleBackColor = true;
            this.open_button.Click += new System.EventHandler(this.open_button_Click);
            // 
            // ok_button
            // 
            this.ok_button.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ok_button.Location = new System.Drawing.Point(468, 156);
            this.ok_button.Name = "ok_button";
            this.ok_button.Size = new System.Drawing.Size(75, 30);
            this.ok_button.TabIndex = 11;
            this.ok_button.Text = "确定";
            this.ok_button.UseVisualStyleBackColor = true;
            this.ok_button.Click += new System.EventHandler(this.ok_button_Click);
            // 
            // sample_file_textBox
            // 
            this.sample_file_textBox.Location = new System.Drawing.Point(211, 67);
            this.sample_file_textBox.Name = "sample_file_textBox";
            this.sample_file_textBox.Size = new System.Drawing.Size(227, 25);
            this.sample_file_textBox.TabIndex = 10;
            // 
            // sample_label
            // 
            this.sample_label.AutoSize = true;
            this.sample_label.Location = new System.Drawing.Point(83, 73);
            this.sample_label.Name = "sample_label";
            this.sample_label.Size = new System.Drawing.Size(82, 15);
            this.sample_label.TabIndex = 9;
            this.sample_label.Text = "样本文件：";
            // 
            // SVMForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(630, 238);
            this.Controls.Add(this.featurelayer_comboBox);
            this.Controls.Add(this.featurel_layer_label);
            this.Controls.Add(this.open_button);
            this.Controls.Add(this.ok_button);
            this.Controls.Add(this.sample_file_textBox);
            this.Controls.Add(this.sample_label);
            this.Name = "SVMForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SVMForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox featurelayer_comboBox;
        private System.Windows.Forms.Label featurel_layer_label;
        private System.Windows.Forms.Button open_button;
        private System.Windows.Forms.Button ok_button;
        private System.Windows.Forms.TextBox sample_file_textBox;
        private System.Windows.Forms.Label sample_label;
    }
}