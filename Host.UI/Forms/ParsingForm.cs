using System;
using System.Windows.Forms;

namespace Host.UI.SettingForm
{
    public partial class ParsingForm : Form
    {
        public ParsingForm()
        {
            InitializeComponent();
        }
        public string TextFullFilename { get; private set; }
        public string ModelFullFilename { get; private set; }
        public string LexiconFullFilename { get; private set; }
        private void OPEN_TEXT_button_Click(object sender, EventArgs e)
        {
            OpenFileDialog opg = new OpenFileDialog();
            opg.Filter = "Text文本文件|*.txt";
            if(opg.ShowDialog() == DialogResult.OK)
            {
                TextFullFilename = TEXT_textBox.Text = opg.FileName;
            }
        }
        private void OPEN_MODEL_button_Click(object sender, EventArgs e)
        {
            OpenFileDialog opg = new OpenFileDialog();
            opg.Filter = "Bin模型文件|*.bin";
            if (opg.ShowDialog() == DialogResult.OK)
            {
                ModelFullFilename  = MODEL_textBox.Text = opg.FileName;
            }
        }
        private void OPEN_LEXICON_button_Click(object sender, EventArgs e)
        {
            OpenFileDialog opg = new OpenFileDialog();
            opg.Filter = "lexicon字典文件|*.txt";
            if (opg.ShowDialog() == DialogResult.OK)
            {
                LexiconFullFilename = LEXICON_textBox.Text = opg.FileName;
            }
        }
        private void OK_button_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
