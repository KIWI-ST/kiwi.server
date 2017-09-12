namespace Engine.Core.Form
{
    partial class Self_ManagerControl
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
            this.workstate = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.workstate_listview_state = new System.Windows.Forms.ListView();
            this.workstate_listview_state_check = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.workstate_listview_state_name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.workstate_listview_state_state = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.workstate_listview_state_dllname = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.workstate_listview_state_dllversion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.workstate_listview_state_classversion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.workstate_listview_state_worklog = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.workstate_listview_type = new System.Windows.Forms.ListView();
            this.workstate_listview_type_check = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.workstate_listview_type_name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.workstate_listview_type_source = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.workstate_listview_type_des = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.workstate.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // workstate
            // 
            this.workstate.Controls.Add(this.tabPage1);
            this.workstate.Controls.Add(this.tabPage2);
            this.workstate.Controls.Add(this.tabPage3);
            this.workstate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.workstate.Location = new System.Drawing.Point(0, 0);
            this.workstate.Name = "workstate";
            this.workstate.SelectedIndex = 0;
            this.workstate.Size = new System.Drawing.Size(735, 512);
            this.workstate.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(727, 486);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "DLL运行状态";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.workstate_listview_state);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 137);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(721, 346);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Dll运行状态";
            // 
            // workstate_listview_state
            // 
            this.workstate_listview_state.CheckBoxes = true;
            this.workstate_listview_state.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.workstate_listview_state_check,
            this.workstate_listview_state_name,
            this.workstate_listview_state_state,
            this.workstate_listview_state_dllname,
            this.workstate_listview_state_dllversion,
            this.workstate_listview_state_classversion,
            this.workstate_listview_state_worklog});
            this.workstate_listview_state.Dock = System.Windows.Forms.DockStyle.Fill;
            this.workstate_listview_state.GridLines = true;
            this.workstate_listview_state.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.workstate_listview_state.Location = new System.Drawing.Point(3, 17);
            this.workstate_listview_state.MultiSelect = false;
            this.workstate_listview_state.Name = "workstate_listview_state";
            this.workstate_listview_state.Size = new System.Drawing.Size(715, 326);
            this.workstate_listview_state.TabIndex = 0;
            this.workstate_listview_state.UseCompatibleStateImageBehavior = false;
            this.workstate_listview_state.View = System.Windows.Forms.View.Details;
            this.workstate_listview_state.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.workstate_listview_state_ItemCheck);
            // 
            // workstate_listview_state_check
            // 
            this.workstate_listview_state_check.Text = "选中状态";
            this.workstate_listview_state_check.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.workstate_listview_state_check.Width = 100;
            // 
            // workstate_listview_state_name
            // 
            this.workstate_listview_state_name.Text = "名称";
            this.workstate_listview_state_name.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.workstate_listview_state_name.Width = 100;
            // 
            // workstate_listview_state_state
            // 
            this.workstate_listview_state_state.Text = "运行状态";
            this.workstate_listview_state_state.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.workstate_listview_state_state.Width = 100;
            // 
            // workstate_listview_state_dllname
            // 
            this.workstate_listview_state_dllname.Text = "所在Dll名称";
            this.workstate_listview_state_dllname.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.workstate_listview_state_dllname.Width = 100;
            // 
            // workstate_listview_state_dllversion
            // 
            this.workstate_listview_state_dllversion.Text = "Dll运行版本";
            this.workstate_listview_state_dllversion.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.workstate_listview_state_dllversion.Width = 100;
            // 
            // workstate_listview_state_classversion
            // 
            this.workstate_listview_state_classversion.Text = "GUID";
            this.workstate_listview_state_classversion.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.workstate_listview_state_classversion.Width = 100;
            // 
            // workstate_listview_state_worklog
            // 
            this.workstate_listview_state_worklog.Text = "运行记录";
            this.workstate_listview_state_worklog.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.workstate_listview_state_worklog.Width = 100;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.workstate_listview_type);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(721, 134);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Dll加载类型";
            // 
            // workstate_listview_type
            // 
            this.workstate_listview_type.CheckBoxes = true;
            this.workstate_listview_type.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.workstate_listview_type_check,
            this.workstate_listview_type_name,
            this.workstate_listview_type_source,
            this.workstate_listview_type_des});
            this.workstate_listview_type.Dock = System.Windows.Forms.DockStyle.Fill;
            this.workstate_listview_type.FullRowSelect = true;
            this.workstate_listview_type.GridLines = true;
            this.workstate_listview_type.Location = new System.Drawing.Point(3, 17);
            this.workstate_listview_type.Name = "workstate_listview_type";
            this.workstate_listview_type.Size = new System.Drawing.Size(715, 114);
            this.workstate_listview_type.TabIndex = 0;
            this.workstate_listview_type.UseCompatibleStateImageBehavior = false;
            this.workstate_listview_type.View = System.Windows.Forms.View.Details;
            // 
            // workstate_listview_type_check
            // 
            this.workstate_listview_type_check.Text = " 选中状态";
            this.workstate_listview_type_check.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.workstate_listview_type_check.Width = 100;
            // 
            // workstate_listview_type_name
            // 
            this.workstate_listview_type_name.Text = "名称";
            this.workstate_listview_type_name.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.workstate_listview_type_name.Width = 100;
            // 
            // workstate_listview_type_source
            // 
            this.workstate_listview_type_source.Text = "加载来源";
            this.workstate_listview_type_source.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.workstate_listview_type_source.Width = 100;
            // 
            // workstate_listview_type_des
            // 
            this.workstate_listview_type_des.Text = "描述";
            this.workstate_listview_type_des.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.workstate_listview_type_des.Width = 400;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(727, 486);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "DLL加载管理";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(727, 486);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "DLL更新";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // Self_ManagerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.workstate);
            this.Name = "Self_ManagerControl";
            this.Size = new System.Drawing.Size(735, 512);
            this.workstate.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl workstate;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListView workstate_listview_state;
        private System.Windows.Forms.ColumnHeader workstate_listview_state_name;
        private System.Windows.Forms.ColumnHeader workstate_listview_state_state;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView workstate_listview_type;
        private System.Windows.Forms.ColumnHeader workstate_listview_type_name;
        private System.Windows.Forms.ColumnHeader workstate_listview_type_source;
        private System.Windows.Forms.ColumnHeader workstate_listview_type_des;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ColumnHeader workstate_listview_state_dllname;
        private System.Windows.Forms.ColumnHeader workstate_listview_state_dllversion;
        private System.Windows.Forms.ColumnHeader workstate_listview_state_classversion;
        private System.Windows.Forms.ColumnHeader workstate_listview_state_check;
        private System.Windows.Forms.ColumnHeader workstate_listview_type_check;
        private System.Windows.Forms.ColumnHeader workstate_listview_state_worklog;
    }
}
