namespace Engine.NLP.Forms
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
            this.Human_Behavior_tabControl = new System.Windows.Forms.TabControl();
            this.Hazard_tabPage = new System.Windows.Forms.TabPage();
            this.Hazard_listView = new System.Windows.Forms.ListView();
            this.Exposure_tabPage = new System.Windows.Forms.TabPage();
            this.Exposure_listView = new System.Windows.Forms.ListView();
            this.Affect_tabPage = new System.Windows.Forms.TabPage();
            this.Affect_listView = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            this.Word_textBox = new System.Windows.Forms.TextBox();
            this.Add_button = new System.Windows.Forms.Button();
            this.Remove_button = new System.Windows.Forms.Button();
            this.Save_button = new System.Windows.Forms.Button();
            this.Visual_button = new System.Windows.Forms.Button();
            this.Scenario_groupBox.SuspendLayout();
            this.Human_Behavior_tabControl.SuspendLayout();
            this.Hazard_tabPage.SuspendLayout();
            this.Exposure_tabPage.SuspendLayout();
            this.Affect_tabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // Scenario_groupBox
            // 
            this.Scenario_groupBox.Controls.Add(this.Human_Behavior_tabControl);
            this.Scenario_groupBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.Scenario_groupBox.Location = new System.Drawing.Point(0, 0);
            this.Scenario_groupBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Scenario_groupBox.Name = "Scenario_groupBox";
            this.Scenario_groupBox.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Scenario_groupBox.Size = new System.Drawing.Size(856, 374);
            this.Scenario_groupBox.TabIndex = 3;
            this.Scenario_groupBox.TabStop = false;
            this.Scenario_groupBox.Text = "Cascading Effetct Analysis";
            // 
            // Human_Behavior_tabControl
            // 
            this.Human_Behavior_tabControl.Controls.Add(this.Hazard_tabPage);
            this.Human_Behavior_tabControl.Controls.Add(this.Exposure_tabPage);
            this.Human_Behavior_tabControl.Controls.Add(this.Affect_tabPage);
            this.Human_Behavior_tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Human_Behavior_tabControl.Location = new System.Drawing.Point(3, 23);
            this.Human_Behavior_tabControl.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Human_Behavior_tabControl.Name = "Human_Behavior_tabControl";
            this.Human_Behavior_tabControl.SelectedIndex = 0;
            this.Human_Behavior_tabControl.Size = new System.Drawing.Size(850, 349);
            this.Human_Behavior_tabControl.TabIndex = 0;
            this.Human_Behavior_tabControl.SelectedIndexChanged += new System.EventHandler(this.Scenario_tabControl_SelectedIndexChanged);
            // 
            // Hazard_tabPage
            // 
            this.Hazard_tabPage.Controls.Add(this.Hazard_listView);
            this.Hazard_tabPage.Location = new System.Drawing.Point(4, 28);
            this.Hazard_tabPage.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Hazard_tabPage.Name = "Hazard_tabPage";
            this.Hazard_tabPage.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Hazard_tabPage.Size = new System.Drawing.Size(842, 317);
            this.Hazard_tabPage.TabIndex = 1;
            this.Hazard_tabPage.Text = "Hazard (H)";
            this.Hazard_tabPage.UseVisualStyleBackColor = true;
            // 
            // Hazard_listView
            // 
            this.Hazard_listView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Hazard_listView.Location = new System.Drawing.Point(3, 2);
            this.Hazard_listView.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Hazard_listView.Name = "Hazard_listView";
            this.Hazard_listView.Size = new System.Drawing.Size(836, 313);
            this.Hazard_listView.TabIndex = 0;
            this.Hazard_listView.UseCompatibleStateImageBehavior = false;
            this.Hazard_listView.View = System.Windows.Forms.View.List;
            this.Hazard_listView.SelectedIndexChanged += new System.EventHandler(this.ListView_SelectedIndexChanged);
            // 
            // Exposure_tabPage
            // 
            this.Exposure_tabPage.Controls.Add(this.Exposure_listView);
            this.Exposure_tabPage.Location = new System.Drawing.Point(4, 28);
            this.Exposure_tabPage.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Exposure_tabPage.Name = "Exposure_tabPage";
            this.Exposure_tabPage.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Exposure_tabPage.Size = new System.Drawing.Size(842, 317);
            this.Exposure_tabPage.TabIndex = 0;
            this.Exposure_tabPage.Text = "Exposure (E)";
            this.Exposure_tabPage.UseVisualStyleBackColor = true;
            // 
            // Exposure_listView
            // 
            this.Exposure_listView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Exposure_listView.Location = new System.Drawing.Point(3, 2);
            this.Exposure_listView.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Exposure_listView.Name = "Exposure_listView";
            this.Exposure_listView.Size = new System.Drawing.Size(836, 313);
            this.Exposure_listView.TabIndex = 1;
            this.Exposure_listView.UseCompatibleStateImageBehavior = false;
            this.Exposure_listView.View = System.Windows.Forms.View.List;
            this.Exposure_listView.SelectedIndexChanged += new System.EventHandler(this.ListView_SelectedIndexChanged);
            // 
            // Affect_tabPage
            // 
            this.Affect_tabPage.Controls.Add(this.Affect_listView);
            this.Affect_tabPage.Location = new System.Drawing.Point(4, 28);
            this.Affect_tabPage.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Affect_tabPage.Name = "Affect_tabPage";
            this.Affect_tabPage.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Affect_tabPage.Size = new System.Drawing.Size(842, 317);
            this.Affect_tabPage.TabIndex = 3;
            this.Affect_tabPage.Text = "Human Behavior (B)";
            this.Affect_tabPage.UseVisualStyleBackColor = true;
            // 
            // Affect_listView
            // 
            this.Affect_listView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Affect_listView.Location = new System.Drawing.Point(3, 2);
            this.Affect_listView.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Affect_listView.Name = "Affect_listView";
            this.Affect_listView.Size = new System.Drawing.Size(836, 313);
            this.Affect_listView.TabIndex = 0;
            this.Affect_listView.UseCompatibleStateImageBehavior = false;
            this.Affect_listView.View = System.Windows.Forms.View.List;
            this.Affect_listView.SelectedIndexChanged += new System.EventHandler(this.ListView_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(54, 418);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 18);
            this.label1.TabIndex = 4;
            this.label1.Text = "领域词汇：";
            // 
            // Word_textBox
            // 
            this.Word_textBox.Location = new System.Drawing.Point(153, 410);
            this.Word_textBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Word_textBox.Name = "Word_textBox";
            this.Word_textBox.Size = new System.Drawing.Size(157, 28);
            this.Word_textBox.TabIndex = 5;
            // 
            // Add_button
            // 
            this.Add_button.Location = new System.Drawing.Point(327, 410);
            this.Add_button.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Add_button.Name = "Add_button";
            this.Add_button.Size = new System.Drawing.Size(84, 30);
            this.Add_button.TabIndex = 6;
            this.Add_button.Text = "添加";
            this.Add_button.UseVisualStyleBackColor = true;
            this.Add_button.Click += new System.EventHandler(this.Add_button_Click);
            // 
            // Remove_button
            // 
            this.Remove_button.Location = new System.Drawing.Point(435, 410);
            this.Remove_button.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Remove_button.Name = "Remove_button";
            this.Remove_button.Size = new System.Drawing.Size(84, 30);
            this.Remove_button.TabIndex = 7;
            this.Remove_button.Text = "删除";
            this.Remove_button.UseVisualStyleBackColor = true;
            this.Remove_button.Click += new System.EventHandler(this.Remove_button_Click);
            // 
            // Save_button
            // 
            this.Save_button.Location = new System.Drawing.Point(693, 409);
            this.Save_button.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Save_button.Name = "Save_button";
            this.Save_button.Size = new System.Drawing.Size(90, 30);
            this.Save_button.TabIndex = 8;
            this.Save_button.Text = "保存";
            this.Save_button.UseVisualStyleBackColor = true;
            this.Save_button.Click += new System.EventHandler(this.Save_button_Click);
            // 
            // Visual_button
            // 
            this.Visual_button.Location = new System.Drawing.Point(572, 409);
            this.Visual_button.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Visual_button.Name = "Visual_button";
            this.Visual_button.Size = new System.Drawing.Size(90, 30);
            this.Visual_button.TabIndex = 9;
            this.Visual_button.Text = "预览";
            this.Visual_button.UseVisualStyleBackColor = true;
            this.Visual_button.Click += new System.EventHandler(this.Visual_button_Click);
            // 
            // NLPExpertiseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(856, 478);
            this.Controls.Add(this.Visual_button);
            this.Controls.Add(this.Save_button);
            this.Controls.Add(this.Remove_button);
            this.Controls.Add(this.Add_button);
            this.Controls.Add(this.Word_textBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Scenario_groupBox);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "NLPExpertiseForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "NLPExpertiseForm";
            this.Scenario_groupBox.ResumeLayout(false);
            this.Human_Behavior_tabControl.ResumeLayout(false);
            this.Hazard_tabPage.ResumeLayout(false);
            this.Exposure_tabPage.ResumeLayout(false);
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
        private System.Windows.Forms.TabControl Human_Behavior_tabControl;
        private System.Windows.Forms.TabPage Exposure_tabPage;
        private System.Windows.Forms.Button Save_button;
        private System.Windows.Forms.TabPage Hazard_tabPage;
        private System.Windows.Forms.ListView Exposure_listView;
        private System.Windows.Forms.ListView Hazard_listView;
        private System.Windows.Forms.TabPage Affect_tabPage;
        private System.Windows.Forms.ListView Affect_listView;
        private System.Windows.Forms.Button Visual_button;
    }
}