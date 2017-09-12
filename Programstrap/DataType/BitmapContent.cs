using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Programstrap
{
    /// <summary>
    /// 带名称的BitMap
    /// </summary>
    public class Bitmap2
    {
        private string _dec;
        private Bitmap _bitmap;
        private TreeNode _treeNode;

        public Bitmap2(Bitmap bmp, string Dec, TreeNode treeNode)
        {
            this._bitmap = bmp;
            this._dec = Dec;
            this._treeNode = treeNode;
        }
        public TreeNode SelectCurrent
        {
            get { return _treeNode; }
        }
        public Bitmap BMP
        {
            get { return _bitmap; }
        }
        public String Dec
        {
            get { return _dec; }
        }
    }
}
