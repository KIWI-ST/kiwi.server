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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NLPScenarioForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.Open_toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.Split_toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.Print_Scenario_Text_toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.Similarity_toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.Preview_toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.Corpus_listBox = new System.Windows.Forms.ListBox();
            this.ListBox_contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.Exprot_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1.SuspendLayout();
            this.ListBox_contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Open_toolStripButton,
            this.toolStripSeparator1,
            this.Split_toolStripButton,
            this.Print_Scenario_Text_toolStripButton,
            this.Similarity_toolStripButton,
            this.Preview_toolStripButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(880, 27);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "NLP_toolStrip";
            // 
            // Open_toolStripButton
            // 
            this.Open_toolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("Open_toolStripButton.Image")));
            this.Open_toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Open_toolStripButton.Name = "Open_toolStripButton";
            this.Open_toolStripButton.Size = new System.Drawing.Size(93, 24);
            this.Open_toolStripButton.Text = "打开文本";
            this.Open_toolStripButton.Click += new System.EventHandler(this.ToolStripButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // Split_toolStripButton
            // 
            this.Split_toolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("Split_toolStripButton.Image")));
            this.Split_toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Split_toolStripButton.Name = "Split_toolStripButton";
            this.Split_toolStripButton.Size = new System.Drawing.Size(182, 24);
            this.Split_toolStripButton.Text = "基于TimeML解析重组";
            this.Split_toolStripButton.Click += new System.EventHandler(this.ToolStripButton_Click);
            // 
            // Print_Scenario_Text_toolStripButton
            // 
            this.Print_Scenario_Text_toolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("Print_Scenario_Text_toolStripButton.Image")));
            this.Print_Scenario_Text_toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Print_Scenario_Text_toolStripButton.Name = "Print_Scenario_Text_toolStripButton";
            this.Print_Scenario_Text_toolStripButton.Size = new System.Drawing.Size(138, 24);
            this.Print_Scenario_Text_toolStripButton.Text = "打印情景要素集";
            this.Print_Scenario_Text_toolStripButton.Click += new System.EventHandler(this.ToolStripButton_Click);
            // 
            // Similarity_toolStripButton
            // 
            this.Similarity_toolStripButton.Image = global::Host.UI.Properties.Resources.dangan;
            this.Similarity_toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Similarity_toolStripButton.Name = "Similarity_toolStripButton";
            this.Similarity_toolStripButton.Size = new System.Drawing.Size(108, 24);
            this.Similarity_toolStripButton.Text = "相似度计算";
            this.Similarity_toolStripButton.Click += new System.EventHandler(this.ToolStripButton_Click);
            // 
            // Preview_toolStripButton
            // 
            this.Preview_toolStripButton.Image = global::Host.UI.Properties.Resources.bofang1;
            this.Preview_toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Preview_toolStripButton.Name = "Preview_toolStripButton";
            this.Preview_toolStripButton.Size = new System.Drawing.Size(63, 24);
            this.Preview_toolStripButton.Text = "预览";
            this.Preview_toolStripButton.Click += new System.EventHandler(this.ToolStripButton_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Location = new System.Drawing.Point(0, 674);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStrip1.Size = new System.Drawing.Size(880, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "NLP_statusStrip";
            // 
            // Corpus_listBox
            // 
            this.Corpus_listBox.ContextMenuStrip = this.ListBox_contextMenuStrip;
            this.Corpus_listBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Corpus_listBox.FormattingEnabled = true;
            this.Corpus_listBox.ItemHeight = 15;
            this.Corpus_listBox.Location = new System.Drawing.Point(0, 27);
            this.Corpus_listBox.Margin = new System.Windows.Forms.Padding(4);
            this.Corpus_listBox.Name = "Corpus_listBox";
            this.Corpus_listBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.Corpus_listBox.Size = new System.Drawing.Size(880, 647);
            this.Corpus_listBox.TabIndex = 2;
            // 
            // ListBox_contextMenuStrip
            // 
            this.ListBox_contextMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ListBox_contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Exprot_ToolStripMenuItem});
            this.ListBox_contextMenuStrip.Name = "ListBox_contextMenuStrip";
            this.ListBox_contextMenuStrip.Size = new System.Drawing.Size(109, 28);
            // 
            // Exprot_ToolStripMenuItem
            // 
            this.Exprot_ToolStripMenuItem.Name = "Exprot_ToolStripMenuItem";
            this.Exprot_ToolStripMenuItem.Size = new System.Drawing.Size(108, 24);
            this.Exprot_ToolStripMenuItem.Text = "导出";
            this.Exprot_ToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // NLPScenarioForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(880, 696);
            this.Controls.Add(this.Corpus_listBox);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "NLPScenarioForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NLPScenarioForm";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ListBox_contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripButton Split_toolStripButton;
        private System.Windows.Forms.ToolStripButton Print_Scenario_Text_toolStripButton;
        private System.Windows.Forms.ToolStripButton Similarity_toolStripButton;
        private System.Windows.Forms.ToolStripButton Open_toolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ListBox Corpus_listBox;
        private System.Windows.Forms.ToolStripButton Preview_toolStripButton;
        private System.Windows.Forms.ContextMenuStrip ListBox_contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem Exprot_ToolStripMenuItem;
    }
}