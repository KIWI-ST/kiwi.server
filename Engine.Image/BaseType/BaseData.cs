using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
/*
 * 黄奎   2012-7-8
 * 基础图像处理类，采用gdal处理图像（读取等）
 */

namespace Engine.Image
{
    /// <summary>
    /// 分波段读取图像
    /// </summary>
    public class RasterBand
    {
        //图像本身有一种默认显示方式，这里是将图层拆散显示
        //波段索引
        private string _layerIndex;
        //波段数据
        private byte[,] _data;
        //
        public string LayerIndex
        {
            get { return _layerIndex; }
        }
        public byte[,] Data
        {
            get { return _data; }
        }
        public RasterBand(string layerIndex, byte[,] data)
        {
            this._layerIndex = layerIndex;
            this._data = data;
        }

        public RasterBand(string layerIndex, byte[] data, int xCount, int yCount)
        {
            this._layerIndex = layerIndex;
            _data = new byte[xCount, yCount];
            for (int count = 0; count < data.Length; count++)
                _data[count % xCount, count / xCount] = data[count];
        }
    }
    /// <summary>
    /// 通用索引器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Container<T>
    {
        T[] array;
        //索引大小
        int _count;
        public Container(int number)
        {
            array = new T[number];
            _count = number;

        }
        /// <summary>
        /// 索引总长度
        /// </summary>
        public int Count
        {
            get { return _count; }
        }
        public T this[int i]
        {
            get
            {
                return array[i];
            }
            set
            {
                array[i] = value;
            }
        }
    }
    /// <summary>
    /// 带名称的BitMap
    /// </summary>
    public class Bitmap2
    {
        /// <summary>
        /// 图标名称
        /// </summary>
        private string _name;
        /// <summary>
        /// 图片
        /// </summary>
        private Bitmap _bitmap;
        /// <summary>
        /// 选中的树形节点
        /// </summary>
        private TreeNode _treeNode;
        public Bitmap2(Bitmap bmp, string dec, TreeNode treeNode)
        {
            this._bitmap = bmp;
            this._name = dec;
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
        public String Name
        {
            get { return _name; }
        }
    }
}
