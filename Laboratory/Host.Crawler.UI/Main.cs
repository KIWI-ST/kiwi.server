using GrainImplement.Crawler.Helper;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Host.Crawler.UI
{
    /// <summary>
    /// 切换线程用于更新UI
    /// </summary>
    public delegate void UpdateUIHandler(string msg);

    public partial class Main : Form
    {

        CrawlerHelper _crawlerHelper;

        OrleansHostWrapper _hostWrapper;

        public Main()
        {
            InitializeComponent();
        }

        private void ClickEventHandler(object sender, EventArgs e)
        {
            Control ctl = sender as Control;
            NotifyIcon nfy = sender as NotifyIcon;
            string name = ctl != null ? ctl.Name : nfy.Text;
            switch (name)
            {
                case "btn_start":
                    label4.Text = DateTime.Now.ToLongDateString() + DateTime.Now.ToLongTimeString();
                    BackgroundWorker _worker = new BackgroundWorker();
                    _worker.DoWork += _worker_DoWork;
                    _worker.RunWorkerAsync();
                    break;
                case "btn_background":
                    WindowState = FormWindowState.Minimized;
                    ShowInTaskbar = false;  //不显示在系统任务栏
                    notifyIcon.ShowBalloonTip(2000);
                    notifyIcon.Visible = true;  //托盘图标可见
                    break;
                case "OSM爬虫":
                    WindowState = FormWindowState.Normal;
                    ShowInTaskbar = true;
                    notifyIcon.Visible = false;
                    break;
                default:
                    break;
            }
        }

        private void _worker_DoWork(object sender, DoWorkEventArgs e)
        {
            _hostWrapper = new OrleansHostWrapper(new string[] { });
            //功能函数初始化
            _crawlerHelper = new CrawlerHelper();
            _crawlerHelper.Inilization("mongodb://root:!admin_1@127.0.0.1:27017");
            _crawlerHelper.OnCrawlerProgress += CrawlerHelper_OnCrawlerProgress;
            _crawlerHelper.Run();
        }

        private void CrawlerHelper_OnCrawlerProgress(int count, string msg)
        {
            if (InvokeRequired)
            {
                UpdateUIHandler handler = new UpdateUIHandler(UpdateUI);
                Invoke(handler, msg);
            }
            else
                UpdateUI(msg);
        }

        private void UpdateUI (string msg)
        {
            if (listBox.Items.Count > 100)
                listBox.Items.RemoveAt(0);
            listBox.Items.Add(msg);
            label2.Text = _crawlerHelper.Count.ToString();
            label6.Text = DateTime.Now.ToLongDateString() + DateTime.Now.ToLongTimeString();
        }

    }
}
