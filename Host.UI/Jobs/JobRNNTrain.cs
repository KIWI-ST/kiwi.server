using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Engine.Brain.Model;
//using Engine.Lexicon.Entity;
using OxyPlot;

namespace Host.UI.Jobs
{
    //public class JobRNNTrain : IJob
    //{
    //    public bool Complete { get; private set; } = false;

    //    public string Name => "JobRNNTrainingTask";

    //    public string Summary { get {

    //            //if (network == null)
    //                //return "";
    //            //else
    //               // return string.Format("loss:{0},liter:{1}", network.Loss, network.liter);
    //        } }

    //    public double Process
    //    {
    //        get
    //        {
    //            if (network == null)
    //                return 0;
    //            else
    //                return network.Process;
    //        }
    //    }

    //    public DateTime StartTime { get; private set; } = DateTime.Now;

    //    public PlotModel[] PlotModels => throw new NotImplementedException();

    //    public event OnTaskCompleteHandler OnTaskComplete;
    //    public event OnStateChangedHandler OnStateChanged;

    //    Thread _t;

    //    //LSTMNetwork network; 

    //    public JobRNNTrain(string rawTextFullFilename, string exitsModelFullFilename = null, string lexiconFullFilename = null)
    //    {
    //        _t = new Thread(() =>
    //        {
    //            //如果没有字典文件，则从原始文本中构建字典
    //            Lexicon lexicon = lexiconFullFilename == null ? Lexicon.FromVocabularyFile(rawTextFullFilename, EncodeScheme.Onehot) : Lexicon.FromExistLexiconFile(lexiconFullFilename, EncodeScheme.Onehot);
    //            //network = !File.Exists(exitsModelFullFilename) ? new LSTMNetwork(lexicon.VocaSize) : LSTMNetwork.Load(exitsModelFullFilename);
    //            //network.LearnFromRawText(rawTextFullFilename,lexicon);
    //            //训练完成
    //            OnTaskComplete?.Invoke(Name);
    //        });
    //    }

    //    public void Export(string fullFilename)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void Start()
    //    {
    //        StartTime = DateTime.Now;
    //        _t.IsBackground = true;
    //        _t.Start();
    //    }
    //}
}
