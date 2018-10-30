
// @author : Arpan Jati <arpan4017@yahoo.com> | 01 June 2010 
// http://www.codeproject.com/KB/graphics/SimpleJpeg.aspx

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Security.Permissions;


namespace JpegEncoder
{    
    public class  BaseJPEGEncoder
    {
        sbyte[] Y_Data = new sbyte[64];
        sbyte[] Cb_Data = new sbyte[64];
        sbyte[] Cr_Data = new sbyte[64];   
       
        private byte[] _luminance_table = Tables.Standard_Luminance_Quantization_Table;
        private byte[] _chromiance_table = Tables.Standard_Chromiance_Quantization_Table;

        /// <summary>
        /// A 64 byte array which corresponds to a JPEG Luminance Quantization table.
        /// </summary>
        public byte[] LuminanceTable
        {
            get { return _luminance_table; }
            set { _luminance_table = value; }
        }

        /// <summary>
        /// A 64 byte array which corresponds to a JPEG Chromiance Quantization table.
        /// </summary>
        public byte[] ChromianceTable
        {
            get { return _chromiance_table; }
            set { _chromiance_table = value; }
        }

        UInt16 [] mask = {1,2,4,8,16,32,64,128,256,512,1024,2048,4096,8192,16384,32768};

        Byte bytenew = 0; 
        SByte bytepos = 7; 

        int Width = 0;
        int Height = 0;

        byte[,,] Bitmap_RGB_Buffer = new byte[1,1,1]; 

        private Int16[] Do_FDCT_Quantization_And_ZigZag(SByte [] channel_data, float[] quant_table)
        {           

	        float tmp0, tmp1, tmp2, tmp3, tmp4, tmp5, tmp6, tmp7;
	        float tmp10, tmp11, tmp12, tmp13;
	        float z1, z2, z3, z4, z5, z11, z13;	
            float[] temp_data =  new float[64]; 
            Int16[] outdata = new Int16[64];
	        float temp;
	        SByte ctr;
	        Byte i;
            int k = 0;

            for (i = 0; i < 64; i++)
            {       
                temp_data[i] = channel_data[i];
            }

	        /* Pass 1: process rows. */	     

	        for (ctr = 7; ctr >= 0; ctr--) 
	        {
                tmp0 = temp_data[0 + k] + temp_data[7 + k];
                tmp7 = temp_data[0 + k] - temp_data[7 + k];
                tmp1 = temp_data[1 + k] + temp_data[6 + k];
                tmp6 = temp_data[1 + k] - temp_data[6 + k];
                tmp2 = temp_data[2 + k] + temp_data[5 + k];
                tmp5 = temp_data[2 + k] - temp_data[5 + k];
                tmp3 = temp_data[3 + k] + temp_data[4 + k];
                tmp4 = temp_data[3 + k] - temp_data[4 + k];

		        /* Even part */

		        tmp10 = tmp0 + tmp3;	/* phase 2 */
		        tmp13 = tmp0 - tmp3;
		        tmp11 = tmp1 + tmp2;
		        tmp12 = tmp1 - tmp2;

                temp_data[0 + k] = tmp10 + tmp11; /* phase 3 */
                temp_data[4 + k] = tmp10 - tmp11;

		        z1 = (tmp12 + tmp13) * ((float) 0.707106781); /* c4 */
                temp_data[2 + k] = tmp13 + z1;	/* phase 5 */
                temp_data[6 + k] = tmp13 - z1;

		        /* Odd part */

		        tmp10 = tmp4 + tmp5;	/* phase 2 */
		        tmp11 = tmp5 + tmp6;
		        tmp12 = tmp6 + tmp7;

		        /* The rotator is modified from fig 4-8 to avoid extra negations. */
		        z5 = (tmp10 - tmp12) * ((float) 0.382683433); /* c6 */
		        z2 = ((float) 0.541196100) * tmp10 + z5; /* c2-c6 */
		        z4 = ((float) 1.306562965) * tmp12 + z5; /* c2+c6 */
		        z3 = tmp11 * ((float) 0.707106781); /* c4 */

		        z11 = tmp7 + z3;		/* phase 5 */
		        z13 = tmp7 - z3;

                temp_data[5 + k] = z13 + z2;	/* phase 6 */
                temp_data[3 + k] = z13 - z2;
                temp_data[1 + k] = z11 + z4;
                temp_data[7 + k] = z11 - z4;	        		

                k += 8;  /* advance pointer to next row */
	        }

          /* Pass 2: process columns. */

            k = 0;
	        
	        for (ctr = 7; ctr >= 0; ctr--) 
	        {
                tmp0 = temp_data[0 + k] + temp_data[56 + k];
                tmp7 = temp_data[0 + k] - temp_data[56 + k];
                tmp1 = temp_data[8 + k] + temp_data[48 + k];
                tmp6 = temp_data[8 + k] - temp_data[48 + k];
                tmp2 = temp_data[16 + k] + temp_data[40 + k];
                tmp5 = temp_data[16 + k] - temp_data[40 + k];
                tmp3 = temp_data[24 + k] + temp_data[32 + k];
                tmp4 = temp_data[24 + k] - temp_data[32 + k];

		        /* Even part */

		        tmp10 = tmp0 + tmp3;	/* phase 2 */
		        tmp13 = tmp0 - tmp3;
		        tmp11 = tmp1 + tmp2;
		        tmp12 = tmp1 - tmp2;

                temp_data[0 + k] = tmp10 + tmp11; /* phase 3 */
                temp_data[32 + k] = tmp10 - tmp11;

		        z1 = (tmp12 + tmp13) * ((float) 0.707106781); /* c4 */
                temp_data[16 + k] = tmp13 + z1; /* phase 5 */
                temp_data[48 + k] = tmp13 - z1;

		        /* Odd part */

		        tmp10 = tmp4 + tmp5;	/* phase 2 */
		        tmp11 = tmp5 + tmp6;
		        tmp12 = tmp6 + tmp7;

		        /* The rotator is modified from fig 4-8 to avoid extra negations. */
		        z5 = (tmp10 - tmp12) * ((float) 0.382683433); /* c6 */
		        z2 = ((float) 0.541196100) * tmp10 + z5; /* c2-c6 */
		        z4 = ((float) 1.306562965) * tmp12 + z5; /* c2+c6 */
		        z3 = tmp11 * ((float) 0.707106781); /* c4 */

		        z11 = tmp7 + z3;		/* phase 5 */
		        z13 = tmp7 - z3;

                temp_data[40 + k] = z13 + z2; /* phase 6 */
                temp_data[24 + k] = z13 - z2;
                temp_data[8  + k] = z11 + z4;
                temp_data[56 + k] = z11 - z4;

		        	
                k++;   /* advance pointer to next column */
	        }

	        // Do Quantization, ZigZag and proper roundoff.
	        for (i = 0; i < 64; i++) 
	        {
                temp = temp_data[i] * quant_table[i];  
		        outdata[Tables.ZigZag[i]] = (Int16) ((Int16)(temp + 16384.5) - 16384);
	        }            

            return outdata;
        }       

        private void Update_Global_Pixel_8_8_Data(int posX, int posY)
        {             
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    byte R = Bitmap_RGB_Buffer[i + posX, j + posY, 0];
                    byte G = Bitmap_RGB_Buffer[i + posX, j + posY, 1];
                    byte B = Bitmap_RGB_Buffer[i + posX, j + posY, 2];

                    /*Y_Data[i, j] = (Byte)(0.299 * R + 0.587 * G + 0.114 * B);
                    Cb_Data[i, j] = (Byte)(-0.1687 * R - 0.3313 * G + 0.5 * B + 128);
                    Cr_Data[i, j] = (Byte)(0.5 * R - 0.4187 * G - 0.0813 * B + 128);  */

                    Y_Data[i + j * 8] = (sbyte)(((Tables.Y_Red_Table[(R)] + Tables.Y_Green_Table[(G)] + Tables.Y_Blue_Table[(B)]) >> 16) - 128);
                    Cb_Data[i + j * 8] = (sbyte)((Tables.Cb_Red_Table[(R)] + Tables.Cb_Green_Table[(G)] + Tables.Cb_Blue_Table[(B)]) >> 16);
                    Cr_Data[i + j * 8] = (sbyte)((Tables.Cr_Red_Table[(R)] + Tables.Cr_Green_Table[(G)] + Tables.Cr_Blue_Table[(B)]) >> 16);
                }
            }            
        }       

        void DoHuffmanEncoding(Int16[] DU, ref Int16 DC, Tables.BitString[] HTDC, Tables.BitString[] HTAC, BinaryWriter bw)
        {
            Tables.BitString EOB = HTAC[0x00];
            Tables.BitString M16zeroes = HTAC[0xF0];
            Byte i;
            Byte startpos;
            Byte end0pos;
            Byte nrzeroes;
            Byte nrmarker;
            Int16 Diff;  

            // Encode DC
            Diff = (Int16)(DU[0] - DC);
            DC = DU[0];

            if (Diff == 0)
                WriteBits(HTDC[0],bw); 
            else
            {
                WriteBits(HTDC[Tables.Category[32767 + Diff]], bw);
                WriteBits(Tables.BitCode[32767 + Diff], bw);
            }

            // Encode ACs
            for (end0pos = 63; (end0pos > 0) && (DU[end0pos] == 0); end0pos--) ;
            //end0pos = first element in reverse order != 0

            i = 1;
            while (i <= end0pos)
            {
                startpos = i;
                for (; (DU[i] == 0) && (i <= end0pos); i++) ;
                nrzeroes = (byte) (i - startpos);
                if (nrzeroes >= 16)
                {
                    for (nrmarker = 1; nrmarker <= nrzeroes / 16; nrmarker++)
                        WriteBits(M16zeroes,bw);
                    nrzeroes = (byte) (nrzeroes % 16);
                }
                WriteBits(HTAC[nrzeroes * 16 + Tables.Category[32767 + DU[i]]], bw);
                WriteBits(Tables.BitCode[32767 + DU[i]], bw);
                i++;
            }

            if (end0pos != 63)
                WriteBits(EOB,bw);
        }

        void WriteBits(Tables.BitString bs, BinaryWriter bw)   
        {
            UInt16 value;
            SByte posval;

            value = bs.value;
            posval = (SByte)(bs.length - 1);
            while (posval >= 0)
            {
                if ((value & mask[posval]) != 0)
                {
                    bytenew = (Byte)(bytenew  | mask[bytepos]);
                }
                posval--;
                bytepos--;
                if (bytepos < 0)
                {
                    // Write to stream
                    if (bytenew == 0xFF)
                    {
                        // Handle special case
                        bw.Write((byte)(0xFF));
                        bw.Write((byte)(0x00));                        
                    }
                    else bw.Write((byte)(bytenew));  
                    // Reinitialize
                    bytepos = 7;
                    bytenew = 0;
                }
            }
        }

        /// <summary>
        /// Encodes a provided ImageBuffer[,,] to a JPG Image.
        /// </summary>
        /// <param name="ImageBuffer">The ImageBuffer containing the pixel data.</param>
        /// <param name="originalDimension">Dimension of the original image. This value is written to the image header.</param>
        /// <param name="actualDimension">Dimension on which the Encoder works. As the Encoder works in 8*8 blocks, if the image size is not divisible by 8 the remaining blocks are set to '0' (in this implementation)</param>
        /// <param name="OutputStream">Stream to which the JPEG data is to be written.</param>
        /// <param name="Quantizer_Quality">Required quantizer quality; Default: 50 , Lower value higher quality.</param>
        /// <param name="progress">Interface for updating Progress.</param>
        /// <param name="currentOperation">Interface for updating CurrentOperation.</param>
        public void EncodeImageBufferToJpg(Byte[, ,] ImageBuffer, Point originalDimension, Point actualDimension, BinaryWriter OutputStream, float Quantizer_Quality, Utils.IProgress progress, Utils.ICurrentOperation currentOperation)
        {
            Width = actualDimension.X;
            Height = actualDimension.Y;

            Bitmap_RGB_Buffer = ImageBuffer;

            UInt16 xpos, ypos;

            currentOperation.SetOperation(Utils.CurrentOperation.InitializingTables);
            Tables.InitializeAllTables(Quantizer_Quality,_luminance_table,_chromiance_table);
            currentOperation.SetOperation(Utils.CurrentOperation.WritingJPEGHeader);
            JpegHeader.WriteJpegHeader(OutputStream, new Point(originalDimension.X, originalDimension.Y));

            Int16 prev_DC_Y = 0;
            Int16 prev_DC_Cb = 0;
            Int16 prev_DC_Cr = 0;

            currentOperation.SetOperation(Utils.CurrentOperation.EncodeImageBufferToJpg);

            for (ypos = 0; ypos < Height; ypos += 8)
            {
                progress.SetProgress(Height * Width, Width * ypos );
                for (xpos = 0; xpos < Width; xpos += 8)
                {
                    Update_Global_Pixel_8_8_Data(xpos, ypos);
                  
                    // Process Y Channel
                    Int16[] DCT_Quant_Y = Do_FDCT_Quantization_And_ZigZag(Y_Data, Tables.FDCT_Y_Quantization_Table);                
                    DoHuffmanEncoding(DCT_Quant_Y, ref prev_DC_Y, Tables.Y_DC_Huffman_Table, Tables.Y_AC_Huffman_Table, OutputStream);

                    // Process Cb Channel
                    Int16[] DCT_Quant_Cb = Do_FDCT_Quantization_And_ZigZag(Cb_Data, Tables.FDCT_CbCr_Quantization_Table);                   
                    DoHuffmanEncoding(DCT_Quant_Cb, ref prev_DC_Cb, Tables.Cb_DC_Huffman_Table, Tables.Cb_AC_Huffman_Table, OutputStream);

                    // Process Cr Channel
                    Int16[] DCT_Quant_Cr = Do_FDCT_Quantization_And_ZigZag(Cr_Data, Tables.FDCT_CbCr_Quantization_Table);                
                    DoHuffmanEncoding(DCT_Quant_Cr, ref prev_DC_Cr, Tables.Cb_DC_Huffman_Table, Tables.Cb_AC_Huffman_Table, OutputStream);
                }
            }
            Utils.WriteHex(OutputStream, 0xFFD9); //Write End of Image Marker    

            currentOperation.SetOperation(Utils.CurrentOperation.Ready);
        }

        /// <summary>
        /// Encodes a provided Image to a JPG Image.
        /// </summary>
        /// <param name="ImageToBeEncoded">The Image to be encoded.</param>
        /// <param name="OutputStream">Stream to which the JPEG data is to be written.</param>
        /// <param name="Quantizer_Quality">Required quantizer quality; Default: 50 , Lower value higher quality.</param>
        /// <param name="progress">Interface for updating Progress.</param>
        /// <param name="currentOperation">Interface for updating CurrentOperation.</param>
        public void EncodeImageToJpg(Image ImageToBeEncoded, BinaryWriter OutputStream, float Quantizer_Quality, Utils.IProgress progress,Utils.ICurrentOperation currentOperation)
        {
            Bitmap b_in = new Bitmap(ImageToBeEncoded);
            Width = b_in.Width;
            Height = b_in.Height;
            Point originalSize =  new Point(b_in.Width, b_in.Height);
            currentOperation.SetOperation(Utils.CurrentOperation.FillImageBuffer);

            Bitmap_RGB_Buffer = Utils.Fill_Image_Buffer(b_in, progress, currentOperation);

            EncodeImageBufferToJpg(Bitmap_RGB_Buffer, originalSize, Utils.GetActualDimension(originalSize), OutputStream, 
                Quantizer_Quality,  progress, currentOperation);            
        }
    }
}
