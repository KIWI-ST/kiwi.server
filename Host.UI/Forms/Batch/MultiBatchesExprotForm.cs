using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Engine.Brain.Extend;
using Engine.GIS.GLayer.GRasterLayer;
using Engine.GIS.GOperation.Tools;

namespace Host.UI.Forms
{
    public partial class MultiBatchesExprotForm : Form
    {
        public MultiBatchesExprotForm()
        {
            InitializeComponent();
            Initialization();
        }

        string[] _pickMethods = new string[] { "select 193x193 pixels from center position"};

        int[] _pickMethodValues = new int[] { 193 };

        private void Initialization()
        {
            Pick_Method_comboBox.Items.Clear();
            Array.ForEach(_pickMethods, (method) =>{
                Pick_Method_comboBox.Items.Add(method);
            });
            Pick_Method_comboBox.SelectedIndex = 0;
        }

        /// <param name="process"></param>
        private delegate void UpdateProcessTipHandler(double process);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="process"></param>
        private void UpdateProcessTip(double process)
        {
            if (process == 1.0)
            {
                Start_button.Text = "导出";
                Start_button.Enabled = true;
                MessageBox.Show("样本导出完成", "结果", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                Start_button.Text = string.Format("{0:P}", process);
        }

        /// <summary>
        /// remove xml
        /// </summary>
        /// <param name="dir"></param>
        private void ClearXML(DirectoryInfo dir)
        {
            foreach (FileInfo file in dir.GetFiles("*.xml"))
                file.Delete();
        }

        /// <summary>
        /// warning: bandCount is not used at this version
        /// </summary>
        /// <param name="fullFilename"></param>
        /// <returns></returns>
        private double[] PickSampleNormalValue(string fullFilename, int row, int col, int bandCount)
        {
            GRasterLayer featureRasterLayer = new GRasterLayer(fullFilename);
            IRasterLayerCursorTool pRasterLayerCursorTool = new GRasterLayerCursorTool();
            pRasterLayerCursorTool.Visit(featureRasterLayer);
            int centerX = featureRasterLayer.XSize / 2;
            int centerY = featureRasterLayer.YSize / 2;
            double[] sampleValue = pRasterLayerCursorTool.PickRagneNormalValue(centerX, centerY, row, col);
            return sampleValue;
        }

        private void CrateSampleBatch(string batchId, string inputDirectory, string outputDirectory, int row, int col, int bandCount)
        {
            DirectoryInfo root = new DirectoryInfo(inputDirectory);
            foreach (DirectoryInfo sampleDir in root.GetDirectories())
            {
                //清理xml
                ClearXML(sampleDir);
                //2.1样本标注
                string key = sampleDir.Name;
                //2.2随机获取样本
                List<FileInfo> files = sampleDir.GetFiles().ToList().RandomTakeBatch(Convert.ToInt32(Each_Class_Size_numericUpDown.Value));
                //2.3 sample filename
                List<string> lines = new List<string>();
                //2.3获取值
                foreach (FileInfo file in files)
                {
                    try
                    {
                        //sampleing
                        double[] sampleValue = PickSampleNormalValue(file.FullName, row, col, bandCount);
                        lines.Add(string.Join(",", sampleValue) + "," + key);
                    }
                    catch
                    {
                        continue;
                    }
                }
                //output sample filename  key_row_col_bandCount
                string filename = string.Format("{0}_{1}_{2}_{3}", key, row, col, bandCount) + ".txt";
                if (!Directory.Exists(outputDirectory + @"\" + batchId))
                    Directory.CreateDirectory(outputDirectory + @"\" + batchId);
                //batchId
                using (StreamWriter sw = new StreamWriter(outputDirectory + @"\" + batchId + @"\" + filename))
                    foreach (string line in lines)
                        sw.WriteLine(line);
            }
        }

        private void Setting_Root_button_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请设原始样本根目录";
            if(dialog.ShowDialog() == DialogResult.OK)
            {
                Root_Directory_textBox.Text = dialog.SelectedPath;
            }
        }

        private void Output_Samples_button_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请设置导出样本根目录";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Output_Samples_textBox.Text = dialog.SelectedPath;
            }
        }

        private void Start_button_Click(object sender, EventArgs e)
        {
            if (Root_Directory_textBox.Text == null || Root_Directory_textBox.Text.Length == 0)
            {
                MessageBox.Show("请设置样本根目录");
                return;
            }
            if(Output_Samples_textBox.Text == null || Output_Samples_textBox.Text.Length==0)
            {
                MessageBox.Show("请设置样本导出目录");
                return;
            }
            //禁止重复构造
            Start_button.Enabled = false;
            //相关参数获取
            string inputDirectory, outputDirectory;
            inputDirectory = Root_Directory_textBox.Text;
            outputDirectory = Output_Samples_textBox.Text;
            int row, col, bandCount, batchSize, repeatNum;
            row = col = _pickMethodValues[Pick_Method_comboBox.SelectedIndex];
            bandCount = Convert.ToInt32(Pick_Band_Count_numericUpDown.Value);
            repeatNum = Convert.ToInt32(Repeat_Pick_numericUpDown.Value);
            batchSize = Convert.ToInt32(Each_Class_Size_numericUpDown.Value);
            //backgorund work for generating mini-batch samples
            Thread t = new Thread(() => {
                Invoke(new UpdateProcessTipHandler(UpdateProcessTip), 0);
                //重复采集
                for (int i = 0; i < repeatNum; i++)
                {
                    string batchID = DateTime.Now.ToFileTimeUtc().ToString();
                    CrateSampleBatch(batchID, inputDirectory, outputDirectory, row, col, bandCount);
                    Invoke(new UpdateProcessTipHandler(UpdateProcessTip), (double)i / repeatNum);
                }
                //smaple batch complete
                Invoke(new UpdateProcessTipHandler(UpdateProcessTip), 1.0);
            });
            t.IsBackground = true;
            t.Start();
        }
    }
}
