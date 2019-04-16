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
            this.Print_Scenario_Text_toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.Corpus_listBox = new System.Windows.Forms.ListBox();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Open_toolStripButton,
            this.toolStripSeparator1,
            this.Split_toolStripButton,
            this.Print_Scenario_Text_toolStripButton,
            this.toolStripButton3});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(660, 25);
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
            // Print_Scenario_Text_toolStripButton
            // 
            this.Print_Scenario_Text_toolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("Print_Scenario_Text_toolStripButton.Image")));
            this.Print_Scenario_Text_toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Print_Scenario_Text_toolStripButton.Name = "Print_Scenario_Text_toolStripButton";
            this.Print_Scenario_Text_toolStripButton.Size = new System.Drawing.Size(128, 22);
            this.Print_Scenario_Text_toolStripButton.Text = "PrintScenarioText";
            this.Print_Scenario_Text_toolStripButton.Click += new System.EventHandler(this.ToolStripButton_Click);
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
            this.statusStrip1.Location = new System.Drawing.Point(0, 535);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(660, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "NLP_statusStrip";
            // 
            // Corpus_listBox
            // 
            this.Corpus_listBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Corpus_listBox.FormattingEnabled = true;
            this.Corpus_listBox.ItemHeight = 12;
            this.Corpus_listBox.Location = new System.Drawing.Point(0, 25);
            this.Corpus_listBox.Name = "Corpus_listBox";
            this.Corpus_listBox.Size = new System.Drawing.Size(660, 510);
            this.Corpus_listBox.TabIndex = 2;
            // 
            // NLPScenarioForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(660, 557);
            this.Controls.Add(this.Corpus_listBox);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "NLPScenarioForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NLPScenarioForm";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripButton Split_toolStripButton;
        private System.Windows.Forms.ToolStripButton Print_Scenario_Text_toolStripButton;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripButton Open_toolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ListBox Corpus_listBox;
    }
}