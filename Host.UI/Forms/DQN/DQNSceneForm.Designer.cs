namespace Host.UI.Forms
{
    partial class DQNSceneForm
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
            this.Sample_Batches_Directory_textBox = new System.Windows.Forms.TextBox();
            this.SampleBatches_button = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Apply_Directory_textBox = new System.Windows.Forms.TextBox();
            this.Apply_button = new System.Windows.Forms.Button();
            this.OK_button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(111, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "样本根目录：";
            // 
            // Sample_Batches_Directory_textBox
            // 
            this.Sample_Batches_Directory_textBox.Location = new System.Drawing.Point(233, 61);
            this.Sample_Batches_Directory_textBox.Name = "Sample_Batches_Directory_textBox";
            this.Sample_Batches_Directory_textBox.Size = new System.Drawing.Size(302, 28);
            this.Sample_Batches_Directory_textBox.TabIndex = 1;
            // 
            // SampleBatches_button
            // 
            this.SampleBatches_button.Location = new System.Drawing.Point(541, 59);
            this.SampleBatches_button.Name = "SampleBatches_button";
            this.SampleBatches_button.Size = new System.Drawing.Size(83, 32);
            this.SampleBatches_button.TabIndex = 2;
            this.SampleBatches_button.Text = "设置";
            this.SampleBatches_button.UseVisualStyleBackColor = true;
            this.SampleBatches_button.Click += new System.EventHandler(this.SampleBatches_button_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(148, 142);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 18);
            this.label2.TabIndex = 3;
            this.label2.Text = "[可选]:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 142);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(134, 18);
            this.label3.TabIndex = 4;
            this.label3.Text = "待分类图片数据";
            // 
            // Apply_Directory_textBox
            // 
            this.Apply_Directory_textBox.Location = new System.Drawing.Point(233, 137);
            this.Apply_Directory_textBox.Name = "Apply_Directory_textBox";
            this.Apply_Directory_textBox.Size = new System.Drawing.Size(302, 28);
            this.Apply_Directory_textBox.TabIndex = 5;
            // 
            // Apply_button
            // 
            this.Apply_button.Location = new System.Drawing.Point(541, 135);
            this.Apply_button.Name = "Apply_button";
            this.Apply_button.Size = new System.Drawing.Size(83, 32);
            this.Apply_button.TabIndex = 6;
            this.Apply_button.Text = "设置";
            this.Apply_button.UseVisualStyleBackColor = true;
            this.Apply_button.Click += new System.EventHandler(this.Apply_button_Click);
            // 
            // OK_button
            // 
            this.OK_button.Location = new System.Drawing.Point(541, 206);
            this.OK_button.Name = "OK_button";
            this.OK_button.Size = new System.Drawing.Size(83, 32);
            this.OK_button.TabIndex = 7;
            this.OK_button.Text = "确定";
            this.OK_button.UseVisualStyleBackColor = true;
            this.OK_button.Click += new System.EventHandler(this.OK_button_Click);
            // 
            // DQNSceneForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(648, 277);
            this.Controls.Add(this.OK_button);
            this.Controls.Add(this.Apply_button);
            this.Controls.Add(this.Apply_Directory_textBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.SampleBatches_button);
            this.Controls.Add(this.Sample_Batches_Directory_textBox);
            this.Controls.Add(this.label1);
            this.Name = "DQNSceneForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "DQNSceneForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Sample_Batches_Directory_textBox;
        private System.Windows.Forms.Button SampleBatches_button;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox Apply_Directory_textBox;
        private System.Windows.Forms.Button Apply_button;
        private System.Windows.Forms.Button OK_button;
    }
}