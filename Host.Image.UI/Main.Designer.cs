namespace Host.Image.UI
{
    partial class Main
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.文件FToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.open_toolstripmenuitem = new System.Windows.Forms.ToolStripMenuItem();
            this.工具TToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SLIO_toolStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.empty_statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.map_processBar = new System.Windows.Forms.ToolStripProgressBar();
            this.map_statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer_main = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tab_treeview = new System.Windows.Forms.TabPage();
            this.map_treeView = new System.Windows.Forms.TreeView();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tab_mapview = new System.Windows.Forms.TabPage();
            this.map_pictureBox = new System.Windows.Forms.PictureBox();
            this.map_contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.open_contextMenuStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.tree_contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.bandCombine_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_main)).BeginInit();
            this.splitContainer_main.Panel1.SuspendLayout();
            this.splitContainer_main.Panel2.SuspendLayout();
            this.splitContainer_main.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tab_treeview.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tab_mapview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.map_pictureBox)).BeginInit();
            this.map_contextMenuStrip.SuspendLayout();
            this.tree_contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.White;
            this.menuStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件FToolStripMenuItem,
            this.工具TToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(887, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 文件FToolStripMenuItem
            // 
            this.文件FToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.open_toolstripmenuitem});
            this.文件FToolStripMenuItem.Name = "文件FToolStripMenuItem";
            this.文件FToolStripMenuItem.Size = new System.Drawing.Size(58, 21);
            this.文件FToolStripMenuItem.Text = "文件(&F)";
            // 
            // open_toolstripmenuitem
            // 
            this.open_toolstripmenuitem.Image = ((System.Drawing.Image)(resources.GetObject("open_toolstripmenuitem.Image")));
            this.open_toolstripmenuitem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.open_toolstripmenuitem.Name = "open_toolstripmenuitem";
            this.open_toolstripmenuitem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.open_toolstripmenuitem.Size = new System.Drawing.Size(165, 22);
            this.open_toolstripmenuitem.Text = "打开(&O)";
            this.open_toolstripmenuitem.Click += new System.EventHandler(this.Map_Click);
            // 
            // 工具TToolStripMenuItem
            // 
            this.工具TToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SLIO_toolStrip});
            this.工具TToolStripMenuItem.Name = "工具TToolStripMenuItem";
            this.工具TToolStripMenuItem.Size = new System.Drawing.Size(59, 21);
            this.工具TToolStripMenuItem.Text = "工具(&T)";
            // 
            // SLIO_toolStrip
            // 
            this.SLIO_toolStrip.Name = "SLIO_toolStrip";
            this.SLIO_toolStrip.Size = new System.Drawing.Size(136, 22);
            this.SLIO_toolStrip.Text = "超像素分割";
            this.SLIO_toolStrip.Click += new System.EventHandler(this.Algorithm_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Location = new System.Drawing.Point(0, 25);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(887, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.empty_statusLabel,
            this.map_processBar,
            this.map_statusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 631);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.ShowItemToolTips = true;
            this.statusStrip1.Size = new System.Drawing.Size(887, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // empty_statusLabel
            // 
            this.empty_statusLabel.Name = "empty_statusLabel";
            this.empty_statusLabel.Size = new System.Drawing.Size(840, 17);
            this.empty_statusLabel.Spring = true;
            // 
            // map_processBar
            // 
            this.map_processBar.Name = "map_processBar";
            this.map_processBar.Size = new System.Drawing.Size(100, 16);
            this.map_processBar.Visible = false;
            // 
            // map_statusLabel
            // 
            this.map_statusLabel.Name = "map_statusLabel";
            this.map_statusLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.map_statusLabel.Size = new System.Drawing.Size(32, 17);
            this.map_statusLabel.Text = "就绪";
            this.map_statusLabel.ToolTipText = "指示当前工具运行状态";
            // 
            // splitContainer_main
            // 
            this.splitContainer_main.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_main.Location = new System.Drawing.Point(0, 50);
            this.splitContainer_main.Name = "splitContainer_main";
            // 
            // splitContainer_main.Panel1
            // 
            this.splitContainer_main.Panel1.Controls.Add(this.tabControl1);
            // 
            // splitContainer_main.Panel2
            // 
            this.splitContainer_main.Panel2.Controls.Add(this.tabControl2);
            this.splitContainer_main.Size = new System.Drawing.Size(887, 581);
            this.splitContainer_main.SplitterDistance = 181;
            this.splitContainer_main.TabIndex = 3;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tab_treeview);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(177, 577);
            this.tabControl1.TabIndex = 0;
            // 
            // tab_treeview
            // 
            this.tab_treeview.Controls.Add(this.map_treeView);
            this.tab_treeview.Location = new System.Drawing.Point(4, 22);
            this.tab_treeview.Name = "tab_treeview";
            this.tab_treeview.Padding = new System.Windows.Forms.Padding(3);
            this.tab_treeview.Size = new System.Drawing.Size(169, 551);
            this.tab_treeview.TabIndex = 0;
            this.tab_treeview.Text = "树视图";
            this.tab_treeview.UseVisualStyleBackColor = true;
            // 
            // map_treeView
            // 
            this.map_treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.map_treeView.Location = new System.Drawing.Point(3, 3);
            this.map_treeView.Name = "map_treeView";
            this.map_treeView.Size = new System.Drawing.Size(163, 545);
            this.map_treeView.TabIndex = 0;
            this.map_treeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.map_treeView_NodeMouseClick);
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tab_mapview);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(698, 577);
            this.tabControl2.TabIndex = 0;
            // 
            // tab_mapview
            // 
            this.tab_mapview.Controls.Add(this.map_pictureBox);
            this.tab_mapview.Location = new System.Drawing.Point(4, 22);
            this.tab_mapview.Name = "tab_mapview";
            this.tab_mapview.Padding = new System.Windows.Forms.Padding(3);
            this.tab_mapview.Size = new System.Drawing.Size(690, 551);
            this.tab_mapview.TabIndex = 1;
            this.tab_mapview.Text = "地图";
            this.tab_mapview.UseVisualStyleBackColor = true;
            // 
            // map_pictureBox
            // 
            this.map_pictureBox.ContextMenuStrip = this.map_contextMenuStrip;
            this.map_pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.map_pictureBox.Location = new System.Drawing.Point(3, 3);
            this.map_pictureBox.Name = "map_pictureBox";
            this.map_pictureBox.Size = new System.Drawing.Size(684, 545);
            this.map_pictureBox.TabIndex = 0;
            this.map_pictureBox.TabStop = false;
            // 
            // map_contextMenuStrip
            // 
            this.map_contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.open_contextMenuStrip});
            this.map_contextMenuStrip.Name = "map_contextMenuStrip";
            this.map_contextMenuStrip.ShowImageMargin = false;
            this.map_contextMenuStrip.Size = new System.Drawing.Size(76, 26);
            // 
            // open_contextMenuStrip
            // 
            this.open_contextMenuStrip.Name = "open_contextMenuStrip";
            this.open_contextMenuStrip.Size = new System.Drawing.Size(75, 22);
            this.open_contextMenuStrip.Text = "打开";
            this.open_contextMenuStrip.Click += new System.EventHandler(this.Map_Click);
            // 
            // tree_contextMenuStrip
            // 
            this.tree_contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bandCombine_ToolStripMenuItem});
            this.tree_contextMenuStrip.Name = "tree_contextMenuStrip";
            this.tree_contextMenuStrip.ShowImageMargin = false;
            this.tree_contextMenuStrip.Size = new System.Drawing.Size(128, 48);
            // 
            // bandCombine_ToolStripMenuItem
            // 
            this.bandCombine_ToolStripMenuItem.Name = "bandCombine_ToolStripMenuItem";
            this.bandCombine_ToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.bandCombine_ToolStripMenuItem.Text = "波段合成";
            this.bandCombine_ToolStripMenuItem.Click += new System.EventHandler(this.Tree_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(887, 653);
            this.Controls.Add(this.splitContainer_main);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Main";
            this.Text = "图像处理可视化工具";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer_main.Panel1.ResumeLayout(false);
            this.splitContainer_main.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_main)).EndInit();
            this.splitContainer_main.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tab_treeview.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tab_mapview.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.map_pictureBox)).EndInit();
            this.map_contextMenuStrip.ResumeLayout(false);
            this.tree_contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 文件FToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem open_toolstripmenuitem;
        private System.Windows.Forms.ToolStripMenuItem 工具TToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SLIO_toolStrip;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.SplitContainer splitContainer_main;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tab_treeview;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tab_mapview;
        private System.Windows.Forms.TreeView map_treeView;
        private System.Windows.Forms.PictureBox map_pictureBox;
        private System.Windows.Forms.ToolStripProgressBar map_processBar;
        private System.Windows.Forms.ToolStripStatusLabel map_statusLabel;
        private System.Windows.Forms.ToolStripStatusLabel empty_statusLabel;
        private System.Windows.Forms.ContextMenuStrip map_contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem open_contextMenuStrip;
        private System.Windows.Forms.ContextMenuStrip tree_contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem bandCombine_ToolStripMenuItem;
    }
}

