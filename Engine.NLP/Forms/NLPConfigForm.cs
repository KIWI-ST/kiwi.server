using System;
using System.Windows.Forms;
using Engine.NLP.Utils;

namespace Engine.NLP.Forms
{
    public partial class NLPConfigForm : Form
    {
        public NLPConfigForm()
        {
            InitializeComponent();
            InitialConfigValue();
        }

        private void InitialConfigValue()
        {
            //3.golVe file
            GloVe_File_textBox.Text = NLPConfiguration.GloVeEmbeddingString;
        }

        private void OK_button_Click(object sender, EventArgs e)
        {
            //3.save golVe file
            NLPConfiguration.GloVeEmbeddingString = GloVe_File_textBox.Text;
            //close directly
            Close();
        }
    }
}
