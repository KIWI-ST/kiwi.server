using Engine.Brain.AI.RL.Env;
using Engine.GIS.GLayer.GRasterLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Host.UI.SettingForm
{
    public partial class BatchExportForm : Form
    {
        public BatchExportForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 
        /// </summary>
        string selectedFeatureLayer;
        /// <summary>
        /// 
        /// </summary>
        string selectLabelLayer;
        /// <summary>
        /// 
        /// </summary>
        string pickMethod;
        /// <summary>
        /// pick value methods enmu
        /// </summary>
        private string[] _PICK_METHODS = new string[] { "Pick pixel value in each band" };
        /// <summary>
        /// natvie store
        /// </summary>
        Dictionary<string, GRasterLayer> _rasterDic;
        /// <summary>
        /// raster dict
        /// </summary>
        public Dictionary<string, GRasterLayer> RasterDic
        {
            set
            {
                _rasterDic = value;
                Initial(_rasterDic);
            }
        }
        //set dict
        public void Initial(Dictionary<string, GRasterLayer> rasterDic)
        {
            //clear item
            RAW_IMAGE_comboBox.Items.Clear();
            LABELED_IMAGE_comboBox.Items.Clear();
            //add item to combox
            rasterDic.Keys.ToList().ForEach(p =>
            {
                RAW_IMAGE_comboBox.Items.Add(p);
                LABELED_IMAGE_comboBox.Items.Add(p);
            });
            //clear item
            PICK_METHOD_comboBox.Items.Clear();
            Array.ForEach(_PICK_METHODS, p =>
            {
                //add item to combox
                PICK_METHOD_comboBox.Items.Add(p);
            });
        }
        /// <summary>
        /// export file path
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EXPORT_PATH_button_Click(object sender, EventArgs e)
        {
            //get parameter
            int repeatNum = Convert.ToInt32(PICK_REPEAT_numericUpDown.Value);
            int sampleSizeLimit = Convert.ToInt32(SAMPLESIZE_LIMIT_numericUpDown.Value);
            bool lerpPick = !LERP_PICK_checkBox.Checked;
            //
            SaveFileDialog sfg = new SaveFileDialog();
            sfg.FileName = string.Format("{0}_{1}_{2}_{3}", selectedFeatureLayer + selectLabelLayer, repeatNum, sampleSizeLimit, lerpPick);
            sfg.AddExtension = true;
            sfg.DefaultExt = ".be";
            if (sfg.ShowDialog() == DialogResult.OK)
            {
                string fullFilename = sfg.FileName;
                string directory = Path.GetDirectoryName(fullFilename) + @"\" + Path.GetFileNameWithoutExtension(fullFilename);
                //1.exist Directory
                if (Directory.Exists(directory))
                {
                    if (MessageBox.Show("文件夹已存在，是否清除文件夹？", "清除文件夹警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    {
                        ClearFolder(directory);
                        ProcessBatchSample(directory, sampleSizeLimit, repeatNum, lerpPick);
                    }
                    else
                    {
                        MessageBox.Show("导出样本取消");
                    }
                }
                //2.create Directory
                else
                {
                    ProcessBatchSample(directory, sampleSizeLimit, repeatNum, lerpPick);
                }
            }
        }

        private void ClearFolder(string directory)
        {
            foreach (string d in Directory.GetFileSystemEntries(directory))
            {
                if (File.Exists(d))
                {
                    FileInfo fi = new FileInfo(d);
                    if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                        fi.Attributes = FileAttributes.Normal;
                    File.Delete(d);//直接删除其中的文件  
                }
                else
                {
                    DirectoryInfo d1 = new DirectoryInfo(d);
                    if (d1.GetFiles().Length != 0)
                    {
                        //递归删除子文件夹
                        Directory.Delete(d1.FullName);
                    }
                    Directory.Delete(d);
                }
            }
        }

        private void ProcessBatchSample(string directory, int sampleSizeLimit, int repeatNum, bool lerpPick)
        {
            Directory.CreateDirectory(directory);
            string indexString = "";
            GRasterLayer featureLayer = _rasterDic[selectedFeatureLayer];
            GRasterLayer labelLayer = _rasterDic[selectLabelLayer];
            //build env
            ImageClassifyEnv env = new ImageClassifyEnv(featureLayer, labelLayer, sampleSizeLimit, lerpPick);
            for (int i = 0; i < repeatNum; i++)
            {
                string filename = DateTime.Now.ToFileTimeUtc().ToString() + ".txt";
                env.Prepare();
                env.Export(directory + @"\" + filename);
                indexString += filename + "\r\n";
            }
            //save index file
            using (StreamWriter sw = new StreamWriter(directory + @"\" + "index.be"))
            {
                sw.Write(indexString);
            }
            //smaple batch complete
            MessageBox.Show("样本处理完成");
        }

        private void RAW_IMAGE_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string key = (sender as ComboBox).SelectedItem as string;
            selectedFeatureLayer = key;
        }

        private void LABELED_IMAGE_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string key = (sender as ComboBox).SelectedItem as string;
            selectLabelLayer = key;
        }

        private void PICK_METHOD_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string key = (sender as ComboBox).SelectedItem as string;
            pickMethod = key;
        }
    }
}
