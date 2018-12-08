namespace Host.UI.SettingForm
{
    partial class RPCForm
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
            this.rpc_file_button = new System.Windows.Forms.Button();
            this.rpc_file_textBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ok_button = new System.Windows.Forms.Button();
            this.bandRegion = new System.Windows.Forms.Label();
            this.add_raw_button = new System.Windows.Forms.Button();
            this.raw_bin_listView = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // rpc_file_button
            // 
            this.rpc_file_button.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rpc_file_button.Location = new System.Drawing.Point(492, 295);
            this.rpc_file_button.Name = "rpc_file_button";
            this.rpc_file_button.Size = new System.Drawing.Size(86, 32);
            this.rpc_file_button.TabIndex = 11;
            this.rpc_file_button.Text = "选择地址";
            this.rpc_file_button.UseVisualStyleBackColor = true;
            this.rpc_file_button.Click += new System.EventHandler(this.rpc_file_button_Click);
            // 
            // rpc_file_textBox
            // 
            this.rpc_file_textBox.Location = new System.Drawing.Point(140, 297);
            this.rpc_file_textBox.Name = "rpc_file_textBox";
            this.rpc_file_textBox.Size = new System.Drawing.Size(346, 25);
            this.rpc_file_textBox.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(25, 300);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 20);
            this.label1.TabIndex = 9;
            this.label1.Text = "RPC信息文件:";
            // 
            // ok_button
            // 
            this.ok_button.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ok_button.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ok_button.Location = new System.Drawing.Point(492, 349);
            this.ok_button.Name = "ok_button";
            this.ok_button.Size = new System.Drawing.Size(86, 32);
            this.ok_button.TabIndex = 8;
            this.ok_button.Text = "确定";
            this.ok_button.UseVisualStyleBackColor = true;
            this.ok_button.Click += new System.EventHandler(this.ok_button_Click);
            // 
            // bandRegion
            // 
            this.bandRegion.AutoSize = true;
            this.bandRegion.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bandRegion.Location = new System.Drawing.Point(25, 30);
            this.bandRegion.Name = "bandRegion";
            this.bandRegion.Size = new System.Drawing.Size(99, 20);
            this.bandRegion.TabIndex = 6;
            this.bandRegion.Text = "待处理图像：";
            // 
            // add_raw_button
            // 
            this.add_raw_button.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.add_raw_button.Location = new System.Drawing.Point(492, 24);
            this.add_raw_button.Name = "add_raw_button";
            this.add_raw_button.Size = new System.Drawing.Size(86, 32);
            this.add_raw_button.TabIndex = 12;
            this.add_raw_button.Text = "添加";
            this.add_raw_button.UseVisualStyleBackColor = true;
            this.add_raw_button.Click += new System.EventHandler(this.add_raw_button_Click);
            // 
            // raw_bin_listView
            // 
            this.raw_bin_listView.AllowColumnReorder = true;
            this.raw_bin_listView.BackColor = System.Drawing.Color.White;
            this.raw_bin_listView.ForeColor = System.Drawing.Color.Black;
            this.raw_bin_listView.Location = new System.Drawing.Point(140, 22);
            this.raw_bin_listView.Name = "raw_bin_listView";
            this.raw_bin_listView.Size = new System.Drawing.Size(343, 245);
            this.raw_bin_listView.TabIndex = 13;
            this.raw_bin_listView.UseCompatibleStateImageBehavior = false;
            this.raw_bin_listView.View = System.Windows.Forms.View.SmallIcon;
            // 
            // RPCForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(601, 395);
            this.Controls.Add(this.raw_bin_listView);
            this.Controls.Add(this.add_raw_button);
            this.Controls.Add(this.rpc_file_button);
            this.Controls.Add(this.rpc_file_textBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ok_button);
            this.Controls.Add(this.bandRegion);
            this.Name = "RPCForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "RPCForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button rpc_file_button;
        private System.Windows.Forms.TextBox rpc_file_textBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ok_button;
        private System.Windows.Forms.Label bandRegion;
        private System.Windows.Forms.Button add_raw_button;
        private System.Windows.Forms.ListView raw_bin_listView;
    }
}