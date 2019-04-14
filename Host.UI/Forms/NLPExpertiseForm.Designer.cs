namespace Host.UI.Forms
{
    partial class NLPExpertiseForm
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
            this.Scenario_groupBox = new System.Windows.Forms.GroupBox();
            this.Scenario_tabControl = new System.Windows.Forms.TabControl();
            this.Factor_tabPage = new System.Windows.Forms.TabPage();
            this.Factor_listView = new System.Windows.Forms.ListView();
            this.Anti_tabPage = new System.Windows.Forms.TabPage();
            this.Anti_listView = new System.Windows.Forms.ListView();
            this.Affect_tabPage = new System.Windows.Forms.TabPage();
            this.Affect_listView = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            this.Word_textBox = new System.Windows.Forms.TextBox();
            this.Add_button = new System.Windows.Forms.Button();
            this.Remove_button = new System.Windows.Forms.Button();
            this.Save_button = new System.Windows.Forms.Button();
            this.Visual_button = new System.Windows.Forms.Button();
            this.Scenario_groupBox.SuspendLayout();
            this.Scenario_tabControl.SuspendLayout();
            this.Factor_tabPage.SuspendLayout();
            this.Anti_tabPage.SuspendLayout();
            this.Affect_tabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // Scenario_groupBox
            // 
            this.Scenario_groupBox.Controls.Add(this.Scenario_tabControl);
            this.Scenario_groupBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.Scenario_groupBox.Location = new System.Drawing.Point(0, 0);
            this.Scenario_groupBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Scenario_groupBox.Name = "Scenario_groupBox";
            this.Scenario_groupBox.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Scenario_groupBox.Size = new System.Drawing.Size(571, 250);
            this.Scenario_groupBox.TabIndex = 3;
            this.Scenario_groupBox.TabStop = false;
            this.Scenario_groupBox.Text = "情景三要素：";
            // 
            // Scenario_tabControl
            // 
            this.Scenario_tabControl.Controls.Add(this.Factor_tabPage);
            this.Scenario_tabControl.Controls.Add(this.Anti_tabPage);
            this.Scenario_tabControl.Controls.Add(this.Affect_tabPage);
            this.Scenario_tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Scenario_tabControl.Location = new System.Drawing.Point(2, 16);
            this.Scenario_tabControl.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Scenario_tabControl.Name = "Scenario_tabControl";
            this.Scenario_tabControl.SelectedIndex = 0;
            this.Scenario_tabControl.Size = new System.Drawing.Size(567, 232);
            this.Scenario_tabControl.TabIndex = 0;
            this.Scenario_tabControl.SelectedIndexChanged += new System.EventHandler(this.Scenario_tabControl_SelectedIndexChanged);
            // 
            // Factor_tabPage
            // 
            this.Factor_tabPage.Controls.Add(this.Factor_listView);
            this.Factor_tabPage.Location = new System.Drawing.Point(4, 22);
            this.Factor_tabPage.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Factor_tabPage.Name = "Factor_tabPage";
            this.Factor_tabPage.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Factor_tabPage.Size = new System.Drawing.Size(559, 206);
            this.Factor_tabPage.TabIndex = 0;
            this.Factor_tabPage.Text = "致灾因子";
            this.Factor_tabPage.UseVisualStyleBackColor = true;
            // 
            // Factor_listView
            // 
            this.Factor_listView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Factor_listView.Location = new System.Drawing.Point(2, 2);
            this.Factor_listView.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Factor_listView.Name = "Factor_listView";
            this.Factor_listView.Size = new System.Drawing.Size(555, 202);
            this.Factor_listView.TabIndex = 1;
            this.Factor_listView.UseCompatibleStateImageBehavior = false;
            this.Factor_listView.SelectedIndexChanged += new System.EventHandler(this.ListView_SelectedIndexChanged);
            // 
            // Anti_tabPage
            // 
            this.Anti_tabPage.Controls.Add(this.Anti_listView);
            this.Anti_tabPage.Location = new System.Drawing.Point(4, 22);
            this.Anti_tabPage.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Anti_tabPage.Name = "Anti_tabPage";
            this.Anti_tabPage.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Anti_tabPage.Size = new System.Drawing.Size(558, 208);
            this.Anti_tabPage.TabIndex = 1;
            this.Anti_tabPage.Text = "抗灾体";
            this.Anti_tabPage.UseVisualStyleBackColor = true;
            // 
            // Anti_listView
            // 
            this.Anti_listView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Anti_listView.Location = new System.Drawing.Point(2, 2);
            this.Anti_listView.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Anti_listView.Name = "Anti_listView";
            this.Anti_listView.Size = new System.Drawing.Size(554, 204);
            this.Anti_listView.TabIndex = 0;
            this.Anti_listView.UseCompatibleStateImageBehavior = false;
            this.Anti_listView.SelectedIndexChanged += new System.EventHandler(this.ListView_SelectedIndexChanged);
            // 
            // Affect_tabPage
            // 
            this.Affect_tabPage.Controls.Add(this.Affect_listView);
            this.Affect_tabPage.Location = new System.Drawing.Point(4, 22);
            this.Affect_tabPage.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Affect_tabPage.Name = "Affect_tabPage";
            this.Affect_tabPage.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Affect_tabPage.Size = new System.Drawing.Size(558, 208);
            this.Affect_tabPage.TabIndex = 3;
            this.Affect_tabPage.Text = "承灾体";
            this.Affect_tabPage.UseVisualStyleBackColor = true;
            // 
            // Affect_listView
            // 
            this.Affect_listView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Affect_listView.Location = new System.Drawing.Point(2, 2);
            this.Affect_listView.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Affect_listView.Name = "Affect_listView";
            this.Affect_listView.Size = new System.Drawing.Size(554, 204);
            this.Affect_listView.TabIndex = 0;
            this.Affect_listView.UseCompatibleStateImageBehavior = false;
            this.Affect_listView.SelectedIndexChanged += new System.EventHandler(this.ListView_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 278);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "领域词汇：";
            // 
            // Word_textBox
            // 
            this.Word_textBox.Location = new System.Drawing.Point(102, 274);
            this.Word_textBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Word_textBox.Name = "Word_textBox";
            this.Word_textBox.Size = new System.Drawing.Size(106, 21);
            this.Word_textBox.TabIndex = 5;
            // 
            // Add_button
            // 
            this.Add_button.Location = new System.Drawing.Point(218, 274);
            this.Add_button.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Add_button.Name = "Add_button";
            this.Add_button.Size = new System.Drawing.Size(56, 20);
            this.Add_button.TabIndex = 6;
            this.Add_button.Text = "添加";
            this.Add_button.UseVisualStyleBackColor = true;
            this.Add_button.Click += new System.EventHandler(this.Add_button_Click);
            // 
            // Remove_button
            // 
            this.Remove_button.Location = new System.Drawing.Point(290, 274);
            this.Remove_button.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Remove_button.Name = "Remove_button";
            this.Remove_button.Size = new System.Drawing.Size(56, 20);
            this.Remove_button.TabIndex = 7;
            this.Remove_button.Text = "删除";
            this.Remove_button.UseVisualStyleBackColor = true;
            this.Remove_button.Click += new System.EventHandler(this.Remove_button_Click);
            // 
            // Save_button
            // 
            this.Save_button.Location = new System.Drawing.Point(462, 273);
            this.Save_button.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Save_button.Name = "Save_button";
            this.Save_button.Size = new System.Drawing.Size(60, 20);
            this.Save_button.TabIndex = 8;
            this.Save_button.Text = "保存";
            this.Save_button.UseVisualStyleBackColor = true;
            this.Save_button.Click += new System.EventHandler(this.Save_button_Click);
            // 
            // Visual_button
            // 
            this.Visual_button.Location = new System.Drawing.Point(381, 273);
            this.Visual_button.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Visual_button.Name = "Visual_button";
            this.Visual_button.Size = new System.Drawing.Size(60, 20);
            this.Visual_button.TabIndex = 9;
            this.Visual_button.Text = "预览";
            this.Visual_button.UseVisualStyleBackColor = true;
            this.Visual_button.Click += new System.EventHandler(this.Visual_button_Click);
            // 
            // NLPExpertiseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(571, 318);
            this.Controls.Add(this.Visual_button);
            this.Controls.Add(this.Save_button);
            this.Controls.Add(this.Remove_button);
            this.Controls.Add(this.Add_button);
            this.Controls.Add(this.Word_textBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Scenario_groupBox);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "NLPExpertiseForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "NLPExpertiseForm";
            this.Scenario_groupBox.ResumeLayout(false);
            this.Scenario_tabControl.ResumeLayout(false);
            this.Factor_tabPage.ResumeLayout(false);
            this.Anti_tabPage.ResumeLayout(false);
            this.Affect_tabPage.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox Scenario_groupBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Word_textBox;
        private System.Windows.Forms.Button Add_button;
        private System.Windows.Forms.Button Remove_button;
        private System.Windows.Forms.TabControl Scenario_tabControl;
        private System.Windows.Forms.TabPage Factor_tabPage;
        private System.Windows.Forms.Button Save_button;
        private System.Windows.Forms.TabPage Anti_tabPage;
        private System.Windows.Forms.ListView Factor_listView;
        private System.Windows.Forms.ListView Anti_listView;
        private System.Windows.Forms.TabPage Affect_tabPage;
        private System.Windows.Forms.ListView Affect_listView;
        private System.Windows.Forms.Button Visual_button;
    }
}