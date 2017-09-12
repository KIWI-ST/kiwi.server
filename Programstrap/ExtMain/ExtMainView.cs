using Programstrap.Attr;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Programstrap.ExtMain
{
    /// <summary>
    /// 带名称的BitMap
    /// </summary>
    public class Bitmap2
    {
        private string _dec;
        private Bitmap _bitmap;
        private TreeNode _treeNode;

        public Bitmap2(Bitmap bmp, string Dec, TreeNode treeNode)
        {
            this._bitmap = bmp;
            this._dec = Dec;
            this._treeNode = treeNode;
        }
        public TreeNode SelectCurrent
        {
            get { return _treeNode; }
        }
        public Bitmap BMP
        {
            get { return _bitmap; }
        }
        public String Dec
        {
            get { return _dec; }
        }
    }

    public delegate void Report(string message);
    /// <summary>
    /// 扩展View功能
    /// </summary>
    public class ExtMainView:IDisposable
    {
        public void Dispose()
        {
        }

        public event Report OnReport;
        //
        private ContextMenuStrip _nodeContextMenu;
        //--------
        private ToolStripMenuItem _bandItem;
        private ToolStripMenuItem _removeItem;
        //
        private ContextMenuStrip _picContextMenu;
        //---------- 
        private ToolStripMenuItem _openImageItem;
        #region 属性
        private TreeView _treeView;

        public TreeView TreeView
        {
            get { return _treeView; }
        }
        private PictureBox _pictureBox;

        public PictureBox PictureBox
        {
            get { return _pictureBox; }
        }
        #endregion

        #region 独立方法
        /// <summary>
        /// 摘要：以stream形式读取文件
        /// </summary>
        /// <param name="filePath">BMP图像路径</param>
        /// <returns></returns>
        private byte[] BmpToStream(string filePath)
        {
            byte[] data;
            FileStream fs = File.OpenRead(filePath);
            data = new byte[fs.Length];
            fs.Read(data, 0, data.Length);
            fs.Close();
            return data;
        }
        /// <summary>
        /// 摘要：以stream形式读取文件
        /// </summary>
        /// <param name="bitmap">BitMap类型图像</param>
        /// <returns></returns>
        private byte[] BmpToStream(Bitmap bitmap)
        {
            byte[] data;
            //将bitmap转化为bytes
            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Bmp);
            data = ms.GetBuffer();
            return data;
        }
        /// <summary>
        /// 摘要：将stream转化为Image
        /// </summary>
        /// <param name="data">Byte[] stream</param>
        /// <returns></returns>
        private Image StreamToBmp(byte[] data)
        {
            MemoryStream ms = new MemoryStream(data);
            Image image = Image.FromStream(ms);
            return image;
        }
        #endregion

        #region 委托方法
        private delegate void UpdatePictureBoxInvoke(Bitmap2 bitmap);
        private delegate Bitmap2 Bitmap2Hendler(Engine.Image.RasterBand dataClass, TreeNode treeNode);
        //多波段加载
        private void AddBand(string filePath, TreeNode tempNode)
        {
            //分波段加载,这个过程可能有点长，需要采用委托防程序假死
            Engine.Image.ImageProcess gdaltest = new Engine.Image.ImageProcess(filePath);
            _dataClassContainerList.Add(gdaltest.RastBands);
            for (int i = 0; i < gdaltest.RastBands.Count; i++)
            {
                Bitmap2Hendler handler = new Bitmap2Hendler(GetBitmap2);
                IAsyncResult result = handler.BeginInvoke(gdaltest.RastBands[i], tempNode, null, null);
                Bitmap2 bitmap2 = handler.EndInvoke(result);
                //
                UpdatePixctureBox(bitmap2);
            }
        }
        //获取bitmap2委托
        private Bitmap2 GetBitmap2(Engine.Image.RasterBand dataClass, TreeNode treeNode)
        {
            Bitmap2 bitmap2 = new Bitmap2(Engine.Image.Analysis.BitmapAndByte.ToGrayBitmap(dataClass.Data, dataClass.Data.GetLength(0), dataClass.Data.GetLength(1)), dataClass.LayerIndex, treeNode);
            return bitmap2;
        }
        private void UpdatePixctureBox(Bitmap2 bitmap)
        {
            try
            {
                _pictureBox.Image = (Image)bitmap.BMP;
                //当前选中节点
                _treeView.SelectedNode = bitmap.SelectCurrent;
                //灰度节点
                TreeNode tmpNode = new TreeNode(_treeView.SelectedNode.Text + "  " + bitmap.Dec + "  " + DateTime.Now.ToLongTimeString());
                //ListBox操作
                string str = tmpNode.Text + " 完成...";
                //给与外部方法让其显示当前工作
                OnReport(str);
                //
                _imageStream.Add(tmpNode.Text, BmpToStream(bitmap.BMP));
                //添加节点
                _treeView.SelectedNode.Nodes.Add(tmpNode);
                //操作完毕后的当前选中节点
                //当前选中节点
                _treeView.SelectedNode = tmpNode;
                //窗体绘制
                _pictureBox.Image = StreamToBmp(_imageStream[_treeView.SelectedNode.Text]);
            }
            catch
            {
                MessageBox.Show("已有相同图层或无数据！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        #endregion

        /// <summary>
        /// 扩展View功能
        /// </summary>
        public ExtMainView()
        {
            _picContextMenu = new ContextMenuStrip();
            _openImageItem = new ToolStripMenuItem();
            _openImageItem.Text = "打开图像";
            _picContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { _openImageItem });
            _treeView = new TreeView();
            _treeView.NodeMouseClick += new TreeNodeMouseClickEventHandler(_treeView_NodeMouseClick);
            _pictureBox = new PictureBox();
            this._pictureBox.ContextMenuStrip = _picContextMenu;
            //绘制contextMenu
            _nodeContextMenu = new ContextMenuStrip();
            _bandItem = new ToolStripMenuItem();
            _removeItem = new ToolStripMenuItem();
            //
            _bandItem.Click += new EventHandler(_bandItem_Click);
            _removeItem.Click += new EventHandler(_removeItem_Click);
            _removeItem.Enabled = false;
            //载入contextMenu
            _nodeContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { _bandItem, _removeItem });
            //
            _pictureBox.Dock = DockStyle.Fill;
            _treeView.Dock = DockStyle.Fill;
            //
            _imageStream = new Dictionary<string, byte[]>();
            _dataClassContainerList = new List<Engine.Image.Container<Engine.Image.RasterBand>>();
        }

        /// <summary>
        /// 移除选中节点事件
        /// </summary>
        void _removeItem_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 树型图节点选择事件
        /// </summary>
        void _treeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //左键，选择图像显示
            if (e.Button == MouseButtons.Left)
                if (_imageStream[e.Node.Text] != null)
                    _pictureBox.Image = StreamToBmp(_imageStream[e.Node.Text]);
                else
                {
                    MessageBox.Show("未选定波段合成图像，请先选定波段...", "警告", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    _pictureBox.Image = null;
                }
            //右键，弹出菜单
            else
            {
                _treeView.SelectedNode = e.Node;
                _nodeContextMenu.Show(_treeView, new Point(e.X, e.Y));
            }
        }

        /// <summary>
        /// 波段合成按钮事件
        /// </summary>
        void _bandItem_Click(object sender, EventArgs e)
        {
            while (_treeView.SelectedNode.Parent != null)
                _treeView.SelectedNode = _treeView.SelectedNode.Parent;
            BandCommand bandCommand = new BandCommand(_dataClassContainerList[_treeView.SelectedNode.Index]);
            bandCommand.ShowDialog();
            if (bandCommand.State)
            {
                _pictureBox.Image = (Image)bandCommand.Bitmap;
                _imageStream[_treeView.SelectedNode.Text] = BmpToStream(bandCommand.Bitmap);
            }
        }

        //PictureBox存放文件流
        Dictionary<string, Byte[]> _imageStream;
        //
        List<Engine.Image.Container<Engine.Image.RasterBand>> _dataClassContainerList;

        /// <summary>
        /// 加载图像
        /// </summary>
        public void OpenImage()
        {
            try
            {
                #region OpenFileDialog设置
                OpenFileDialog openfiledialog = new OpenFileDialog();
                openfiledialog.Multiselect = false;
                openfiledialog.RestoreDirectory = true;
                openfiledialog.Filter = "IMG文件|*.img|TIF 文件|*.tif|BMP 文件|*.bmp|所有文件|*.*";
                #endregion
                if (openfiledialog.ShowDialog() == DialogResult.OK)
                {
                    TreeNode tmpNode = new TreeNode(Path.GetFileNameWithoutExtension(openfiledialog.FileName));
                    if (Path.GetExtension(openfiledialog.FileName) == ".bmp" || Path.GetExtension(openfiledialog.FileName) == ".jpg")
                    {
                        //
                        _imageStream.Add(Path.GetFileNameWithoutExtension(openfiledialog.FileName), BmpToStream(openfiledialog.FileName));
                        //结点名称
                        //添加节点
                        _treeView.Nodes.Add(tmpNode);
                        //当前选中节点
                        _treeView.SelectedNode = tmpNode;
                        //窗体绘制
                        _pictureBox.Image = StreamToBmp(_imageStream[_treeView.SelectedNode.Text]);
                        //
                        //
                    }
                    else
                    {
                        //添加节点
                        _treeView.Nodes.Add(tmpNode);
                        _imageStream.Add(Path.GetFileNameWithoutExtension(openfiledialog.FileName), null);
                    }
                    //多波段加载
                    ThreadStart starter = delegate { AddBand(openfiledialog.FileName, tmpNode); };
                    Thread t = new Thread(starter);
                    t.IsBackground = true;
                    t.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "警告", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

    }
}
