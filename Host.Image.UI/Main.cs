using Engine.Image.Eneity.GLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Host.Image.UI
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        #region 顶部栏目功能按钮事件处理块

        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            switch (item.Name)
            {
                case "open_toolstripmenuitem"://添加图像
                    var sss = "";
                    
                    break;
                default:
                    break;
            }
        }

        private void ReadImage()
        {
            #region OpenFileDialog设置
            OpenFileDialog openfiledialog = new OpenFileDialog();
            openfiledialog.Multiselect = false;
            openfiledialog.RestoreDirectory = true;
            openfiledialog.Filter = "所有文件|*.*|IMG文件|*.img|TIF 文件|*.tif|BMP 文件|*.bmp";
            #endregion
            if (openfiledialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = Path.GetFileNameWithoutExtension(openfiledialog.FileName);
                string extension = Path.GetExtension(openfiledialog.FileName);
                //1.分波段读取图像并加载，开辟新的线程分波段读取数据

            }
        }

        private void ReadBand(string filePath)
        {
            IGdalLayer _layer = new GdalRasterLayer();
            _layer.ReadFromFile(filePath);
            for(int i = 0; i < _layer.BandCollection.Count; i++)
            {
                var band = _layer.BandCollection[i];
                //获取band对应的bitmap格式图像，载入treedNode中
                band.GetBitmap();
            }
           
        }


        #endregion
    }
}
