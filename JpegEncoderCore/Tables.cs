
// @author : Arpan Jati <arpan4017@yahoo.com> | 01 June 2010 
// http://www.codeproject.com/KB/graphics/SimpleJpeg.aspx

using System;
using System.Collections.Generic;
using System.Text;

namespace JpegEncoder
{
    public static class Tables
    {
        public struct BitString
        {
            public Byte length;
            public UInt16 value;

            public BitString(Byte len, UInt16 val)
            {
                length = len;
                value = val;
            }
        };

        public static Byte[] Category = new Byte[65535];
        public static Tables.BitString[] BitCode = new Tables.BitString[65535];       

        public static BitString[] Y_DC_Huffman_Table = new BitString[12];
        public static BitString[] Cb_DC_Huffman_Table = new BitString[12];
        public static BitString[] Y_AC_Huffman_Table = new BitString[256];
        public static BitString[] Cb_AC_Huffman_Table = new BitString[256];

        // Y, Cb, Cr Tables
        public static Int32[] Y_Red_Table    =  new Int32[256];
        public static Int32[] Y_Green_Table  =  new Int32[256];
        public static Int32[] Y_Blue_Table   =  new Int32[256];
        public static Int32[] Cb_Red_Table   =  new Int32[256];
        public static Int32[] Cb_Green_Table =  new Int32[256];
        public static Int32[] Cb_Blue_Table  =  new Int32[256];
        public static Int32[] Cr_Red_Table   =  new Int32[256];
        public static Int32[] Cr_Green_Table =  new Int32[256];
        public static Int32[] Cr_Blue_Table  =  new Int32[256];

        // Quant data tables
        public static Byte[] Y_Table = new Byte[64];
        public static Byte[] CbCr_Table = new Byte[64];

        // Quant data tables after scaling and cos.
        public static float[] FDCT_Y_Quantization_Table = new float[64];
        public static float[] FDCT_CbCr_Quantization_Table = new float[64];

        public static Byte[] Standard_Luminance_Quantization_Table = 
        {
            16,  11,  10,  16,  24,  40,  51,  61,
            12,  12,  14,  19,  26,  58,  60,  55,
            14,  13,  16,  24,  40,  57,  69,  56,
            14,  17,  22,  29,  51,  87,  80,  62,
            18,  22,  37,  56,  68, 109, 103,  77,
            24,  35,  55,  64,  81, 104, 113,  92,
            49,  64,  78,  87, 103, 121, 120, 101,
            72,  92,  95,  98, 112, 100, 103,  99
        };
        public static Byte[] Standard_Chromiance_Quantization_Table = 
        {
            17,  18,  24,  47,  99,  99,  99,  99,
            18,  21,  26,  66,  99,  99,  99,  99,
            24,  26,  56,  99,  99,  99,  99,  99,
            47,  66,  99,  99,  99,  99,  99,  99,
            99,  99,  99,  99,  99,  99,  99,  99,
            99,  99,  99,  99,  99,  99,  99,  99,
            99,  99,  99,  99,  99,  99,  99,  99,
            99,  99,  99,  99,  99,  99,  99,  99
        };

        public static Byte[] Standard_DC_Luminance_NRCodes = { 0, 0, 1, 5, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0 };
        public static Byte[] Standard_DC_Luminance_Values = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };

        public static Byte[] Standard_DC_Chromiance_NRCodes = { 0, 0, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0 };
        public static Byte[] Standard_DC_Chromiance_Values = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };

        public static Byte[] Standard_AC_Luminance_NRCodes = { 0, 0, 2, 1, 3, 3, 2, 4, 3, 5, 5, 4, 4, 0, 0, 1, 0x7d };
        public static Byte[] Standard_AC_Luminance_Values = 
        {
            0x01, 0x02, 0x03, 0x00, 0x04, 0x11, 0x05, 0x12,
            0x21, 0x31, 0x41, 0x06, 0x13, 0x51, 0x61, 0x07,
            0x22, 0x71, 0x14, 0x32, 0x81, 0x91, 0xa1, 0x08,
            0x23, 0x42, 0xb1, 0xc1, 0x15, 0x52, 0xd1, 0xf0,
            0x24, 0x33, 0x62, 0x72, 0x82, 0x09, 0x0a, 0x16,
            0x17, 0x18, 0x19, 0x1a, 0x25, 0x26, 0x27, 0x28,
            0x29, 0x2a, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39,
            0x3a, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49,
            0x4a, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59,
            0x5a, 0x63, 0x64, 0x65, 0x66, 0x67, 0x68, 0x69,
            0x6a, 0x73, 0x74, 0x75, 0x76, 0x77, 0x78, 0x79,
            0x7a, 0x83, 0x84, 0x85, 0x86, 0x87, 0x88, 0x89,
            0x8a, 0x92, 0x93, 0x94, 0x95, 0x96, 0x97, 0x98,
            0x99, 0x9a, 0xa2, 0xa3, 0xa4, 0xa5, 0xa6, 0xa7,
            0xa8, 0xa9, 0xaa, 0xb2, 0xb3, 0xb4, 0xb5, 0xb6,
            0xb7, 0xb8, 0xb9, 0xba, 0xc2, 0xc3, 0xc4, 0xc5,
            0xc6, 0xc7, 0xc8, 0xc9, 0xca, 0xd2, 0xd3, 0xd4,
            0xd5, 0xd6, 0xd7, 0xd8, 0xd9, 0xda, 0xe1, 0xe2,
            0xe3, 0xe4, 0xe5, 0xe6, 0xe7, 0xe8, 0xe9, 0xea,
            0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8,
            0xf9, 0xfa
        };

        public static Byte[] Standard_AC_Chromiance_NRCodes = { 0, 0, 2, 1, 2, 4, 4, 3, 4, 7, 5, 4, 4, 0, 1, 2, 0x77 };
        public static Byte[] Standard_AC_Chromiance_Values =
        {
            0x00, 0x01, 0x02, 0x03, 0x11, 0x04, 0x05, 0x21,
            0x31, 0x06, 0x12, 0x41, 0x51, 0x07, 0x61, 0x71,
            0x13, 0x22, 0x32, 0x81, 0x08, 0x14, 0x42, 0x91,
            0xa1, 0xb1, 0xc1, 0x09, 0x23, 0x33, 0x52, 0xf0,
            0x15, 0x62, 0x72, 0xd1, 0x0a, 0x16, 0x24, 0x34,
            0xe1, 0x25, 0xf1, 0x17, 0x18, 0x19, 0x1a, 0x26,
            0x27, 0x28, 0x29, 0x2a, 0x35, 0x36, 0x37, 0x38,
            0x39, 0x3a, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48,
            0x49, 0x4a, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58,
            0x59, 0x5a, 0x63, 0x64, 0x65, 0x66, 0x67, 0x68,
            0x69, 0x6a, 0x73, 0x74, 0x75, 0x76, 0x77, 0x78,
            0x79, 0x7a, 0x82, 0x83, 0x84, 0x85, 0x86, 0x87,
            0x88, 0x89, 0x8a, 0x92, 0x93, 0x94, 0x95, 0x96,
            0x97, 0x98, 0x99, 0x9a, 0xa2, 0xa3, 0xa4, 0xa5,
            0xa6, 0xa7, 0xa8, 0xa9, 0xaa, 0xb2, 0xb3, 0xb4,
            0xb5, 0xb6, 0xb7, 0xb8, 0xb9, 0xba, 0xc2, 0xc3,
            0xc4, 0xc5, 0xc6, 0xc7, 0xc8, 0xc9, 0xca, 0xd2,
            0xd3, 0xd4, 0xd5, 0xd6, 0xd7, 0xd8, 0xd9, 0xda,
            0xe2, 0xe3, 0xe4, 0xe5, 0xe6, 0xe7, 0xe8, 0xe9,
            0xea, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8,
            0xf9, 0xfa
        };
        
        public static Byte[] ZigZag =
        { 
            0, 1, 5, 6,14,15,27,28,
            2, 4, 7,13,16,26,29,42,
            3, 8,12,17,25,30,41,43,
            9,11,18,24,31,40,44,53,
            10,19,23,32,39,45,52,54,
            20,22,33,38,46,51,55,60,
            21,34,37,47,50,56,59,61,
            35,36,48,49,57,58,62,63 
        };     



        static Byte[] Scale_And_ZigZag_Quantization_Table(Byte[] inTable, float quality_scale)
        {
            Byte[] outTable = new Byte[64];
            long temp;
            for (Byte i = 0; i < 64; i++)
            {
                temp = ((long)(inTable[i] * quality_scale + 50L) / 100L);
                if (temp <= 0L)
                    temp = 1L;
                if (temp > 255L)
                    temp = 255L;
                outTable[Tables.ZigZag[i]] = (Byte)temp;
            }
            return outTable;
        }

        static void Initialize_Quantization_Tables(float scaleFactor, byte[] luminance_table, byte[] chromiance_table)
        {
            Tables.Y_Table = Scale_And_ZigZag_Quantization_Table(luminance_table, scaleFactor);
            Tables.CbCr_Table = Scale_And_ZigZag_Quantization_Table(chromiance_table, scaleFactor);
        }

        static void Compute_Huffman_Table(Byte[] nrCodes, Byte[] std_table, ref Tables.BitString[] Huffman_Table)
        {
            Byte k, j;
            Byte pos_in_table;
            UInt16 code_value;

            code_value = 0;
            pos_in_table = 0;
            for (k = 1; k <= 16; k++)
            {
                for (j = 1; j <= nrCodes[k]; j++)
                {
                    Huffman_Table[std_table[pos_in_table]].value = code_value;
                    Huffman_Table[std_table[pos_in_table]].length = k;
                    pos_in_table++;
                    code_value++;
                }
                code_value <<= 1;
            }            
        }

        static void Initialize_Huffman_Tables()
        {
            // Compute the Huffman tables used for encoding
            Compute_Huffman_Table(Tables.Standard_DC_Luminance_NRCodes, Tables.Standard_DC_Luminance_Values, ref Tables.Y_DC_Huffman_Table);
            Compute_Huffman_Table(Tables.Standard_AC_Luminance_NRCodes, Tables.Standard_AC_Luminance_Values, ref Tables.Y_AC_Huffman_Table);
            Compute_Huffman_Table(Tables.Standard_DC_Chromiance_NRCodes, Tables.Standard_DC_Chromiance_Values, ref Tables.Cb_DC_Huffman_Table);
            Compute_Huffman_Table(Tables.Standard_AC_Chromiance_NRCodes, Tables.Standard_AC_Chromiance_Values, ref Tables.Cb_AC_Huffman_Table);
        }

        static void Initialize_Category_And_Bitcode()
        {
            Int32 nr;
            Int32 nr_lower, nr_upper;
            Byte cat;

            nr_lower = 1;
            nr_upper = 2;
            for (cat = 1; cat <= 15; cat++)
            {
                //Positive numbers
                for (nr = nr_lower; nr < nr_upper; nr++)
                {
                    Category[32767 + nr] = cat;
                    BitCode[32767 + nr].length = cat;
                    BitCode[32767 + nr].value = (ushort)nr;
                }
                //Negative numbers
                for (nr = -(nr_upper - 1); nr <= -nr_lower; nr++)
                {
                    Category[32767 + nr] = cat;
                    BitCode[32767 + nr].length = cat;
                    BitCode[32767 + nr].value = (ushort)(nr_upper - 1 + nr);
                }
                nr_lower <<= 1;
                nr_upper <<= 1;
            }
        }

        static void Initialize_FDCT_Quantization_Tables()
        {
            double[] CosineScaleFactor = { 1.0, 1.387039845, 1.306562965, 1.175875602, 1.0, 0.785694958, 0.541196100, 0.275899379 };
            
            Byte i = 0;

            for (Byte row = 0; row < 8; row++)
            {
                for (Byte col = 0; col < 8; col++)
                {
                    Tables.FDCT_Y_Quantization_Table[i] = (float)(1.0 / ((double)Tables.Y_Table[Tables.ZigZag[i]] *
                        CosineScaleFactor[row] * CosineScaleFactor[col] * 8.0));
                    Tables.FDCT_CbCr_Quantization_Table[i] = (float)(1.0 / ((double)Tables.CbCr_Table[Tables.ZigZag[i]] *
                        CosineScaleFactor[row] * CosineScaleFactor[col] * 8.0));
                    i++;
                }
            }
        }

        public static void Precalculate_YCbCr_Tables()
        {
            UInt16 R, G, B;

            for (R = 0; R < 256; R++)
            {
                Tables.Y_Red_Table[R] = (Int32)((65536 * 0.299 + 0.5) * R);
                Tables.Cb_Red_Table[R] = (Int32)((65536 * -0.16874 + 0.5) * R);
                Tables.Cr_Red_Table[R] = (Int32)((32768) * R);
            }
            for (G = 0; G < 256; G++)
            {
                Tables.Y_Green_Table[G] = (Int32)((65536 * 0.587 + 0.5) * G);
                Tables.Cb_Green_Table[G] = (Int32)((65536 * -0.33126 + 0.5) * G);
                Tables.Cr_Green_Table[G] = (Int32)((65536 * -0.41869 + 0.5) * G);
            }
            for (B = 0; B < 256; B++)
            {
                Tables.Y_Blue_Table[B] = (Int32)((65536 * 0.114 + 0.5) * B);
                Tables.Cb_Blue_Table[B] = (Int32)((32768) * B);
                Tables.Cr_Blue_Table[B] = (Int32)((65536 * -0.08131 + 0.5) * B);
            }
        }

        public static void InitializeAllTables(float quantizer_quality, byte[] luminance_table, byte[] chromiance_table)
        {
            Precalculate_YCbCr_Tables();
            Initialize_Quantization_Tables((float)quantizer_quality,luminance_table,chromiance_table);
            Initialize_Huffman_Tables();
            Initialize_Category_And_Bitcode();
            Initialize_FDCT_Quantization_Tables();
        }                
        
    }

}
