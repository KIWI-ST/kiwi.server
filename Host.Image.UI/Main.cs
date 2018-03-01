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
using Host.Image.UI.SettingForm;
using Engine.Image.Entity;
using Host.Image.UI.SettingForm.SLIC;
using System.Data;
using OfficeOpenXml;

namespace Host.Image.UI
{
    /// <summary>
    /// 标识颜色
    /// </summary>
    public enum STATUE_ENUM
    {
        ERROR = 0,
        NORMAL = 1,
        WARNING = 2
    }

    public partial class Main : Form
    {

        #region 初始化

        public Main()
        {
            InitializeComponent();
        }

        #endregion

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

        #region 算法执行

        private byte Convolution(Bitmap bmp, int centerX, int centerY, int[] mask)
        {
            int d = (int)Math.Sqrt(mask.Length);
            if (centerX - d < 0 || centerY - d < 0)
                return bmp.GetPixel(centerX, centerY).R;
            else
            {
                int halfd = (int)Math.Floor(d / 2.0);
                Rectangle rect = new Rectangle(centerX - halfd, centerY - halfd, d, d);
                Bitmap rectBmp = bmp.Clone(rect, bmp.PixelFormat);
                Bitmap3 bitmap3 = new Bitmap3(rectBmp);
                double v = 0;
                for (int i = 0; i < mask.Length; i++)
                    v += bitmap3.Bitplane[0].GetPixel(i % d, (int)1 / d);
                return Convert.ToByte(v / mask.Length);
            }
        }

        /// <summary>
        /// }{小萌娃记得改哟
        /// </summary>
        /// <param name="fileNameCollection"></param>
        /// <param name="centers"></param>
        private void RunCenter(List<string> fileNameCollection, Center[] centers)
        {
            DataTable dt = new DataTable();
            int count = fileNameCollection.Count;
            for (int r = 0; r <= centers.Length; r++)
                dt.Rows.Add(dt.NewRow());
            for (int k = 0; k < count; k++)
            {
                string fileName = fileNameCollection[k];
                Bitmap bmp = new Bitmap(fileName);
                //添加1列
                DataColumn dc = dt.Columns.Add(System.IO.Path.GetFileNameWithoutExtension(fileName));
                dt.Rows[0][dc] = System.IO.Path.GetFileNameWithoutExtension(fileName);
                //1.提取x,y位置的像素值
                for (int i = 0; i < centers.Length; i++)
                    //}{小萌娃记得改哟  new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1 } 是 3x3 mask 都是1的算子，这里可以修改 自动识别的
                    dt.Rows[i + 1][dc] = Convolution(bmp, (int)centers[i].X, (int)centers[i].Y, new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1 });
            }
            Invoke(new SaveExcelHandler(SaveExcel), dt);
        }

        private void RunSLIC(Bitmap bmp)
        {
            string resultText = SLIC.Run(bmp, 3000, 3, Color.White);
            Invoke(new SaveJsonHandler(SaveJson), resultText);
        }

        private void SaveJson(string jsonText)
        {
            SaveFileDialog sfg = new SaveFileDialog();
            sfg.DefaultExt = ".json";
            if (sfg.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter sw = new StreamWriter(sfg.FileName))
                {
                    sw.Write(jsonText);
                }
            }
        }

        public delegate void SaveJsonHandler(string jsonText);

        private void SaveBitmap(Bitmap bmp)
        {
            SaveFileDialog sfg = new SaveFileDialog();
            sfg.DefaultExt = ".jpg";
            if (sfg.ShowDialog() == DialogResult.OK)
            {
                bmp.Save(sfg.FileName);
            }
        }

        private delegate void SaveExcelHandler(DataTable dt);

        private void SaveExcel(DataTable dt)
        {
            SaveFileDialog sfg = new SaveFileDialog();
            sfg.DefaultExt = ".xls";
            if (sfg.ShowDialog() == DialogResult.OK)
            {
                using(var excel = new ExcelPackage(new FileInfo(sfg.FileName)))
                {
                    var ws = excel.Workbook.Worksheets.Add("Sheet1");
                    for(int r = 0; r < dt.Rows.Count; r++)
                    {
                        for(int c = 0; c < dt.Columns.Count; c++)
                        {
                            ws.Cells[r+1, c+1].Value = dt.Rows[r][c];
                        }
                    }
                    excel.Save();
                }
            }
        }

        #endregion

        #region 界面更新

        private void UpdateStatusLabel(string msg, STATUE_ENUM statue = STATUE_ENUM.NORMAL)
        {
            if (statue == STATUE_ENUM.ERROR)
            {
                map_statusLabel.Image = Properties.Resources.smile_sad_64;
                map_statusLabel.ForeColor = Color.Red;
            }
            else if (statue == STATUE_ENUM.WARNING)
            {
                map_statusLabel.Image = Properties.Resources.smile_sad_64;
                map_statusLabel.ForeColor = Color.Red;
            }
            else
            {
                map_statusLabel.Image = Properties.Resources.smile_64;
                map_statusLabel.ForeColor = Color.Black;
            }
            //
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
        /// <summary>
        /// 算法功能按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Algorithm_Click(object sender, EventArgs e)
        {
            ToolStripItem item = sender as ToolStripItem;
            switch (item.Name)
            {
                //超像素分割
                case "SLIC_toolStripButton":
                case "SLIC_toolStripMenu":
                    Bitmap bmp = map_pictureBox.Image as Bitmap;
                    if (bmp != null)
                    {
                        ThreadStart s = delegate { RunSLIC(bmp); };
                        Thread t = new Thread(s);
                        t.IsBackground = true;
                        t.Start();
                    }
                    else
                    {
                        UpdateStatusLabel("未选中待计算图像，地图区域无图片", STATUE_ENUM.ERROR);
                    }
                    break;
                //超像素中心应用
                case "SLIC_Center_toolStripButton":
                case "SLIC_Center_toolStripMenu":
                    OpenFileDialog opg = new OpenFileDialog();
                    opg.Filter = "JSON文件|*.json";
                    if (opg.ShowDialog() == DialogResult.OK)
                    {
                        //1.读取center中心
                        using (StreamReader sr = new StreamReader(opg.FileName))
                        {
                            List<byte> colors = new List<byte>();
                            Center[] centers = SLIC.ReadCenter(sr.ReadToEnd());
                            //2.设置使用图层
                            CenterApplyForm centerApplyForm = new CenterApplyForm();
                            if (centerApplyForm.ShowDialog() == DialogResult.OK)
                            {
                                ThreadStart s = delegate { RunCenter(centerApplyForm.FileNameCollection, centers); };
                                Thread t = new Thread(s);
                                t.IsBackground = true;
                                t.Start();
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 底图区域功能按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// 树视图区
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tree_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            Bitmap2 bmp2 = _imageDic[map_treeView.SelectedNode.Text];
            switch (item.Name)
            {
                case "bandCombine_ToolStripMenuItem":
                    BandForm bandModal = new BandForm();
                    bandModal.GdalLayer = bmp2.GdalLayer;
                    if (bandModal.ShowDialog() == DialogResult.OK)
                    {
                        List<int> combineIndex = bandModal.BanCombineIndex;
                        Bitmap layerBitmap = BitmapAndByte.ToRgbBitmap(
                            bandModal.GdalLayer.BandCollection[combineIndex[0]].GetByteData(),
                            bandModal.GdalLayer.BandCollection[combineIndex[1]].GetByteData(),
                            bandModal.GdalLayer.BandCollection[combineIndex[2]].GetByteData());
                        Bitmap2 layerBitmap2 = new Bitmap2(bmp: layerBitmap, name: bandModal.GdalLayer.LayerName, gdalLayer: bandModal.GdalLayer);
                        //获取band对应的bitmap格式图像，载入treedNode中
                        _imageDic[bandModal.GdalLayer.LayerName] = layerBitmap2;
                        map_pictureBox.Image = layerBitmap2.BMP;
                    }
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
                        UpdateStatusLabel("图像不能重复添加", STATUE_ENUM.WARNING);
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

        private void ReadBand(string filePath, TreeNode parentNode)
        {
            string name = parentNode.Text;
            IGdalLayer _layer = new GdalRasterLayer();
            _layer.ReadFromFile(filePath);
            for (int i = 0; i < _layer.BandCollection.Count; i++)
            {
                IGdalBand band = _layer.BandCollection[i];
                band.BandName = name + "_波段_" + i;
                Bitmap2 bmp2 = new Bitmap2(bmp: band.GetBitmap(), name: band.BandName, gdalBand: band, gdalLayer: _layer);
                //获取band对应的bitmap格式图像，载入treedNode中
                _imageDic.Add(band.BandName, bmp2);
                TreeNode childrenNode = new TreeNode(band.BandName);
                Invoke(new UpdateTreeNodeHandler(UpdateTreeNode), parentNode, childrenNode);
            }
        }

        private void map_treeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //左键，选择图像显示
            if (e.Button == MouseButtons.Left)
            {
                map_treeView.SelectedNode = e.Node;
                _selectBmp2 = _imageDic[e.Node.Text];
                map_pictureBox.Image = _selectBmp2 == null ? null : _selectBmp2.BMP;
            }
            else if (e.Button == MouseButtons.Right)
            {
                map_treeView.SelectedNode = e.Node;
                tree_contextMenuStrip.Show(map_treeView, new Point(e.X, e.Y));
            }
        }

        #endregion

    }
}
