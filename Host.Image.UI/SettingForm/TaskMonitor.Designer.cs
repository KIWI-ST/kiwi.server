namespace Host.Image.UI.SettingForm
{
    partial class TaskMonitor
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
            this.task_listView = new System.Windows.Forms.ListView();
            this.task_name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.task_progress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.task_summary = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.task_contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.result_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.accuracy_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.统计曲线ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.task_contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // task_listView
            // 
            this.task_listView.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.task_listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.task_name,
            this.task_progress,
            this.task_summary});
            this.task_listView.ContextMenuStrip = this.task_contextMenuStrip;
            this.task_listView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.task_listView.FullRowSelect = true;
            this.task_listView.GridLines = true;
            this.task_listView.Location = new System.Drawing.Point(0, 0);
            this.task_listView.MultiSelect = false;
            this.task_listView.Name = "task_listView";
            this.task_listView.Size = new System.Drawing.Size(494, 341);
            this.task_listView.TabIndex = 1;
            this.task_listView.TileSize = new System.Drawing.Size(128, 38);
            this.task_listView.UseCompatibleStateImageBehavior = false;
            this.task_listView.View = System.Windows.Forms.View.Details;
            this.task_listView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.task_listView_MouseClick);
            // 
            // task_name
            // 
            this.task_name.Text = "任务名";
            this.task_name.Width = 119;
            // 
            // task_progress
            // 
            this.task_progress.Text = "任务进度";
            this.task_progress.Width = 145;
            // 
            // task_summary
            // 
            this.task_summary.Text = "总结";
            this.task_summary.Width = 224;
            // 
            // task_contextMenuStrip
            // 
            this.task_contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.result_ToolStripMenuItem,
            this.toolStripMenuItem1,
            this.accuracy_ToolStripMenuItem,
            this.统计曲线ToolStripMenuItem});
            this.task_contextMenuStrip.Name = "task_contextMenuStrip";
            this.task_contextMenuStrip.ShowImageMargin = false;
            this.task_contextMenuStrip.Size = new System.Drawing.Size(100, 76);
            // 
            // result_ToolStripMenuItem
            // 
            this.result_ToolStripMenuItem.Name = "result_ToolStripMenuItem";
            this.result_ToolStripMenuItem.Size = new System.Drawing.Size(99, 22);
            this.result_ToolStripMenuItem.Text = "处理结果";
            this.result_ToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(96, 6);
            // 
            // accuracy_ToolStripMenuItem
            // 
            this.accuracy_ToolStripMenuItem.Name = "accuracy_ToolStripMenuItem";
            this.accuracy_ToolStripMenuItem.Size = new System.Drawing.Size(99, 22);
            this.accuracy_ToolStripMenuItem.Text = "精度曲线";
            this.accuracy_ToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // 统计曲线ToolStripMenuItem
            // 
            this.统计曲线ToolStripMenuItem.Name = "统计曲线ToolStripMenuItem";
            this.统计曲线ToolStripMenuItem.Size = new System.Drawing.Size(99, 22);
            this.统计曲线ToolStripMenuItem.Text = "统计曲线";
            // 
            // TaskMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(494, 341);
            this.Controls.Add(this.task_listView);
            this.Name = "TaskMonitor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "TaskMonitor";
            this.task_contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView task_listView;
        private System.Windows.Forms.ColumnHeader task_name;
        private System.Windows.Forms.ColumnHeader task_progress;
        private System.Windows.Forms.ColumnHeader task_summary;
        private System.Windows.Forms.ContextMenuStrip task_contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem result_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem accuracy_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 统计曲线ToolStripMenuItem;
    }
}