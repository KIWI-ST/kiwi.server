using System.Diagnostics;
using System.Windows.Forms;

namespace Engine.NLP.Forms
{
    public partial class NLPProcessForm : Form
    {
        public NLPProcessForm()
        {
            InitializeComponent();
        }

        private Process _process;

        public void SetProcess(Process process)
        {
            _process = process;
            _process.Start();
            _process.BeginOutputReadLine();
            _process.BeginErrorReadLine();
            _process.OutputDataReceived += Process_OutputDataReceived;
            _process.ErrorDataReceived += Process_OutputDataReceived;
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Invoke(new UpdateListBoxHandler(UpdateMapListBox), e.Data);
        }

        private delegate void UpdateListBoxHandler(string msg);

        private void UpdateMapListBox(string msg)
        {
            if (msg == null) return;
            CoreNLP_listBox.Items.Add(msg);
            CoreNLP_listBox.SelectedIndex = CoreNLP_listBox.Items.Count - 1;
        }

        private void NLPProcessForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _process.OutputDataReceived -= Process_OutputDataReceived;
            _process.ErrorDataReceived -= Process_OutputDataReceived;
            _process.Kill();
        }
    }
}
