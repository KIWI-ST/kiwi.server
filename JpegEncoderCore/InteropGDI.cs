
// @author : Arpan Jati <arpan4017@yahoo.com> | 01 June 2010 
// http://www.codeproject.com/KB/graphics/SimpleJpeg.aspx

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace JpegEncoder
{
    public class InteropGDI
    {
        /// <summary>
        /// The CreateCompatibleDC function creates a memory device context (DC) compatible with the specified device. 
        /// </summary>
        /// <param name="hdc">[in] Handle to an existing DC. If this handle is NULL, the function creates a memory DC compatible with the application's current screen. </param>
        /// <returns>
        /// If the function succeeds, the return value is the handle to a memory DC.
        /// If the function fails, the return value is NULL. 
        /// </returns>
        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        /// <summary>
        /// The SelectObject function selects an object into the specified device context (DC). 
        /// The new object replaces the previous object of the same type. 
        /// </summary>
        /// <param name="hdc">[in] Handle to the DC.</param>
        /// <param name="hgdiobj">[in] Handle to the object to be selected. The specified object must have been created by using one of the following functions. </param>
        /// <returns></returns>
        [DllImport("gdi32.dll", ExactSpelling = true, PreserveSig = true, SetLastError = true)]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);  

        /// <summary>
        /// The SetStretchBltMode function sets the bitmap stretching mode in the specified device context. 
        /// </summary>
        /// <param name="hdc">[in] Handle to the device context. </param>
        /// <param name="iStretchMode">[in] Specifies the stretching mode. This parameter can be one of the values from StretchBltModes enum.</param>
        /// <returns>
        /// If the function succeeds, the return value is the previous stretching mode.
        /// If the function fails, the return value is zero. 
        /// </returns>
        [DllImport("gdi32.dll")]
        public static extern int SetStretchBltMode(IntPtr hdc, int iStretchMode);

        /// <summary>
        /// The GetObject function retrieves information for the specified graphics object. 
        /// </summary>
        /// <param name="hgdiobj">[in] Handle to the graphics object of interest. This can be a handle to one of the following: a logical bitmap, a brush, a font, a palette, a pen, or a device independent bitmap created by calling the CreateDIBSection function. </param>
        /// <param name="cbBuffer">[in] Specifies the number of bytes of information to be written to the buffer. </param>
        /// <param name="lpvObject">[out] Pointer to a buffer that receives the information about the specified graphics object. </param>
        /// <returns>
        /// If the function succeeds, and lpvObject is a valid pointer, the return value is the number of bytes stored into the buffer.
        /// If the function succeeds, and lpvObject is NULL, the return value is the number of bytes required to hold the information the function would store into the buffer.
        /// If the function fails, the return value is zero. 
        /// </returns>
        [DllImport("gdi32.dll")]
        public static extern int GetObject(IntPtr hgdiobj, int cbBuffer, ref BITMAP lpvObject);

        /// <summary>
        /// The StretchBlt function copies a bitmap from a source rectangle into a destination 
        /// rectangle, stretching or compressing the bitmap to fit the dimensions of the destination 
        /// rectangle, if necessary. The system stretches or compresses the bitmap according to 
        /// the stretching mode currently set in the destination device context. 
        /// </summary>
        /// <param name="hdcDest">[in] Handle to the destination device context. </param>
        /// <param name="nXOriginDest">[in] Specifies the x-coordinate, in logical units, of the upper-left corner of the destination rectangle. </param>
        /// <param name="nYOriginDest">[in] Specifies the y-coordinate, in logical units, of the upper-left corner of the destination rectangle. </param>
        /// <param name="nWidthDest">[in] Specifies the width, in logical units, of the destination rectangle. </param>
        /// <param name="nHeightDest">[in] Specifies the height, in logical units, of the destination rectangle. </param>
        /// <param name="hdcSrc">[in] Handle to the source device context. </param>
        /// <param name="nXOriginSrc">[in] Specifies the x-coordinate, in logical units, of the upper-left corner of the source rectangle. </param>
        /// <param name="nYOriginSrc">[in] Specifies the y-coordinate, in logical units, of the upper-left corner of the source rectangle. </param>
        /// <param name="nWidthSrc">[in] Specifies the width, in logical units, of the source rectangle. </param>
        /// <param name="nHeightSrc">[in] Specifies the height, in logical units, of the source rectangle. </param>
        /// <param name="dwRop">[in] Specifies the raster operation to be performed. Raster operation codes define how the system combines colors in output operations that involve a brush, a source bitmap, and a destination bitmap. </param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero. 
        /// If the function fails, the return value is zero. 
        /// </returns>
        [DllImport("gdi32.dll")]
        public static extern bool StretchBlt(IntPtr hdcDest, int nXOriginDest, int nYOriginDest,
        int nWidthDest, int nHeightDest, IntPtr hdcSrc, int nXOriginSrc, int nYOriginSrc, 
        int nWidthSrc, int nHeightSrc, TernaryRasterOperations dwRop);

        /// <summary>
        /// The CreateCompatibleBitmap function creates a bitmap compatible with the device that is associated with the specified device context. 
        /// </summary>
        /// <param name="hdc">[in] Handle to a device context. </param>
        /// <param name="nWidth">[in] Specifies the bitmap width, in pixels. </param>
        /// <param name="nHeight">[in] Specifies the bitmap height, in pixels. </param>
        /// <returns>
        /// If the function succeeds, the return value is a handle to the compatible bitmap (DDB).
        /// If the function fails, the return value is NULL.
        /// </returns>
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth,
          int nHeight);

        /// <summary>
        /// The GetDIBits function retrieves the bits of the specified compatible bitmap 
        /// and copies them into a buffer as a DIB using the specified format. 
        /// </summary>
        /// <param name="hdc">[in] Handle to the device context. </param>
        /// <param name="hbmp">[in] Handle to the bitmap. This must be a compatible bitmap (DDB). </param>
        /// <param name="uStartScan">[in] Specifies the first scan line to retrieve.</param>
        /// <param name="cScanLines">[in] Specifies the number of scan lines to retrieve.</param>
        /// <param name="lpvBits">[out] Pointer to a buffer to receive the bitmap data. If this parameter is NULL, the function passes the dimensions and format of the bitmap to the BITMAPINFOHEADER structure pointed to by the lpbi parameter.</param>
        /// <param name="lpbmi">[in/out] Pointer to a BITMAPINFOHEADER structure that specifies the desired format for the DIB data. </param>
        /// <param name="uUsage">[in] Specifies the format of the bmiColors member of the BITMAPINFOHEADER structure.</param>
        /// <returns>If the lpvBits parameter is non-NULL and the function succeeds, the return value is the number of scan lines copied from the bitmap.</returns>
        [DllImport("gdi32.dll")]
        public static extern int GetDIBits(IntPtr hdc, IntPtr hbmp, uint uStartScan,
          uint cScanLines, [Out] byte[] lpvBits, ref BITMAPINFOHEADER lpbmi, uint uUsage);

        /// <summary>
        /// The SetDIBits function sets the pixels in a compatible bitmap (DDB) 
        /// using the color data found in the specified DIB . 
        /// </summary>
        /// <param name="hdc">[in] Handle to a device context. </param>
        /// <param name="hbmp">[in] Handle to the compatible bitmap (DDB) that is to be altered using the color data from the specified DIB.</param>
        /// <param name="uStartScan">[in] Specifies the starting scan line for the device-independent color data in the array pointed to by the lpvBits parameter. </param>
        /// <param name="cScanLines">[in] Specifies the number of scan lines found in the array containing device-independent color data. </param>
        /// <param name="lpvBits">[in] Pointer to the DIB color data, stored as an array of bytes. The format of the bitmap values depends on the biBitCount member of the BITMAPINFO structure pointed to by the lpbmi parameter. </param>
        /// <param name="lpbmi">[in] Pointer to a BITMAPINFOHEADER structure that contains information about the DIB. </param>
        /// <param name="fuColorUse">[in] Specifies whether the bmiColors member of the BITMAPINFO structure was provided and, if so, whether bmiColors contains explicit red, green, blue (RGB) values or palette indexes.</param>
        /// <returns>If the function succeeds, the return value is the number of scan lines copied.</returns>
        [DllImport("gdi32.dll")]
        public static extern int SetDIBits(IntPtr hdc, IntPtr hbmp, uint uStartScan, uint
          cScanLines, byte[] lpvBits, [In] ref BITMAPINFOHEADER lpbmi, uint fuColorUse);

        /// <summary>
        /// The GetDC function retrieves a handle to a display device context (DC) 
        /// for the client area of a specified window or for the entire screen.        
        /// </summary>
        /// <param name="hWnd">[in] Handle to the window whose DC is to be retrieved. If this value is NULL, GetDC retrieves the DC for the entire screen. </param>
        /// <returns>If the function succeeds, the return value is a handle to the DC for the specified window's client area. I
        /// If the function fails, the return value is NULL. 
        /// </returns>  
        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hWnd);
       
        /// <summary>
        /// The GetClientRect function retrieves the coordinates of a window's client area.
        /// The client coordinates specify the upper-left and lower-right corners of the client area. 
        /// </summary>
        /// <param name="hWnd">[in] Handle to the window whose client coordinates are to be retrieved.</param>
        /// <param name="lpRect">[out] Pointer to a RECT structure that receives the client coordinates.</param>
        /// <returns>If the function succeeds, the return value is nonzero.</returns>
        [DllImport("user32.dll")]
        static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

        /// <summary>
        ///    Performs a bit-block transfer of the color data corresponding to a
        ///    rectangle of pixels from the specified source device context into
        ///    a destination device context.
        /// </summary>
        /// <param name="hdc">Handle to the destination device context.</param>
        /// <param name="nXDest">The leftmost x-coordinate of the destination rectangle (in pixels).</param>
        /// <param name="nYDest">The topmost y-coordinate of the destination rectangle (in pixels).</param>
        /// <param name="nWidth">The width of the source and destination rectangles (in pixels).</param>
        /// <param name="nHeight">The height of the source and the destination rectangles (in pixels).</param>
        /// <param name="hdcSrc">Handle to the source device context.</param>
        /// <param name="nXSrc">The leftmost x-coordinate of the source rectangle (in pixels).</param>
        /// <param name="nYSrc">The topmost y-coordinate of the source rectangle (in pixels).</param>
        /// <param name="dwRop">A raster-operation code.</param>
        /// <returns>
        ///    <c>true</c> if the operation succeeded, <c>false</c> otherwise.
        /// </returns>
        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BitBlt(IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, TernaryRasterOperations dwRop);

        /// <summary>
        /// The DeleteObject function deletes a logical pen, brush, font, bitmap, region, or palette, 
        /// freeing all system resources associated with the object. After the object is deleted, 
        /// the specified handle is no longer valid. 
        /// </summary>
        /// <param name="hObject">[in] Handle to a logical pen, brush, font, bitmap, region, or palette.</param>
        /// <returns>If the function succeeds, the return value is nonzero.</returns>
        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        /// <summary>
        /// The ReleaseDC function releases a device context (DC), freeing it for use by other applications. 
        /// The effect of the ReleaseDC function depends on the type of DC.
        /// </summary>
        /// <param name="hWnd">[in] Handle to the window whose DC is to be released. </param>
        /// <param name="hDC">[in] Handle to the DC to be released. </param>
        /// <returns>
        /// The return value indicates whether the DC was released. 
        /// If the DC was released, the return value is 1.
        /// If the DC was not released, the return value is zero.
        /// </returns>
        [DllImport("user32.dll", EntryPoint = "ReleaseDC", SetLastError = true)]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        /// <summary>
        /// The SetPixel function sets the pixel at the specified coordinates to the specified color. 
        /// </summary>
        /// <param name="hdc">[in] Handle to the device context. </param>
        /// <param name="X">[in] Specifies the x-coordinate, in logical units, of the point to be set. </param>
        /// <param name="Y">[in] Specifies the y-coordinate, in logical units, of the point to be set. </param>
        /// <param name="crColor">[in] Specifies the color to be used to paint the point.</param>
        /// <returns>If the function succeeds, the return value is the RGB value that the function sets the pixel to. 
        /// This value may differ from the color specified by crColor; that occurs when an exact match for the 
        /// specified color cannot be found.</returns>
        [DllImport("gdi32.dll")]
        public static extern uint SetPixel(IntPtr hdc, int X, int Y, uint crColor);                
        
        /// <summary>
        ///     Specifies a raster-operation code. These codes define how the color data for the
        ///     source rectangle is to be combined with the color data for the destination
        ///     rectangle to achieve the final color.
        /// </summary>
        public enum TernaryRasterOperations : uint
        {
            /// <summary>dest = source</summary>
            SRCCOPY = 0x00CC0020,
            /// <summary>dest = source OR dest</summary>
            SRCPAINT = 0x00EE0086,
            /// <summary>dest = source AND dest</summary>
            SRCAND = 0x008800C6,
            /// <summary>dest = source XOR dest</summary>
            SRCINVERT = 0x00660046,
            /// <summary>dest = source AND (NOT dest)</summary>
            SRCERASE = 0x00440328,
            /// <summary>dest = (NOT source)</summary>
            NOTSRCCOPY = 0x00330008,
            /// <summary>dest = (NOT src) AND (NOT dest)</summary>
            NOTSRCERASE = 0x001100A6,
            /// <summary>dest = (source AND pattern)</summary>
            MERGECOPY = 0x00C000CA,
            /// <summary>dest = (NOT source) OR dest</summary>
            MERGEPAINT = 0x00BB0226,
            /// <summary>dest = pattern</summary>
            PATCOPY = 0x00F00021,
            /// <summary>dest = DPSnoo</summary>
            PATPAINT = 0x00FB0A09,
            /// <summary>dest = pattern XOR dest</summary>
            PATINVERT = 0x005A0049,
            /// <summary>dest = (NOT dest)</summary>
            DSTINVERT = 0x00550009,
            /// <summary>dest = BLACK</summary>
            BLACKNESS = 0x00000042,
            /// <summary>dest = WHITE</summary>
            WHITENESS = 0x00FF0062
        }

        public enum StretchBltModes { BLACKONWHITE = 1, WHITEONBLACK = 2, COLORONCOLOR = 3, HALFTONE = 4, MAXSTRETCHBLTMODE = 4 };

        public enum DIB_COLORS { DIB_RGB_COLORS = 0, DIB_PAL_COLORS = 1 };

        public enum BMP_Compression_Modes { BI_RGB = 0, BI_RLE8 = 1, BI_RLE4 = 2, BI_BITFIELDS = 3, BI_JPEG = 4, BI_PNG = 5 };

        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct BITMAPINFOHEADER : IEquatable<BITMAPINFOHEADER>
        {            
            public uint biSize;           
            public int biWidth;            
            public int biHeight;
            public ushort biPlanes;            
            public ushort biBitCount;            
            public uint biCompression;
            public uint biSizeImage;
            public int biXPelsPerMeter;
            public int biYPelsPerMeter;
            public uint biClrUsed;
            public uint biClrImportant;

            public static bool operator == (BITMAPINFOHEADER left, BITMAPINFOHEADER right)
            {
                return ((left.biSize == right.biSize) &&
                        (left.biWidth == right.biWidth) &&
                        (left.biHeight == right.biHeight) &&
                        (left.biPlanes == right.biPlanes) &&
                        (left.biBitCount == right.biBitCount) &&
                        (left.biCompression == right.biCompression) &&
                        (left.biSizeImage == right.biSizeImage) &&
                        (left.biXPelsPerMeter == right.biXPelsPerMeter) &&
                        (left.biYPelsPerMeter == right.biYPelsPerMeter) &&
                        (left.biClrUsed == right.biClrUsed) &&
                        (left.biClrImportant == right.biClrImportant));
            }

            public static bool operator !=(BITMAPINFOHEADER left, BITMAPINFOHEADER right)
            {
                return !(left == right);
            }
            public bool Equals(BITMAPINFOHEADER other)
            {
                return (this == other);
            }
            public override bool Equals(object obj)
            {
                return ((obj is BITMAPINFOHEADER) && (this == (BITMAPINFOHEADER)obj));
            }
            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct RGBQUAD
        {
            public byte rgbBlue;
            public byte rgbGreen;
            public byte rgbRed;
            public byte rgbReserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct BITMAPINFO
        {
            public BITMAPINFOHEADER bmiHeader;
            public RGBQUAD bmiColors;
        }  

        [StructLayout(LayoutKind.Sequential)]
        public struct BITMAP
        {
            public int bmType;
            public int bmWidth;
            public int bmHeight;
            public int bmWidthBytes;
            public ushort bmPlanes;
            public ushort bmBitsPixel;
            public IntPtr bmBits;
        }

        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct BITMAPFILEHEADER
        {
            public ushort bfType;
            public ulong bfSize;
            public ushort bfReserved1;
            public ushort bfReserved2;
            public ulong bfOffBits;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct COLORREF
        {
            public uint ColorDWORD;

            public COLORREF(System.Drawing.Color color)
            {
                ColorDWORD = (uint)color.R + (((uint)color.G) << 8) + (((uint)color.B) << 16);
            }

            public System.Drawing.Color GetColor()
            {
                return System.Drawing.Color.FromArgb((int)(0x000000FFU & ColorDWORD),
               (int)(0x0000FF00U & ColorDWORD) >> 8, (int)(0x00FF0000U & ColorDWORD) >> 16);
            }

            public void SetColor(System.Drawing.Color color)
            {
                ColorDWORD = (uint)color.R + (((uint)color.G) << 8) + (((uint)color.B) << 16);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public RECT(int left, int top, int right, int bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }

            public RECT(Rectangle rectangle)
            {
                Left = rectangle.X;
                Top = rectangle.Y;
                Right = rectangle.Right;
                Bottom = rectangle.Bottom;
            }

            public Rectangle ToRectangle()
            {
                return new Rectangle(Left, Top, Right - Left, Bottom - Top);
            }

            public override string ToString()
            {
                return "Left: " + Left + ", " + "Top: " + Top + ", Right: " + Right + ", Bottom: " + Bottom;
            }
        }

        public static RECT GetClientRect(IntPtr hWnd)
        {
            RECT result = new RECT();
            GetClientRect(hWnd, out result);
            return result;
        }        

        public static int MakeCOLORREF(byte Red, byte Green, byte Blue)
        {
            return (int)(((uint)Red) | (((uint)Green) << 8) | (((uint)Blue) << 16));
        }


    }
}
