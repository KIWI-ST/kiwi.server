namespace Host.Image.UI.PlotForm
{
    partial class DQNPlotFrom
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
            this.dqn_plotView = new OxyPlot.WindowsForms.PlotView();
            this.DQNPlotForm_contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.另存为ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DQNPlotForm_contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // dqn_plotView
            // 
            this.dqn_plotView.BackColor = System.Drawing.Color.White;
            this.dqn_plotView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dqn_plotView.Location = new System.Drawing.Point(0, 0);
            this.dqn_plotView.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dqn_plotView.Name = "dqn_plotView";
            this.dqn_plotView.Padding = new System.Windows.Forms.Padding(10);
            this.dqn_plotView.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.dqn_plotView.Size = new System.Drawing.Size(668, 385);
            this.dqn_plotView.TabIndex = 0;
            this.dqn_plotView.Text = "plot1";
            this.dqn_plotView.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.dqn_plotView.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.dqn_plotView.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // DQNPlotForm_contextMenuStrip
            // 
            this.DQNPlotForm_contextMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.DQNPlotForm_contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.另存为ToolStripMenuItem});
            this.DQNPlotForm_contextMenuStrip.Name = "DQNPlotForm_contextMenuStrip";
            this.DQNPlotForm_contextMenuStrip.ShowImageMargin = false;
            this.DQNPlotForm_contextMenuStrip.Size = new System.Drawing.Size(99, 28);
            // 
            // 另存为ToolStripMenuItem
            // 
            this.另存为ToolStripMenuItem.Name = "另存为ToolStripMenuItem";
            this.另存为ToolStripMenuItem.Size = new System.Drawing.Size(98, 24);
            this.另存为ToolStripMenuItem.Text = "另存为";
            this.另存为ToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // DQNPlotFrom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(668, 385);
            this.ContextMenuStrip = this.DQNPlotForm_contextMenuStrip;
            this.Controls.Add(this.dqn_plotView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "DQNPlotFrom";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PlotForm";
            this.DQNPlotForm_contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private OxyPlot.WindowsForms.PlotView dqn_plotView;
        private System.Windows.Forms.ContextMenuStrip DQNPlotForm_contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem 另存为ToolStripMenuItem;
    }
}