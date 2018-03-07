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
            this.map_menuStrip = new System.Windows.Forms.MenuStrip();
            this.文件FToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.open_toolstripmenuitem = new System.Windows.Forms.ToolStripMenuItem();
            this.工具TToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SLIC_toolStripMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.SLIC_Center_toolStripMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.map_toolStrip = new System.Windows.Forms.ToolStrip();
            this.SLIC_toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.SLIC_Center_toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.map_statusStrip = new System.Windows.Forms.StatusStrip();
            this.empty_statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.map_processBar = new System.Windows.Forms.ToolStripProgressBar();
            this.map_statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer_main = new System.Windows.Forms.SplitContainer();
            this.map_tabControl = new System.Windows.Forms.TabControl();
            this.tab_treeview = new System.Windows.Forms.TabPage();
            this.map_treeView = new System.Windows.Forms.TreeView();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tab_mapview = new System.Windows.Forms.TabPage();
            this.map_pictureBox = new System.Windows.Forms.PictureBox();
            this.map_contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.open_contextMenuStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.tree_contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.bandCombine_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.map_menuStrip.SuspendLayout();
            this.map_toolStrip.SuspendLayout();
            this.map_statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_main)).BeginInit();
            this.splitContainer_main.Panel1.SuspendLayout();
            this.splitContainer_main.Panel2.SuspendLayout();
            this.splitContainer_main.SuspendLayout();
            this.map_tabControl.SuspendLayout();
            this.tab_treeview.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tab_mapview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.map_pictureBox)).BeginInit();
            this.map_contextMenuStrip.SuspendLayout();
            this.tree_contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // map_menuStrip
            // 
            this.map_menuStrip.BackColor = System.Drawing.Color.White;
            this.map_menuStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.map_menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件FToolStripMenuItem,
            this.工具TToolStripMenuItem});
            this.map_menuStrip.Location = new System.Drawing.Point(0, 0);
            this.map_menuStrip.Name = "map_menuStrip";
            this.map_menuStrip.Size = new System.Drawing.Size(887, 25);
            this.map_menuStrip.TabIndex = 0;
            this.map_menuStrip.Text = "menuStrip1";
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
            this.open_toolstripmenuitem.Click += new System.EventHandler(this.Map_function_Click);
            // 
            // 工具TToolStripMenuItem
            // 
            this.工具TToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SLIC_toolStripMenu,
            this.SLIC_Center_toolStripMenu});
            this.工具TToolStripMenuItem.Name = "工具TToolStripMenuItem";
            this.工具TToolStripMenuItem.Size = new System.Drawing.Size(59, 21);
            this.工具TToolStripMenuItem.Text = "工具(&T)";
            // 
            // SLIC_toolStripMenu
            // 
            this.SLIC_toolStripMenu.Image = global::Host.Image.UI.Properties.Resources.calculator_64;
            this.SLIC_toolStripMenu.Name = "SLIC_toolStripMenu";
            this.SLIC_toolStripMenu.Size = new System.Drawing.Size(160, 22);
            this.SLIC_toolStripMenu.Text = "超像素分割";
            this.SLIC_toolStripMenu.Click += new System.EventHandler(this.Map_function_Click);
            // 
            // SLIC_Center_toolStripMenu
            // 
            this.SLIC_Center_toolStripMenu.Image = global::Host.Image.UI.Properties.Resources.cut_64;
            this.SLIC_Center_toolStripMenu.Name = "SLIC_Center_toolStripMenu";
            this.SLIC_Center_toolStripMenu.Size = new System.Drawing.Size(160, 22);
            this.SLIC_Center_toolStripMenu.Text = "超像素特征提取";
            this.SLIC_Center_toolStripMenu.Click += new System.EventHandler(this.Map_function_Click);
            // 
            // map_toolStrip
            // 
            this.map_toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SLIC_toolStripButton,
            this.SLIC_Center_toolStripButton});
            this.map_toolStrip.Location = new System.Drawing.Point(0, 25);
            this.map_toolStrip.Name = "map_toolStrip";
            this.map_toolStrip.Size = new System.Drawing.Size(887, 40);
            this.map_toolStrip.TabIndex = 1;
            this.map_toolStrip.Text = "toolStrip1";
            // 
            // SLIC_toolStripButton
            // 
            this.SLIC_toolStripButton.Image = global::Host.Image.UI.Properties.Resources.calculator_64;
            this.SLIC_toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SLIC_toolStripButton.Name = "SLIC_toolStripButton";
            this.SLIC_toolStripButton.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.SLIC_toolStripButton.Size = new System.Drawing.Size(48, 37);
            this.SLIC_toolStripButton.Text = "超像素";
            this.SLIC_toolStripButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.SLIC_toolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.SLIC_toolStripButton.Click += new System.EventHandler(this.Map_function_Click);
            // 
            // SLIC_Center_toolStripButton
            // 
            this.SLIC_Center_toolStripButton.Image = global::Host.Image.UI.Properties.Resources.cut_64;
            this.SLIC_Center_toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SLIC_Center_toolStripButton.Name = "SLIC_Center_toolStripButton";
            this.SLIC_Center_toolStripButton.Size = new System.Drawing.Size(60, 37);
            this.SLIC_Center_toolStripButton.Text = "中心提取";
            this.SLIC_Center_toolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.SLIC_Center_toolStripButton.Click += new System.EventHandler(this.Map_function_Click);
            // 
            // map_statusStrip
            // 
            this.map_statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.empty_statusLabel,
            this.map_processBar,
            this.map_statusLabel});
            this.map_statusStrip.Location = new System.Drawing.Point(0, 631);
            this.map_statusStrip.Name = "map_statusStrip";
            this.map_statusStrip.ShowItemToolTips = true;
            this.map_statusStrip.Size = new System.Drawing.Size(887, 22);
            this.map_statusStrip.TabIndex = 2;
            this.map_statusStrip.Text = "statusStrip1";
            // 
            // empty_statusLabel
            // 
            this.empty_statusLabel.Name = "empty_statusLabel";
            this.empty_statusLabel.Size = new System.Drawing.Size(824, 17);
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
            this.map_statusLabel.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.map_statusLabel.Image = global::Host.Image.UI.Properties.Resources.smile_64;
            this.map_statusLabel.Name = "map_statusLabel";
            this.map_statusLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.map_statusLabel.Size = new System.Drawing.Size(48, 17);
            this.map_statusLabel.Text = "就绪";
            this.map_statusLabel.ToolTipText = "指示当前工具运行状态";
            // 
            // splitContainer_main
            // 
            this.splitContainer_main.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_main.Location = new System.Drawing.Point(0, 65);
            this.splitContainer_main.Name = "splitContainer_main";
            // 
            // splitContainer_main.Panel1
            // 
            this.splitContainer_main.Panel1.Controls.Add(this.map_tabControl);
            // 
            // splitContainer_main.Panel2
            // 
            this.splitContainer_main.Panel2.Controls.Add(this.tabControl2);
            this.splitContainer_main.Size = new System.Drawing.Size(887, 566);
            this.splitContainer_main.SplitterDistance = 181;
            this.splitContainer_main.TabIndex = 3;
            // 
            // map_tabControl
            // 
            this.map_tabControl.Controls.Add(this.tab_treeview);
            this.map_tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.map_tabControl.Location = new System.Drawing.Point(0, 0);
            this.map_tabControl.Name = "map_tabControl";
            this.map_tabControl.SelectedIndex = 0;
            this.map_tabControl.Size = new System.Drawing.Size(177, 562);
            this.map_tabControl.TabIndex = 0;
            // 
            // tab_treeview
            // 
            this.tab_treeview.Controls.Add(this.map_treeView);
            this.tab_treeview.Location = new System.Drawing.Point(4, 22);
            this.tab_treeview.Name = "tab_treeview";
            this.tab_treeview.Padding = new System.Windows.Forms.Padding(3);
            this.tab_treeview.Size = new System.Drawing.Size(169, 536);
            this.tab_treeview.TabIndex = 0;
            this.tab_treeview.Text = "视图";
            this.tab_treeview.UseVisualStyleBackColor = true;
            // 
            // map_treeView
            // 
            this.map_treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.map_treeView.Location = new System.Drawing.Point(3, 3);
            this.map_treeView.Name = "map_treeView";
            this.map_treeView.Size = new System.Drawing.Size(163, 530);
            this.map_treeView.TabIndex = 0;
            this.map_treeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.Map_treeView_NodeMouseClick);
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tab_mapview);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(698, 562);
            this.tabControl2.TabIndex = 0;
            // 
            // tab_mapview
            // 
            this.tab_mapview.Controls.Add(this.map_pictureBox);
            this.tab_mapview.Location = new System.Drawing.Point(4, 22);
            this.tab_mapview.Name = "tab_mapview";
            this.tab_mapview.Padding = new System.Windows.Forms.Padding(3);
            this.tab_mapview.Size = new System.Drawing.Size(690, 536);
            this.tab_mapview.TabIndex = 1;
            this.tab_mapview.Text = "地图";
            this.tab_mapview.UseVisualStyleBackColor = true;
            // 
            // map_pictureBox
            // 
            this.map_pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.map_pictureBox.ContextMenuStrip = this.map_contextMenuStrip;
            this.map_pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.map_pictureBox.Location = new System.Drawing.Point(3, 3);
            this.map_pictureBox.Name = "map_pictureBox";
            this.map_pictureBox.Size = new System.Drawing.Size(684, 530);
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
            this.open_contextMenuStrip.Click += new System.EventHandler(this.Map_function_Click);
            // 
            // tree_contextMenuStrip
            // 
            this.tree_contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bandCombine_ToolStripMenuItem});
            this.tree_contextMenuStrip.Name = "tree_contextMenuStrip";
            this.tree_contextMenuStrip.ShowImageMargin = false;
            this.tree_contextMenuStrip.Size = new System.Drawing.Size(100, 26);
            // 
            // bandCombine_ToolStripMenuItem
            // 
            this.bandCombine_ToolStripMenuItem.Name = "bandCombine_ToolStripMenuItem";
            this.bandCombine_ToolStripMenuItem.Size = new System.Drawing.Size(99, 22);
            this.bandCombine_ToolStripMenuItem.Text = "波段合成";
            this.bandCombine_ToolStripMenuItem.Click += new System.EventHandler(this.Map_treeView_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(887, 653);
            this.Controls.Add(this.splitContainer_main);
            this.Controls.Add(this.map_statusStrip);
            this.Controls.Add(this.map_toolStrip);
            this.Controls.Add(this.map_menuStrip);
            this.MainMenuStrip = this.map_menuStrip;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "图像处理可视化工具";
            this.map_menuStrip.ResumeLayout(false);
            this.map_menuStrip.PerformLayout();
            this.map_toolStrip.ResumeLayout(false);
            this.map_toolStrip.PerformLayout();
            this.map_statusStrip.ResumeLayout(false);
            this.map_statusStrip.PerformLayout();
            this.splitContainer_main.Panel1.ResumeLayout(false);
            this.splitContainer_main.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_main)).EndInit();
            this.splitContainer_main.ResumeLayout(false);
            this.map_tabControl.ResumeLayout(false);
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

        private System.Windows.Forms.MenuStrip map_menuStrip;
        private System.Windows.Forms.ToolStripMenuItem 文件FToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem open_toolstripmenuitem;
        private System.Windows.Forms.ToolStripMenuItem 工具TToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SLIC_toolStripMenu;
        private System.Windows.Forms.ToolStrip map_toolStrip;
        private System.Windows.Forms.StatusStrip map_statusStrip;
        private System.Windows.Forms.SplitContainer splitContainer_main;
        private System.Windows.Forms.TabControl map_tabControl;
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
        private System.Windows.Forms.ToolStripMenuItem SLIC_Center_toolStripMenu;
        private System.Windows.Forms.ToolStripButton SLIC_toolStripButton;
        private System.Windows.Forms.ToolStripButton SLIC_Center_toolStripButton;
    }
}

