using System;
using System.Windows.Forms;
using Engine.NLP;

namespace Host.UI.Forms
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
            //1.CoreNLPSetupString
            CoreNLP_Start_Command_textBox.Text = NLPConfiguration.CoreNLPCommandString;
            //2.CoreNLP dir
            CoreNLP_Dir_textBox.Text = NLPConfiguration.CoreNLPDirString;
            //3.golVe file
            GloVe_File_textBox.Text = NLPConfiguration.GloVeEmbeddingString;
        }

        private void OK_button_Click(object sender, EventArgs e)
        {
            //1. save coreNLP start command
            NLPConfiguration.CoreNLPCommandString = CoreNLP_Start_Command_textBox.Text;
            //2.save CoreNLP dir
            NLPConfiguration.CoreNLPDirString = CoreNLP_Dir_textBox.Text;
            //3.save golVe file
            NLPConfiguration.GloVeEmbeddingString = GloVe_File_textBox.Text;
            //close directly
            Close();
        }
    }
}
