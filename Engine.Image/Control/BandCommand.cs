using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

//
using Engine.Image;

namespace Engine.Image.Control
{
    public partial class BandCommand : UserControl
    {
        //与主窗体交互
        private Container<RasterBand> _dataClassContainer;
        //波段合成顺序记录
        private List<int> _bandIndexSave;
        //对话框状态
        private bool _state;
        private Bitmap _bitmap;

        public Bitmap Bitmap
        {
            get { return _bitmap; }
        }
        public bool State
        {
            get { return _state; }
        }

        public BandCommand(Container<RasterBand> dataClassContanier)
        {
            InitializeComponent();
            //
            this._dataClassContainer = dataClassContanier;
            this._bandIndexSave = new List<int>();
            this._state = false;
            //加载
            ItemsAdd();
        }
        private void ItemsAdd()
        {
            listView1.Bounds = new Rectangle(new Point(10, 10), new Size(300, 200));
            // Allow the user to rearrange columns.
            listView1.AllowColumnReorder = true;
            // Display check boxes.
            listView1.CheckBoxes = true;
            // Select the item and subitems when selection is made.
            listView1.FullRowSelect = true;

            //缓存图
            ImageList imageList = new ImageList();
            //
            ListViewItem[] listViewItems = new ListViewItem[_dataClassContainer.Count];

            for (int count = 0; count < _dataClassContainer.Count; count++)
            {
                //count图像索引
                listViewItems[count] = new ListViewItem(_dataClassContainer[count].LayerIndex, count);
                //
                listViewItems[count].SubItems.Add(_dataClassContainer[count].Data.GetLength(0).ToString() + " * " + _dataClassContainer[count].Data.GetLength(1).ToString());
                //bitmap
                imageList.Images.Add(Engine.Image.Analysis.BitmapAndByte.ToGrayBitmap(_dataClassContainer[count].Data, _dataClassContainer[count].Data.GetLength(0), _dataClassContainer[count].Data.GetLength(1)));
            }
            //Add the items to the ListView.
            listView1.Items.AddRange(listViewItems);
            listView1.SmallImageList = imageList;
            listView1.LargeImageList = imageList;
            // Add the ListView to the control collection.
            //this.Controls.Add(listView1);
        }
        //默认选中三个开始波段合成，合成顺序为选中顺序
        private void listView1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            //选中时,这里实际返回结果是反向的
            if (e.CurrentValue == CheckState.Unchecked)
                if (!_bandIndexSave.Contains(e.Index))
                    _bandIndexSave.Add(e.Index);
                else
                    return;
            else
                _bandIndexSave.Remove(e.Index);
            //总波段数达到3时，合并图层
            if (_bandIndexSave.Count == 3)
            {
                _bitmap = BandInsert();
                _state = true;
                ((Form)this.Parent).Close();
            }

        }
        private Bitmap BandInsert()
        {
            return Engine.Image.Analysis.BitmapAndByte.ToRgbBitmap(_dataClassContainer[_bandIndexSave[0]].Data, _dataClassContainer[_bandIndexSave[1]].Data, _dataClassContainer[_bandIndexSave[2]].Data);
        }
    }
}
