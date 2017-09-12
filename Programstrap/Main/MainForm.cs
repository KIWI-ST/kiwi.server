using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using System.Threading;

namespace Programstrap.Main
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            //窗体构建完毕后初始化core
            CoreInitialization();
            //加载增强视图窗体
            ExtViewInitialization();
        }

        /// <summary>
        /// 加强视图加载
        /// </summary>
        private void ExtViewInitialization()
        {
            _imageStream = new Dictionary<string, byte[]>();
            _dataClassContainerList = new List<Engine.Image.Container<Engine.Image.RasterBand>>();
        }

        //PictureBox存放文件流
        Dictionary<string, Byte[]> _imageStream;
        //
        List<Engine.Image.Container<Engine.Image.RasterBand>> _dataClassContainerList;
        //

        #region 图像处理委托
        private delegate void UpdatePictureBoxInvoke(Bitmap2 bitmap);
        private delegate Bitmap2 Bitmap2Hendler(Engine.Image.RasterBand dataClass, TreeNode treeNode);
        private Engine.Image.ImageProcess _gdaltest;
        //多波段加载
        private void AddBand(string filePath, TreeNode tempNode)
        {
            Bitmap2 _testBitmap=new Bitmap2(null,"",null);
            //分波段加载,这个过程可能有点长，需要采用委托防程序假死
            _gdaltest = new Engine.Image.ImageProcess(filePath);
            _dataClassContainerList.Add(_gdaltest.RastBands);
            for (int i = 0; i < _gdaltest.RastBands.Count; i++)
            {
                Bitmap2Hendler handler = new Bitmap2Hendler(GetBitmap2);
                IAsyncResult result = handler.BeginInvoke(_gdaltest.RastBands[i], tempNode, null, null);
                Bitmap2 bitmap2 = handler.EndInvoke(result);
                //
                _testBitmap = bitmap2;
                //
                UpdatePictureBoxInvoke invoker = new UpdatePictureBoxInvoke(UpdatePixctureBox);
                this.BeginInvoke(invoker, bitmap2);
            }
            //_gdaltest.CutMap(_testBitmap.BMP);
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
                 pic_view.Image = (Image)bitmap.BMP;
                //当前选中节点
                 tree_view.SelectedNode = bitmap.SelectCurrent;
                //灰度节点
                 TreeNode tmpNode = new TreeNode(tree_view.SelectedNode.Text + "  " + bitmap.Dec + "  " + DateTime.Now.ToLongTimeString());
                //ListBox操作
                string str = tmpNode.Text + " 完成...";
                //给与外部方法让其显示当前工作
                _imageStream.Add(tmpNode.Text, BmpToStream(bitmap.BMP));
                //添加节点
                tree_view.SelectedNode.Nodes.Add(tmpNode);
                //操作完毕后的当前选中节点
                //当前选中节点
                tree_view.SelectedNode = tmpNode;
                //窗体绘制
                pic_view.Image = StreamToBmp(_imageStream[tree_view.SelectedNode.Text]);
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
                openfiledialog.Filter = "所有文件|*.*|IMG文件|*.img|TIF 文件|*.tif|BMP 文件|*.bmp";
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
                        tree_view.Nodes.Add(tmpNode);
                        //当前选中节点
                        tree_view.SelectedNode = tmpNode;
                        //窗体绘制
                        pic_view.Image = StreamToBmp(_imageStream[tree_view.SelectedNode.Text]);
                        //
                    }
                    else if (Path.GetExtension(openfiledialog.FileName) == ".txt")
                    {
                        using (StreamReader sr = new StreamReader(openfiledialog.FileName))
                        {
                            string textblock = "";
                            string line = sr.ReadLine();
                            int row=0, col=0;
                            while (line != null)
                            {
                                if (col==0)
                                    col = line.Split(' ').Length;
                                row++;
                                textblock = line;
                                sr.ReadLine();
                            }
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
        Bitmap _cutbitmap;
        /// <summary>
        /// 波段合成
        /// </summary>
        private void BandCombine()
        {
            while (tree_view.SelectedNode.Parent != null)
                tree_view.SelectedNode = tree_view.SelectedNode.Parent;
            Programstrap.Attr.BandCommand bandCommand = new Programstrap.Attr.BandCommand(_dataClassContainerList[tree_view.SelectedNode.Index]);
            bandCommand.ShowDialog();
            if (bandCommand.State)
            {
                pic_view.Image = (Image)bandCommand.Bitmap;
                _cutbitmap = bandCommand.Bitmap;
                _imageStream[tree_view.SelectedNode.Text] = BmpToStream(bandCommand.Bitmap);
            }
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
        private Image StreamToBmp(byte[] data)
        {
            MemoryStream ms = new MemoryStream(data);
            Image image = Image.FromStream(ms);
            return image;
        }
        #endregion

        #endregion

        /// <summary>
        /// 初始化Core
        /// </summary>
        private void CoreInitialization()
        {
            //
            Engine.Core.StartFunciton.Run();
            //
            Engine.Core.StartFunciton.AttributeControlUI.SelfControl.Dock = DockStyle.Fill;
            _dllManagerForm = new Attr.DllManager();
            _dllManagerForm.Controls.Add(Engine.Core.StartFunciton.AttributeControlUI.SelfControl);
            //
            Engine.Core.StartFunciton.AttributeControlUI.Attribute.Dock = DockStyle.Fill;
            _settingForm = new Attr.Setting();
            _settingForm.Controls.Add(Engine.Core.StartFunciton.AttributeControlUI.Attribute);
            //打印线程
            _backgroundworkOutPut = new BackgroundWorker();
            _backgroundworkOutPut.DoWork += new DoWorkEventHandler(_backgroundworkOutPut_DoWork);
            _backgroundworkOutPut.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_backgroundworkOutPut_RunWorkerCompleted);
            _backgroundworkOutPut.RunWorkerAsync();
        }

        void _backgroundworkOutPut_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        { 
            _backgroundworkOutPut.RunWorkerAsync();
        }

        void _backgroundworkOutPut_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(1000);
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new UpdateOutPutEventHandler(UpdateOutPut), Engine.Core.StartFunciton.OutPutList);
            }
            else
                UpdateOutPut(Engine.Core.StartFunciton.OutPutList);
        }

        private void UpdateOutPut(List<Engine.Core.IOutPut> outPutList)
        {
            foreach (var outputElements in outPutList)
            {
                foreach (var element in outputElements.OutPutElements)
                {
                    if (element == null) continue;
                    //分类打印日志
                    switch (element.LogType)
                    {
                        case Engine.Core.LogType.Info:
                            listBox_info.Items.Add(element.Content);
                            listBox_info.SetSelected(this.listBox_info.Items.Count - 1, true);
                            break;
                        case Engine.Core.LogType.Error:
                            listBox_error.Items.Add(element.Content);
                            listBox_error.SetSelected(this.listBox_error.Items.Count - 1, true);
                            break;
                        case Engine.Core.LogType.Log:
                            listBox_log.Items.Add(element.Content);
                            listBox_log.SetSelected(this.listBox_log.Items.Count - 1, true);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        delegate void UpdateOutPutEventHandler(List<Engine.Core.IOutPut> outOutList);

        delegate void UpdateSettingFormEventHandler(Attr.Setting settingForm);

        private void UpdateSettingForm(Attr.Setting settingForm)
        {
            settingForm.Controls.Clear();
            settingForm.Controls.Add(Engine.Core.StartFunciton.AttributeControlUI.Attribute);
            Engine.Core.StartFunciton.AttributeControlUI.Attribute.Dock = DockStyle.Fill;
            settingForm.ShowDialog();
        }

        #region 全局窗体容器
        //dll管理器容器
        Attr.DllManager _dllManagerForm = new Attr.DllManager();
        //设置容器
        Attr.Setting _settingForm = new Attr.Setting();
        #endregion

        /// <summary>
        /// 打印output后台执行线程
        /// </summary>
        BackgroundWorker _backgroundworkOutPut;

        /// <summary>
        /// 按键事件集合
        /// </summary>
        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            switch (sender.ToString())
            {
                case "Dll管理器(&O)":
                    _dllManagerForm.ShowDialog();
                    break;
                case "设置":
                    if (this.InvokeRequired)
                        this.Invoke(new UpdateSettingFormEventHandler(UpdateSettingForm), _settingForm);
                    else
                        UpdateSettingForm(_settingForm);
                    break;
                case "启动":
                    Engine.Core.StartFunciton.AssemblyManager.StartAll();
                    break;
                case "停止":
                    Engine.Core.StartFunciton.AssemblyManager.StopAll();
                    break;
                case "选择启动":
                    break;
                case "打开图像":
                    OpenImage();
                    break;
                case "切图":
                    _gdaltest.CutMap(_cutbitmap);
                    break;
                case "波段合成":
                    BandCombine();
                    break;
                case " [System.Windows.Forms.NotifyIcon]":
                    if (this.WindowState == FormWindowState.Minimized)
                    {
                        this.ShowInTaskbar = true;
                        this.WindowState = FormWindowState.Normal;
                        notifyIcon.Visible = false;
                    }
                    break;
                case "Programstrap.Main.MainForm, Text: MainForm":
                    if (this.WindowState == FormWindowState.Minimized)  //判断是否最小化
                    {
                        this.ShowInTaskbar = false;  //不显示在系统任务栏
                        this.notifyIcon.ShowBalloonTip(2000);
                        notifyIcon.Visible = true;  //托盘图标可见
                    }
                    break;
                case "退出":
                    Application.Exit();
                    this.Close();
                    break;
                default:
                    break;
            }
        }
        //数视图节点点击
        private void tree_view_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //左键，选择图像显示
            if (e.Button == MouseButtons.Left)
                if (_imageStream[e.Node.Text] != null)
                   pic_view.Image = StreamToBmp(_imageStream[e.Node.Text]);
                else
                {
                    MessageBox.Show("未选定波段合成图像，请先选定波段...", "警告", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    pic_view.Image = null;
                }
            //右键，弹出菜单
            else
            {
                tree_view.SelectedNode = e.Node;
                context_tree_view.Show(tree_view, new Point(e.X, e.Y));
            }
        }

    }
}
