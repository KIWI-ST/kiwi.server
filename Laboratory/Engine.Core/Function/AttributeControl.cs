using System;
using System.Collections.Generic;
using System.Collections;

namespace Engine.Core.Function
{
    /// <summary>
    /// 存储启动前需加载的内容
    /// </summary>
    public class ExtSington
    {
        public static Form.Public_AttributeControl AttributeControl = new Form.Public_AttributeControl();
    }
    /// <summary>
    /// 黄奎，2012-6-25
    /// 重构单例模式
    /// 为队列提供额外的单例，剥离自举机，主程序无需管理自举机
    /// </summary>
    public class AttributeControlUI
    {
        /// <summary>
        /// dll管理器
        /// </summary>
        private  Form.Self_ManagerControl _selfControl;
        /// <summary>
        /// 属性设置容器
        /// </summary>
        private  Form.Public_AttributeControl _attribute = new Form.Public_AttributeControl();
        /// <summary>
        ///  客户端提供UI的索引缓存
        /// </summary>
        private  System.Windows.Forms.UserControl _userControl;
        /// <summary>
        /// 额外自定义窗体
        /// </summary>
        private  Container<System.Windows.Forms.UserControl> _userControlConatiner;
        /// <summary>
        /// 客户端提供UI的索引，只做缓存
        /// </summary>
        public  Container<System.Windows.Forms.UserControl> UserControlConatiner
        {
            get { return _userControlConatiner; }
            set { _userControlConatiner = value; }
        }
        /// <summary>
        /// DLL管理器
        /// </summary>
        public  Form.Self_ManagerControl SelfControl
        {
            get { return _selfControl; }
            set { _selfControl = value; }
        }
        /// <summary>
        /// 额外自定义窗体
        /// </summary>
        public  System.Windows.Forms.UserControl UserControl
        {
            get { return _userControl; }
            set { _userControl = value; }
        }
        /// <summary>
        /// Container窗体，用来承载属性设置
        /// </summary>
        public  Form.Public_AttributeControl Attribute
        {
            get { return _attribute; }
            set { _attribute = value; }
        }

    }
}
