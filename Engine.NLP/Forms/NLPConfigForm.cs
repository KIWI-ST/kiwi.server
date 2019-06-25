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
            //1. mocrosoft cognitive endpoint
            Cognitive_Endpoint_Command_textBox.Text = NLPConfiguration.Endpoint;
            //2. mocrosoft cognitive key
            Cognitive_Key_textBox.Text = NLPConfiguration.SubscriptionKey;
            //3.golVe file
            GloVe_File_textBox.Text = NLPConfiguration.GloVeEmbeddingString;
        }

        private void OK_button_Click(object sender, EventArgs e)
        {
            //1. save coreNLP start command
            NLPConfiguration.Endpoint = Cognitive_Endpoint_Command_textBox.Text;
            //2.save CoreNLP dir
            NLPConfiguration.SubscriptionKey = Cognitive_Key_textBox.Text;
            //3.save golVe file
            NLPConfiguration.GloVeEmbeddingString = GloVe_File_textBox.Text;
            //close directly
            Close();
        }
    }
}
