namespace Engine.Image.Control
{
    partial class MapContainer
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.tree_view = new System.Windows.Forms.TreeView();
            this.context_tree_view = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.波段合成ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.移除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.map_view = new System.Windows.Forms.PictureBox();
            this.contextMenu_pic_View = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.打开图像ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.保存图像ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.context_tree_view.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.map_view)).BeginInit();
            this.contextMenu_pic_View.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.tree_view);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.map_view);
            this.splitContainer.Size = new System.Drawing.Size(551, 356);
            this.splitContainer.SplitterDistance = 165;
            this.splitContainer.TabIndex = 1;
            // 
            // tree_view
            // 
            this.tree_view.ContextMenuStrip = this.context_tree_view;
            this.tree_view.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tree_view.Location = new System.Drawing.Point(0, 0);
            this.tree_view.Name = "tree_view";
            this.tree_view.Size = new System.Drawing.Size(161, 352);
            this.tree_view.TabIndex = 0;
            this.tree_view.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tree_view_NodeMouseClick);
            // 
            // context_tree_view
            // 
            this.context_tree_view.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.波段合成ToolStripMenuItem,
            this.toolStripSeparator1,
            this.移除ToolStripMenuItem});
            this.context_tree_view.Name = "context_tree_view";
            this.context_tree_view.ShowImageMargin = false;
            this.context_tree_view.Size = new System.Drawing.Size(100, 54);
            // 
            // 波段合成ToolStripMenuItem
            // 
            this.波段合成ToolStripMenuItem.Name = "波段合成ToolStripMenuItem";
            this.波段合成ToolStripMenuItem.Size = new System.Drawing.Size(99, 22);
            this.波段合成ToolStripMenuItem.Text = "波段合成";
            this.波段合成ToolStripMenuItem.Click += new System.EventHandler(this.ImageUIToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(96, 6);
            // 
            // 移除ToolStripMenuItem
            // 
            this.移除ToolStripMenuItem.Name = "移除ToolStripMenuItem";
            this.移除ToolStripMenuItem.Size = new System.Drawing.Size(99, 22);
            this.移除ToolStripMenuItem.Text = "移除";
            // 
            // map_view
            // 
            this.map_view.BackColor = System.Drawing.Color.White;
            this.map_view.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.map_view.ContextMenuStrip = this.contextMenu_pic_View;
            this.map_view.Dock = System.Windows.Forms.DockStyle.Fill;
            this.map_view.Location = new System.Drawing.Point(0, 0);
            this.map_view.Name = "map_view";
            this.map_view.Size = new System.Drawing.Size(378, 352);
            this.map_view.TabIndex = 1;
            this.map_view.TabStop = false;
            this.map_view.MouseDown += new System.Windows.Forms.MouseEventHandler(this.map_view_MouseDown);
            this.map_view.MouseMove += new System.Windows.Forms.MouseEventHandler(this.map_view_MouseMove);
            this.map_view.MouseUp += new System.Windows.Forms.MouseEventHandler(this.map_view_MouseUp);
            // 
            // contextMenu_pic_View
            // 
            this.contextMenu_pic_View.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.打开图像ToolStripMenuItem,
            this.toolStripMenuItem1,
            this.保存图像ToolStripMenuItem});
            this.contextMenu_pic_View.Name = "contextMenu_pic_View";
            this.contextMenu_pic_View.ShowImageMargin = false;
            this.contextMenu_pic_View.Size = new System.Drawing.Size(100, 54);
            // 
            // 打开图像ToolStripMenuItem
            // 
            this.打开图像ToolStripMenuItem.Name = "打开图像ToolStripMenuItem";
            this.打开图像ToolStripMenuItem.Size = new System.Drawing.Size(99, 22);
            this.打开图像ToolStripMenuItem.Text = "打开图像";
            this.打开图像ToolStripMenuItem.Click += new System.EventHandler(this.ImageUIToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(96, 6);
            // 
            // 保存图像ToolStripMenuItem
            // 
            this.保存图像ToolStripMenuItem.Name = "保存图像ToolStripMenuItem";
            this.保存图像ToolStripMenuItem.Size = new System.Drawing.Size(99, 22);
            this.保存图像ToolStripMenuItem.Text = "保存图像";
            // 
            // MapContainer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer);
            this.Name = "MapContainer";
            this.Size = new System.Drawing.Size(551, 356);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.context_tree_view.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.map_view)).EndInit();
            this.contextMenu_pic_View.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.PictureBox map_view;
        private System.Windows.Forms.TreeView tree_view;
        private System.Windows.Forms.ContextMenuStrip contextMenu_pic_View;
        private System.Windows.Forms.ToolStripMenuItem 打开图像ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 保存图像ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip context_tree_view;
        private System.Windows.Forms.ToolStripMenuItem 波段合成ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem 移除ToolStripMenuItem;
    }
}
