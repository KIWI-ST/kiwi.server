using Engine.GIS.GLayer.GRasterLayer;
using Engine.GIS.GOperation.Arithmetic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Host.UI.SettingForm
{
    public partial class KappaForm : Form
    {
        public KappaForm()
        {
            InitializeComponent();
        }

        Dictionary<string, GRasterLayer> _rasterDic;

        public Dictionary<string, GRasterLayer> RasterDic
        {
            set
            {
                _rasterDic = value;
                Initial(_rasterDic);
            }
        }

        public void Initial(Dictionary<string, GRasterLayer> rasterDic)
        {
            comboBox1.Items.Clear();
            rasterDic.Keys.ToList().ForEach(p => {
                comboBox1.Items.Add(p);
            });
            rasterDic.Keys.ToList().ForEach(p => {
                comboBox2.Items.Add(p);
            });
        }

        string truthKey, predKey;

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string key = (sender as ComboBox).SelectedItem as string;
            predKey = key;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //计算kappa,并输出矩阵
            GRasterLayer truthLayer = _rasterDic[truthKey];
            GRasterLayer predLayer = _rasterDic[predKey];
            var (matrix, kappa, actionsNumber,oa) = KappaIndex.Calcute(truthLayer, predLayer);
            //matrix for binding and kappa for display
            kappa_label.Text = string.Format("kappa:{0:P} oa:{1:P}", kappa,oa);
            //绘表头
            webBrowser1.DocumentText = GenericTable(matrix);
        }
        /// <summary>
        /// html绘制表
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        private string GenericTable(int[,] matrix)
        {
            StringBuilder sb = new StringBuilder("");
            int rows = matrix.GetLength(0);
            int cols = rows;
            sb.Append("<table style='margin:0 auto;'>");
            for (int i = 0; i < rows; i++)
            {
                sb.Append("<tr>"); 
                for (int j = 0; j < cols; j++)
                {
                    sb.Append("<td style='border:1px solid gray;'>");
                    sb.Append(matrix[i, j]);
                    sb.Append("</td>");
                }
                sb.Append("</tr>");
            }
            sb.Append("</table>");
            return sb.ToString();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string key = (sender as ComboBox).SelectedItem as string;
            truthKey = key;
        }
    }
}
