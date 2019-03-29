using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Engine.GIS.Entity;
using Engine.GIS.GLayer.GRasterLayer;
using Engine.GIS.GOperation.Arithmetic;
using Host.UI.Jobs;
using Host.UI.PlotForm;
using Host.UI.SettingForm;
using Host.UI.SettingForm.SLIC;
using OfficeOpenXml;
using OxyPlot;

namespace Host.UI
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

        bool _is_firstBallon = true;

        /// <summary>
        /// Main UI Management
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                main_notifyIcon.Visible = true;
                //only show at the frist time
                if (_is_firstBallon)
                {
                    main_notifyIcon.ShowBalloonTip(1600);
                    _is_firstBallon = false;
                }
                Hide();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void main_notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            main_notifyIcon.Visible = false;
            Show();
            WindowState = FormWindowState.Normal;
            Focus();
        }

        #endregion

        #region 资源释放
        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            //释放process缓存
            _processCache.ForEach(process =>
            {
                process.OutputDataReceived -= UpdateProcessOutput;
                process.ErrorDataReceived -= UpdateProcessOutput;
                //process.OutputDataReceived
                process.Kill();
            });
        }
        #endregion

        #region 缓存管理
        /// <summary>
        /// process cache
        /// </summary>
        List<Process> _processCache = new List<Process>();
        /// <summary>
        /// get time string
        /// </summary>
        string Now => DateTime.Now.ToLongDateString() + DateTime.Now.ToLongTimeString();
        /// <summary>
        /// raster layer
        /// </summary>
        Dictionary<string, GRasterLayer> _rasterDic = new Dictionary<string, GRasterLayer>();
        /// <summary>
        /// 管理全局的图像与树视图区域的缓存对应
        /// key是图片名称或图+波段名称，值为对应的bitmap
        /// </summary>
        Dictionary<string, Bitmap2> _imageDic = new Dictionary<string, Bitmap2>();
        /// <summary>
        /// store jobs
        /// </summary>
        List<IJob> _jobs = new List<IJob>();
        /// <summary>
        /// 当前选中的Bitmap2信息，包含图层，波段，索引等
        /// </summary>
        Bitmap2 _selectBmp2 = null;

        #endregion

        #region 算法执行

        /// <summary>
        /// }{小萌娃记得改哟
        /// </summary>
        /// <param name="fileNameCollection"></param>
        /// <param name="centers"></param>loul
        private void RunCenter(List<string> fileNameCollection, Center[] centers)
        {
            DataTable dt = new DataTable();
            int count = fileNameCollection.Count;
            for (int r = 0; r <= centers.Length; r++)
                dt.Rows.Add(dt.NewRow());
            for (int k = 0; k < count; k++)
            {
                Invoke(new UpdateStatusLabelHandler(UpdateStatusLabel), "应用超像素中心计算开始，进度 " + k + "/" + fileNameCollection.Count, STATUE_ENUM.WARNING);
                string fileName = fileNameCollection[k];
                Bitmap bmp = new Bitmap(fileName);
                //添加1列
                DataColumn dc = dt.Columns.Add(Path.GetFileNameWithoutExtension(fileName));
                dt.Rows[0][dc] = Path.GetFileNameWithoutExtension(fileName);
                //1.提取x,y位置的像素值
                for (int i = 0; i < centers.Length; i++)
                    //}{小萌娃记得改哟  new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1 } 是 3x3 mask 都是1的算子，这里可以修改 自动识别的
                    dt.Rows[i + 1][dc] = GConvolution.Run(bmp, (int)centers[i].X, (int)centers[i].Y, new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1 });
            }
            Invoke(new SaveExcelHandler(SaveExcel), dt);
        }
        /// <summary>
        /// 超像素计算
        /// </summary>
        /// <param name="bmp"></param>
        private void RunSLIC(Bitmap bmp)
        {
            Invoke(new UpdateStatusLabelHandler(UpdateStatusLabel), "超像素中心计算开始...", STATUE_ENUM.WARNING);
            SlicPackage pkg = SuperPixelSegment.Run(bmp, 10000, 3, Color.White);
            Invoke(new SaveJsonHandler(SaveJson), pkg.CENTER);
            Invoke(new SaveJsonHandler(SaveJson), pkg.Label);
            Invoke(new SaveBitmapHandler(SaveBitmap), pkg.Edge);
            Invoke(new SaveBitmapHandler(SaveBitmap), pkg.Average);
        }

        #endregion

        #region 界面更新
        /// <summary>
        /// 保存json结果
        /// </summary>
        /// <param name="jsonText"></param>
        private void SaveJson(string jsonText)
        {
            UpdateStatusLabel("保存Json结果文件");
            SaveFileDialog sfg = new SaveFileDialog
            {
                DefaultExt = ".json"
            };
            if (sfg.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter sw = new StreamWriter(sfg.FileName))
                {
                    sw.Write(jsonText);
                }
            }
        }
        /// <summary>
        /// 保存bmp位图结果
        /// </summary>
        /// <param name="bmp"></param>
        private void SaveBitmap(Bitmap bmp)
        {
            UpdateStatusLabel("保存.jpg结果文件");
            SaveFileDialog sfg = new SaveFileDialog();
            sfg.DefaultExt = ".jpg";
            if (sfg.ShowDialog() == DialogResult.OK)
                bmp.Save(sfg.FileName);
        }
        /// <summary>
        /// 保存Excel结果
        /// </summary>
        /// <param name="dt"></param>
        private void SaveExcel(DataTable dt)
        {
            SaveFileDialog sfg = new SaveFileDialog
            {
                DefaultExt = ".xls"
            };
            if (sfg.ShowDialog() == DialogResult.OK)
            {
                using (var excel = new ExcelPackage(new FileInfo(sfg.FileName)))
                {
                    var ws = excel.Workbook.Worksheets.Add("Sheet1");
                    for (int r = 0; r < dt.Rows.Count; r++)
                    {
                        for (int c = 0; c < dt.Columns.Count; c++)
                        {
                            ws.Cells[r + 1, c + 1].Value = dt.Rows[r][c];
                        }
                    }
                    excel.Save();
                }
            }
        }
        /// <summary>
        /// 更新底部栏提示
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="statue"></param>
        private void UpdateStatusLabel(string msg, STATUE_ENUM statue = STATUE_ENUM.NORMAL)
        {
            if (statue == STATUE_ENUM.ERROR)
            {
                map_statusLabel.ForeColor = Color.Red;
            }
            else if (statue == STATUE_ENUM.WARNING)
            {
                map_statusLabel.ForeColor = Color.Red;
            }
            else
            {
                map_statusLabel.ForeColor = Color.Black;
            }
            //
            map_statusLabel.Text = msg;
        }
        /// <summary>
        /// 更新地图相关树视图节点内容
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="childrenNode"></param>
        private void UpdateTreeNode(TreeNode parentNode, TreeNode childrenNode)
        {
            //1.更新左侧视图
            if (parentNode != null)
            {
                parentNode.Nodes.Add(childrenNode);
                parentNode.Expand();
            }
            else
                map_treeView.Nodes.Add(childrenNode);
            //2.应对picture更新
            if (_imageDic.ContainsKey(childrenNode.Text))
                map_pictureBox.Image = _imageDic[childrenNode.Text]?.BMP;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        private void UpdateMapListBox(string msg)
        {
            if (msg == null) return;
            MAP_listBox.Items.Add(msg);
            MAP_listBox.SelectedIndex = MAP_listBox.Items.Count - 1;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="nodeName"></param>
        private void PaintBitmap(Bitmap bmp, string nodeName)
        {
            _imageDic[nodeName].BMP = bmp;
            map_pictureBox.Image = bmp;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        private void UpdateTextViewListBox(string msg)
        {
            TEXTVIEW_listBox.Items.Add(msg);
        }
        /// <summary>
        /// update process output
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateProcessOutput(object sender, DataReceivedEventArgs e)
        {
            Invoke(new UpdateListBoxHandler(UpdateTextViewListBox), e.Data);
        }
        /// <summary>
        /// 更新listbox区域显示内容
        /// </summary>
        /// <param name="msg"></param>
        private delegate void UpdateListBoxHandler(string msg);
        /// <summary>
        /// 更新树视图委托
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="childrenNode"></param>
        private delegate void UpdateTreeNodeHandler(TreeNode parentNode, TreeNode childrenNode);
        /// <summary>
        ///  保存Json委托
        /// </summary>
        /// <param name="jsonText"></param>
        private delegate void SaveJsonHandler(string jsonText);
        /// <summary>
        ///  保存位图委托
        /// </summary>
        /// <param name="bmp"></param>
        private delegate void SaveBitmapHandler(Bitmap bmp);
        /// <summary>
        ///  保存Excel委托
        /// </summary>
        /// <param name="dt"></param>
        private delegate void SaveExcelHandler(DataTable dt);
        /// <summary>
        /// 更新底部信息栏提示内容委托
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="statue"></param>
        private delegate void UpdateStatusLabelHandler(string msg, STATUE_ENUM statue = STATUE_ENUM.NORMAL);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="value"></param>
        private delegate void PaintPointHandler(Bitmap bmp, int x, int y, byte value);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="nodeName"></param>
        private delegate void PaintBitmapHandler(Bitmap bmp, string nodeName);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="plotModel"></param>
        private delegate void PaintPlotModelHandler(PlotModel plotModel);
        /// <summary>
        /// 
        /// </summary>
        private delegate void RefreshPlotModelHandler();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullFileName"></param>
        private delegate void ReadRasterHandler(string fullFileName);

        #endregion

        #region 主功能

        /// <summary>
        /// 读取图像数据
        /// </summary>
        private void ReadImage()
        {
            #region OpenFileDialog设置
            OpenFileDialog openfiledialog = new OpenFileDialog
            {
                Multiselect = false,
                RestoreDirectory = true,
                Filter = "图像文件|*.img;*.tif;*.tiff;*.bmp;*.jpg;*.png;*.bin|矢量文件|*.shp"
            };
            #endregion
            if (openfiledialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = Path.GetFileNameWithoutExtension(openfiledialog.FileName);
                string extension = Path.GetExtension(openfiledialog.FileName);
                if (extension == ".tif" || extension == ".tiff" || extension == ".img" || extension == ".bmp" || extension == ".jpg" || extension == ".png" || extension == ".bin")
                {
                    ReadRaster(openfiledialog.FileName);
                }
                else if (extension == "shp")
                {
                    //IShpReader pShpReader = new ShpReader(fileName);
                    //后台读取shp并绘制到bmp
                }
            }
        }
        /// <summary>
        /// read raster data
        /// </summary>
        /// <param name="fullFilename"></param>
        private void ReadRaster(string fullFilename)
        {
            string name = Path.GetFileNameWithoutExtension(fullFilename);
            if (map_treeView.Nodes.ContainsKey(name))
                Invoke(new UpdateStatusLabelHandler(UpdateStatusLabel), "请勿重复加载影像", STATUE_ENUM.ERROR);
            else
            {
                TreeNode node = new TreeNode(name) { Name = name };
                Invoke(new UpdateTreeNodeHandler(UpdateTreeNode), null, node);
                IJob readRasterJob = new JobReadRaster(fullFilename);
                RegisterJob(readRasterJob);
                readRasterJob.Start();
            }
        }
        /// <summary>
        /// Update Read Raster UI
        /// </summary>
        private void UpdateReadRasterUI(string nodeName, Dictionary<string, Bitmap2> dict, GRasterLayer rasterLayer)
        {
            TreeNode node = map_treeView.Nodes[nodeName];
            _imageDic[nodeName] = null;
            _rasterDic[nodeName] = rasterLayer;
            foreach (var key in dict.Keys)
            {
                TreeNode childNode = new TreeNode(key);
                _imageDic[key] = dict[key];
                Invoke(new UpdateTreeNodeHandler(UpdateTreeNode), node, childNode);
            }
        }

        #endregion

        #region jobs

        /// <summary>
        /// register job
        /// </summary>
        /// <param name="job"></param>
        private void RegisterJob(IJob job)
        {
            //task complete
            job.OnTaskComplete += Job_OnTaskComplete;
            //state changed
            job.OnStateChanged += Job_OnStateChanged;
            //add to jobs
            _jobs.Add(job);
            //print job register information
            string msg = string.Format("time:{0},task:{1} registered", Now, job.Name);
            Invoke(new UpdateListBoxHandler(UpdateMapListBox), msg);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskName"></param>
        /// <param name="outputs"></param>
        private void Job_OnStateChanged(string taskName, params object[] outputs)
        {
            switch (taskName)
            {
                case "ParsingTextTask":
                    Invoke(new UpdateListBoxHandler(UpdateTextViewListBox), outputs[0] as string);
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// job complete
        /// </summary>
        /// <param name="taskName"></param>
        /// <param name="outputs"></param>
        private void Job_OnTaskComplete(string taskName, params object[] outputs)
        {
            switch (taskName)
            {
                //load image classification result
                case "CNNSVMClassificationTask":
                case "COVRasterTask":
                case "RFClassificationTask":
                case "CnnClassificationTask":
                case "SVMClassificationTask":
                case "DqnClassificationTask":
                    string fullFilename = outputs[0] as string;
                    ReadRaster(fullFilename);
                    break;
                //load image
                case "ReadRasterTask":
                    string nodeName = outputs[0] as string;
                    Dictionary<string, Bitmap2> dict = outputs[1] as Dictionary<string, Bitmap2>;
                    GRasterLayer rasterLayer = outputs[2] as GRasterLayer;
                    UpdateReadRasterUI(nodeName, dict, rasterLayer);
                    break;
                // rpc rester rectify
                case "RPCRasterRectifyTask":
                    break;
                default:
                    break;
            }
            //print completed information
            string msg = string.Format("time:{0},task:{1} completed", Now, taskName);
            Invoke(new UpdateListBoxHandler(UpdateMapListBox), msg);
        }

        #endregion

        #region UI事件相应方法
        /// <summary>
        /// 底图区域功能按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Map_function_Click(object sender, EventArgs e)
        {
            ToolStripItem item = sender as ToolStripItem;
            switch (item.Name)
            {
                //rpc transform
                case "RPC_ToolStripMenuItem":
                    RPCForm rpcForm = new RPCForm();
                    if (rpcForm.ShowDialog() == DialogResult.OK)
                    {
                        IJob rpcRectifyJob = new JobRPCRectify(rpcForm.A, rpcForm.B, rpcForm.C, rpcForm.D, rpcForm.RPCParamaters, rpcForm.RawBinRasterFullFilenames);
                        RegisterJob(rpcRectifyJob);
                        rpcRectifyJob.Start();
                    }
                    break;
                //cov matrix
                case "cov_toolStripButton":
                    COVForm covForm = new COVForm();
                    covForm.RasterDic = _rasterDic;
                    if (covForm.ShowDialog() == DialogResult.OK)
                    {
                        GRasterBand band1 = _rasterDic[covForm.Target1Key].BandCollection[0];
                        GRasterBand band2 = _rasterDic[covForm.Target2Key].BandCollection[0];
                        IJob covRasterJob = new JobCOVRaster(band1, band2);
                        RegisterJob(covRasterJob);
                        covRasterJob.Start();
                    }
                    break;
                //task
                case "task_toolStripButton":
                    TaskMonitor taskForm = new TaskMonitor();
                    taskForm.Jobs = _jobs;
                    taskForm.ShowDialog();
                    break;
                //calucte kappa
                case "kappa_toolStripButton":
                    KappaForm kappaForm = new KappaForm();
                    kappaForm.RasterDic = _rasterDic;
                    kappaForm.ShowDialog();
                    break;
                //添加图像
                case "open_toolstripmenuitem":
                case "open_contextMenuStrip":
                    ReadImage();
                    break;
                //超像素分割
                case "SLIC_toolStripButton":
                case "SLIC_toolStripMenu":
                    Bitmap bmp = map_pictureBox.Image as Bitmap;
                    if (bmp != null)
                    {
                        ThreadStart slic_ts = delegate { RunSLIC(bmp); };
                        Thread slic_t = new Thread(slic_ts);
                        slic_t.IsBackground = true;
                        slic_t.Start();
                    }
                    else
                        UpdateStatusLabel("未选中待计算图像，地图区域无图片", STATUE_ENUM.ERROR);
                    break;
                //super pixel
                case "SLIC_Center_toolStripButton":
                case "SLIC_Center_toolStripMenu":
                    OpenFileDialog opg = new OpenFileDialog
                    {
                        Filter = "JSON文件|*.json"
                    };
                    if (opg.ShowDialog() == DialogResult.OK)
                    {
                        //1.读取center中心
                        using (StreamReader sr = new StreamReader(opg.FileName))
                        {
                            List<byte> colors = new List<byte>();
                            Center[] centers = SuperPixelSegment.ReadCenter(sr.ReadToEnd());
                            //2.设置使用图层
                            SLICForm centerApplyForm = new SLICForm();
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
                //dqn classification 
                case "DQN_ToolStripMenuItem":
                    DQNForm dqnForm = new DQNForm();
                    dqnForm.RasterDic = _rasterDic;
                    if (dqnForm.ShowDialog() == DialogResult.OK)
                    {
                        //"Image Classification",
                        if (dqnForm.TaskName == "Image Classification")
                        {
                            IJob dqnClassifyJob = new JobDQNClassify(_rasterDic[dqnForm.SelectedFeatureRasterLayer], _rasterDic[dqnForm.SelectedLabelRasterLayer], dqnForm.Epochs, dqnForm.SampeSizeLimit, dqnForm.LerpPick);
                            RegisterJob(dqnClassifyJob);
                            dqnClassifyJob.Start();
                        }
                        //"Road Extraction"
                        else if (dqnForm.TaskName == "Road Extraction")
                        {

                        }
                    }
                    break;
                //cnn classification
                case "LeNet_ToolStripMenuItem":
                    LeNetForm cnnForm = new LeNetForm();
                    cnnForm.RasterDic = _rasterDic;
                    if (cnnForm.ShowDialog() == DialogResult.OK)
                    {
                        IJob cnnClassifyJob = new JobCNNClassify(_rasterDic[cnnForm.SelectedFeatureRasterLayer], cnnForm.Epochs, cnnForm.Model, cnnForm.ImageWidth, cnnForm.ImageHeight, cnnForm.ImageDepth, cnnForm.FullFilename);
                        RegisterJob(cnnClassifyJob);
                        cnnClassifyJob.Start();
                    }
                    break;
                //random forest classification
                case "RF_ToolStripMenuItem":
                    RFForm rfForm = new RFForm();
                    rfForm.RasterDic = _rasterDic;
                    if (rfForm.ShowDialog() == DialogResult.OK)
                    {
                        if(rfForm.Model == 1)
                        {
                            IJob rfJob = new JobRFClassify(rfForm.TreeCount, rfForm.SampleFullFilename, _rasterDic[rfForm.FeatureKey]);
                            RegisterJob(rfJob);
                            rfJob.Start();
                        }else if (rfForm.Model == 2)
                        {
                            IJob rfJob2 = new JobRFCSV(rfForm.TreeCount, rfForm.SampleFullFilename, rfForm.WaitFullFilename, rfForm.SaveFullFilename);
                            RegisterJob(rfJob2);
                            rfJob2.Start();
                        }
                    }
                    break;
                //drawing comparsion multi-reslut curve
                case "Compare_Plot_toolStripButton":
                    ComparedPlotForm cp_form = new ComparedPlotForm();
                    cp_form.ShowDialog();
                    break;
                //make samples in minibatch
                case "BATCHS_toolStripButton":
                    BatchExportForm be_form = new BatchExportForm();
                    be_form.RasterDic = _rasterDic;
                    be_form.ShowDialog();
                    break;
                //svm function
                case "SVM_ToolStripMenuItem":
                    SVMFrom svm_Form = new SVMFrom();
                    svm_Form.RasterDic = _rasterDic;
                    if (svm_Form.ShowDialog() == DialogResult.OK)
                    {
                        if(svm_Form.Model == 1)
                        {
                            IJob svmJob = new JobSVMClassify(svm_Form.SampleFullFilename, _rasterDic[svm_Form.FeatureKey]);
                            RegisterJob(svmJob);
                            svmJob.Start();
                        }else if (svm_Form.Model == 2)
                        {
                            IJob svmJob2 = new JobSVMCSV(svm_Form.SampleFullFilename, svm_Form.WaitFullFilename, svm_Form.SaveFullFilename);
                            RegisterJob(svmJob2);
                            svmJob2.Start();
                        }
                    }
                    break;
                //cnn svm classification
                case "CNN_SVM_toolStripButton":
                    CNN_SVMFrom c_s_form = new CNN_SVMFrom();
                    c_s_form.RasterDic = _rasterDic;
                    if (c_s_form.ShowDialog() == DialogResult.OK)
                    {
                        IJob cnnClassifyJob = new JobCNNSVMClassify(_rasterDic[c_s_form.SelectedFeatureRasterLayer], c_s_form.Epochs, c_s_form.Model, c_s_form.ImageWidth, c_s_form.ImageHeight, 1, c_s_form.FullFilename);
                        RegisterJob(cnnClassifyJob);
                        cnnClassifyJob.Start();
                    }
                    break;
                //cnn dqn classification
                case "CNN_DQN_toolStripButton":
                    CNN_DQNForm c_d_form = new CNN_DQNForm();
                    if (c_d_form.ShowDialog() == DialogResult.OK)
                    {
                        
                    }
                    break;
                    //lstm + stanford nlp tool -> analysis scenerio
                case "Parsing_toolStripButton":
                    ParsingForm p_Form = new ParsingForm();
                    if(p_Form.ShowDialog() == DialogResult.OK)
                    {
                        //1. tab view change
                        Main_tabControl.SelectedIndex = 1;
                        //2. create job
                        IJob parsingJob = new JobParsingText(p_Form.TextFullFilename, p_Form.ModelFullFilename, p_Form.LexiconFullFilename);
                        RegisterJob(parsingJob);
                        parsingJob.Start();
                    }
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// NLP功能栏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NLP_funciton_Click(Object sender, EventArgs e)
        {
            ToolStripItem item = sender as ToolStripItem;
            switch (item.Name)
            {
                //start stanford nlp server
                case "STAR_NLPSERVER_toolStripButton":
                    //change selected index
                    Main_tabControl.SelectedIndex = 1;
                    STAR_NLPSERVER_toolStripButton.Enabled = false;
                    string msg = string.Format("time:{0}, {1}", Now, "NLP Server Starting.......");
                    Invoke(new UpdateListBoxHandler(UpdateTextViewListBox), msg);
                    Process process = new Process();
                    process.StartInfo.WorkingDirectory = Directory.GetCurrentDirectory() + @"\stanford-corenlp-full\";
                    //process.StartInfo.FileName = "powershell.exe"; //powershell
                    process.StartInfo.FileName = "java";
                    process.StartInfo.Arguments = "-mx4g -cp * edu.stanford.nlp.pipeline.StanfordCoreNLPServer -port 9000 -timeout 15000";
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardInput = true;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.CreateNoWindow = true;
                    process.Start();
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();
                    //register information
                    process.OutputDataReceived += UpdateProcessOutput;
                    process.ErrorDataReceived += UpdateProcessOutput;
                    //process cache
                    _processCache.Add(process);
                    break;
                //scenario
                case "SCENARIO_BUILD_toolStripButton":
                    break;
                //lstm test 
                case "LSTM_toolStripButton":
                    string rawTextFullFilename = Directory.GetCurrentDirectory() + @"\tmp\RawText.txt";
                    string autosave = Directory.GetCurrentDirectory() + @"\tmp\autolstm.bin";
                    //IJob rnnTrainJob = new JobRNNTrain(rawTextFullFilename, autosave);
                    //RegisterJob(rnnTrainJob);
                    //rnnTrainJob.Start();
                    break;
            }
        }
        /// <summary>
        ///  树视图点击捕捉，用于邮件弹出功能栏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Map_treeView_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            Bitmap2 bmp2 = _imageDic[map_treeView.SelectedNode.Text];
            switch (item.Name)
            {
                case "bandExport_ToolStripMenuItem":
                    if (bmp2.GdalBand == null)
                        return;
                    BandExportForm bandExportModel = new BandExportForm()
                    {
                        RasterLayer = bmp2.GdalLayer,
                        RasterDic = _rasterDic,
                        Index = bmp2.GdalBand.Index - 1
                    };
                    if (bandExportModel.ShowDialog() == DialogResult.OK)
                    {
                        if (!bandExportModel.HasChecked)
                            return;
                        bandExportModel.Save();
                        MessageBox.Show("导出成功", "结果", MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
                    }
                    break;
                case "bandCombine_ToolStripMenuItem":
                    BandForm bandModal = new BandForm
                    {
                        GdalLayer = bmp2.GdalLayer
                    };
                    if (bandModal.ShowDialog() == DialogResult.OK)
                    {
                        List<int> combineIndex = bandModal.BanCombineIndex;
                        Bitmap layerBitmap = GRGBCombine.Run(
                            bandModal.GdalLayer.BandCollection[combineIndex[0]].GetByteBuffer(),
                            bandModal.GdalLayer.BandCollection[combineIndex[1]].GetByteBuffer(),
                            bandModal.GdalLayer.BandCollection[combineIndex[2]].GetByteBuffer());
                        Bitmap2 layerBitmap2 = new Bitmap2(bmp: layerBitmap, name: bandModal.GdalLayer.Name, gdalLayer: bandModal.GdalLayer);
                        //获取band对应的bitmap格式图像，载入treedNode中
                        _imageDic[bandModal.GdalLayer.Name] = layerBitmap2;
                        map_pictureBox.Image = layerBitmap2.BMP;
                    }
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 树视图结点操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Map_treeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //左键，选择图像显示
            if (e.Button == MouseButtons.Left)
            {
                map_treeView.SelectedNode = e.Node;
                _selectBmp2 = _imageDic[e.Node.Text];
                map_pictureBox.Image = _selectBmp2?.BMP;
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
