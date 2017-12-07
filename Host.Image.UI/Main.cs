using Engine.GIS.File;
using Engine.Image;
using Engine.Image.Analysis;
using Engine.Image.Eneity.GBand;
using Engine.Image.Eneity.GLayer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Host.Image.UI
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        #region 缓存管理
        /// <summary>
        /// 管理全局的图像与树视图区域的缓存对应
        /// key是图片名称或图+波段名称，值为对应的bitmap
        /// </summary>
        Dictionary<string, Bitmap2> _imageDic = new Dictionary<string, Bitmap2>();
        /// <summary>
        /// 当前选中的Bitmap2信息，包含图层，波段，索引等
        /// </summary>
        Bitmap2 _selectBmp2 = null;

        #endregion

        #region 界面更新

        private void map_treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            _selectBmp2 = _imageDic[e.Node.Text];
            map_pictureBox.Image = _selectBmp2 == null ? null : _selectBmp2.BMP;
        }

        private void UpdateStatusLabel(string msg)
        {
            map_statusLabel.Text = msg;
        }

        private void UpdateTreeNode(TreeNode parentNode, TreeNode childrenNode)
        {
            //1.更新左侧视图
            parentNode.Nodes.Add(childrenNode);
            parentNode.Expand();
            //2.应对picture更新
            map_pictureBox.Image = _imageDic[childrenNode.Text].BMP;
        }

        private delegate void UpdateTreeNodeHandler(TreeNode parentNode, TreeNode childrenNode);

        #endregion

        #region 顶部栏目功能按钮事件处理块

        private void Algorithm_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            switch (item.Name)
            {
                case "SLIO_toolStrip"://超像素
                    SLICO _slico = new SLICO();
                    int[] _klabels;
                    int _numlabels;
                    int K = 100;
                    Bitmap bmp = map_pictureBox.Image as Bitmap;
                    Bitmap imgSuperpixel = _slico.PerformSLICO_ForGivenK(ref bmp, out _klabels, out _numlabels, K, Color.Red, 10);
                    map_pictureBox.Image = imgSuperpixel;
                    break;
                default:
                    break;
            }
        }

        private void Map_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            switch (item.Name)
            {
                case "open_toolstripmenuitem"://添加图像
                    ReadImage();
                    break;
                case "open_contextMenuStrip":
                    ReadImage();
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
            openfiledialog.Filter = "图像文件|*.img;*.tif;*.bmp;*.jpg;*.png|矢量文件|*.shp";
            #endregion
            if (openfiledialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = Path.GetFileNameWithoutExtension(openfiledialog.FileName);
                string extension = Path.GetExtension(openfiledialog.FileName);
                if (extension == ".tif" || extension == ".img" || extension == ".bmp" || extension == ".jpg" || extension == ".png")
                {
                    //1.判断图像是否已加载
                    if (map_treeView.Nodes.OfType<TreeNode>().FirstOrDefault(p => p.Tag.Equals(fileName)) != null)
                    {
                        UpdateStatusLabel("图像不能重复添加");
                        return;
                    }
                    //2.构建TreeNode用于存储数据和结点
                    TreeNode node = new TreeNode(fileName);
                    node.Tag = fileName;
                    map_treeView.Nodes.Add(node);
                    _imageDic.Add(fileName, null);
                    //3.分波段读取图像并加载，开辟新的线程分波段读取数据
                    ThreadStart s = delegate { ReadBand(openfiledialog.FileName, node); };
                    Thread t = new Thread(s);
                    t.IsBackground = true;
                    t.Start();
                }
                else if (extension == "shp")
                {
                    IShpReader pShpReader = new ShpReader(fileName);
                    //后台读取shp并绘制到bmp
                }

            }
        }

        /// <summary>
        /// 异步读取
        /// </summary>
        /// <param name="pReader"></param>
        private void ReadFeautre(IShpReader pReader)
        {

        }


        private void ReadBand(string filePath, TreeNode parentNode)
        {
            string name = parentNode.Text;
            IGdalLayer _layer = new GdalRasterLayer();
            _layer.ReadFromFile(filePath);
            for (int i = 0; i < _layer.BandCollection.Count; i++)
            {
                string key = name + "_波段_" + i;
                IGdalBand band = _layer.BandCollection[i];
                Bitmap2 bmp2 = new Bitmap2(bmp: band.GetBitmap(), name: key, gdalBand: band, gdalLayer: _layer);
                //获取band对应的bitmap格式图像，载入treedNode中
                _imageDic.Add(key, bmp2);
                TreeNode childrenNode = new TreeNode(key);
                Invoke(new UpdateTreeNodeHandler(UpdateTreeNode), parentNode, childrenNode);
            }
        }



        #endregion


    }
}
