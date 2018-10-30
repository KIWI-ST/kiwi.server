
// @author : Arpan Jati <arpan4017@yahoo.com> | 01 June 2010 
// http://www.codeproject.com/KB/graphics/SimpleJpeg.aspx

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using JpegEncoder;

namespace EncoderUI
{  
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        byte[,,] image_array;

        Bitmap mainLoadedImage ;

        String InputFileName = "";
        String OutputFileName = "";
        Int64 outputFileLength = 0;

        Point originalDimension = new Point(0, 0);
        Point actualDimension = new Point(0, 0);

        Utils.ProgressUpdater progressObj = new Utils.ProgressUpdater();
        Utils.CurrentOperationUpdater currentOperationObj = new Utils.CurrentOperationUpdater();

        private void Write_Channel_Images(bool WriteYCbCrChannel, Bitmap bmpImage, bool DrawInBW, Utils.IProgress progress, Utils.ICurrentOperation operation)
        { 
            byte[, ,]  y_channel = Imaging.Get_Channel_Data(image_array, DrawInBW, bmpImage.Width, bmpImage.Height, WriteYCbCrChannel ? Imaging.ChannelType.Y : Imaging.ChannelType.R, progress, operation);
            YChPictureBox.Image  = Utils.Write_Bmp_From_Data(y_channel, new Point(bmpImage.Width, bmpImage.Height), progress, operation);

            byte[, ,] cb_channel = Imaging.Get_Channel_Data(image_array, DrawInBW, bmpImage.Width, bmpImage.Height, WriteYCbCrChannel ? Imaging.ChannelType.Cb : Imaging.ChannelType.G, progress, operation);
            CbChPictureBox.Image = Utils.Write_Bmp_From_Data(cb_channel, new Point(bmpImage.Width, bmpImage.Height), progress, operation);

            byte[, ,] cr_channel = Imaging.Get_Channel_Data(image_array, DrawInBW, bmpImage.Width, bmpImage.Height, WriteYCbCrChannel ? Imaging.ChannelType.Cr : Imaging.ChannelType.B, progress, operation);
            CrChPictureBox.Image = Utils.Write_Bmp_From_Data(cr_channel, new Point(bmpImage.Width, bmpImage.Height), progress, operation);
        }

        private void open_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.png;*.gif;*.tiff|BMP Files|*.bmp|JPEG Files|*.jpeg;*.jpg|PNG Files|*.png|GIF Files|*.GIF|All Files|*.*";

            if (DialogResult.OK == ofd.ShowDialog())
            {
                mainPictureBox.Image = null;
                YChPictureBox.Image = null;
                CbChPictureBox.Image = null;
                CrChPictureBox.Image = null;

                InputFileName = ofd.FileName;

                Bitmap bmp = new Bitmap(InputFileName);
                mainLoadedImage = bmp;
                mainPictureBox.Image = bmp;

                originalDimension = new Point(bmp.Width, bmp.Height);

                actualDimension = Utils.GetActualDimension(originalDimension);

                ImageLoadBackgroundWorker.RunWorkerAsync(true);

                ButtonState(true);
            }            
        }  

        private void Form1_Load(object sender, EventArgs e)
        {
            quantizerQualityComboBox.SelectedIndex = 5;
            Tables.Precalculate_YCbCr_Tables(); // For getting tables to convert RGB to YCbCr
            
        }

        private void WriteJpgButton_Click(object sender, EventArgs e)
        { 
            
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.RestoreDirectory = true;
            sfd.Filter = "Jpeg (*.jpg)|*.jpg|All files (*.*)|*.*";

            if (DialogResult.OK == sfd.ShowDialog())
            {
                OutputFileName = sfd.FileName;
                ButtonState(false);
                EncodeBackgroundWorker.RunWorkerAsync(false);              
            }
        }

        private void YChPictureBox_Click(object sender, EventArgs e)
        {
            if (mainPictureBox.Image != null)
            {                
                if (YChPictureBox.Image != null)
                    mainPictureBox.Image = YChPictureBox.Image;
            }
        }

        private void mainPictureBox_Click(object sender, EventArgs e)
        {
            if (mainLoadedImage != null)
                mainPictureBox.Image = mainLoadedImage;
        }

        private void CbChPictureBox_Click(object sender, EventArgs e)
        {
            if (mainPictureBox.Image != null)
            {               
                if (CbChPictureBox.Image != null)
                    mainPictureBox.Image = CbChPictureBox.Image;
            }
        }

        private void CrChPictureBox_Click(object sender, EventArgs e)
        {
            if (mainPictureBox.Image != null)
            {                
                if (CrChPictureBox.Image != null)
                    mainPictureBox.Image = CrChPictureBox.Image;
            }
        }

        private void YCbCrDisplayTypeRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            LoadChannelImages();           
        }

        private void DrawInBWCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (RGBDisplayTypeRadioButton.Checked == true)
            {
                LoadChannelImages();
            }
        }

        private void LoadChannelImages()
        {
            if (mainLoadedImage != null)
            {
                ImageLoadBackgroundWorker.RunWorkerAsync(false);                
            }
            if (YCbCrDisplayTypeRadioButton.Checked)
            {
                Y_Label.Text = "Y Channel";
                Cb_Label.Text = "Cb Channel";
                Cr_Label.Text = "Cr Channel";
            }
            else
            {
                Y_Label.Text = "Red Channel";
                Cb_Label.Text = "Green Channel";
                Cr_Label.Text = "Blue Channel";
            }
        }

        private int getCurretProgress()
        {
            float progress = (float)(((Double)progressObj.Current / (Double)progressObj.Full) * 100.0);

            if (progress <= 100.0 && progress >= 0)
                return (int)progress;
            else return 0;
            
        }

        private void ProgressTimer_Tick(object sender, EventArgs e)
        {
            int progress = getCurretProgress();
            if (EncodeBackgroundWorker.IsBusy || ImageLoadBackgroundWorker.IsBusy)
            {

                switch (currentOperationObj.operation)
                {
                    case Utils.CurrentOperation.EncodeImageBufferToJpg:
                        StatusLabel.Text = "Encoding Jpeg: " + progress + " %";

                        break;

                    case Utils.CurrentOperation.FillImageBuffer:
                        StatusLabel.Text = "Loading Image to Buffer: " + progress + " %";

                        break;

                    case Utils.CurrentOperation.GetChannelData:
                        StatusLabel.Text = "Getting Channel Data: " + progress + " %";

                        break;

                    case Utils.CurrentOperation.InitializingTables:
                        StatusLabel.Text = "Initializing JPEG Tables: " + progress + " %";
                        break;

                    case Utils.CurrentOperation.PrecalculateYCbCrTables:
                        StatusLabel.Text = "Calculating Y, Cb, Cr Tables: " + progress + " %";
                        break;

                    case Utils.CurrentOperation.WritingJPEGHeader:
                        StatusLabel.Text = "Writing Jpeg Header: " + progress + " %";

                        break;

                    case Utils.CurrentOperation.WriteChannelImages:
                        StatusLabel.Text = "Writing Channel Images: " + progress + " %";

                        break;
                }

                if (MainProgressBar.Visible == false) MainProgressBar.Visible = true;                
                MainProgressBar.Value = progress; ;
                
            }
            else
            {
                StatusLabel.Text = "Ready"; 
                MainProgressBar.Visible = false;
            }
        }

        /// <summary>
        /// Gets the luminance and chromiance quantization tables.
        /// </summary>
        /// <param name="IsChromiance">True if chromiance data is required else false.</param>
        /// <returns>Luminance and Chromiance quantization tables.</returns>
        private byte[] Get_Quant_Table_Data(bool IsChromiance)
        {
            byte [] data = new byte[64]; 

            String data_string = IsChromiance ? Chromiance_Data_RichTextBox.Text : Luminance_Data_RichTextBox.Text;
            data_string = data_string.Replace("\n", "");
            String[] separator = { "," }; // Comma separated list.
            String[] data_array = data_string.Split(separator, (char)64, StringSplitOptions.None);
            try
            {
                for (int i = 0; i < 64; i++)
                {
                    data[i] = byte.Parse(data_array[i]);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid " + (IsChromiance ? "Chromiance" : "Luminance") + " Data: "+
                    Environment.NewLine+ 
                    data_string+
                    Environment.NewLine + Environment.NewLine +
                    "Using default tables to encode.",Application.ProductName,MessageBoxButtons.OK,MessageBoxIcon.Error);

                return IsChromiance ? Tables.Standard_Chromiance_Quantization_Table : Tables.Standard_Luminance_Quantization_Table; 
            }
            return data;
        }

        private void EncodeBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {  
            FileStream fs = new FileStream(OutputFileName, FileMode.Create, FileAccess.Write, FileShare.None);
            BinaryWriter bw = new BinaryWriter(fs);

            JpegEncoder.BaseJPEGEncoder encoder = new BaseJPEGEncoder();

            encoder.LuminanceTable = Get_Quant_Table_Data(false);
            encoder.ChromianceTable = Get_Quant_Table_Data(true);            
            
            if ((bool)e.Argument == false)
            {  
                encoder.EncodeImageBufferToJpg(image_array, originalDimension, actualDimension,
                    bw, float.Parse(quantizerQualityComboBox.Text),                    
                    progressObj, currentOperationObj);
            }
            else
            {
                encoder.EncodeImageToJpg(mainPictureBox.Image, bw, 
                    float.Parse(quantizerQualityComboBox.Text),                   
                    progressObj,currentOperationObj);

            }
            fs.Flush();
            outputFileLength = fs.Length;
            fs.Close();      
        }

        private void EncodeBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ButtonState(true);
            MessageBox.Show("Finished encoding to JPEG." + Environment.NewLine + "Output file size: " + (float)(outputFileLength / 1024.0) + " Kilobyte(s)",
                Application.ProductName,MessageBoxButtons.OK,MessageBoxIcon.Information );
        }

        private void ButtonState(bool state)
        {
            SettingsGroupBox.Enabled = state;
            DisplayGroupBox.Enabled = state;
            writeCurrentToJpgToolStripMenuItem.Enabled = state;
            writeJpgToolStripMenuItem.Enabled = state; 
            openToolStripMenuItem.Enabled = state;            
        }

        private void ImageLoadBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            ButtonState(false);            
            
            bool fillBuffer =  (bool) e.Argument;

            if (fillBuffer == true)
            {
                Bitmap bmp = new Bitmap(InputFileName);
                image_array = Utils.Fill_Image_Buffer(bmp, progressObj, currentOperationObj);
            }

            Write_Channel_Images(YCbCrDisplayTypeRadioButton.Checked, mainLoadedImage, DrawInBWCheckBox.Checked,progressObj,currentOperationObj);         

        }

        private void ImageLoadBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ButtonState(true);  
        }

        private void writeCurrentImageButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.RestoreDirectory = true;
            sfd.Filter = "Jpeg (*.jpg)|*.jpg|All files (*.*)|*.*";

            if (DialogResult.OK == sfd.ShowDialog())
            {
                OutputFileName = sfd.FileName;
                ButtonState(false);
                EncodeBackgroundWorker.RunWorkerAsync(true);
            }
        }

        private void helpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            
           
            
            

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox abt = new AboutBox();
            abt.ShowDialog();
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }   
        
    }
}
