using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows.Forms;


namespace Engine.Image.Control
{
    /// <summary>
    /// 黄奎 2012-10-25
    /// 自定义控件
    /// 用于支持地图的拖拽、平移、放大缩小
    /// </summary>
    public partial class MapContainer : UserControl
    {
        public MapContainer()
        {
            InitializeComponent();
            //
            CoreInitialization();
            //
            ExtViewInitialization();
        }

        private void CoreInitialization()
        {
            //监听鼠标滚轮是否超过缩放级别
            ContorlMessageFilter filter = new ContorlMessageFilter();
            filter.OnQueryFilterEvent += new QueryFilterEventHandler(filter_OnQueryFilterEvent);
            Application.AddMessageFilter(filter);
        }

        #region 内部属性
        /// <summary>
        /// 平移
        /// </summary>
        bool m_pan = false;
        /// <summary>
        /// 放大缩小
        /// </summary>
        bool m_zoom = false;
        /// <summary>
        /// 绘图实例
        /// </summary>
        Graphics m_graphic;
        /// <summary>
        /// 记录起点
        /// </summary>
        Point m_startpoint;
        /// <summary>
        /// 记录上次结束点
        /// </summary>
        Point m_lastpoint;
        /// <summary>
        /// 绘图Buffer缓冲区
        /// </summary>
        System.Drawing.Image m_buffer;
        /// <summary>
        /// 缩放比例尺
        /// </summary>
        float m_scale = 0.1f;
        #endregion

        #region 属性
        /// <summary>
        /// 是否允许放大缩小
        /// </summary>
        public bool Zoom
        {
            get { return m_zoom; }
            set { m_zoom = value; }
        }
        /// <summary>
        /// 每次滚轮操作缩放比例
        /// </summary>
        public float ShowScale
        {
            get { return m_scale; }
            set { m_scale = value; }
        }
        #endregion

        #region  事件支持 地图缩放
        void filter_OnQueryFilterEvent(int zoomindex, Point screenPoint)
        {
            float scale = 1 + zoomindex * m_scale;
            //
            Point clientPoint = PointToClient(screenPoint);
            
        }
        #endregion

        #region 事件支持 地图平移

        private void map_view_MouseMove(object sender, MouseEventArgs e)
        {
            //平移时的操作
            if (m_pan)
            {
                m_graphic = map_view.CreateGraphics();
                m_graphic.Clear(Color.White);
                m_graphic.DrawImage(m_buffer,new Point(e.X,e.Y));
                m_buffer = (System.Drawing.Image)map_view.Image.Clone();
            }
        }

        private void map_view_MouseDown(object sender, MouseEventArgs e)
        {
            //如果按下的是左键，则开始平移操作(同时需要存在地图)
            if (e.Button == System.Windows.Forms.MouseButtons.Left&map_view.Image!=null)
            {
                m_pan = true;
                this.Cursor = System.Windows.Forms.Cursors.Hand;
                m_buffer = map_view.Image;
            }
        }

        private void map_view_MouseUp(object sender, MouseEventArgs e)
        {
            //左键弹起
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                m_pan = false;
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
        }

        #endregion

        #region 图像加载部分

        //PictureBox存放文件流
        Dictionary<string, Byte[]> _imageStream;
        //
        List<Engine.Image.Container<Engine.Image.RasterBand>> _dataClassContainerList;
        private Settiing m_setForm = new Settiing();
        private void ExtViewInitialization()
        {
            _imageStream = new Dictionary<string, byte[]>();
            _dataClassContainerList = new List<Engine.Image.Container<Engine.Image.RasterBand>>();
        }
        #region 图像处理委托
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
                UpdatePictureBoxInvoke invoker = new UpdatePictureBoxInvoke(UpdatePixctureBox);
                this.BeginInvoke(invoker, bitmap2);
            }
        }
        private void tree_view_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //左键，选择图像显示
            if (e.Button == MouseButtons.Left)
                if (_imageStream[e.Node.Text] != null)
                    map_view.Image = StreamToBmp(_imageStream[e.Node.Text]);
                else
                {
                    MessageBox.Show("未选定波段合成图像，请先选定波段...", "警告", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    map_view.Image = null;
                }
            //右键，弹出菜单
            else
            {
                tree_view.SelectedNode = e.Node;
                context_tree_view.Show(tree_view, new Point(e.X, e.Y));
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
                map_view.Image = (System.Drawing.Image)bitmap.BMP;
                //当前选中节点
                tree_view.SelectedNode = bitmap.SelectCurrent;
                //灰度节点
                TreeNode tmpNode = new TreeNode(tree_view.SelectedNode.Text + "  " + bitmap.Name + "  " + DateTime.Now.ToLongTimeString());
                //ListBox操作
                string str = tmpNode.Text + " 完成...";
                //给与外部方法让其显示当前工作

                //
                _imageStream.Add(tmpNode.Text, BmpToStream(bitmap.BMP));
                //添加节点
                tree_view.SelectedNode.Nodes.Add(tmpNode);
                //操作完毕后的当前选中节点
                //当前选中节点
                tree_view.SelectedNode = tmpNode;
                //窗体绘制
                map_view.Image = StreamToBmp(_imageStream[tree_view.SelectedNode.Text]);
            }
            catch
            {
                MessageBox.Show("已有相同图层或无数据！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }
        //处理函数
        #region 独立方法
        /// <summary>
        /// 加载图像
        /// </summary>
        private void OpenImage()
        {
            try
            {
                #region OpenFileDialog设置
                OpenFileDialog openfiledialog = new OpenFileDialog();
                openfiledialog.Multiselect = false;
                openfiledialog.RestoreDirectory = true;
                //openfiledialog.Filter = "TIF 文件|*.tif|BMP 文件|*.bmp|所有文件|*.*";
                openfiledialog.Filter = "IMG 文件|*.img|TXT 文件|*.txt|所有文件|*.*";
                #endregion
                if (openfiledialog.ShowDialog() == DialogResult.OK)
                {
                    string location = null;

                    TreeNode tmpNode = new TreeNode(Path.GetFileNameWithoutExtension(openfiledialog.FileName));
                    foreach (TreeNode node in tree_view.Nodes)
                    {
                        if (node.Text == tmpNode.Text)
                        {
                            MessageBox.Show("已加入相同影像,本次操作将取消...", "警告", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            return;
                        }
                    }

                    if (Path.GetExtension(openfiledialog.FileName) == ".bmp" || Path.GetExtension(openfiledialog.FileName) == ".jpg")
                    {
                        //
                        _imageStream.Add(Path.GetFileNameWithoutExtension(openfiledialog.FileName), BmpToStream(openfiledialog.FileName));
                        //结点名称
                        //添加节点
                        tree_view.Nodes.Add(tmpNode);
                        //当前选中节点
                        tree_view.SelectedNode = tmpNode;
                        //窗体绘制
                        map_view.Image = StreamToBmp(_imageStream[tree_view.SelectedNode.Text]);
                        //
                    }
                    // 2012.10.11 添加对txt文件的支持
                    else if (Path.GetExtension(openfiledialog.FileName) == ".asc")
                    {
                        using (System.IO.StreamReader sr = new StreamReader(openfiledialog.FileName))
                        {
                            string text = sr.ReadLine();
                            //解析头
                            int width = 0, height = 0;
                            string[] temp;
                            #region 文件解析
                            while (text.Length > 0)
                            {
                                if (text.Contains("ncols"))
                                {
                                    temp = text.Split(' ');
                                    width = Convert.ToInt32(temp[9]);
                                }
                                if (text.Contains("nrows"))
                                {
                                    temp = text.Split(' ');
                                    height = Convert.ToInt32(temp[9]);
                                }
                                else if (text.Contains("NODATA_value"))
                                {
                                    text = sr.ReadToEnd();
                                    break;
                                }
                                text = sr.ReadLine();
                            }
                            if (width == 0 | height == 0)
                                return;
                            double[,] source = new double[width, height];
                            //解析体
                            double max = 0, min = 0;
                            while (text.Length > 0)
                            {
                                text = text.Replace("\n", "");
                                text = text.Replace("\r", "");
                                temp = text.Split(' ');
                                for (int i = 0; i < temp.Length; i++)
                                    if (temp[i] != "-9999")
                                    {
                                        min = max = Convert.ToDouble(temp[i]);
                                        break;
                                    }

                                //开始逐一比较入值
                                for (int count = 0; count < temp.Length - 1; count++)
                                {
                                    double value = Convert.ToDouble(temp[count]);
                                    if (value != -9999)
                                    {
                                        if (min > value)
                                            min = value;
                                        else if (max < value)
                                            max = value;
                                    }
                                    source[count / height, count % height] = Convert.ToDouble(value);
                                    //
                                }
                                break;
                            }
                            #endregion
                            string savefilepath = openfiledialog.FileName.Replace(".txt", ".bmp");
                            location = savefilepath;
                            //
                            Engine.Image.Analysis.BitmapAndByte.ToGrayBitmap(ConvertToByte(source, max, min), width, height).Save(savefilepath);
                            //
                            _imageStream.Add(Path.GetFileNameWithoutExtension(savefilepath), BmpToStream(savefilepath));
                            //结点名称
                            //添加节点
                            tree_view.Nodes.Add(tmpNode);
                            //当前选中节点
                            tree_view.SelectedNode = tmpNode;
                            //窗体绘制
                            map_view.Image = StreamToBmp(_imageStream[tree_view.SelectedNode.Text]);
                            //
                        }
                    }
                    else
                    {

                        //添加节点
                        tree_view.Nodes.Add(tmpNode);
                        _imageStream.Add(Path.GetFileNameWithoutExtension(openfiledialog.FileName), null);
                    }
                    //多波段加载
                    ThreadStart starter;
                    if (location != null)
                        starter = delegate { AddBand(location, tmpNode); };
                    else
                        starter = delegate { AddBand(openfiledialog.FileName, tmpNode); };
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
        /// <summary>
        /// 将二维数组转换成byte数组
        /// </summary>
        private byte[,] ConvertToByte(double[,] source, double max, double min)
        {
            int width = source.GetLength(0);
            int height = source.GetLength(1);
            //
            byte[,] result = new byte[width, height];
            double interval = max - min;
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                {
                    if (source[i, j] != -9999)
                        result[i, j] = Convert.ToByte(255 * (source[i, j] - min) / interval);
                    else
                        result[i, j] = 255;
                }
            //
            return result;
        }
        /// <summary>
        /// 波段合成
        /// </summary>
        private void BandCombine()
        {
            while (tree_view.SelectedNode.Parent != null)
                tree_view.SelectedNode = tree_view.SelectedNode.Parent;
            BandCommand bandCommand = new BandCommand(_dataClassContainerList[tree_view.SelectedNode.Index]);
            ShowSettingDialog(bandCommand);
            if (bandCommand.State)
            {
                map_view.Image = (System.Drawing.Image)bandCommand.Bitmap;
                _imageStream[tree_view.SelectedNode.Text] = BmpToStream(bandCommand.Bitmap);
            }
        }
        private void ShowSettingDialog(System.Windows.Forms.Control control)
        {
            control.Dock = DockStyle.None;
            m_setForm.Controls.Clear();
            m_setForm.Width = control.Width + 5;
            m_setForm.Height = control.Height + 30;
            m_setForm.Controls.Add(control);
            m_setForm.ShowDialog();
        }
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
        private System.Drawing.Image StreamToBmp(byte[] data)
        {
            MemoryStream ms = new MemoryStream(data);
            System.Drawing.Image image = System.Drawing.Image.FromStream(ms);
            return image;
        }
        #endregion

        #endregion
        private void ImageUIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            switch (sender.ToString())
            {
                case "停止":
                    break;
                case "选择启动":
                    break;
                case "打开图像":
                    OpenImage();
                    break;
                case "波段合成":
                    BandCombine();
                    break;
                default:
                    break;
            }
        }


        #endregion
    }
}
