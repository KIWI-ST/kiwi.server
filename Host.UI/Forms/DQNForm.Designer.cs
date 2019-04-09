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
            this.state_label = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SampleFile_textBox = new System.Windows.Forms.TextBox();
            this.Open_button = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.epochs_numericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // epochs_numericUpDown
            // 
            this.epochs_numericUpDown.Location = new System.Drawing.Point(124, 119);
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
            this.epochs_label.Location = new System.Drawing.Point(56, 123);
            this.epochs_label.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.epochs_label.Name = "epochs_label";
            this.epochs_label.Size = new System.Drawing.Size(41, 12);
            this.epochs_label.TabIndex = 21;
            this.epochs_label.Text = "轮次：";
            // 
            // ok_button
            // 
            this.ok_button.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ok_button.Location = new System.Drawing.Point(361, 150);
            this.ok_button.Margin = new System.Windows.Forms.Padding(2);
            this.ok_button.Name = "ok_button";
            this.ok_button.Size = new System.Drawing.Size(59, 28);
            this.ok_button.TabIndex = 20;
            this.ok_button.Text = "确定";
            this.ok_button.UseVisualStyleBackColor = true;
            this.ok_button.Click += new System.EventHandler(this.Ok_button_Click);
            // 
            // state_comboBox
            // 
            this.state_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.state_comboBox.FormattingEnabled = true;
            this.state_comboBox.Location = new System.Drawing.Point(124, 23);
            this.state_comboBox.Margin = new System.Windows.Forms.Padding(2);
            this.state_comboBox.Name = "state_comboBox";
            this.state_comboBox.Size = new System.Drawing.Size(296, 20);
            this.state_comboBox.TabIndex = 19;
            this.state_comboBox.SelectedIndexChanged += new System.EventHandler(this.State_comboBox_SelectedIndexChanged);
            // 
            // state_label
            // 
            this.state_label.AutoSize = true;
            this.state_label.Location = new System.Drawing.Point(21, 25);
            this.state_label.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.state_label.Name = "state_label";
            this.state_label.Size = new System.Drawing.Size(77, 12);
            this.state_label.TabIndex = 16;
            this.state_label.Text = "待分类图层：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 74);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 23;
            this.label1.Text = "环境样本：";
            // 
            // SampleFile_textBox
            // 
            this.SampleFile_textBox.Location = new System.Drawing.Point(124, 70);
            this.SampleFile_textBox.Name = "SampleFile_textBox";
            this.SampleFile_textBox.Size = new System.Drawing.Size(215, 21);
            this.SampleFile_textBox.TabIndex = 24;
            // 
            // Open_button
            // 
            this.Open_button.Location = new System.Drawing.Point(361, 68);
            this.Open_button.Margin = new System.Windows.Forms.Padding(2);
            this.Open_button.Name = "Open_button";
            this.Open_button.Size = new System.Drawing.Size(59, 26);
            this.Open_button.TabIndex = 25;
            this.Open_button.Text = "打开";
            this.Open_button.UseVisualStyleBackColor = true;
            this.Open_button.Click += new System.EventHandler(this.Open_button_Click);
            // 
            // DQNForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(448, 201);
            this.Controls.Add(this.Open_button);
            this.Controls.Add(this.SampleFile_textBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.epochs_numericUpDown);
            this.Controls.Add(this.epochs_label);
            this.Controls.Add(this.ok_button);
            this.Controls.Add(this.state_comboBox);
            this.Controls.Add(this.state_label);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "DQNForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "DQNForm";
            ((System.ComponentModel.ISupportInitialize)(this.epochs_numericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown epochs_numericUpDown;
        private System.Windows.Forms.Label epochs_label;
        private System.Windows.Forms.Button ok_button;
        private System.Windows.Forms.ComboBox state_comboBox;
        private System.Windows.Forms.Label state_label;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox SampleFile_textBox;
        private System.Windows.Forms.Button Open_button;
    }
}