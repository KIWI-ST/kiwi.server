using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Engine.Brain.Method;
using Engine.GIS.Entity;
using Engine.GIS.GLayer.GRasterLayer;
using Engine.GIS.GOperation.Arithmetic;
using Engine.NLP.Entity;
using Engine.NLP.Forms;
using Engine.NLP.Process;
using Engine.NLP.Process.Tools;
using Engine.NLP.Utils;
using Host.UI.Forms;
using Host.UI.Jobs;
using Host.UI.SettingForm;
using Host.UI.Util;

namespace Host.UI
{
    public partial class Main : Form
    {

        #region 初始化

        public Main()
        {
            InitializeComponent();
            SettingInitialization();
            //}{debug
            DebugCode();
        }

        private void DebugCode()
        {
            string text = "2010年7月15日Tanker Pacific Management (Singapore) Pte Ltd（新加坡太平洋油轮公司）所属利比里亚籍（最初误报道为巴拿马籍）“COSMIC JEWEL”(“宇宙宝石”)号30万吨级VLCC油轮在大连新港向国际储运公司原油灌区卸送中石油控股的中油燃料油股份有限公司委托中国联合石油有限责任公司进口的委内瑞拉祖阿塔原油15.3万吨，卸载入中国联合石油有限责任公司租赁的国际储运公司原油灌区304、401、403号罐。由于该原油H2S含量较高，中油燃料油股份有限公司委托天津辉盛达石化技术有限公司负责加入原油脱硫剂作业。辉盛达公司委托上海祥诚商品检验技术服务有限公司大连分公司在国际储运公司原油罐区输油管道上进行现场作业。所添加的“HD-硫化氢脱除剂”原油脱硫剂由辉盛达公司生产。卸油作业于7月15日15时30分开始，在两条输油管道同时进行。7月15日20时，油轮开始用2号输油管线向国际储运公司的原油罐区卸送，祥诚公司作业人员开始通过原油罐区内一套内径90厘米输油管道上的排空阀向输油管道内注入脱硫剂。加剂过程中由于由于输油管内压力高，加注软管多处出现超压鼓泡，连接处脱落造成脱硫化剂泄漏等情况，致使加注作业多次中断共计约4个小时，以致未能按计划在17小时卸油作业中加入全部的脱硫剂。7月16日13时，油轮进行原油洗舱集油作业，停止向岸上卸油并关闭船岸间控制阀。此时，中石油大连石化公司石油储运公司生产调度通知上海祥诚大连分公司经理“船上停止卸油了”，但注入脱硫剂的作业没有停止，又继续加入了22.6t脱硫剂。18时，在注入了全部的88立方米脱硫剂后，现场作业人员用消防泵房（位于103号油罐东侧）内的消防水对脱硫剂管路和泵进行冲洗，冲洗液0.1t直接经加剂口入该输油管线。18时02分，靠近脱硫剂注入部位的输油管道突然发生爆炸，引发火灾，造成部分输油管道、附近储罐阀门、输油泵房和电力系统损坏和大量原油泄漏。事故导致储罐阀门无法及时关闭，火灾不断扩大。原油顺地下管沟流淌，形成地面流淌火，火势蔓延。";
            IRegimentTool tool = new RegimentTool();
            tool.RegimentTextByTimeline(text);
        }

        /// <summary>
        /// 提升用户体验，缓存习惯设置
        /// </summary>
        private void SettingInitialization()
        {
            string mainTabIndex = HostConfiguration.MainTabViewIndex;
            if (mainTabIndex != null)
                Main_tabControl.SelectedIndex = Convert.ToInt32(mainTabIndex);
        }

        /// <summary>
        /// 关闭操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            HostConfiguration.MainTabViewIndex = Main_tabControl.SelectedIndex.ToString();
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
        private void Main_notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            main_notifyIcon.Visible = false;
            Show();
            WindowState = FormWindowState.Normal;
            Focus();
        }

        #endregion

        #region 缓存管理

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
        readonly Dictionary<string, Bitmap2> _imageDic = new Dictionary<string, Bitmap2>();

        /// <summary>
        /// store jobs
        /// </summary>
        List<IJob> _jobs = new List<IJob>();

        /// <summary>
        /// 当前选中的Bitmap2信息，包含图层，波段，索引等
        /// </summary>
        Bitmap2 _selectBmp2 = null;

        /// <summary>
        /// glovenet for nlp
        /// </summary>
        IDEmbeddingNet _gloVeNet = null;

        /// <summary>
        /// nlp server process
        /// </summary>
        Process _process;

        #endregion

        #region 界面更新

        /// <summary>
        /// 接受process ouput
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Invoke(new UpdateListBoxHandler((msg) =>
            {
                if (msg == null) return;
                NLP_listBox.Items.Add(msg);
                NLP_listBox.SelectedIndex = NLP_listBox.Items.Count - 1;
            }), e.Data);
        }

        /// <summary>
        /// 更新底部栏提示
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="statue"></param>
        private void UpdateStatusLabel(string msg)
        {
            Main_statusLabel.Text = msg;
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
        /// 更新listbox区域显示内容
        /// </summary>
        /// <param name="msg"></param>
        private delegate void UpdateListBoxHandler(string msg);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        private delegate void UpdateGenericHandler(double msg);

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
        private delegate void UpdateStatusLabelHandler(string msg);

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
                Invoke(new UpdateStatusLabelHandler(UpdateStatusLabel), "请勿重复打开相同的影像!");
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
                    Invoke(new UpdateListBoxHandler(UpdateMapListBox), outputs[0] as string);
                    break;
                case "LoadGloVeNetTask":
                    Invoke(new UpdateGenericHandler((msg) =>
                    {
                        Main_processBar.Visible = true;
                        Main_processBar.Value = (int)(msg * 100);
                        Main_statusLabel.Text = string.Format("Loading: {0:p}", msg);
                    }), Convert.ToDouble(outputs[0]));
                    break;
                default:
                    Invoke(new UpdateListBoxHandler(UpdateMapListBox), outputs[0] as string);
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
                    {
                        string fullFilename = outputs[0] as string;
                        ReadRaster(fullFilename);
                    }
                    break;
                //load image
                case "ReadRasterTask":
                    {
                        string nodeName = outputs[0] as string;
                        Dictionary<string, Bitmap2> dict = outputs[1] as Dictionary<string, Bitmap2>;
                        GRasterLayer rasterLayer = outputs[2] as GRasterLayer;
                        UpdateReadRasterUI(nodeName, dict, rasterLayer);
                    }
                    break;
                // rpc rester rectify
                case "RPCRasterRectifyTask":
                    break;
                //case load GloveNet
                case "LoadGloVeNetTask":
                    {
                        //report loading progress
                        Invoke(new UpdateGenericHandler((text) =>
                        {
                            Main_processBar.Visible = false;
                            Main_statusLabel.Text = "ready";
                            Expertise_Knowledge_toolStripButton.Enabled = true;
                        }), 0);
                        //model
                        _gloVeNet = outputs[0] as IDEmbeddingNet;
                    }

                    break;
                default:
                    break;
            }
            //print completed information
            string msg = string.Format("time:{0},task:{1} completed", Now, taskName);
            Invoke(new UpdateListBoxHandler(UpdateMapListBox), msg);
        }

        #endregion

        #region UI事件响应

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
                //task
                case "Tasks_Monitor_toolStripButton":
                    TaskMonitor mapTaskForm = new TaskMonitor();
                    mapTaskForm.Jobs = _jobs;
                    mapTaskForm.ShowDialog();
                    break;
                //calucte kappa and oa
                case "Accuracy_toolStripButton":
                    KappaOaForm koaForm = new KappaOaForm();
                    koaForm.RasterDic = _rasterDic;
                    koaForm.ShowDialog();
                    break;
                //添加图像
                case "Open_toolstripmenuitem":
                case "Open_contextMenuStrip":
                    ReadImage();
                    break;
                //DQN PolSAR Classification 
                case "DQN_PolSAR_Classification_ToolStripMenuItem":
                    DQNPolSARForm dqnForm = new DQNPolSARForm();
                    dqnForm.RasterDic = _rasterDic;
                    if (dqnForm.ShowDialog() == DialogResult.OK)
                    {
                        IJob dqnClassifyJob = new JobDQNClassify(
                            _rasterDic[dqnForm.RasterLayerName],
                            dqnForm.SampleFilename,
                            dqnForm.NetName,
                            dqnForm.DeviceName,
                            dqnForm.ImageWidth,
                            dqnForm.ImageHeight,
                            dqnForm.ImageDepth,
                            dqnForm.Epochs);
                        RegisterJob(dqnClassifyJob);
                        dqnClassifyJob.Start();
                    }
                    break;
                //cnn classification
                case "CNN_toolStripButton":
                    CNNForm convForm = new CNNForm();
                    convForm.RasterDic = _rasterDic;
                    if (convForm.ShowDialog() == DialogResult.OK)
                    {
                        IJob cnnTrainingJob = new JobCNNClassify(_rasterDic[convForm.RasterLayerName], convForm.NetName, convForm.SampleFilename, convForm.SaveModelFilename, convForm.Epochs, convForm.ImageWidth, convForm.ImageHeight, convForm.ImageDepth, convForm.DeviceName);
                        RegisterJob(cnnTrainingJob);
                        cnnTrainingJob.Start();
                    }
                    break;
                //random forest classification
                case "RandomForest_ToolStripMenuItem":
                    RFForm rfForm = new RFForm();
                    rfForm.RasterDic = _rasterDic;
                    if (rfForm.ShowDialog() == DialogResult.OK)
                    {
                        if (rfForm.Model == 1)
                        {
                            IJob rfJob = new JobRFClassify(rfForm.TreeCount, rfForm.SampleFullFilename, _rasterDic[rfForm.FeatureKey]);
                            RegisterJob(rfJob);
                            rfJob.Start();
                        }
                        else if (rfForm.Model == 2)
                        {
                            IJob rfJob2 = new JobRFCSV(rfForm.TreeCount, rfForm.SampleFullFilename, rfForm.WaitFullFilename, rfForm.SaveFullFilename);
                            RegisterJob(rfJob2);
                            rfJob2.Start();
                        }
                    }
                    break;
                //make samples in single-minibatch
                case "Single_Batch_ToolStripMenuItem":
                    SingleBatchExportForm Single_batch_form = new SingleBatchExportForm();
                    Single_batch_form.RasterDic = _rasterDic;
                    Single_batch_form.ShowDialog();
                    break;
                //make sample in multi-minibatches
                case "Multi_Batches_ToolStripMenuItem":
                    {
                        MultiBatchesExprotForm multi_batch_form = new MultiBatchesExprotForm();
                        multi_batch_form.ShowDialog();
                    }
                    break;
                //svm function
                case "L2SVM_ToolStripMenuItem":
                    SVMFrom svm_Form = new SVMFrom();
                    svm_Form.RasterDic = _rasterDic;
                    if (svm_Form.ShowDialog() == DialogResult.OK)
                    {
                        if (svm_Form.Model == 1)
                        {
                            IJob svmJob = new JobSVMClassify(svm_Form.SampleFullFilename, _rasterDic[svm_Form.FeatureKey]);
                            RegisterJob(svmJob);
                            svmJob.Start();
                        }
                        else if (svm_Form.Model == 2)
                        {
                            IJob svmJob2 = new JobSVMCSV(svm_Form.SampleFullFilename, svm_Form.WaitFullFilename, svm_Form.SaveFullFilename);
                            RegisterJob(svmJob2);
                            svmJob2.Start();
                        }
                    }
                    break;
                //cnn svm classification
                case "CNN_SVM_toolStripButton":
                    CNNSVMFrom c_s_form = new CNNSVMFrom();
                    c_s_form.RasterDic = _rasterDic;
                    if (c_s_form.ShowDialog() == DialogResult.OK)
                    {
                        //IJob cnnClassifyJob = new JobCNNSVMClassify(
                        //    _rasterDic[c_s_form.SelectedFeatureRasterLayer],
                        //    c_s_form.Epochs, 
                        //    c_s_form.Model,
                        //    c_s_form.ImageWidth, 
                        //    c_s_form.ImageHeight, 
                        //    1, c_s_form.FullFilename);
                        //RegisterJob(cnnClassifyJob);
                        //cnnClassifyJob.Start();
                    }
                    break;
                //cnn dqn classification
                case "CNN_DQN_toolStripButton":
                    CNNDQNForm c_d_form = new CNNDQNForm();
                    if (c_d_form.ShowDialog() == DialogResult.OK)
                    {

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
            //change view
            Main_tabControl.SelectedIndex = 1;
            //switch tool
            ToolStripItem item = sender as ToolStripItem;
            switch (item.Name)
            {
                //open raw text file
                case "AddText_toolStripMenuItem":
                case "Open_RawFile_toolStripButton":
                    {
                        OpenFileDialog opg = new OpenFileDialog();
                        opg.Filter = "报道记录文件|*.doc;*.docx|IOPF记录文件|*.pdf";
                        if (opg.ShowDialog() == DialogResult.OK)
                        {
                            string[] text = HostHelper.ReadFlatText(opg.FileName);
                            int seed = 1;
                            Array.ForEach(text, paragraph =>
                            {
                                string formatText = string.Format("{0} 段落 {1} : {2}", HostHelper.Now, seed, paragraph);
                                NLP_listBox.Items.Add(formatText);
                                seed++;
                            });
                        }
                    }
                    break;
                //clear nlp raw text view items
                case "Clear_NLPRawTextView_toolStripButton":
                    {
                        if (MessageBox.Show("是否清除原始文本数据？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                            NLP_listBox.Items.Clear();
                    }
                    break;
                case "Annotation_toolStripButton":
                    {
                        
                        SentenceGroup pipline = new SentenceGroup(HostHelper.FullText);
                        pipline.RegroupByTimeline("zh-cn");
                        //逐行处理
                        //1. regorup incident
                        //string incidentText = HostHelper.Iopf.IncidentText;
                        //
                        //incidentGroup.RegroupByTimeline();

                        //2. regroup impact

                        //3. regroup response

                        //4. mixture three aspects

                        //5. annotator
                    }
                    break;
                //load gloVe model
                case "Load_Words_Embedding_ToolStripMenuItem":
                    {
                        Load_Words_Embedding_ToolStripMenuItem.Enabled = false;
                        IJob gloVeNetJob = new JobLoadGloVeNet(NLPConfiguration.GloVeEmbeddingString);
                        RegisterJob(gloVeNetJob);
                        gloVeNetJob.Start();
                    }
                    break;
                //setting domain knowledge for custer algorihtm
                case "Expertise_Knowledge_toolStripButton":
                    {
                        //NLPExpertiseForm nlp_expertise_form = new NLPExpertiseForm();
                        //nlp_expertise_form.GloveNet = _gloVeNet;
                        //nlp_expertise_form.ShowDialog();
                    }
                    break;
                //create scenario
                case "Scenario_toolStripButton":
                    {
                        //NLPScenarioForm nlp_scenario_form = new NLPScenarioForm();
                        //nlp_scenario_form.GloveNet = _gloVeNet;
                        //nlp_scenario_form.Show();
                    }
                    break;
                //setting configuration
                case "Tools_Configuration_ToolStripMenuItem":
                    {
                        NLPConfigForm nlpConfigForm = new NLPConfigForm();
                        nlpConfigForm.ShowDialog();
                    }
                    break;
                //lstm test 
                case "LSTM_toolStripButton":
                    {
                        //string rawTextFullFilename = Directory.GetCurrentDirectory() + @"\tmp\RawText.txt";
                        //string autosave = Directory.GetCurrentDirectory() + @"\tmp\autolstm.bin";
                        //IJob rnnTrainJob = new JobRNNTrain(rawTextFullFilename, autosave);
                        //RegisterJob(rnnTrainJob);
                        //rnnTrainJob.Start();
                    }
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
