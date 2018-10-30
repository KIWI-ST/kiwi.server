
// @author : Arpan Jati <arpan4017@yahoo.com> | 01 June 2010 
// http://www.codeproject.com/KB/graphics/SimpleJpeg.aspx

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace JpegEncoder
{    
    public class JpegHeader
    {
        public class APP0infotype
        {
            UInt16 marker = 0xFFE0;
            UInt16 length = 16; // = 16 for usual JPEG, no thumbnail		
            byte versionhi = 1; // 1
            byte versionlo = 1; // 1
            byte xyunits = 0;   // 0 = no units, normal density
            UInt16 xdensity = 1;  // 1
            UInt16 ydensity = 1;  // 1
            byte thumbnwidth = 0; // 0
            byte thumbnheight = 0; // 0

            public void write_APP0info(BinaryWriter bw)
            {
                Utils.WriteHex(bw, 0xFFD8); // JPEG INIT
                Utils.WriteHex(bw, marker);
                Utils.WriteHex(bw, length);
                bw.Write('J');
                bw.Write('F');
                bw.Write('I');
                bw.Write('F');
                bw.Write((byte)0x0);
                bw.Write(versionhi);
                bw.Write(versionlo);
                bw.Write(xyunits);
                Utils.WriteHex(bw, xdensity);
                Utils.WriteHex(bw, ydensity);
                bw.Write(thumbnheight);
                bw.Write(thumbnwidth);

            }
        } ;

        public class SOF0infotype
        {
            UInt16 marker = 0xFFC0;
            UInt16 length = 17; // = 17 for a truecolor YCbCr JPG
            byte precision = 8;// Should be 8: 8 bits/sample            
            byte nrofcomponents = 3;//Should be 3: We encode a truecolor JPG
            byte IdY = 1;  // = 1
            byte HVY = 0x11; // sampling factors for Y (bit 0-3 vert., 4-7 hor.)
            byte QTY = 0;  // Quantization Table number for Y = 0
            byte IdCb = 2; // = 2
            byte HVCb = 0x11;
            byte QTCb = 1; // 1
            byte IdCr = 3; // = 3
            byte HVCr = 0x11;
            byte QTCr = 1; // Normally equal to QTCb = 1
            public void write_S0FInfo(BinaryWriter bw, int wid, int ht)
            {
                Utils.WriteHex(bw, marker);
                Utils.WriteHex(bw, length);
                bw.Write(precision);
                Utils.WriteHex(bw, ht);
                Utils.WriteHex(bw, wid);
                bw.Write(nrofcomponents);
                bw.Write(IdY);
                bw.Write(HVY);
                bw.Write(QTY);
                bw.Write(IdCb);
                bw.Write(HVCb);
                bw.Write(QTCb);
                bw.Write(IdCr);
                bw.Write(HVCr);
                bw.Write(QTCr);

            }
        };        
        
        public class DQTinfotype
        {
            UInt16 marker = 0xFFDB;
            UInt16 length = 132;  // = 132
            byte QTYinfo = 0;// = 0:  bit 0..3: number of QT = 0 (table for Y)
            //       bit 4..7: precision of QT, 0 = 8 bit		 
            byte QTCbinfo = 1; // = 1 (quantization table for Cb,Cr}             

            public void write_DQT(BinaryWriter bw)
            {
                Utils.WriteHex(bw, marker);
                Utils.WriteHex(bw, length);                
                bw.Write(QTYinfo);
                Utils.WriteByteArray(bw, Tables.Y_Table, 0);
                bw.Write(QTCbinfo);
                Utils.WriteByteArray(bw, Tables.CbCr_Table, 0);
            }
        };        

        public class DHTinfotype
        {
            UInt16 marker = 0xFFC4;
            UInt16 length = 0x01A2;
            byte HTYDCinfo = 0x00; // bit 0..3: number of HT (0..3), for Y =0
            //bit 4  :type of HT, 0 = DC table,1 = AC table
            //bit 5..7: not used, must be 0
            byte[] YDC_nrcodes = Tables.Standard_DC_Luminance_NRCodes; //at index i = nr of codes with length i
            byte[] YDC_values = Tables.Standard_DC_Luminance_Values;
            byte HTYACinfo = 0x10; // = 0x10
            byte[] YAC_nrcodes = Tables.Standard_AC_Luminance_NRCodes;
            byte[] YAC_values = Tables.Standard_AC_Luminance_Values;//we'll use the standard Huffman tables
            byte HTCbDCinfo = 0x01; // = 1
            byte[] CbDC_nrcodes = Tables.Standard_DC_Chromiance_NRCodes;
            byte[] CbDC_values = Tables.Standard_DC_Chromiance_Values;
            byte HTCbACinfo = 0x11; //  = 0x11
            byte[] CbAC_nrcodes = Tables.Standard_AC_Chromiance_NRCodes;
            byte[] CbAC_values = Tables.Standard_AC_Chromiance_Values;
            public void write_DHT(BinaryWriter bw)
            {
                Utils.WriteHex(bw, marker);
                Utils.WriteHex(bw, length);              
                bw.Write(HTYDCinfo);
                Utils.WriteByteArray(bw, YDC_nrcodes, 1);
                Utils.WriteByteArray(bw, YDC_values, 0);
                bw.Write(HTYACinfo);
                Utils.WriteByteArray(bw, YAC_nrcodes, 1);
                Utils.WriteByteArray(bw, YAC_values, 0);
                bw.Write(HTCbDCinfo);
                Utils.WriteByteArray(bw, CbDC_nrcodes, 1);
                Utils.WriteByteArray(bw, CbDC_values, 0);
                bw.Write(HTCbACinfo);
                Utils.WriteByteArray(bw, CbAC_nrcodes, 1);
                Utils.WriteByteArray(bw, CbAC_values, 0);
            }
        };
                
        public class SOSinfotype
        {
            UInt16 marker = 0xFFDA;
            UInt16 length = 12;
            byte nrofcomponents = 3; // Should be 3: truecolor JPG
            byte IdY = 1; 
            byte HTY = 0; // bits 0..3: AC table (0..3)
                              // bits 4..7: DC table (0..3)
            byte IdCb = 2; 
            byte HTCb = 0x11; 
            byte IdCr = 3; 
            byte HTCr = 0x11; 
            byte Ss = 0, Se = 0x3F, Bf = 0; // not interesting, they should be 0,63,0

            public void write_S0S(BinaryWriter bw)
            {
                Utils.WriteHex(bw, marker);
                Utils.WriteHex(bw, length);
                bw.Write(nrofcomponents);
                bw.Write(IdY);
                bw.Write(HTY);
                bw.Write(IdCb);
                bw.Write(HTCb);
                bw.Write(IdCr);
                bw.Write(HTCr);
                bw.Write(Ss);
                bw.Write(Se);
                bw.Write(Bf);
            }
        };

        public static void WriteJpegHeader(BinaryWriter writer, System.Drawing.Point imageDimensions)
        {
            JpegHeader.APP0infotype App0Info = new JpegHeader.APP0infotype();
            JpegHeader.DHTinfotype HuffmanTables = new JpegHeader.DHTinfotype();
            JpegHeader.DQTinfotype QuantizationTables = new JpegHeader.DQTinfotype();
            JpegHeader.SOF0infotype S0F0 = new JpegHeader.SOF0infotype();
            JpegHeader.SOSinfotype S0S = new JpegHeader.SOSinfotype();

            App0Info.write_APP0info(writer);
            QuantizationTables.write_DQT(writer);
            S0F0.write_S0FInfo(writer, imageDimensions.X, imageDimensions.Y);
            HuffmanTables.write_DHT(writer);
            S0S.write_S0S(writer);

        }

    }
}
