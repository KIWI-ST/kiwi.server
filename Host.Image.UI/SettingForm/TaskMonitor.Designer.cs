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
            this.SuspendLayout();
            // 
            // task_listView
            // 
            this.task_listView.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.task_listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.task_name,
            this.task_progress,
            this.task_summary});
            this.task_listView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.task_listView.FullRowSelect = true;
            this.task_listView.GridLines = true;
            this.task_listView.Location = new System.Drawing.Point(0, 0);
            this.task_listView.Name = "task_listView";
            this.task_listView.Size = new System.Drawing.Size(494, 341);
            this.task_listView.TabIndex = 1;
            this.task_listView.TileSize = new System.Drawing.Size(128, 38);
            this.task_listView.UseCompatibleStateImageBehavior = false;
            this.task_listView.View = System.Windows.Forms.View.Details;
            this.task_listView.Click += new System.EventHandler(this.task_listView_Click);
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
            this.task_contextMenuStrip.Name = "task_contextMenuStrip";
            this.task_contextMenuStrip.Size = new System.Drawing.Size(61, 4);
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
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView task_listView;
        private System.Windows.Forms.ColumnHeader task_name;
        private System.Windows.Forms.ColumnHeader task_progress;
        private System.Windows.Forms.ColumnHeader task_summary;
        private System.Windows.Forms.ContextMenuStrip task_contextMenuStrip;
    }
}