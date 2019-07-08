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
        /// 
        /// </summary>
        /// <param name="fullFilename"></param>
        /// <returns></returns>
        private double[] PickSampleNormalValue(string fullFilename)
        {
            GRasterLayer featureRasterLayer = new GRasterLayer(fullFilename);
            IRasterLayerCursorTool pRasterLayerCursorTool = new GRasterLayerCursorTool();
            pRasterLayerCursorTool.Visit(featureRasterLayer);
            int centerX = featureRasterLayer.XSize / 2;
            int centerY = featureRasterLayer.YSize / 2;
            //double[] sampleValue = pRasterLayerCursorTool.PickRagneNormalValue(centerX, centerY, row, col);
            //return sampleValue;
            return null;
        }

        private void CrateSampleBatch(string batchId)
        {
            DirectoryInfo root = new DirectoryInfo(Root_Directory_textBox.Text);
            foreach (DirectoryInfo sampleDir in root.GetDirectories())
            {
                //清理xml
                ClearXML(sampleDir);
                //2.1样本标注
                //int key = _namekey[sampleDir.Name];
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
                        double[] sampleValue = PickSampleNormalValue(file.FullName);
                        lines.Add(string.Join(",", sampleValue) + "," + key);
                    }
                    catch
                    {
                        continue;
                    }
                }
                //key
                //string filename = string.Format("{0}_{1}_{2}_{3}", key, row, col, bandcount) + ".txt";
                //if (!Directory.Exists(outputworkspace + @"\" + batchId))
                //    Directory.CreateDirectory(outputworkspace + @"\" + batchId);
                ////batchId
                //using (StreamWriter sw = new StreamWriter(outputworkspace + @"\" + batchId + @"\" + filename))
                //    foreach (string line in lines)
                //        sw.WriteLine(line);
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
            //
            Start_button.Enabled = false;
            //重复采集
            for(int i = 0; i < Convert.ToInt32(Repeat_Pick_numericUpDown.Value); i++)
            {

            }
        }
    }
}
