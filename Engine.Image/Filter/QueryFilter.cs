using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;


namespace Engine.Image
{
    public class ContorlMessageFilter : System.Windows.Forms.IMessageFilter
    {
        public event QueryFilterEventHandler OnQueryFilterEvent;
        private void QueryFilter(int zoomindex, System.Drawing.Point screenPoint)
        {
            if(OnQueryFilterEvent!=null)
                OnQueryFilterEvent(zoomindex,screenPoint);
        }
        #region 常量定义
        /// <summary>
        /// 鼠标滚轮
        /// </summary>
        int WM_MouseWheel = 522;
        #endregion
        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == WM_MouseWheel)
            {
                //当前鼠标位置
                uint lparm = (uint)m.LParam;
                uint heigh = lparm >> 16;
                uint low = (UInt16)lparm;
                System.Drawing.Point screenPoint = new System.Drawing.Point((int)low, (int)heigh);
                //向上 放大
                if ((int)m.WParam > 0) 
                    QueryFilter(1,screenPoint);
                else
                    QueryFilter(-1,screenPoint);
            }
            return false;
        }
    }
}
