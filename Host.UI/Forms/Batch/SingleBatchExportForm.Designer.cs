namespace Host.UI.SettingForm
{
    partial class SingleBatchExportForm
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
            this.RAW_IMAGE_label = new System.Windows.Forms.Label();
            this.LABELED_IMAGE_comboBox = new System.Windows.Forms.ComboBox();
            this.RAW_IMAGE_comboBox = new System.Windows.Forms.ComboBox();
            this.LABELED_IMAGE_label = new System.Windows.Forms.Label();
            this.DATAPICK_METHOD_label = new System.Windows.Forms.Label();
            this.PICK_METHOD_comboBox = new System.Windows.Forms.ComboBox();
            this.SAMPLESIZE_LIMIT_label = new System.Windows.Forms.Label();
            this.SAMPLESIZE_LIMIT_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.LERP_PICK_checkBox = new System.Windows.Forms.CheckBox();
            this.PICK_REPEAT_label = new System.Windows.Forms.Label();
            this.PICK_REPEAT_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.EXPORT_PATH_button = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.SAMPLESIZE_LIMIT_numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PICK_REPEAT_numericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // RAW_IMAGE_label
            // 
            this.RAW_IMAGE_label.AutoSize = true;
            this.RAW_IMAGE_label.Location = new System.Drawing.Point(60, 66);
            this.RAW_IMAGE_label.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.RAW_IMAGE_label.Name = "RAW_IMAGE_label";
            this.RAW_IMAGE_label.Size = new System.Drawing.Size(77, 12);
            this.RAW_IMAGE_label.TabIndex = 0;
            this.RAW_IMAGE_label.Text = "原始数据图：";
            // 
            // LABELED_IMAGE_comboBox
            // 
            this.LABELED_IMAGE_comboBox.FormattingEnabled = true;
            this.LABELED_IMAGE_comboBox.Location = new System.Drawing.Point(166, 17);
            this.LABELED_IMAGE_comboBox.Margin = new System.Windows.Forms.Padding(2);
            this.LABELED_IMAGE_comboBox.Name = "LABELED_IMAGE_comboBox";
            this.LABELED_IMAGE_comboBox.Size = new System.Drawing.Size(224, 20);
            this.LABELED_IMAGE_comboBox.TabIndex = 1;
            this.LABELED_IMAGE_comboBox.SelectedIndexChanged += new System.EventHandler(this.LABELED_IMAGE_comboBox_SelectedIndexChanged);
            // 
            // RAW_IMAGE_comboBox
            // 
            this.RAW_IMAGE_comboBox.FormattingEnabled = true;
            this.RAW_IMAGE_comboBox.Location = new System.Drawing.Point(166, 62);
            this.RAW_IMAGE_comboBox.Margin = new System.Windows.Forms.Padding(2);
            this.RAW_IMAGE_comboBox.Name = "RAW_IMAGE_comboBox";
            this.RAW_IMAGE_comboBox.Size = new System.Drawing.Size(224, 20);
            this.RAW_IMAGE_comboBox.TabIndex = 2;
            this.RAW_IMAGE_comboBox.SelectedIndexChanged += new System.EventHandler(this.RAW_IMAGE_comboBox_SelectedIndexChanged);
            // 
            // LABELED_IMAGE_label
            // 
            this.LABELED_IMAGE_label.AutoSize = true;
            this.LABELED_IMAGE_label.Location = new System.Drawing.Point(82, 20);
            this.LABELED_IMAGE_label.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LABELED_IMAGE_label.Name = "LABELED_IMAGE_label";
            this.LABELED_IMAGE_label.Size = new System.Drawing.Size(53, 12);
            this.LABELED_IMAGE_label.TabIndex = 3;
            this.LABELED_IMAGE_label.Text = "标注图：";
            // 
            // DATAPICK_METHOD_label
            // 
            this.DATAPICK_METHOD_label.AutoSize = true;
            this.DATAPICK_METHOD_label.Location = new System.Drawing.Point(26, 116);
            this.DATAPICK_METHOD_label.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.DATAPICK_METHOD_label.Name = "DATAPICK_METHOD_label";
            this.DATAPICK_METHOD_label.Size = new System.Drawing.Size(113, 12);
            this.DATAPICK_METHOD_label.TabIndex = 4;
            this.DATAPICK_METHOD_label.Text = "原始数据采集方法：";
            // 
            // PICK_METHOD_comboBox
            // 
            this.PICK_METHOD_comboBox.FormattingEnabled = true;
            this.PICK_METHOD_comboBox.Location = new System.Drawing.Point(166, 112);
            this.PICK_METHOD_comboBox.Margin = new System.Windows.Forms.Padding(2);
            this.PICK_METHOD_comboBox.Name = "PICK_METHOD_comboBox";
            this.PICK_METHOD_comboBox.Size = new System.Drawing.Size(224, 20);
            this.PICK_METHOD_comboBox.TabIndex = 5;
            this.PICK_METHOD_comboBox.SelectedIndexChanged += new System.EventHandler(this.PICK_METHOD_comboBox_SelectedIndexChanged);
            // 
            // SAMPLESIZE_LIMIT_label
            // 
            this.SAMPLESIZE_LIMIT_label.AutoSize = true;
            this.SAMPLESIZE_LIMIT_label.Location = new System.Drawing.Point(26, 168);
            this.SAMPLESIZE_LIMIT_label.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.SAMPLESIZE_LIMIT_label.Name = "SAMPLESIZE_LIMIT_label";
            this.SAMPLESIZE_LIMIT_label.Size = new System.Drawing.Size(113, 12);
            this.SAMPLESIZE_LIMIT_label.TabIndex = 6;
            this.SAMPLESIZE_LIMIT_label.Text = "每类样本数量限制：";
            // 
            // SAMPLESIZE_LIMIT_numericUpDown
            // 
            this.SAMPLESIZE_LIMIT_numericUpDown.Location = new System.Drawing.Point(166, 164);
            this.SAMPLESIZE_LIMIT_numericUpDown.Margin = new System.Windows.Forms.Padding(2);
            this.SAMPLESIZE_LIMIT_numericUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.SAMPLESIZE_LIMIT_numericUpDown.Name = "SAMPLESIZE_LIMIT_numericUpDown";
            this.SAMPLESIZE_LIMIT_numericUpDown.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.SAMPLESIZE_LIMIT_numericUpDown.Size = new System.Drawing.Size(136, 21);
            this.SAMPLESIZE_LIMIT_numericUpDown.TabIndex = 7;
            this.SAMPLESIZE_LIMIT_numericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.SAMPLESIZE_LIMIT_numericUpDown.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // LERP_PICK_checkBox
            // 
            this.LERP_PICK_checkBox.AutoSize = true;
            this.LERP_PICK_checkBox.Checked = true;
            this.LERP_PICK_checkBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.LERP_PICK_checkBox.Location = new System.Drawing.Point(314, 167);
            this.LERP_PICK_checkBox.Margin = new System.Windows.Forms.Padding(2);
            this.LERP_PICK_checkBox.Name = "LERP_PICK_checkBox";
            this.LERP_PICK_checkBox.Size = new System.Drawing.Size(168, 16);
            this.LERP_PICK_checkBox.TabIndex = 9;
            this.LERP_PICK_checkBox.Text = "随机采集（默认均匀采集）";
            this.LERP_PICK_checkBox.UseVisualStyleBackColor = true;
            // 
            // PICK_REPEAT_label
            // 
            this.PICK_REPEAT_label.AutoSize = true;
            this.PICK_REPEAT_label.Location = new System.Drawing.Point(50, 222);
            this.PICK_REPEAT_label.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.PICK_REPEAT_label.Name = "PICK_REPEAT_label";
            this.PICK_REPEAT_label.Size = new System.Drawing.Size(89, 12);
            this.PICK_REPEAT_label.TabIndex = 10;
            this.PICK_REPEAT_label.Text = "重复采集次数：";
            // 
            // PICK_REPEAT_numericUpDown
            // 
            this.PICK_REPEAT_numericUpDown.Location = new System.Drawing.Point(166, 218);
            this.PICK_REPEAT_numericUpDown.Margin = new System.Windows.Forms.Padding(2);
            this.PICK_REPEAT_numericUpDown.Name = "PICK_REPEAT_numericUpDown";
            this.PICK_REPEAT_numericUpDown.Size = new System.Drawing.Size(136, 21);
            this.PICK_REPEAT_numericUpDown.TabIndex = 11;
            this.PICK_REPEAT_numericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.PICK_REPEAT_numericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // EXPORT_PATH_button
            // 
            this.EXPORT_PATH_button.Location = new System.Drawing.Point(314, 217);
            this.EXPORT_PATH_button.Margin = new System.Windows.Forms.Padding(2);
            this.EXPORT_PATH_button.Name = "EXPORT_PATH_button";
            this.EXPORT_PATH_button.Size = new System.Drawing.Size(151, 23);
            this.EXPORT_PATH_button.TabIndex = 14;
            this.EXPORT_PATH_button.Text = "导出";
            this.EXPORT_PATH_button.UseVisualStyleBackColor = true;
            this.EXPORT_PATH_button.Click += new System.EventHandler(this.EXPORT_PATH_button_Click);
            // 
            // BatchExportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(493, 267);
            this.Controls.Add(this.EXPORT_PATH_button);
            this.Controls.Add(this.PICK_REPEAT_numericUpDown);
            this.Controls.Add(this.PICK_REPEAT_label);
            this.Controls.Add(this.LERP_PICK_checkBox);
            this.Controls.Add(this.SAMPLESIZE_LIMIT_numericUpDown);
            this.Controls.Add(this.SAMPLESIZE_LIMIT_label);
            this.Controls.Add(this.PICK_METHOD_comboBox);
            this.Controls.Add(this.DATAPICK_METHOD_label);
            this.Controls.Add(this.LABELED_IMAGE_label);
            this.Controls.Add(this.RAW_IMAGE_comboBox);
            this.Controls.Add(this.LABELED_IMAGE_comboBox);
            this.Controls.Add(this.RAW_IMAGE_label);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "BatchExportForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "BatchExportForm";
            ((System.ComponentModel.ISupportInitialize)(this.SAMPLESIZE_LIMIT_numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PICK_REPEAT_numericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label RAW_IMAGE_label;
        private System.Windows.Forms.ComboBox LABELED_IMAGE_comboBox;
        private System.Windows.Forms.ComboBox RAW_IMAGE_comboBox;
        private System.Windows.Forms.Label LABELED_IMAGE_label;
        private System.Windows.Forms.Label DATAPICK_METHOD_label;
        private System.Windows.Forms.ComboBox PICK_METHOD_comboBox;
        private System.Windows.Forms.Label SAMPLESIZE_LIMIT_label;
        private System.Windows.Forms.NumericUpDown SAMPLESIZE_LIMIT_numericUpDown;
        private System.Windows.Forms.CheckBox LERP_PICK_checkBox;
        private System.Windows.Forms.Label PICK_REPEAT_label;
        private System.Windows.Forms.NumericUpDown PICK_REPEAT_numericUpDown;
        private System.Windows.Forms.Button EXPORT_PATH_button;
    }
}