namespace Engine.NLP.Forms
{
    partial class NLPProcessForm
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
            this.CoreNLP_listBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // CoreNLP_listBox
            // 
            this.CoreNLP_listBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CoreNLP_listBox.FormattingEnabled = true;
            this.CoreNLP_listBox.ItemHeight = 15;
            this.CoreNLP_listBox.Location = new System.Drawing.Point(0, 0);
            this.CoreNLP_listBox.Name = "CoreNLP_listBox";
            this.CoreNLP_listBox.Size = new System.Drawing.Size(633, 419);
            this.CoreNLP_listBox.TabIndex = 0;
            // 
            // NLPProcessForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(633, 419);
            this.Controls.Add(this.CoreNLP_listBox);
            this.Name = "NLPProcessForm";
            this.Text = "NLPProcessForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.NLPProcessForm_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox CoreNLP_listBox;
    }
}