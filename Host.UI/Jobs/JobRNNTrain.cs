using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Engine.Brain.Model.DL;
using Engine.Lexicon.Entity;
using OxyPlot;

namespace Host.UI.Jobs
{
    public class JobRNNTrain : IJob
    {
        public bool Complete { get; private set; } = false;

        public string Name => "JobRNNTrainingTask";

        public string Summary { get; private set; } = "";

        public double Process { get; private set; } = 0.0;

        public DateTime StartTime { get; private set; } = DateTime.Now;

        public PlotModel[] PlotModels => throw new NotImplementedException();

        public event OnTaskCompleteHandler OnTaskComplete;

        Thread _t;

        public JobRNNTrain(string rawTextFullFilename, string exitsModelFullFilename = null, string lexiconFullFilename = null)
        {
            _t = new Thread(() =>
            {
                //如果没有字典文件，则从原始文本中构建字典
                Summary = string.Format("statical lexicon in progress ...");
                Lexicon lexicon = lexiconFullFilename == null ? Lexicon.FromVocabularyFile(rawTextFullFilename, EncodeScheme.Onehot) : Lexicon.FromExistLexiconFile(lexiconFullFilename, EncodeScheme.Onehot);
                LSTMNetwork network = exitsModelFullFilename == null&&!File.Exists(exitsModelFullFilename) ? new LSTMNetwork(lexicon.VocaSize) : LSTMNetwork.Load(exitsModelFullFilename);
                network.OnTrainingProgress += Network_OnTrainingProgress;
                network.LearnFromRawText(rawTextFullFilename,lexicon);
                //训练中
                Summary = string.Format("RNN training complete");
                OnTaskComplete?.Invoke(Name);
            });
        }

        private void Network_OnTrainingProgress(double loss, int liter,double process)
        {
            Process = process;
            Summary = string.Format("Liter:{0}, Loss:{1} ", liter, loss);
        }

        public void Export(string fullFilename)
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            StartTime = DateTime.Now;
            _t.IsBackground = true;
            _t.Start();
        }
    }
}
