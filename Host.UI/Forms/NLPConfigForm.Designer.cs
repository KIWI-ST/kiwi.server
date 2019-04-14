namespace Host.UI.Forms
{
    partial class NLPConfigForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.CoreNLP_Start_Command_textBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.CoreNLP_Dir_button = new System.Windows.Forms.Button();
            this.CoreNLP_Dir_textBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.OK_button = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.GloVe_File_button = new System.Windows.Forms.Button();
            this.GloVe_File_textBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.CoreNLP_Start_Command_textBox);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.CoreNLP_Dir_button);
            this.groupBox1.Controls.Add(this.CoreNLP_Dir_textBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Size = new System.Drawing.Size(554, 132);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Stanford NLP Core";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(106, 87);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(52, 12);
            this.label6.TabIndex = 8;
            this.label6.Text = "[可选]:";
            // 
            // CoreNLP_Start_Command_textBox
            // 
            this.CoreNLP_Start_Command_textBox.Location = new System.Drawing.Point(168, 84);
            this.CoreNLP_Start_Command_textBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.CoreNLP_Start_Command_textBox.Name = "CoreNLP_Start_Command_textBox";
            this.CoreNLP_Start_Command_textBox.Size = new System.Drawing.Size(331, 21);
            this.CoreNLP_Start_Command_textBox.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 87);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(95, 12);
            this.label5.TabIndex = 3;
            this.label5.Text = "CoreNLP启动参数";
            // 
            // CoreNLP_Dir_button
            // 
            this.CoreNLP_Dir_button.Location = new System.Drawing.Point(442, 29);
            this.CoreNLP_Dir_button.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.CoreNLP_Dir_button.Name = "CoreNLP_Dir_button";
            this.CoreNLP_Dir_button.Size = new System.Drawing.Size(56, 20);
            this.CoreNLP_Dir_button.TabIndex = 2;
            this.CoreNLP_Dir_button.Text = "设置";
            this.CoreNLP_Dir_button.UseVisualStyleBackColor = true;
            // 
            // CoreNLP_Dir_textBox
            // 
            this.CoreNLP_Dir_textBox.Location = new System.Drawing.Point(168, 30);
            this.CoreNLP_Dir_textBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.CoreNLP_Dir_textBox.Name = "CoreNLP_Dir_textBox";
            this.CoreNLP_Dir_textBox.Size = new System.Drawing.Size(250, 21);
            this.CoreNLP_Dir_textBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(79, 34);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "CoreNLP目录：";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.OK_button);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.textBox2);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.GloVe_File_button);
            this.groupBox2.Controls.Add(this.GloVe_File_textBox);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 132);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox2.Size = new System.Drawing.Size(554, 190);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Words Emebdding";
            // 
            // OK_button
            // 
            this.OK_button.Location = new System.Drawing.Point(441, 151);
            this.OK_button.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.OK_button.Name = "OK_button";
            this.OK_button.Size = new System.Drawing.Size(60, 22);
            this.OK_button.TabIndex = 8;
            this.OK_button.Text = "保存";
            this.OK_button.UseVisualStyleBackColor = true;
            this.OK_button.Click += new System.EventHandler(this.OK_button_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(109, 106);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "[可选]:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(442, 102);
            this.button1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(56, 20);
            this.button1.TabIndex = 6;
            this.button1.Text = "设置";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(168, 102);
            this.textBox2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(250, 21);
            this.textBox2.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(38, 106);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "训练语料地址";
            // 
            // GloVe_File_button
            // 
            this.GloVe_File_button.Location = new System.Drawing.Point(442, 48);
            this.GloVe_File_button.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.GloVe_File_button.Name = "GloVe_File_button";
            this.GloVe_File_button.Size = new System.Drawing.Size(56, 20);
            this.GloVe_File_button.TabIndex = 3;
            this.GloVe_File_button.Text = "设置";
            this.GloVe_File_button.UseVisualStyleBackColor = true;
            // 
            // GloVe_File_textBox
            // 
            this.GloVe_File_textBox.Location = new System.Drawing.Point(168, 48);
            this.GloVe_File_textBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.GloVe_File_textBox.Name = "GloVe_File_textBox";
            this.GloVe_File_textBox.Size = new System.Drawing.Size(250, 21);
            this.GloVe_File_textBox.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(68, 52);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "GloVe文件地址：";
            // 
            // NLPConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(554, 322);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "NLPConfigForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "NLPConfigForm";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button CoreNLP_Dir_button;
        private System.Windows.Forms.TextBox CoreNLP_Dir_textBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox GloVe_File_textBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button GloVe_File_button;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox CoreNLP_Start_Command_textBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button OK_button;
        private System.Windows.Forms.Label label6;
    }
}