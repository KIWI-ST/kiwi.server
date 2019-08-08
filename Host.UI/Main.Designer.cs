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
            //release process
            if (_process != null)
            {
                _process.OutputDataReceived -= Process_OutputDataReceived;
                _process.ErrorDataReceived -= Process_OutputDataReceived;
                _process?.Kill();
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
            this.map_toolStrip = new System.Windows.Forms.ToolStrip();
            this.Haplo_toolStripSplitButton = new System.Windows.Forms.ToolStripSplitButton();
            this.RandomForest_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.L2SVM_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Hybrid_toolStripSplitButton = new System.Windows.Forms.ToolStripSplitButton();
            this.CNN_SVM_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.CNN_toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.DQN_Classification_toolStripSplitButton = new System.Windows.Forms.ToolStripSplitButton();
            this.DQN_PolSAR_Classification_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.Smaple_Batches_toolStripSplitButton = new System.Windows.Forms.ToolStripSplitButton();
            this.Single_Batch_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Accuracy_toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.Open_RawFile_toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.Clear_NLPRawTextView_toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.Annotation_toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.Load_Words_Embedding_ToolStripMenuItem = new System.Windows.Forms.ToolStripButton();
            this.Expertise_Knowledge_toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.Tasks_Monitor_toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.map_statusStrip = new System.Windows.Forms.StatusStrip();
            this.empty_statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.Main_processBar = new System.Windows.Forms.ToolStripProgressBar();
            this.Main_statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer_main = new System.Windows.Forms.SplitContainer();
            this.map_tabControl = new System.Windows.Forms.TabControl();
            this.tab_treeview = new System.Windows.Forms.TabPage();
            this.map_treeView = new System.Windows.Forms.TreeView();
            this.Main_tabControl = new System.Windows.Forms.TabControl();
            this.tab_mapview = new System.Windows.Forms.TabPage();
            this.Map_splitContainer = new System.Windows.Forms.SplitContainer();
            this.map_pictureBox = new System.Windows.Forms.PictureBox();
            this.map_contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.Open_contextMenuStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.MAP_listBox = new System.Windows.Forms.ListBox();
            this.tab_nlpview = new System.Windows.Forms.TabPage();
            this.Nlp_splitContainer = new System.Windows.Forms.SplitContainer();
            this.NLP_splitContainer_top = new System.Windows.Forms.SplitContainer();
            this.NLP_RawText_listBox = new System.Windows.Forms.ListBox();
            this.NLP_Timeline_listBox = new System.Windows.Forms.ListBox();
            this.NLP_pictureBox = new System.Windows.Forms.PictureBox();
            this.tree_contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.bandCombine_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.bandExport_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.main_notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.文件FToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Open_toolstripmenuitem = new System.Windows.Forms.ToolStripMenuItem();
            this.工具TToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RPC_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.map_menuStrip = new System.Windows.Forms.MenuStrip();
            this.mapToolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Tools_Configuration_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.map_toolStrip.SuspendLayout();
            this.map_statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_main)).BeginInit();
            this.splitContainer_main.Panel1.SuspendLayout();
            this.splitContainer_main.Panel2.SuspendLayout();
            this.splitContainer_main.SuspendLayout();
            this.map_tabControl.SuspendLayout();
            this.tab_treeview.SuspendLayout();
            this.Main_tabControl.SuspendLayout();
            this.tab_mapview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Map_splitContainer)).BeginInit();
            this.Map_splitContainer.Panel1.SuspendLayout();
            this.Map_splitContainer.Panel2.SuspendLayout();
            this.Map_splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.map_pictureBox)).BeginInit();
            this.map_contextMenuStrip.SuspendLayout();
            this.tab_nlpview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Nlp_splitContainer)).BeginInit();
            this.Nlp_splitContainer.Panel1.SuspendLayout();
            this.Nlp_splitContainer.Panel2.SuspendLayout();
            this.Nlp_splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NLP_splitContainer_top)).BeginInit();
            this.NLP_splitContainer_top.Panel1.SuspendLayout();
            this.NLP_splitContainer_top.Panel2.SuspendLayout();
            this.NLP_splitContainer_top.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NLP_pictureBox)).BeginInit();
            this.tree_contextMenuStrip.SuspendLayout();
            this.map_menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // map_toolStrip
            // 
            this.map_toolStrip.BackColor = System.Drawing.Color.White;
            this.map_toolStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.map_toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Haplo_toolStripSplitButton,
            this.Hybrid_toolStripSplitButton,
            this.toolStripSeparator5,
            this.CNN_toolStripButton,
            this.DQN_Classification_toolStripSplitButton,
            this.toolStripSeparator3,
            this.Smaple_Batches_toolStripSplitButton,
            this.Accuracy_toolStripButton,
            this.toolStripSeparator1,
            this.Open_RawFile_toolStripButton,
            this.Clear_NLPRawTextView_toolStripButton,
            this.Annotation_toolStripButton,
            this.toolStripSeparator2,
            this.Load_Words_Embedding_ToolStripMenuItem,
            this.Expertise_Knowledge_toolStripButton,
            this.toolStripSeparator10,
            this.Tasks_Monitor_toolStripButton});
            this.map_toolStrip.Location = new System.Drawing.Point(0, 32);
            this.map_toolStrip.Name = "map_toolStrip";
            this.map_toolStrip.Padding = new System.Windows.Forms.Padding(0);
            this.map_toolStrip.Size = new System.Drawing.Size(1460, 65);
            this.map_toolStrip.Stretch = true;
            this.map_toolStrip.TabIndex = 1;
            this.map_toolStrip.Text = "classification tools";
            // 
            // Haplo_toolStripSplitButton
            // 
            this.Haplo_toolStripSplitButton.DropDownButtonWidth = 23;
            this.Haplo_toolStripSplitButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RandomForest_ToolStripMenuItem,
            this.L2SVM_ToolStripMenuItem});
            this.Haplo_toolStripSplitButton.Image = global::Host.UI.Properties.Resources.icons8_mind_map_50;
            this.Haplo_toolStripSplitButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Haplo_toolStripSplitButton.Name = "Haplo_toolStripSplitButton";
            this.Haplo_toolStripSplitButton.Size = new System.Drawing.Size(90, 60);
            this.Haplo_toolStripSplitButton.Text = "Haplo";
            this.Haplo_toolStripSplitButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // RandomForest_ToolStripMenuItem
            // 
            this.RandomForest_ToolStripMenuItem.Image = global::Host.UI.Properties.Resources.icons8_radar_plot_50;
            this.RandomForest_ToolStripMenuItem.Name = "RandomForest_ToolStripMenuItem";
            this.RandomForest_ToolStripMenuItem.Size = new System.Drawing.Size(278, 42);
            this.RandomForest_ToolStripMenuItem.Text = "RandomForest";
            this.RandomForest_ToolStripMenuItem.Click += new System.EventHandler(this.Map_function_Click);
            // 
            // L2SVM_ToolStripMenuItem
            // 
            this.L2SVM_ToolStripMenuItem.Image = global::Host.UI.Properties.Resources.icons8_scatter_plot_50;
            this.L2SVM_ToolStripMenuItem.Name = "L2SVM_ToolStripMenuItem";
            this.L2SVM_ToolStripMenuItem.Size = new System.Drawing.Size(278, 42);
            this.L2SVM_ToolStripMenuItem.Text = "L2-SVM";
            this.L2SVM_ToolStripMenuItem.Click += new System.EventHandler(this.Map_function_Click);
            // 
            // Hybrid_toolStripSplitButton
            // 
            this.Hybrid_toolStripSplitButton.DropDownButtonWidth = 23;
            this.Hybrid_toolStripSplitButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CNN_SVM_ToolStripMenuItem});
            this.Hybrid_toolStripSplitButton.Image = global::Host.UI.Properties.Resources.icons8_table_50;
            this.Hybrid_toolStripSplitButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Hybrid_toolStripSplitButton.Name = "Hybrid_toolStripSplitButton";
            this.Hybrid_toolStripSplitButton.Size = new System.Drawing.Size(98, 60);
            this.Hybrid_toolStripSplitButton.Text = "Hybrid";
            this.Hybrid_toolStripSplitButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // CNN_SVM_ToolStripMenuItem
            // 
            this.CNN_SVM_ToolStripMenuItem.Image = global::Host.UI.Properties.Resources.icons8_flow_chart_50;
            this.CNN_SVM_ToolStripMenuItem.Name = "CNN_SVM_ToolStripMenuItem";
            this.CNN_SVM_ToolStripMenuItem.Size = new System.Drawing.Size(278, 42);
            this.CNN_SVM_ToolStripMenuItem.Text = "CNN-SVM";
            this.CNN_SVM_ToolStripMenuItem.Click += new System.EventHandler(this.Map_function_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 65);
            // 
            // CNN_toolStripButton
            // 
            this.CNN_toolStripButton.Image = global::Host.UI.Properties.Resources.icons8_bar_chart_50;
            this.CNN_toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CNN_toolStripButton.Name = "CNN_toolStripButton";
            this.CNN_toolStripButton.Size = new System.Drawing.Size(66, 60);
            this.CNN_toolStripButton.Text = " CNN ";
            this.CNN_toolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.CNN_toolStripButton.Click += new System.EventHandler(this.Map_function_Click);
            // 
            // DQN_Classification_toolStripSplitButton
            // 
            this.DQN_Classification_toolStripSplitButton.DropDownButtonWidth = 23;
            this.DQN_Classification_toolStripSplitButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DQN_PolSAR_Classification_ToolStripMenuItem});
            this.DQN_Classification_toolStripSplitButton.Image = global::Host.UI.Properties.Resources.icons8_area_chart_50;
            this.DQN_Classification_toolStripSplitButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DQN_Classification_toolStripSplitButton.Name = "DQN_Classification_toolStripSplitButton";
            this.DQN_Classification_toolStripSplitButton.Size = new System.Drawing.Size(82, 60);
            this.DQN_Classification_toolStripSplitButton.Text = "DQN";
            this.DQN_Classification_toolStripSplitButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // DQN_PolSAR_Classification_ToolStripMenuItem
            // 
            this.DQN_PolSAR_Classification_ToolStripMenuItem.Image = global::Host.UI.Properties.Resources.icons8_account_50;
            this.DQN_PolSAR_Classification_ToolStripMenuItem.Name = "DQN_PolSAR_Classification_ToolStripMenuItem";
            this.DQN_PolSAR_Classification_ToolStripMenuItem.Size = new System.Drawing.Size(297, 42);
            this.DQN_PolSAR_Classification_ToolStripMenuItem.Text = "PolSAR Classification";
            this.DQN_PolSAR_Classification_ToolStripMenuItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.DQN_PolSAR_Classification_ToolStripMenuItem.Click += new System.EventHandler(this.Map_function_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 65);
            // 
            // Smaple_Batches_toolStripSplitButton
            // 
            this.Smaple_Batches_toolStripSplitButton.DropDownButtonWidth = 23;
            this.Smaple_Batches_toolStripSplitButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Single_Batch_ToolStripMenuItem});
            this.Smaple_Batches_toolStripSplitButton.Image = global::Host.UI.Properties.Resources.icons8_database_export_50;
            this.Smaple_Batches_toolStripSplitButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Smaple_Batches_toolStripSplitButton.Name = "Smaple_Batches_toolStripSplitButton";
            this.Smaple_Batches_toolStripSplitButton.Size = new System.Drawing.Size(104, 60);
            this.Smaple_Batches_toolStripSplitButton.Text = "Batches";
            this.Smaple_Batches_toolStripSplitButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // Single_Batch_ToolStripMenuItem
            // 
            this.Single_Batch_ToolStripMenuItem.Image = global::Host.UI.Properties.Resources.icons8_database_50;
            this.Single_Batch_ToolStripMenuItem.Name = "Single_Batch_ToolStripMenuItem";
            this.Single_Batch_ToolStripMenuItem.Size = new System.Drawing.Size(278, 42);
            this.Single_Batch_ToolStripMenuItem.Text = "Single Batch";
            this.Single_Batch_ToolStripMenuItem.Click += new System.EventHandler(this.Map_function_Click);
            // 
            // Accuracy_toolStripButton
            // 
            this.Accuracy_toolStripButton.Image = global::Host.UI.Properties.Resources.icons8_line_chart_50;
            this.Accuracy_toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Accuracy_toolStripButton.Name = "Accuracy_toolStripButton";
            this.Accuracy_toolStripButton.Size = new System.Drawing.Size(92, 60);
            this.Accuracy_toolStripButton.Text = "Accuracy";
            this.Accuracy_toolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.Accuracy_toolStripButton.Click += new System.EventHandler(this.Map_function_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 65);
            // 
            // Open_RawFile_toolStripButton
            // 
            this.Open_RawFile_toolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("Open_RawFile_toolStripButton.Image")));
            this.Open_RawFile_toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Open_RawFile_toolStripButton.Name = "Open_RawFile_toolStripButton";
            this.Open_RawFile_toolStripButton.Size = new System.Drawing.Size(87, 60);
            this.Open_RawFile_toolStripButton.Text = "AddText";
            this.Open_RawFile_toolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.Open_RawFile_toolStripButton.Click += new System.EventHandler(this.NLP_funciton_Click);
            // 
            // Clear_NLPRawTextView_toolStripButton
            // 
            this.Clear_NLPRawTextView_toolStripButton.Image = global::Host.UI.Properties.Resources.icons8_cleaning_tool_50;
            this.Clear_NLPRawTextView_toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Clear_NLPRawTextView_toolStripButton.Name = "Clear_NLPRawTextView_toolStripButton";
            this.Clear_NLPRawTextView_toolStripButton.Size = new System.Drawing.Size(99, 60);
            this.Clear_NLPRawTextView_toolStripButton.Text = "ClearView";
            this.Clear_NLPRawTextView_toolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.Clear_NLPRawTextView_toolStripButton.Click += new System.EventHandler(this.NLP_funciton_Click);
            // 
            // Annotation_toolStripButton
            // 
            this.Annotation_toolStripButton.Image = global::Host.UI.Properties.Resources.icons8_table_50;
            this.Annotation_toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Annotation_toolStripButton.Name = "Annotation_toolStripButton";
            this.Annotation_toolStripButton.Size = new System.Drawing.Size(102, 60);
            this.Annotation_toolStripButton.Text = "Annotator";
            this.Annotation_toolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.Annotation_toolStripButton.Click += new System.EventHandler(this.NLP_funciton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 65);
            // 
            // Load_Words_Embedding_ToolStripMenuItem
            // 
            this.Load_Words_Embedding_ToolStripMenuItem.Image = global::Host.UI.Properties.Resources.icons8_upload_link_document_50;
            this.Load_Words_Embedding_ToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Load_Words_Embedding_ToolStripMenuItem.Name = "Load_Words_Embedding_ToolStripMenuItem";
            this.Load_Words_Embedding_ToolStripMenuItem.Size = new System.Drawing.Size(73, 60);
            this.Load_Words_Embedding_ToolStripMenuItem.Text = "Loader";
            this.Load_Words_Embedding_ToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Load_Words_Embedding_ToolStripMenuItem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.Load_Words_Embedding_ToolStripMenuItem.Click += new System.EventHandler(this.NLP_funciton_Click);
            // 
            // Expertise_Knowledge_toolStripButton
            // 
            this.Expertise_Knowledge_toolStripButton.Enabled = false;
            this.Expertise_Knowledge_toolStripButton.Image = global::Host.UI.Properties.Resources.icons8_head_silhouette_with_cogwheels_50;
            this.Expertise_Knowledge_toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Expertise_Knowledge_toolStripButton.Name = "Expertise_Knowledge_toolStripButton";
            this.Expertise_Knowledge_toolStripButton.Size = new System.Drawing.Size(69, 60);
            this.Expertise_Knowledge_toolStripButton.Text = "Expert";
            this.Expertise_Knowledge_toolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.Expertise_Knowledge_toolStripButton.Click += new System.EventHandler(this.NLP_funciton_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(6, 65);
            // 
            // Tasks_Monitor_toolStripButton
            // 
            this.Tasks_Monitor_toolStripButton.Image = global::Host.UI.Properties.Resources.icons8_tasks_50;
            this.Tasks_Monitor_toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Tasks_Monitor_toolStripButton.Name = "Tasks_Monitor_toolStripButton";
            this.Tasks_Monitor_toolStripButton.Size = new System.Drawing.Size(60, 60);
            this.Tasks_Monitor_toolStripButton.Text = "Tasks";
            this.Tasks_Monitor_toolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.Tasks_Monitor_toolStripButton.Click += new System.EventHandler(this.Map_function_Click);
            // 
            // map_statusStrip
            // 
            this.map_statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.map_statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.empty_statusLabel,
            this.Main_processBar,
            this.Main_statusLabel});
            this.map_statusStrip.Location = new System.Drawing.Point(0, 940);
            this.map_statusStrip.Name = "map_statusStrip";
            this.map_statusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 21, 0);
            this.map_statusStrip.ShowItemToolTips = true;
            this.map_statusStrip.Size = new System.Drawing.Size(1460, 31);
            this.map_statusStrip.TabIndex = 2;
            this.map_statusStrip.Text = "statusStrip1";
            // 
            // empty_statusLabel
            // 
            this.empty_statusLabel.Name = "empty_statusLabel";
            this.empty_statusLabel.Size = new System.Drawing.Size(1379, 24);
            this.empty_statusLabel.Spring = true;
            // 
            // Main_processBar
            // 
            this.Main_processBar.Name = "Main_processBar";
            this.Main_processBar.Size = new System.Drawing.Size(150, 23);
            this.Main_processBar.Visible = false;
            // 
            // Main_statusLabel
            // 
            this.Main_statusLabel.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Main_statusLabel.Name = "Main_statusLabel";
            this.Main_statusLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Main_statusLabel.Size = new System.Drawing.Size(59, 24);
            this.Main_statusLabel.Text = "ready";
            this.Main_statusLabel.ToolTipText = "指示当前工具运行状态";
            // 
            // splitContainer_main
            // 
            this.splitContainer_main.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_main.Location = new System.Drawing.Point(0, 97);
            this.splitContainer_main.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.splitContainer_main.Name = "splitContainer_main";
            // 
            // splitContainer_main.Panel1
            // 
            this.splitContainer_main.Panel1.Controls.Add(this.map_tabControl);
            // 
            // splitContainer_main.Panel2
            // 
            this.splitContainer_main.Panel2.Controls.Add(this.Main_tabControl);
            this.splitContainer_main.Size = new System.Drawing.Size(1460, 843);
            this.splitContainer_main.SplitterDistance = 334;
            this.splitContainer_main.SplitterWidth = 6;
            this.splitContainer_main.TabIndex = 3;
            // 
            // map_tabControl
            // 
            this.map_tabControl.Controls.Add(this.tab_treeview);
            this.map_tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.map_tabControl.Location = new System.Drawing.Point(0, 0);
            this.map_tabControl.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.map_tabControl.Name = "map_tabControl";
            this.map_tabControl.SelectedIndex = 0;
            this.map_tabControl.Size = new System.Drawing.Size(330, 839);
            this.map_tabControl.TabIndex = 0;
            // 
            // tab_treeview
            // 
            this.tab_treeview.Controls.Add(this.map_treeView);
            this.tab_treeview.Location = new System.Drawing.Point(4, 28);
            this.tab_treeview.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tab_treeview.Name = "tab_treeview";
            this.tab_treeview.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tab_treeview.Size = new System.Drawing.Size(322, 807);
            this.tab_treeview.TabIndex = 0;
            this.tab_treeview.Text = "TreeView";
            this.tab_treeview.UseVisualStyleBackColor = true;
            // 
            // map_treeView
            // 
            this.map_treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.map_treeView.Location = new System.Drawing.Point(4, 5);
            this.map_treeView.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.map_treeView.Name = "map_treeView";
            this.map_treeView.Size = new System.Drawing.Size(314, 797);
            this.map_treeView.TabIndex = 0;
            this.map_treeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.Map_treeView_NodeMouseClick);
            // 
            // Main_tabControl
            // 
            this.Main_tabControl.Controls.Add(this.tab_mapview);
            this.Main_tabControl.Controls.Add(this.tab_nlpview);
            this.Main_tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Main_tabControl.Location = new System.Drawing.Point(0, 0);
            this.Main_tabControl.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Main_tabControl.Name = "Main_tabControl";
            this.Main_tabControl.SelectedIndex = 0;
            this.Main_tabControl.Size = new System.Drawing.Size(1116, 839);
            this.Main_tabControl.TabIndex = 0;
            // 
            // tab_mapview
            // 
            this.tab_mapview.Controls.Add(this.Map_splitContainer);
            this.tab_mapview.Location = new System.Drawing.Point(4, 28);
            this.tab_mapview.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tab_mapview.Name = "tab_mapview";
            this.tab_mapview.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tab_mapview.Size = new System.Drawing.Size(1108, 807);
            this.tab_mapview.TabIndex = 1;
            this.tab_mapview.Text = "MapView";
            this.tab_mapview.UseVisualStyleBackColor = true;
            // 
            // Map_splitContainer
            // 
            this.Map_splitContainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Map_splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Map_splitContainer.Location = new System.Drawing.Point(4, 5);
            this.Map_splitContainer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Map_splitContainer.Name = "Map_splitContainer";
            this.Map_splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // Map_splitContainer.Panel1
            // 
            this.Map_splitContainer.Panel1.Controls.Add(this.map_pictureBox);
            // 
            // Map_splitContainer.Panel2
            // 
            this.Map_splitContainer.Panel2.Controls.Add(this.MAP_listBox);
            this.Map_splitContainer.Size = new System.Drawing.Size(1100, 797);
            this.Map_splitContainer.SplitterDistance = 550;
            this.Map_splitContainer.SplitterWidth = 5;
            this.Map_splitContainer.TabIndex = 1;
            // 
            // map_pictureBox
            // 
            this.map_pictureBox.ContextMenuStrip = this.map_contextMenuStrip;
            this.map_pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.map_pictureBox.Location = new System.Drawing.Point(0, 0);
            this.map_pictureBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.map_pictureBox.Name = "map_pictureBox";
            this.map_pictureBox.Size = new System.Drawing.Size(1096, 546);
            this.map_pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.map_pictureBox.TabIndex = 0;
            this.map_pictureBox.TabStop = false;
            // 
            // map_contextMenuStrip
            // 
            this.map_contextMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.map_contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Open_contextMenuStrip});
            this.map_contextMenuStrip.Name = "map_contextMenuStrip";
            this.map_contextMenuStrip.ShowImageMargin = false;
            this.map_contextMenuStrip.Size = new System.Drawing.Size(136, 34);
            // 
            // Open_contextMenuStrip
            // 
            this.Open_contextMenuStrip.Name = "Open_contextMenuStrip";
            this.Open_contextMenuStrip.Size = new System.Drawing.Size(135, 30);
            this.Open_contextMenuStrip.Text = "Open (&O)";
            this.Open_contextMenuStrip.Click += new System.EventHandler(this.Map_function_Click);
            // 
            // MAP_listBox
            // 
            this.MAP_listBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MAP_listBox.FormattingEnabled = true;
            this.MAP_listBox.ItemHeight = 18;
            this.MAP_listBox.Location = new System.Drawing.Point(0, 0);
            this.MAP_listBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MAP_listBox.Name = "MAP_listBox";
            this.MAP_listBox.Size = new System.Drawing.Size(1096, 238);
            this.MAP_listBox.TabIndex = 0;
            // 
            // tab_nlpview
            // 
            this.tab_nlpview.Controls.Add(this.Nlp_splitContainer);
            this.tab_nlpview.Location = new System.Drawing.Point(4, 28);
            this.tab_nlpview.Name = "tab_nlpview";
            this.tab_nlpview.Size = new System.Drawing.Size(1108, 807);
            this.tab_nlpview.TabIndex = 2;
            this.tab_nlpview.Text = "NLPView";
            this.tab_nlpview.UseVisualStyleBackColor = true;
            // 
            // Nlp_splitContainer
            // 
            this.Nlp_splitContainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Nlp_splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Nlp_splitContainer.Location = new System.Drawing.Point(0, 0);
            this.Nlp_splitContainer.Name = "Nlp_splitContainer";
            this.Nlp_splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // Nlp_splitContainer.Panel1
            // 
            this.Nlp_splitContainer.Panel1.Controls.Add(this.NLP_splitContainer_top);
            // 
            // Nlp_splitContainer.Panel2
            // 
            this.Nlp_splitContainer.Panel2.Controls.Add(this.NLP_pictureBox);
            this.Nlp_splitContainer.Size = new System.Drawing.Size(1108, 807);
            this.Nlp_splitContainer.SplitterDistance = 309;
            this.Nlp_splitContainer.TabIndex = 0;
            // 
            // NLP_splitContainer_top
            // 
            this.NLP_splitContainer_top.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.NLP_splitContainer_top.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NLP_splitContainer_top.Location = new System.Drawing.Point(0, 0);
            this.NLP_splitContainer_top.Name = "NLP_splitContainer_top";
            // 
            // NLP_splitContainer_top.Panel1
            // 
            this.NLP_splitContainer_top.Panel1.Controls.Add(this.NLP_RawText_listBox);
            // 
            // NLP_splitContainer_top.Panel2
            // 
            this.NLP_splitContainer_top.Panel2.Controls.Add(this.NLP_Timeline_listBox);
            this.NLP_splitContainer_top.Size = new System.Drawing.Size(1108, 309);
            this.NLP_splitContainer_top.SplitterDistance = 548;
            this.NLP_splitContainer_top.TabIndex = 0;
            // 
            // NLP_RawText_listBox
            // 
            this.NLP_RawText_listBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NLP_RawText_listBox.FormattingEnabled = true;
            this.NLP_RawText_listBox.ItemHeight = 18;
            this.NLP_RawText_listBox.Location = new System.Drawing.Point(0, 0);
            this.NLP_RawText_listBox.Name = "NLP_RawText_listBox";
            this.NLP_RawText_listBox.Size = new System.Drawing.Size(544, 305);
            this.NLP_RawText_listBox.TabIndex = 0;
            // 
            // NLP_Timeline_listBox
            // 
            this.NLP_Timeline_listBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NLP_Timeline_listBox.FormattingEnabled = true;
            this.NLP_Timeline_listBox.ItemHeight = 18;
            this.NLP_Timeline_listBox.Location = new System.Drawing.Point(0, 0);
            this.NLP_Timeline_listBox.Name = "NLP_Timeline_listBox";
            this.NLP_Timeline_listBox.Size = new System.Drawing.Size(552, 305);
            this.NLP_Timeline_listBox.TabIndex = 0;
            // 
            // NLP_pictureBox
            // 
            this.NLP_pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NLP_pictureBox.Location = new System.Drawing.Point(0, 0);
            this.NLP_pictureBox.Name = "NLP_pictureBox";
            this.NLP_pictureBox.Size = new System.Drawing.Size(1104, 490);
            this.NLP_pictureBox.TabIndex = 0;
            this.NLP_pictureBox.TabStop = false;
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
            this.tree_contextMenuStrip.Size = new System.Drawing.Size(181, 70);
            // 
            // bandCombine_ToolStripMenuItem
            // 
            this.bandCombine_ToolStripMenuItem.Name = "bandCombine_ToolStripMenuItem";
            this.bandCombine_ToolStripMenuItem.Size = new System.Drawing.Size(180, 30);
            this.bandCombine_ToolStripMenuItem.Text = "Layer Stacking";
            this.bandCombine_ToolStripMenuItem.Click += new System.EventHandler(this.Map_treeView_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(177, 6);
            // 
            // bandExport_ToolStripMenuItem
            // 
            this.bandExport_ToolStripMenuItem.Name = "bandExport_ToolStripMenuItem";
            this.bandExport_ToolStripMenuItem.Size = new System.Drawing.Size(180, 30);
            this.bandExport_ToolStripMenuItem.Text = "Band Export";
            this.bandExport_ToolStripMenuItem.Click += new System.EventHandler(this.Map_treeView_Click);
            // 
            // main_notifyIcon
            // 
            this.main_notifyIcon.BalloonTipText = "toolkit working in the background";
            this.main_notifyIcon.BalloonTipTitle = "AI-Based Toolkit";
            this.main_notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("main_notifyIcon.Icon")));
            this.main_notifyIcon.Text = "please double click to restore the view";
            this.main_notifyIcon.Visible = true;
            this.main_notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Main_notifyIcon_MouseDoubleClick);
            // 
            // 文件FToolStripMenuItem
            // 
            this.文件FToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Open_toolstripmenuitem});
            this.文件FToolStripMenuItem.Name = "文件FToolStripMenuItem";
            this.文件FToolStripMenuItem.Size = new System.Drawing.Size(83, 28);
            this.文件FToolStripMenuItem.Text = "File (&F)";
            // 
            // Open_toolstripmenuitem
            // 
            this.Open_toolstripmenuitem.Image = ((System.Drawing.Image)(resources.GetObject("Open_toolstripmenuitem.Image")));
            this.Open_toolstripmenuitem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Open_toolstripmenuitem.Name = "Open_toolstripmenuitem";
            this.Open_toolstripmenuitem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.Open_toolstripmenuitem.Size = new System.Drawing.Size(259, 34);
            this.Open_toolstripmenuitem.Text = "Open (&O)";
            this.Open_toolstripmenuitem.Click += new System.EventHandler(this.Map_function_Click);
            // 
            // 工具TToolStripMenuItem
            // 
            this.工具TToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RPC_ToolStripMenuItem});
            this.工具TToolStripMenuItem.Name = "工具TToolStripMenuItem";
            this.工具TToolStripMenuItem.Size = new System.Drawing.Size(151, 28);
            this.工具TToolStripMenuItem.Text = "Map Tools (&M)";
            // 
            // RPC_ToolStripMenuItem
            // 
            this.RPC_ToolStripMenuItem.Name = "RPC_ToolStripMenuItem";
            this.RPC_ToolStripMenuItem.Size = new System.Drawing.Size(209, 34);
            this.RPC_ToolStripMenuItem.Text = "RPC Rectify";
            this.RPC_ToolStripMenuItem.Click += new System.EventHandler(this.Map_function_Click);
            // 
            // map_menuStrip
            // 
            this.map_menuStrip.BackColor = System.Drawing.Color.White;
            this.map_menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.map_menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件FToolStripMenuItem,
            this.工具TToolStripMenuItem,
            this.mapToolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.map_menuStrip.Location = new System.Drawing.Point(0, 0);
            this.map_menuStrip.Name = "map_menuStrip";
            this.map_menuStrip.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.map_menuStrip.Size = new System.Drawing.Size(1460, 32);
            this.map_menuStrip.TabIndex = 0;
            this.map_menuStrip.Text = "menuStrip1";
            // 
            // mapToolsToolStripMenuItem
            // 
            this.mapToolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Tools_Configuration_ToolStripMenuItem});
            this.mapToolsToolStripMenuItem.Name = "mapToolsToolStripMenuItem";
            this.mapToolsToolStripMenuItem.Size = new System.Drawing.Size(133, 28);
            this.mapToolsToolStripMenuItem.Text = "NLPTools(&N)";
            // 
            // Tools_Configuration_ToolStripMenuItem
            // 
            this.Tools_Configuration_ToolStripMenuItem.Image = global::Host.UI.Properties.Resources.icons8_configurator_50;
            this.Tools_Configuration_ToolStripMenuItem.Name = "Tools_Configuration_ToolStripMenuItem";
            this.Tools_Configuration_ToolStripMenuItem.Size = new System.Drawing.Size(279, 34);
            this.Tools_Configuration_ToolStripMenuItem.Text = "Tools Configuration";
            this.Tools_Configuration_ToolStripMenuItem.Click += new System.EventHandler(this.NLP_funciton_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(93, 28);
            this.helpToolStripMenuItem.Text = "Help(&H)";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1460, 971);
            this.Controls.Add(this.splitContainer_main);
            this.Controls.Add(this.map_statusStrip);
            this.Controls.Add(this.map_toolStrip);
            this.Controls.Add(this.map_menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.map_menuStrip;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AI-Based Toolkit";
            this.Resize += new System.EventHandler(this.Main_Resize);
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
            this.Main_tabControl.ResumeLayout(false);
            this.tab_mapview.ResumeLayout(false);
            this.Map_splitContainer.Panel1.ResumeLayout(false);
            this.Map_splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Map_splitContainer)).EndInit();
            this.Map_splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.map_pictureBox)).EndInit();
            this.map_contextMenuStrip.ResumeLayout(false);
            this.tab_nlpview.ResumeLayout(false);
            this.Nlp_splitContainer.Panel1.ResumeLayout(false);
            this.Nlp_splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Nlp_splitContainer)).EndInit();
            this.Nlp_splitContainer.ResumeLayout(false);
            this.NLP_splitContainer_top.Panel1.ResumeLayout(false);
            this.NLP_splitContainer_top.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.NLP_splitContainer_top)).EndInit();
            this.NLP_splitContainer_top.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.NLP_pictureBox)).EndInit();
            this.tree_contextMenuStrip.ResumeLayout(false);
            this.map_menuStrip.ResumeLayout(false);
            this.map_menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip map_toolStrip;
        private System.Windows.Forms.StatusStrip map_statusStrip;
        private System.Windows.Forms.SplitContainer splitContainer_main;
        private System.Windows.Forms.TabControl map_tabControl;
        private System.Windows.Forms.TabPage tab_treeview;
        private System.Windows.Forms.TabControl Main_tabControl;
        private System.Windows.Forms.TabPage tab_mapview;
        private System.Windows.Forms.TreeView map_treeView;
        private System.Windows.Forms.ToolStripProgressBar Main_processBar;
        private System.Windows.Forms.ToolStripStatusLabel Main_statusLabel;
        private System.Windows.Forms.ToolStripStatusLabel empty_statusLabel;
        private System.Windows.Forms.ContextMenuStrip map_contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem Open_contextMenuStrip;
        private System.Windows.Forms.ContextMenuStrip tree_contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem bandCombine_ToolStripMenuItem;
        private System.Windows.Forms.SplitContainer Map_splitContainer;
        private System.Windows.Forms.PictureBox map_pictureBox;
        private System.Windows.Forms.ListBox MAP_listBox;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem bandExport_ToolStripMenuItem;
        private System.Windows.Forms.NotifyIcon main_notifyIcon;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem 文件FToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem Open_toolstripmenuitem;
        private System.Windows.Forms.ToolStripMenuItem 工具TToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem RPC_ToolStripMenuItem;
        private System.Windows.Forms.MenuStrip map_menuStrip;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton Tasks_Monitor_toolStripButton;
        private System.Windows.Forms.TabPage tab_nlpview;
        private System.Windows.Forms.SplitContainer Nlp_splitContainer;
        private System.Windows.Forms.SplitContainer NLP_splitContainer_top;
        private System.Windows.Forms.ToolStripButton Open_RawFile_toolStripButton;
        private System.Windows.Forms.ToolStripButton Clear_NLPRawTextView_toolStripButton;
        private System.Windows.Forms.ToolStripButton Load_Words_Embedding_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton Expertise_Knowledge_toolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripMenuItem mapToolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ListBox NLP_Timeline_listBox;
        private System.Windows.Forms.ListBox NLP_RawText_listBox;
        private System.Windows.Forms.ToolStripButton Annotation_toolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSplitButton Haplo_toolStripSplitButton;
        private System.Windows.Forms.ToolStripMenuItem RandomForest_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem L2SVM_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSplitButton Hybrid_toolStripSplitButton;
        private System.Windows.Forms.ToolStripMenuItem CNN_SVM_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton CNN_toolStripButton;
        private System.Windows.Forms.ToolStripButton Accuracy_toolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem Tools_Configuration_ToolStripMenuItem;
        private System.Windows.Forms.PictureBox NLP_pictureBox;
        private System.Windows.Forms.ToolStripSplitButton DQN_Classification_toolStripSplitButton;
        private System.Windows.Forms.ToolStripMenuItem DQN_PolSAR_Classification_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSplitButton Smaple_Batches_toolStripSplitButton;
        private System.Windows.Forms.ToolStripMenuItem Single_Batch_ToolStripMenuItem;
    }
}

