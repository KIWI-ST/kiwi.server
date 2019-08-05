using System;
using System.Threading;
using Engine.Brain.Method;
using Engine.Brain.Utils;

namespace Host.UI.Jobs
{
    public class JobLoadGloVeNet: IJob
    {
        public double Process { get; private set; } = 0.0;

        public string Name { get; private set; } = "LoadGloVeNetTask";

        public string Summary { get; private set; } = "";

        public DateTime StartTime { get; private set; } = DateTime.Now;

        public bool Complete { get; private set; } = false;

        public event OnTaskCompleteHandler OnTaskComplete;

        public event OnStateChangedHandler OnStateChanged;

        Thread _t;

        public JobLoadGloVeNet(string gloVeFilename)
        {
            _t = new Thread(() =>
            {
                string deviceName = NP.CNTK.DeviceCollection[0];
                Summary = "构建GloveNet开始";
                IDEmbeddingNet gloVeNet = new GloVeNet(deviceName, gloVeFilename);
                gloVeNet.OnLoading += (double percentage) => {
                    Process = percentage;
                    OnStateChanged?.Invoke(Name, percentage);
                };
                gloVeNet.Load();
                Summary = "加载GloveNet完成";
                Complete = true;
                OnTaskComplete?.Invoke(Name, gloVeNet);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullFilename"></param>
        public void Export(string fullFilename)
        {

        }
        /// <summary>
        /// start task
        /// </summary>
        public void Start()
        {
            StartTime = DateTime.Now;
            _t.IsBackground = true;
            _t.Start();
        }
    }
}
