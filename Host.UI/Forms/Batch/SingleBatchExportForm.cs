using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Engine.Brain.Method.DeepQNet.Env;
using Engine.GIS.GLayer.GRasterLayer;

namespace Host.UI.SettingForm
{
    public partial class SingleBatchExportForm : Form
    {
        public SingleBatchExportForm()
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

        Dictionary<string, RowCol> ROWCOL_DICT;

        /// <summary>
        /// 
        /// </summary>
        int[] _masks = new int[] { 1, 3, 5, 7, 9, 11, 13, 15, 17, 19 };

        /// <summary>
        /// pick value methods enmu
        /// </summary>
        private string[] _PICK_METHODS = new string[] {
            " Single Pixel picked in each band",
            " 3x3 Mask picked in each band",
            " 5x5 Mask picked in each band",
            " 7x7 Mask picked in each band",
            " 9x9 Mask picked in each band",
            " 11x11 Mask picked in each band",
            " 13x13 Mask picked in each band",
            " 15x15 Mask picked in each band",
            " 17x17 Mask picked in each band"
        };

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
            //init rowcol dict
            ROWCOL_DICT = new Dictionary<string, RowCol>();
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
            for (int i = 0; i < _PICK_METHODS.Length; i++)
            {
                string pName = _PICK_METHODS[i];
                PICK_METHOD_comboBox.Items.Add(pName);
                ROWCOL_DICT[pName] = new RowCol(_masks[i], _masks[i]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
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
                EXPORT_PATH_button.Text = "导出";
                EXPORT_PATH_button.Enabled = true;
                MessageBox.Show("样本导出完成", "结果", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                EXPORT_PATH_button.Text = string.Format("{0:P}", process);
        }

        /// <summary>
        /// select feature layer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RAW_IMAGE_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string key = (sender as ComboBox).SelectedItem as string;
            selectedFeatureLayer = key;
        }

        /// <summary>
        /// labeled layer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LABELED_IMAGE_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string key = (sender as ComboBox).SelectedItem as string;
            selectLabelLayer = key;
        }

        /// <summary>
        /// pick method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PICK_METHOD_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string key = (sender as ComboBox).SelectedItem as string;
            pickMethod = key;
        }

        /// <summary>
        /// 
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
            sfg.FileName = string.Format("{0}_{1}_{2}_LerpPick_{3}", selectedFeatureLayer + selectLabelLayer, pickMethod, sampleSizeLimit, lerpPick);
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
                        MessageBox.Show("导出样本取消", "结果", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                //2.create Directory
                else
                {
                    ProcessBatchSample(directory, sampleSizeLimit, repeatNum, lerpPick);
                }
            }
        }

        /// <summary>
        /// clear directory
        /// </summary>
        /// <param name="directory"></param>
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

        /// <summary>
        /// create batch samples in new thread
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="sampleSizeLimit"></param>
        /// <param name="repeatNum"></param>
        /// <param name="lerpPick"></param>
        private void ProcessBatchSample(string directory, int sampleSizeLimit, int repeatNum, bool lerpPick)
        {
            EXPORT_PATH_button.Enabled = false;
            EXPORT_PATH_button.Text = string.Format("{0:P}", 0.0);
            Thread t = new Thread(() =>
            {
                RowCol rowcol = ROWCOL_DICT[pickMethod];
                Directory.CreateDirectory(directory);
                string indexString = "";
                GRasterLayer featureLayer = _rasterDic[selectedFeatureLayer], labelLayer = _rasterDic[selectLabelLayer];
                //build env
                ImageClassifyEnv env = new ImageClassifyEnv(featureLayer, labelLayer, sampleSizeLimit, lerpPick);
                for (int i = 0; i < repeatNum; i++)
                {
                    string filename = string.Format("{0}_{1}_{2}_{3}", DateTime.Now.ToFileTimeUtc().ToString(), rowcol.Row, rowcol.Col, featureLayer.BandCount) + ".txt";
                    env.Prepare();
                    env.Export(directory + @"\" + filename, rowcol.Row, rowcol.Col);
                    indexString += filename + "\r\n";
                    Invoke(new UpdateProcessTipHandler(UpdateProcessTip), (double)i / repeatNum);
                }
                //save index file
                using (StreamWriter sw = new StreamWriter(directory + @"\" + "index.be"))
                {
                    sw.Write(indexString);
                }
                //smaple batch complete
                Invoke(new UpdateProcessTipHandler(UpdateProcessTip), 1.0);
            });
            t.IsBackground = true;
            t.Start();
        }
    }


    class RowCol
    {

        public int Row { get; set; }
        public int Col { get; set; }

        public RowCol(int row, int col)
        {
            Row = row;
            Col = col;
        }
    }

}
