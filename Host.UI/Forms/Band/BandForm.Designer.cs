namespace Host.UI.SettingForm
{
    partial class BandForm
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
            this.band_listView = new System.Windows.Forms.ListView();
            this.col_check = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_bandname = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.col_bandthumbnail = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // band_listView
            // 
            this.band_listView.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.band_listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.col_check,
            this.col_bandname,
            this.col_bandthumbnail});
            this.band_listView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.band_listView.FullRowSelect = true;
            this.band_listView.GridLines = true;
            this.band_listView.Location = new System.Drawing.Point(0, 0);
            this.band_listView.Name = "band_listView";
            this.band_listView.Size = new System.Drawing.Size(284, 261);
            this.band_listView.TabIndex = 0;
            this.band_listView.TileSize = new System.Drawing.Size(128, 38);
            this.band_listView.UseCompatibleStateImageBehavior = false;
            this.band_listView.View = System.Windows.Forms.View.Details;
            this.band_listView.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ListView_ItemCheck);
            // 
            // col_check
            // 
            this.col_check.Text = "选中";
            this.col_check.Width = 44;
            // 
            // col_bandname
            // 
            this.col_bandname.Text = "波段名称";
            this.col_bandname.Width = 112;
            // 
            // col_bandthumbnail
            // 
            this.col_bandthumbnail.Text = "分辨率";
            this.col_bandthumbnail.Width = 123;
            // 
            // Band
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.band_listView);
            this.Name = "Band";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "波段合成";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView band_listView;
        private System.Windows.Forms.ColumnHeader col_bandname;
        private System.Windows.Forms.ColumnHeader col_bandthumbnail;
        private System.Windows.Forms.ColumnHeader col_check;
    }
}