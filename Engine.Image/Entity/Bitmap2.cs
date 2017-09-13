using System.Drawing;
using System.Windows.Forms;

namespace Engine.Image
{
    /// <summary>
    /// 带名称的BitMap
    /// </summary>
    public class Bitmap2
    {
        string _name, _dec;

        Bitmap _bitmap;

        TreeNode _treeNode;

        public Bitmap2(Bitmap bmp, string name, TreeNode treeNode, string dec = "")
        {
            _bitmap = bmp;
            _name = name;
            _dec = dec;
            _treeNode = treeNode;
        }
        /// <summary>
        /// 选中的树形节点
        /// </summary>
        public TreeNode SelectCurrent
        {
            get { return _treeNode; }
        }
        /// <summary>
        /// bitmap原始数据
        /// </summary>
        public Bitmap BMP
        {
            get { return _bitmap; }
        }
        /// <summary>
        /// 图片名
        /// </summary>
        public string Name
        {
            get { return _name; }
        }
        /// <summary>
        /// 图片描述
        /// </summary>
        public string Dec
        {
            get { return _dec; }
        }
    }
}
