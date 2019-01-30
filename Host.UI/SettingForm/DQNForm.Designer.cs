namespace Host.UI.SettingForm
{
    partial class DQNForm
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
            this.epochs_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.epochs_label = new System.Windows.Forms.Label();
            this.ok_button = new System.Windows.Forms.Button();
            this.state_comboBox = new System.Windows.Forms.ComboBox();
            this.feedback_comboBox = new System.Windows.Forms.ComboBox();
            this.feedback_label = new System.Windows.Forms.Label();
            this.state_label = new System.Windows.Forms.Label();
            this.task_name_label = new System.Windows.Forms.Label();
            this.task_name_comboBox = new System.Windows.Forms.ComboBox();
            this.sample_size_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.sampe_size_label = new System.Windows.Forms.Label();
            this.lerpPick_checkBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.epochs_numericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sample_size_numericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // epochs_numericUpDown
            // 
            this.epochs_numericUpDown.Location = new System.Drawing.Point(146, 146);
            this.epochs_numericUpDown.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.epochs_numericUpDown.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.epochs_numericUpDown.Name = "epochs_numericUpDown";
            this.epochs_numericUpDown.Size = new System.Drawing.Size(120, 21);
            this.epochs_numericUpDown.TabIndex = 22;
            this.epochs_numericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.epochs_numericUpDown.Value = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            // 
            // epochs_label
            // 
            this.epochs_label.AutoSize = true;
            this.epochs_label.Location = new System.Drawing.Point(65, 148);
            this.epochs_label.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.epochs_label.Name = "epochs_label";
            this.epochs_label.Size = new System.Drawing.Size(41, 12);
            this.epochs_label.TabIndex = 21;
            this.epochs_label.Text = "轮次：";
            // 
            // ok_button
            // 
            this.ok_button.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ok_button.Location = new System.Drawing.Point(372, 222);
            this.ok_button.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ok_button.Name = "ok_button";
            this.ok_button.Size = new System.Drawing.Size(68, 28);
            this.ok_button.TabIndex = 20;
            this.ok_button.Text = "确定";
            this.ok_button.UseVisualStyleBackColor = true;
            this.ok_button.Click += new System.EventHandler(this.Ok_button_Click);
            // 
            // state_comboBox
            // 
            this.state_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.state_comboBox.FormattingEnabled = true;
            this.state_comboBox.Location = new System.Drawing.Point(146, 64);
            this.state_comboBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.state_comboBox.Name = "state_comboBox";
            this.state_comboBox.Size = new System.Drawing.Size(296, 20);
            this.state_comboBox.TabIndex = 19;
            this.state_comboBox.SelectedIndexChanged += new System.EventHandler(this.State_comboBox_SelectedIndexChanged);
            // 
            // feedback_comboBox
            // 
            this.feedback_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.feedback_comboBox.FormattingEnabled = true;
            this.feedback_comboBox.Location = new System.Drawing.Point(146, 105);
            this.feedback_comboBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.feedback_comboBox.Name = "feedback_comboBox";
            this.feedback_comboBox.Size = new System.Drawing.Size(296, 20);
            this.feedback_comboBox.TabIndex = 18;
            this.feedback_comboBox.SelectedIndexChanged += new System.EventHandler(this.Feedback_comboBox_SelectedIndexChanged);
            // 
            // feedback_label
            // 
            this.feedback_label.AutoSize = true;
            this.feedback_label.Location = new System.Drawing.Point(43, 107);
            this.feedback_label.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.feedback_label.Name = "feedback_label";
            this.feedback_label.Size = new System.Drawing.Size(65, 12);
            this.feedback_label.TabIndex = 17;
            this.feedback_label.Text = "反馈图层：";
            // 
            // state_label
            // 
            this.state_label.AutoSize = true;
            this.state_label.Location = new System.Drawing.Point(43, 66);
            this.state_label.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.state_label.Name = "state_label";
            this.state_label.Size = new System.Drawing.Size(65, 12);
            this.state_label.TabIndex = 16;
            this.state_label.Text = "要素图层：";
            // 
            // task_name_label
            // 
            this.task_name_label.AutoSize = true;
            this.task_name_label.Location = new System.Drawing.Point(43, 25);
            this.task_name_label.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.task_name_label.Name = "task_name_label";
            this.task_name_label.Size = new System.Drawing.Size(65, 12);
            this.task_name_label.TabIndex = 23;
            this.task_name_label.Text = "任务类型：";
            // 
            // task_name_comboBox
            // 
            this.task_name_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.task_name_comboBox.FormattingEnabled = true;
            this.task_name_comboBox.Location = new System.Drawing.Point(146, 22);
            this.task_name_comboBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.task_name_comboBox.Name = "task_name_comboBox";
            this.task_name_comboBox.Size = new System.Drawing.Size(296, 20);
            this.task_name_comboBox.TabIndex = 24;
            this.task_name_comboBox.SelectedIndexChanged += new System.EventHandler(this.task_name_comboBox_SelectedIndexChanged);
            // 
            // sample_size_numericUpDown
            // 
            this.sample_size_numericUpDown.Location = new System.Drawing.Point(146, 190);
            this.sample_size_numericUpDown.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.sample_size_numericUpDown.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.sample_size_numericUpDown.Name = "sample_size_numericUpDown";
            this.sample_size_numericUpDown.Size = new System.Drawing.Size(120, 21);
            this.sample_size_numericUpDown.TabIndex = 26;
            this.sample_size_numericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.sample_size_numericUpDown.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
            // 
            // sampe_size_label
            // 
            this.sampe_size_label.AutoSize = true;
            this.sampe_size_label.Location = new System.Drawing.Point(20, 191);
            this.sampe_size_label.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.sampe_size_label.Name = "sampe_size_label";
            this.sampe_size_label.Size = new System.Drawing.Size(89, 12);
            this.sampe_size_label.TabIndex = 25;
            this.sampe_size_label.Text = "每类样本上限：";
            // 
            // lerpPick_checkBox
            // 
            this.lerpPick_checkBox.AutoSize = true;
            this.lerpPick_checkBox.Checked = true;
            this.lerpPick_checkBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.lerpPick_checkBox.Location = new System.Drawing.Point(320, 191);
            this.lerpPick_checkBox.Name = "lerpPick_checkBox";
            this.lerpPick_checkBox.Size = new System.Drawing.Size(120, 16);
            this.lerpPick_checkBox.TabIndex = 27;
            this.lerpPick_checkBox.Text = "是否随机选取样本";
            this.lerpPick_checkBox.UseVisualStyleBackColor = true;
            // 
            // DQNForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(481, 266);
            this.Controls.Add(this.lerpPick_checkBox);
            this.Controls.Add(this.sample_size_numericUpDown);
            this.Controls.Add(this.sampe_size_label);
            this.Controls.Add(this.task_name_comboBox);
            this.Controls.Add(this.task_name_label);
            this.Controls.Add(this.epochs_numericUpDown);
            this.Controls.Add(this.epochs_label);
            this.Controls.Add(this.ok_button);
            this.Controls.Add(this.state_comboBox);
            this.Controls.Add(this.feedback_comboBox);
            this.Controls.Add(this.feedback_label);
            this.Controls.Add(this.state_label);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "DQNForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "DQNForm";
            ((System.ComponentModel.ISupportInitialize)(this.epochs_numericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sample_size_numericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown epochs_numericUpDown;
        private System.Windows.Forms.Label epochs_label;
        private System.Windows.Forms.Button ok_button;
        private System.Windows.Forms.ComboBox state_comboBox;
        private System.Windows.Forms.ComboBox feedback_comboBox;
        private System.Windows.Forms.Label feedback_label;
        private System.Windows.Forms.Label state_label;
        private System.Windows.Forms.Label task_name_label;
        private System.Windows.Forms.ComboBox task_name_comboBox;
        private System.Windows.Forms.NumericUpDown sample_size_numericUpDown;
        private System.Windows.Forms.Label sampe_size_label;
        private System.Windows.Forms.CheckBox lerpPick_checkBox;
    }
}