using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Engine.GIS.GLayer.GRasterLayer;

namespace Host.UI.SettingForm
{
    public partial class BandForm : Form
    {
        /// <summary>
        /// rasterlayer
        /// </summary>
        private GRasterLayer _gdalLayer;

        /// <summary>
        /// 合并波段
        /// </summary>
        public List<int> BanCombineIndex { get; private set; } = new List<int>();

        public BandForm()
        {
            InitializeComponent();
            //
            band_listView.Bounds = new Rectangle(new Point(10, 10), new Size(300, 200));
            // Allow the user to rearrange columns.
            band_listView.AllowColumnReorder = true;
            // Display check boxes.
            band_listView.CheckBoxes = true;
            // Select the item and subitems when selection is made.
            band_listView.FullRowSelect = true;
        }

        public GRasterLayer GdalLayer
        {
            set
            {
                if(_gdalLayer != value)
                {
                    _gdalLayer = value;
                    //update band modal
                    UpdateBands();
                }
            }
            get
            {
                return _gdalLayer;
            }
        }

        private void UpdateBands()
        {
            BanCombineIndex = new List<int>();
            ImageList imageList = new ImageList();
            for(int i = 0; i < _gdalLayer.BandCollection.Count; i++)
            {
                GRasterBand band = _gdalLayer.BandCollection[i];
                ListViewItem lvi = new ListViewItem
                {
                    ImageIndex = i
                };
                lvi.SubItems.Add(band.BandName);
                lvi.SubItems.Add(band.Width + "x" + band.Height);
                band_listView.Items.Add(lvi);
                imageList.Images.Add(band.GrayscaleImage);
            }
            band_listView.SmallImageList = imageList;
            band_listView.LargeImageList = imageList;
        }

        private void ListView_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            //选中时,这里实际返回结果是反向的
            if (e.CurrentValue == CheckState.Unchecked)
                if (!BanCombineIndex.Contains(e.Index))
                    BanCombineIndex.Add(e.Index);
                else
                    return;
            else
                BanCombineIndex.Remove(e.Index);
            //总波段数达到3时，合并图层
            if (BanCombineIndex.Count == 3)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

    }
}
