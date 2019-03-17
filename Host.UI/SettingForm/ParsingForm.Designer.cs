namespace Host.UI.SettingForm
{
    partial class ParsingForm
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
            this.OK_button = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.TEXT_textBox = new System.Windows.Forms.TextBox();
            this.OPEN_TEXT_button = new System.Windows.Forms.Button();
            this.OPEN_MODEL_button = new System.Windows.Forms.Button();
            this.MODEL_textBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.OPEN_LEXICON_button = new System.Windows.Forms.Button();
            this.LEXICON_textBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // OK_button
            // 
            this.OK_button.Location = new System.Drawing.Point(581, 276);
            this.OK_button.Name = "OK_button";
            this.OK_button.Size = new System.Drawing.Size(79, 28);
            this.OK_button.TabIndex = 0;
            this.OK_button.Text = "确定";
            this.OK_button.UseVisualStyleBackColor = true;
            this.OK_button.Click += new System.EventHandler(this.OK_button_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(57, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "待处理文本：";
            // 
            // TEXT_textBox
            // 
            this.TEXT_textBox.Location = new System.Drawing.Point(168, 43);
            this.TEXT_textBox.Name = "TEXT_textBox";
            this.TEXT_textBox.Size = new System.Drawing.Size(388, 25);
            this.TEXT_textBox.TabIndex = 2;
            // 
            // OPEN_TEXT_button
            // 
            this.OPEN_TEXT_button.Location = new System.Drawing.Point(582, 41);
            this.OPEN_TEXT_button.Name = "OPEN_TEXT_button";
            this.OPEN_TEXT_button.Size = new System.Drawing.Size(75, 28);
            this.OPEN_TEXT_button.TabIndex = 3;
            this.OPEN_TEXT_button.Text = "打开";
            this.OPEN_TEXT_button.UseVisualStyleBackColor = true;
            this.OPEN_TEXT_button.Click += new System.EventHandler(this.OPEN_TEXT_button_Click);
            // 
            // OPEN_MODEL_button
            // 
            this.OPEN_MODEL_button.Location = new System.Drawing.Point(582, 126);
            this.OPEN_MODEL_button.Name = "OPEN_MODEL_button";
            this.OPEN_MODEL_button.Size = new System.Drawing.Size(75, 28);
            this.OPEN_MODEL_button.TabIndex = 6;
            this.OPEN_MODEL_button.Text = "打开";
            this.OPEN_MODEL_button.UseVisualStyleBackColor = true;
            this.OPEN_MODEL_button.Click += new System.EventHandler(this.OPEN_MODEL_button_Click);
            // 
            // MODEL_textBox
            // 
            this.MODEL_textBox.Location = new System.Drawing.Point(168, 127);
            this.MODEL_textBox.Name = "MODEL_textBox";
            this.MODEL_textBox.Size = new System.Drawing.Size(388, 25);
            this.MODEL_textBox.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(41, 132);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(114, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "LSTM文本模型：";
            // 
            // OPEN_LEXICON_button
            // 
            this.OPEN_LEXICON_button.Location = new System.Drawing.Point(582, 203);
            this.OPEN_LEXICON_button.Name = "OPEN_LEXICON_button";
            this.OPEN_LEXICON_button.Size = new System.Drawing.Size(75, 28);
            this.OPEN_LEXICON_button.TabIndex = 9;
            this.OPEN_LEXICON_button.Text = "打开";
            this.OPEN_LEXICON_button.UseVisualStyleBackColor = true;
            this.OPEN_LEXICON_button.Click += new System.EventHandler(this.OPEN_LEXICON_button_Click);
            // 
            // LEXICON_textBox
            // 
            this.LEXICON_textBox.Location = new System.Drawing.Point(168, 204);
            this.LEXICON_textBox.Name = "LEXICON_textBox";
            this.LEXICON_textBox.Size = new System.Drawing.Size(388, 25);
            this.LEXICON_textBox.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 209);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(128, 15);
            this.label3.TabIndex = 7;
            this.label3.Text = "语言字典[可选]：";
            // 
            // ParsingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(719, 331);
            this.Controls.Add(this.OPEN_LEXICON_button);
            this.Controls.Add(this.LEXICON_textBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.OPEN_MODEL_button);
            this.Controls.Add(this.MODEL_textBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.OPEN_TEXT_button);
            this.Controls.Add(this.TEXT_textBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.OK_button);
            this.Name = "ParsingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ParsingForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OK_button;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TEXT_textBox;
        private System.Windows.Forms.Button OPEN_TEXT_button;
        private System.Windows.Forms.Button OPEN_MODEL_button;
        private System.Windows.Forms.TextBox MODEL_textBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button OPEN_LEXICON_button;
        private System.Windows.Forms.TextBox LEXICON_textBox;
        private System.Windows.Forms.Label label3;
    }
}