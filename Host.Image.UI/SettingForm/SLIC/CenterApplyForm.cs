using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Host.Image.UI.SettingForm.SLIC
{
    public partial class CenterApplyForm : Form
    {
        List<string> _files;

        public CenterApplyForm()
        {
            InitializeComponent();
            _files = new List<string>();
        }
        public List<string> FileNameCollection
        {
            get { return _files;
            }
        }
        private void center_read_button_Click(object sender, EventArgs e)
        {
            OpenFileDialog opg = new OpenFileDialog();
            opg.Multiselect = true;
            if (opg.ShowDialog() == DialogResult.OK)
            {
                for (int i = 0; i < opg.FileNames.Length; i++)
                {
                    string fileName = opg.FileNames[i];
                    listBox1.Items.Add(fileName);
                    _files.Add(fileName);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
