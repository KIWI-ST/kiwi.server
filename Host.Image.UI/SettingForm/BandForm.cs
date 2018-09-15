using Engine.GIS.GLayer.GRasterLayer;
using Engine.GIS.GLayer.GRasterLayer.GBand;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Host.Image.UI.SettingForm
{
    public partial class BandForm : Form
    {
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

        private Engine.GIS.GLayer.GRasterLayer.GRasterLayer _gdalLayer;

        private List<int>_bandIndexSave = new List<int>();

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

        public List<int> BanCombineIndex
        {
            get { return _bandIndexSave; }
        }

        private void UpdateBands()
        {
            _bandIndexSave = new List<int>();
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
                if (!_bandIndexSave.Contains(e.Index))
                    _bandIndexSave.Add(e.Index);
                else
                    return;
            else
                _bandIndexSave.Remove(e.Index);
            //总波段数达到3时，合并图层
            if (_bandIndexSave.Count == 3)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

    }
}
