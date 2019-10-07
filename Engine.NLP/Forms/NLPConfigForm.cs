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
            GloVe_File_textBox.Text = NLPConfiguration.GloVeEmbeddingString;
            StanfordNLP_Server_Url_textBox.Text = NLPConfiguration.CoreNLPAddress;
            StanfordNLP_Server_Port_textBox.Text = NLPConfiguration.CoreNLPPort;
        }


        private void OK_button_Click(object sender, EventArgs e)
        {
            NLPConfiguration.GloVeEmbeddingString = GloVe_File_textBox.Text;
            NLPConfiguration.CoreNLPAddress = StanfordNLP_Server_Url_textBox.Text;
            NLPConfiguration.CoreNLPPort = StanfordNLP_Server_Port_textBox.Text;
            //close directly
            Close();
        }
    }
}
