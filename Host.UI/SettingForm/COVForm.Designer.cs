namespace Host.UI.SettingForm
{
    partial class COVForm
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
            this.target2_comboBox = new System.Windows.Forms.ComboBox();
            this.target1_comboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ok_button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // target2_comboBox
            // 
            this.target2_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.target2_comboBox.FormattingEnabled = true;
            this.target2_comboBox.Location = new System.Drawing.Point(135, 81);
            this.target2_comboBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.target2_comboBox.Name = "target2_comboBox";
            this.target2_comboBox.Size = new System.Drawing.Size(615, 23);
            this.target2_comboBox.TabIndex = 13;
            this.target2_comboBox.SelectedIndexChanged += new System.EventHandler(this.target2_comboBox_SelectedIndexChanged);
            // 
            // target1_comboBox
            // 
            this.target1_comboBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.target1_comboBox.FormattingEnabled = true;
            this.target1_comboBox.ItemHeight = 15;
            this.target1_comboBox.Location = new System.Drawing.Point(135, 41);
            this.target1_comboBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.target1_comboBox.Name = "target1_comboBox";
            this.target1_comboBox.Size = new System.Drawing.Size(615, 23);
            this.target1_comboBox.TabIndex = 12;
            this.target1_comboBox.SelectedIndexChanged += new System.EventHandler(this.target1_comboBox_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(54, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 15);
            this.label2.TabIndex = 11;
            this.label2.Text = "对比图2：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(54, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 15);
            this.label1.TabIndex = 10;
            this.label1.Text = "对比图1：";
            // 
            // ok_button
            // 
            this.ok_button.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ok_button.Location = new System.Drawing.Point(666, 149);
            this.ok_button.Name = "ok_button";
            this.ok_button.Size = new System.Drawing.Size(84, 32);
            this.ok_button.TabIndex = 14;
            this.ok_button.Text = "确定";
            this.ok_button.UseVisualStyleBackColor = true;
            this.ok_button.Click += new System.EventHandler(this.ok_button_Click);
            // 
            // COVForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(763, 201);
            this.Controls.Add(this.ok_button);
            this.Controls.Add(this.target2_comboBox);
            this.Controls.Add(this.target1_comboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "COVForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "CovarianceForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox target2_comboBox;
        private System.Windows.Forms.ComboBox target1_comboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ok_button;
    }
}