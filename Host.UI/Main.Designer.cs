namespace Host.UI
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
            this.CNN_toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.rf_toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.DQN_toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.main_toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.cov_toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.kappa_toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.Compare_Plot_toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.task_toolStripButton = new System.Windows.Forms.ToolStripButton();
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
            this.map_splitContainer = new System.Windows.Forms.SplitContainer();
            this.map_pictureBox = new System.Windows.Forms.PictureBox();
            this.map_contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.open_contextMenuStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.map_listBox = new System.Windows.Forms.ListBox();
            this.tree_contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.bandCombine_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.bandExport_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            ((System.ComponentModel.ISupportInitialize)(this.map_splitContainer)).BeginInit();
            this.map_splitContainer.Panel1.SuspendLayout();
            this.map_splitContainer.Panel2.SuspendLayout();
            this.map_splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.map_pictureBox)).BeginInit();
            this.map_contextMenuStrip.SuspendLayout();
            this.tree_contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // map_menuStrip
            // 
            this.map_menuStrip.BackColor = System.Drawing.Color.White;
            this.map_menuStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.map_menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.map_menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件FToolStripMenuItem,
            this.工具TToolStripMenuItem});
            this.map_menuStrip.Location = new System.Drawing.Point(0, 0);
            this.map_menuStrip.Name = "map_menuStrip";
            this.map_menuStrip.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.map_menuStrip.Size = new System.Drawing.Size(1183, 28);
            this.map_menuStrip.TabIndex = 0;
            this.map_menuStrip.Text = "menuStrip1";
            // 
            // 文件FToolStripMenuItem
            // 
            this.文件FToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.open_toolstripmenuitem});
            this.文件FToolStripMenuItem.Name = "文件FToolStripMenuItem";
            this.文件FToolStripMenuItem.Size = new System.Drawing.Size(69, 24);
            this.文件FToolStripMenuItem.Text = "文件(&F)";
            // 
            // open_toolstripmenuitem
            // 
            this.open_toolstripmenuitem.Image = ((System.Drawing.Image)(resources.GetObject("open_toolstripmenuitem.Image")));
            this.open_toolstripmenuitem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.open_toolstripmenuitem.Name = "open_toolstripmenuitem";
            this.open_toolstripmenuitem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.open_toolstripmenuitem.Size = new System.Drawing.Size(194, 26);
            this.open_toolstripmenuitem.Text = "打开(&O)";
            this.open_toolstripmenuitem.Click += new System.EventHandler(this.Map_function_Click);
            // 
            // 工具TToolStripMenuItem
            // 
            this.工具TToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SLIC_toolStripMenu,
            this.SLIC_Center_toolStripMenu});
            this.工具TToolStripMenuItem.Name = "工具TToolStripMenuItem";
            this.工具TToolStripMenuItem.Size = new System.Drawing.Size(70, 24);
            this.工具TToolStripMenuItem.Text = "工具(&T)";
            // 
            // SLIC_toolStripMenu
            // 
            this.SLIC_toolStripMenu.Image = global::Host.UI.Properties.Resources.calculator_64;
            this.SLIC_toolStripMenu.Name = "SLIC_toolStripMenu";
            this.SLIC_toolStripMenu.Size = new System.Drawing.Size(216, 26);
            this.SLIC_toolStripMenu.Text = "超像素分割";
            this.SLIC_toolStripMenu.Click += new System.EventHandler(this.Map_function_Click);
            // 
            // SLIC_Center_toolStripMenu
            // 
            this.SLIC_Center_toolStripMenu.Image = global::Host.UI.Properties.Resources.cut_64;
            this.SLIC_Center_toolStripMenu.Name = "SLIC_Center_toolStripMenu";
            this.SLIC_Center_toolStripMenu.Size = new System.Drawing.Size(216, 26);
            this.SLIC_Center_toolStripMenu.Text = "超像素特征提取";
            this.SLIC_Center_toolStripMenu.Click += new System.EventHandler(this.Map_function_Click);
            // 
            // map_toolStrip
            // 
            this.map_toolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.map_toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SLIC_toolStripButton,
            this.SLIC_Center_toolStripButton,
            this.CNN_toolStripButton,
            this.rf_toolStripButton,
            this.DQN_toolStripButton,
            this.main_toolStripSeparator,
            this.cov_toolStripButton,
            this.kappa_toolStripButton,
            this.toolStripSeparator1,
            this.Compare_Plot_toolStripButton,
            this.task_toolStripButton});
            this.map_toolStrip.Location = new System.Drawing.Point(0, 28);
            this.map_toolStrip.Name = "map_toolStrip";
            this.map_toolStrip.Size = new System.Drawing.Size(1183, 47);
            this.map_toolStrip.TabIndex = 1;
            this.map_toolStrip.Text = "toolStrip1";
            // 
            // SLIC_toolStripButton
            // 
            this.SLIC_toolStripButton.Image = global::Host.UI.Properties.Resources.calculator_64;
            this.SLIC_toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SLIC_toolStripButton.Name = "SLIC_toolStripButton";
            this.SLIC_toolStripButton.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.SLIC_toolStripButton.Size = new System.Drawing.Size(58, 44);
            this.SLIC_toolStripButton.Text = "超像素";
            this.SLIC_toolStripButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.SLIC_toolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.SLIC_toolStripButton.Click += new System.EventHandler(this.Map_function_Click);
            // 
            // SLIC_Center_toolStripButton
            // 
            this.SLIC_Center_toolStripButton.Image = global::Host.UI.Properties.Resources.cut_64;
            this.SLIC_Center_toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SLIC_Center_toolStripButton.Name = "SLIC_Center_toolStripButton";
            this.SLIC_Center_toolStripButton.Size = new System.Drawing.Size(73, 44);
            this.SLIC_Center_toolStripButton.Text = "中心提取";
            this.SLIC_Center_toolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.SLIC_Center_toolStripButton.Click += new System.EventHandler(this.Map_function_Click);
            // 
            // CNN_toolStripButton
            // 
            this.CNN_toolStripButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.CNN_toolStripButton.Image = global::Host.UI.Properties.Resources.disk_64;
            this.CNN_toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CNN_toolStripButton.Name = "CNN_toolStripButton";
            this.CNN_toolStripButton.Size = new System.Drawing.Size(47, 44);
            this.CNN_toolStripButton.Text = "CNN";
            this.CNN_toolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.CNN_toolStripButton.Click += new System.EventHandler(this.Map_function_Click);
            // 
            // rf_toolStripButton
            // 
            this.rf_toolStripButton.Image = global::Host.UI.Properties.Resources.bulb_off_64;
            this.rf_toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.rf_toolStripButton.Name = "rf_toolStripButton";
            this.rf_toolStripButton.Size = new System.Drawing.Size(73, 44);
            this.rf_toolStripButton.Text = "随机森林";
            this.rf_toolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.rf_toolStripButton.Click += new System.EventHandler(this.Map_function_Click);
            // 
            // DQN_toolStripButton
            // 
            this.DQN_toolStripButton.Image = global::Host.UI.Properties.Resources.laptop_64;
            this.DQN_toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DQN_toolStripButton.Name = "DQN_toolStripButton";
            this.DQN_toolStripButton.Size = new System.Drawing.Size(73, 44);
            this.DQN_toolStripButton.Text = "强化学习";
            this.DQN_toolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.DQN_toolStripButton.Click += new System.EventHandler(this.Map_function_Click);
            // 
            // main_toolStripSeparator
            // 
            this.main_toolStripSeparator.Name = "main_toolStripSeparator";
            this.main_toolStripSeparator.Size = new System.Drawing.Size(6, 47);
            // 
            // cov_toolStripButton
            // 
            this.cov_toolStripButton.Image = global::Host.UI.Properties.Resources.chart_area_64;
            this.cov_toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cov_toolStripButton.Name = "cov_toolStripButton";
            this.cov_toolStripButton.Size = new System.Drawing.Size(73, 44);
            this.cov_toolStripButton.Text = "相关性图";
            this.cov_toolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.cov_toolStripButton.Click += new System.EventHandler(this.Map_function_Click);
            // 
            // kappa_toolStripButton
            // 
            this.kappa_toolStripButton.Image = global::Host.UI.Properties.Resources.copy_64;
            this.kappa_toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.kappa_toolStripButton.Name = "kappa_toolStripButton";
            this.kappa_toolStripButton.Size = new System.Drawing.Size(87, 44);
            this.kappa_toolStripButton.Text = "kappa计算";
            this.kappa_toolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.kappa_toolStripButton.Click += new System.EventHandler(this.Map_function_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 47);
            // 
            // Compare_Plot_toolStripButton
            // 
            this.Compare_Plot_toolStripButton.Image = global::Host.UI.Properties.Resources.brush_64;
            this.Compare_Plot_toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Compare_Plot_toolStripButton.Name = "Compare_Plot_toolStripButton";
            this.Compare_Plot_toolStripButton.Size = new System.Drawing.Size(73, 44);
            this.Compare_Plot_toolStripButton.Text = "对比曲线";
            this.Compare_Plot_toolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.Compare_Plot_toolStripButton.Click += new System.EventHandler(this.Map_function_Click);
            // 
            // task_toolStripButton
            // 
            this.task_toolStripButton.Image = global::Host.UI.Properties.Resources.sum_64;
            this.task_toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.task_toolStripButton.Name = "task_toolStripButton";
            this.task_toolStripButton.Size = new System.Drawing.Size(73, 44);
            this.task_toolStripButton.Text = "任务管理";
            this.task_toolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.task_toolStripButton.Click += new System.EventHandler(this.Map_function_Click);
            // 
            // map_statusStrip
            // 
            this.map_statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.map_statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.empty_statusLabel,
            this.map_processBar,
            this.map_statusLabel});
            this.map_statusStrip.Location = new System.Drawing.Point(0, 791);
            this.map_statusStrip.Name = "map_statusStrip";
            this.map_statusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.map_statusStrip.ShowItemToolTips = true;
            this.map_statusStrip.Size = new System.Drawing.Size(1183, 25);
            this.map_statusStrip.TabIndex = 2;
            this.map_statusStrip.Text = "statusStrip1";
            // 
            // empty_statusLabel
            // 
            this.empty_statusLabel.Name = "empty_statusLabel";
            this.empty_statusLabel.Size = new System.Drawing.Size(1104, 20);
            this.empty_statusLabel.Spring = true;
            // 
            // map_processBar
            // 
            this.map_processBar.Name = "map_processBar";
            this.map_processBar.Size = new System.Drawing.Size(133, 24);
            this.map_processBar.Visible = false;
            // 
            // map_statusLabel
            // 
            this.map_statusLabel.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.map_statusLabel.Image = global::Host.UI.Properties.Resources.smile_64;
            this.map_statusLabel.Name = "map_statusLabel";
            this.map_statusLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.map_statusLabel.Size = new System.Drawing.Size(59, 20);
            this.map_statusLabel.Text = "就绪";
            this.map_statusLabel.ToolTipText = "指示当前工具运行状态";
            // 
            // splitContainer_main
            // 
            this.splitContainer_main.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_main.Location = new System.Drawing.Point(0, 75);
            this.splitContainer_main.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer_main.Name = "splitContainer_main";
            // 
            // splitContainer_main.Panel1
            // 
            this.splitContainer_main.Panel1.Controls.Add(this.map_tabControl);
            // 
            // splitContainer_main.Panel2
            // 
            this.splitContainer_main.Panel2.Controls.Add(this.tabControl2);
            this.splitContainer_main.Size = new System.Drawing.Size(1183, 716);
            this.splitContainer_main.SplitterDistance = 237;
            this.splitContainer_main.SplitterWidth = 5;
            this.splitContainer_main.TabIndex = 3;
            // 
            // map_tabControl
            // 
            this.map_tabControl.Controls.Add(this.tab_treeview);
            this.map_tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.map_tabControl.Location = new System.Drawing.Point(0, 0);
            this.map_tabControl.Margin = new System.Windows.Forms.Padding(4);
            this.map_tabControl.Name = "map_tabControl";
            this.map_tabControl.SelectedIndex = 0;
            this.map_tabControl.Size = new System.Drawing.Size(233, 712);
            this.map_tabControl.TabIndex = 0;
            // 
            // tab_treeview
            // 
            this.tab_treeview.Controls.Add(this.map_treeView);
            this.tab_treeview.Location = new System.Drawing.Point(4, 25);
            this.tab_treeview.Margin = new System.Windows.Forms.Padding(4);
            this.tab_treeview.Name = "tab_treeview";
            this.tab_treeview.Padding = new System.Windows.Forms.Padding(4);
            this.tab_treeview.Size = new System.Drawing.Size(225, 683);
            this.tab_treeview.TabIndex = 0;
            this.tab_treeview.Text = "视图";
            this.tab_treeview.UseVisualStyleBackColor = true;
            // 
            // map_treeView
            // 
            this.map_treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.map_treeView.Location = new System.Drawing.Point(4, 4);
            this.map_treeView.Margin = new System.Windows.Forms.Padding(4);
            this.map_treeView.Name = "map_treeView";
            this.map_treeView.Size = new System.Drawing.Size(217, 675);
            this.map_treeView.TabIndex = 0;
            this.map_treeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.Map_treeView_NodeMouseClick);
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tab_mapview);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(937, 712);
            this.tabControl2.TabIndex = 0;
            // 
            // tab_mapview
            // 
            this.tab_mapview.Controls.Add(this.map_splitContainer);
            this.tab_mapview.Location = new System.Drawing.Point(4, 25);
            this.tab_mapview.Margin = new System.Windows.Forms.Padding(4);
            this.tab_mapview.Name = "tab_mapview";
            this.tab_mapview.Padding = new System.Windows.Forms.Padding(4);
            this.tab_mapview.Size = new System.Drawing.Size(929, 683);
            this.tab_mapview.TabIndex = 1;
            this.tab_mapview.Text = "地图";
            this.tab_mapview.UseVisualStyleBackColor = true;
            // 
            // map_splitContainer
            // 
            this.map_splitContainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.map_splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.map_splitContainer.Location = new System.Drawing.Point(4, 4);
            this.map_splitContainer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.map_splitContainer.Name = "map_splitContainer";
            this.map_splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // map_splitContainer.Panel1
            // 
            this.map_splitContainer.Panel1.Controls.Add(this.map_pictureBox);
            // 
            // map_splitContainer.Panel2
            // 
            this.map_splitContainer.Panel2.Controls.Add(this.map_listBox);
            this.map_splitContainer.Size = new System.Drawing.Size(921, 675);
            this.map_splitContainer.SplitterDistance = 437;
            this.map_splitContainer.TabIndex = 1;
            // 
            // map_pictureBox
            // 
            this.map_pictureBox.ContextMenuStrip = this.map_contextMenuStrip;
            this.map_pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.map_pictureBox.Location = new System.Drawing.Point(0, 0);
            this.map_pictureBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.map_pictureBox.Name = "map_pictureBox";
            this.map_pictureBox.Size = new System.Drawing.Size(917, 433);
            this.map_pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.map_pictureBox.TabIndex = 0;
            this.map_pictureBox.TabStop = false;
            // 
            // map_contextMenuStrip
            // 
            this.map_contextMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.map_contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.open_contextMenuStrip});
            this.map_contextMenuStrip.Name = "map_contextMenuStrip";
            this.map_contextMenuStrip.ShowImageMargin = false;
            this.map_contextMenuStrip.Size = new System.Drawing.Size(84, 28);
            // 
            // open_contextMenuStrip
            // 
            this.open_contextMenuStrip.Name = "open_contextMenuStrip";
            this.open_contextMenuStrip.Size = new System.Drawing.Size(83, 24);
            this.open_contextMenuStrip.Text = "打开";
            this.open_contextMenuStrip.Click += new System.EventHandler(this.Map_function_Click);
            // 
            // map_listBox
            // 
            this.map_listBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.map_listBox.FormattingEnabled = true;
            this.map_listBox.ItemHeight = 15;
            this.map_listBox.Location = new System.Drawing.Point(0, 0);
            this.map_listBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.map_listBox.Name = "map_listBox";
            this.map_listBox.Size = new System.Drawing.Size(917, 230);
            this.map_listBox.TabIndex = 0;
            // 
            // tree_contextMenuStrip
            // 
            this.tree_contextMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.tree_contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bandCombine_ToolStripMenuItem,
            this.toolStripMenuItem1,
            this.bandExport_ToolStripMenuItem});
            this.tree_contextMenuStrip.Name = "tree_contextMenuStrip";
            this.tree_contextMenuStrip.ShowImageMargin = false;
            this.tree_contextMenuStrip.Size = new System.Drawing.Size(114, 58);
            // 
            // bandCombine_ToolStripMenuItem
            // 
            this.bandCombine_ToolStripMenuItem.Name = "bandCombine_ToolStripMenuItem";
            this.bandCombine_ToolStripMenuItem.Size = new System.Drawing.Size(113, 24);
            this.bandCombine_ToolStripMenuItem.Text = "波段合成";
            this.bandCombine_ToolStripMenuItem.Click += new System.EventHandler(this.Map_treeView_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(110, 6);
            // 
            // bandExport_ToolStripMenuItem
            // 
            this.bandExport_ToolStripMenuItem.Name = "bandExport_ToolStripMenuItem";
            this.bandExport_ToolStripMenuItem.Size = new System.Drawing.Size(113, 24);
            this.bandExport_ToolStripMenuItem.Text = "波段导出";
            this.bandExport_ToolStripMenuItem.Click += new System.EventHandler(this.Map_treeView_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1183, 816);
            this.Controls.Add(this.splitContainer_main);
            this.Controls.Add(this.map_statusStrip);
            this.Controls.Add(this.map_toolStrip);
            this.Controls.Add(this.map_menuStrip);
            this.MainMenuStrip = this.map_menuStrip;
            this.Margin = new System.Windows.Forms.Padding(4);
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
            this.map_splitContainer.Panel1.ResumeLayout(false);
            this.map_splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.map_splitContainer)).EndInit();
            this.map_splitContainer.ResumeLayout(false);
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
        private System.Windows.Forms.ToolStripButton DQN_toolStripButton;
        private System.Windows.Forms.ToolStripSeparator main_toolStripSeparator;
        private System.Windows.Forms.SplitContainer map_splitContainer;
        private System.Windows.Forms.PictureBox map_pictureBox;
        private System.Windows.Forms.ListBox map_listBox;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem bandExport_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton kappa_toolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton Compare_Plot_toolStripButton;
        private System.Windows.Forms.ToolStripButton CNN_toolStripButton;
        private System.Windows.Forms.ToolStripButton task_toolStripButton;
        private System.Windows.Forms.ToolStripButton rf_toolStripButton;
        private System.Windows.Forms.ToolStripButton cov_toolStripButton;
    }
}

