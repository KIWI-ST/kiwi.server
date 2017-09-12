using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Programstrap.Main
{
    /// <summary>
    /// 扩展主窗体的视图功能
    /// </summary>
    public partial class MainForm
    {
        /// <summary>
        /// 初始化视图功能
        /// </summary>
        private void ViewInitialization()
        {
            _imageStream = new Dictionary<string, byte[]>();
            _dataClassContainerList = new List<BaseImageProcess.Container<BaseImageProcess.DataClass>>();
        }

        //PictureBox存放文件流
        Dictionary<string, byte[]> _imageStream;
        //
        List<BaseImageProcess.Container<BaseImageProcess.DataClass>> _dataClassContainerList;
        //
      
    }
}
