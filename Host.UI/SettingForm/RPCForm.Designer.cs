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
            this.rpc_layers_comboBox = new System.Windows.Forms.ComboBox();
            this.bandRegion = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // rpc_file_button
            // 
            this.rpc_file_button.Location = new System.Drawing.Point(479, 91);
            this.rpc_file_button.Name = "rpc_file_button";
            this.rpc_file_button.Size = new System.Drawing.Size(86, 25);
            this.rpc_file_button.TabIndex = 11;
            this.rpc_file_button.Text = "选择地址";
            this.rpc_file_button.UseVisualStyleBackColor = true;
            this.rpc_file_button.Click += new System.EventHandler(this.rpc_file_button_Click);
            // 
            // rpc_file_textBox
            // 
            this.rpc_file_textBox.Location = new System.Drawing.Point(127, 91);
            this.rpc_file_textBox.Name = "rpc_file_textBox";
            this.rpc_file_textBox.Size = new System.Drawing.Size(346, 25);
            this.rpc_file_textBox.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 94);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 15);
            this.label1.TabIndex = 9;
            this.label1.Text = "RPC信息文件:";
            // 
            // ok_button
            // 
            this.ok_button.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ok_button.Location = new System.Drawing.Point(479, 147);
            this.ok_button.Name = "ok_button";
            this.ok_button.Size = new System.Drawing.Size(86, 28);
            this.ok_button.TabIndex = 8;
            this.ok_button.Text = "确定";
            this.ok_button.UseVisualStyleBackColor = true;
            this.ok_button.Click += new System.EventHandler(this.ok_button_Click);
            // 
            // rpc_layers_comboBox
            // 
            this.rpc_layers_comboBox.FormattingEnabled = true;
            this.rpc_layers_comboBox.Location = new System.Drawing.Point(127, 42);
            this.rpc_layers_comboBox.Name = "rpc_layers_comboBox";
            this.rpc_layers_comboBox.Size = new System.Drawing.Size(346, 23);
            this.rpc_layers_comboBox.TabIndex = 7;
            this.rpc_layers_comboBox.SelectedIndexChanged += new System.EventHandler(this.rpc_layers_comboBox_SelectedIndexChanged);
            // 
            // bandRegion
            // 
            this.bandRegion.AutoSize = true;
            this.bandRegion.Location = new System.Drawing.Point(24, 45);
            this.bandRegion.Name = "bandRegion";
            this.bandRegion.Size = new System.Drawing.Size(97, 15);
            this.bandRegion.TabIndex = 6;
            this.bandRegion.Text = "待处理图像：";
            // 
            // RPCForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(580, 191);
            this.Controls.Add(this.rpc_file_button);
            this.Controls.Add(this.rpc_file_textBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ok_button);
            this.Controls.Add(this.rpc_layers_comboBox);
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
        private System.Windows.Forms.ComboBox rpc_layers_comboBox;
        private System.Windows.Forms.Label bandRegion;
    }
}