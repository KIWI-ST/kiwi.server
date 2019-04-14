namespace Host.UI.Forms
{
    partial class NLPScenarioForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NLPScenarioForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.Open_toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.Split_toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.Left_Right_splitContainer = new System.Windows.Forms.SplitContainer();
            this.Corpus_listBox = new System.Windows.Forms.ListBox();
            this.Left2_Right2_splitContainer = new System.Windows.Forms.SplitContainer();
            this.Split_listBox = new System.Windows.Forms.ListBox();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Left_Right_splitContainer)).BeginInit();
            this.Left_Right_splitContainer.Panel1.SuspendLayout();
            this.Left_Right_splitContainer.Panel2.SuspendLayout();
            this.Left_Right_splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Left2_Right2_splitContainer)).BeginInit();
            this.Left2_Right2_splitContainer.Panel1.SuspendLayout();
            this.Left2_Right2_splitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Open_toolStripButton,
            this.toolStripSeparator1,
            this.Split_toolStripButton,
            this.toolStripButton2,
            this.toolStripButton3});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(918, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "NLP_toolStrip";
            // 
            // Open_toolStripButton
            // 
            this.Open_toolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("Open_toolStripButton.Image")));
            this.Open_toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Open_toolStripButton.Name = "Open_toolStripButton";
            this.Open_toolStripButton.Size = new System.Drawing.Size(84, 22);
            this.Open_toolStripButton.Text = "OpenText";
            this.Open_toolStripButton.Click += new System.EventHandler(this.ToolStripButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // Split_toolStripButton
            // 
            this.Split_toolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("Split_toolStripButton.Image")));
            this.Split_toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Split_toolStripButton.Name = "Split_toolStripButton";
            this.Split_toolStripButton.Size = new System.Drawing.Size(113, 22);
            this.Split_toolStripButton.Text = "SplitByTimeML";
            this.Split_toolStripButton.Click += new System.EventHandler(this.ToolStripButton_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(107, 22);
            this.toolStripButton2.Text = "PartOfSpeech";
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(95, 22);
            this.toolStripButton3.Text = "Embedding";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 436);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(918, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "NLP_statusStrip";
            // 
            // Left_Right_splitContainer
            // 
            this.Left_Right_splitContainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Left_Right_splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Left_Right_splitContainer.Location = new System.Drawing.Point(0, 25);
            this.Left_Right_splitContainer.Name = "Left_Right_splitContainer";
            // 
            // Left_Right_splitContainer.Panel1
            // 
            this.Left_Right_splitContainer.Panel1.Controls.Add(this.Corpus_listBox);
            // 
            // Left_Right_splitContainer.Panel2
            // 
            this.Left_Right_splitContainer.Panel2.Controls.Add(this.Left2_Right2_splitContainer);
            this.Left_Right_splitContainer.Size = new System.Drawing.Size(918, 411);
            this.Left_Right_splitContainer.SplitterDistance = 208;
            this.Left_Right_splitContainer.TabIndex = 2;
            // 
            // Corpus_listBox
            // 
            this.Corpus_listBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Corpus_listBox.FormattingEnabled = true;
            this.Corpus_listBox.ItemHeight = 12;
            this.Corpus_listBox.Location = new System.Drawing.Point(0, 0);
            this.Corpus_listBox.Name = "Corpus_listBox";
            this.Corpus_listBox.Size = new System.Drawing.Size(204, 407);
            this.Corpus_listBox.TabIndex = 0;
            // 
            // Left2_Right2_splitContainer
            // 
            this.Left2_Right2_splitContainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Left2_Right2_splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Left2_Right2_splitContainer.Location = new System.Drawing.Point(0, 0);
            this.Left2_Right2_splitContainer.Name = "Left2_Right2_splitContainer";
            // 
            // Left2_Right2_splitContainer.Panel1
            // 
            this.Left2_Right2_splitContainer.Panel1.Controls.Add(this.Split_listBox);
            this.Left2_Right2_splitContainer.Size = new System.Drawing.Size(706, 411);
            this.Left2_Right2_splitContainer.SplitterDistance = 269;
            this.Left2_Right2_splitContainer.TabIndex = 0;
            // 
            // Split_listBox
            // 
            this.Split_listBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Split_listBox.FormattingEnabled = true;
            this.Split_listBox.ItemHeight = 12;
            this.Split_listBox.Location = new System.Drawing.Point(0, 0);
            this.Split_listBox.Name = "Split_listBox";
            this.Split_listBox.Size = new System.Drawing.Size(265, 407);
            this.Split_listBox.TabIndex = 0;
            // 
            // NLPScenarioForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(918, 458);
            this.Controls.Add(this.Left_Right_splitContainer);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "NLPScenarioForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "NLPScenarioForm";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.Left_Right_splitContainer.Panel1.ResumeLayout(false);
            this.Left_Right_splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Left_Right_splitContainer)).EndInit();
            this.Left_Right_splitContainer.ResumeLayout(false);
            this.Left2_Right2_splitContainer.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Left2_Right2_splitContainer)).EndInit();
            this.Left2_Right2_splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.SplitContainer Left_Right_splitContainer;
        private System.Windows.Forms.ToolStripButton Split_toolStripButton;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripButton Open_toolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ListBox Corpus_listBox;
        private System.Windows.Forms.SplitContainer Left2_Right2_splitContainer;
        private System.Windows.Forms.ListBox Split_listBox;
    }
}