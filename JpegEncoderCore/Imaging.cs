
// @author : Arpan Jati <arpan4017@yahoo.com> | 01 June 2010 
// http://www.codeproject.com/KB/graphics/SimpleJpeg.aspx

using System;
using System.Collections.Generic;
using System.Text;

namespace JpegEncoder
{
    /// <summary>
    /// Generates Y, Cb, Cr, R, G and B values from given RGB_Buffer
    /// </summary>
    public class Imaging
    {
        /// <summary>
        /// Defines the different possible channel types.
        /// </summary>
        public enum ChannelType {Y,Cb,Cr,R,G,B};

        /// <summary>
        /// Generates Y, Cb, Cr, R, G and B values from given RGB_Buffer
        /// </summary>
        /// <param name="RGB_Buffer">The input RGB_Buffer.</param>
        /// <param name="drawInGrayscale">Draw in grayscale.</param>
        /// <param name="width">Width of the image.</param>
        /// <param name="height">Height of the image.</param>
        /// <param name="channel">Enum specifying the channel type required.</param>
        /// <param name="progress">Interface for updating progress.</param>
        /// <param name="operation">Interface for updating current operation.</param>
        /// <returns>3D array of the specified channel type.</returns>
        public static byte[,,] Get_Channel_Data(byte[, ,] RGB_Buffer,bool drawInGrayscale, int width, int height, ChannelType channel, Utils.IProgress progress, Utils.ICurrentOperation operation)
        {
            operation.SetOperation(Utils.CurrentOperation.GetChannelData);

            int fullProgress = width * height;
            byte[, ,] outData = new byte[width, height, 3];

            switch (channel)
            {                    
                case ChannelType.R:

                    if (drawInGrayscale)
                    {
                        for (int i = 0; i < width; i++)
                        {
                            progress.SetProgress(fullProgress, height * i);
                            for (int j = 0; j < height; j++)
                            {
                                outData[i, j, 0] = RGB_Buffer[i, j, 0];
                                outData[i, j, 1] = RGB_Buffer[i, j, 0];
                                outData[i, j, 2] = RGB_Buffer[i, j, 0];
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < width; i++)
                        {
                            progress.SetProgress(fullProgress, height * i);
                            for (int j = 0; j < height; j++)
                            {
                                outData[i, j, 0] = RGB_Buffer[i, j, 0];
                            }
                        }
                    }
                    break;

                case ChannelType.G:

                    if (drawInGrayscale)
                    {
                        for (int i = 0; i < width; i++)
                        {
                            progress.SetProgress(fullProgress, height * i);
                            for (int j = 0; j < height; j++)
                            {
                                outData[i, j, 0] = RGB_Buffer[i, j, 1];
                                outData[i, j, 1] = RGB_Buffer[i, j, 1];
                                outData[i, j, 2] = RGB_Buffer[i, j, 1];
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < width; i++)
                        {
                            progress.SetProgress(fullProgress, height * i);
                            for (int j = 0; j < height; j++)
                            {
                                outData[i, j, 1] = RGB_Buffer[i, j, 1];
                            }
                        }
                    }
                    break;

                case ChannelType.B:

                    if (drawInGrayscale)
                    {
                        for (int i = 0; i < width; i++)
                        {
                            progress.SetProgress(fullProgress, height * i);
                            for (int j = 0; j < height; j++)
                            {
                                outData[i, j, 0] = RGB_Buffer[i, j, 2];
                                outData[i, j, 1] = RGB_Buffer[i, j, 2];
                                outData[i, j, 2] = RGB_Buffer[i, j, 2];
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < width; i++)
                        {
                            progress.SetProgress(fullProgress, height * i);
                            for (int j = 0; j < height; j++)
                            {
                                outData[i, j, 2] = RGB_Buffer[i, j, 2];
                            }
                        }
                    }
                    break;

                case ChannelType.Y:

                    for (int i = 0; i < width; i++)
                    {
                        progress.SetProgress(fullProgress, height * i);
                        for (int j = 0; j < height; j++)
                        { 
                            outData[i, j, 0] = ((Byte)  (((Tables.Y_Red_Table[(RGB_Buffer[i, j, 0])] + Tables.Y_Green_Table[(RGB_Buffer[i, j, 1])] + Tables.Y_Blue_Table[(RGB_Buffer[i, j, 2])]) >> 16) - 128));
                            outData[i, j, 1] = outData[i, j, 0];
                            outData[i, j, 2] = outData[i, j, 0];
                        }
                    } 
                    break;

                case ChannelType.Cb:

                    for (int i = 0; i < width; i++)
                    {
                        progress.SetProgress(fullProgress, height * i);
                        for (int j = 0; j < height; j++)
                        {
                            outData[i, j, 0] = ((Byte)((Tables.Cb_Red_Table[(RGB_Buffer[i, j, 0])] + Tables.Cb_Green_Table[(RGB_Buffer[i, j, 1])] + Tables.Cb_Blue_Table[(RGB_Buffer[i, j, 2])]) >> 16));
                            outData[i, j, 1] = outData[i, j, 0];
                            outData[i, j, 2] = outData[i, j, 0];
                        }
                    }
                    break;

                case ChannelType.Cr:

                    for (int i = 0; i < width; i++)
                    {
                        progress.SetProgress(fullProgress, height * i);
                        for (int j = 0; j < height; j++)
                        {
                            outData[i, j, 0] = ((Byte)((Tables.Cr_Red_Table[(RGB_Buffer[i, j, 0])] + Tables.Cr_Green_Table[(RGB_Buffer[i, j, 1])] + Tables.Cr_Blue_Table[(RGB_Buffer[i, j, 2])]) >> 16));
                            outData[i, j, 1] = outData[i, j, 0];
                            outData[i, j, 2] = outData[i, j, 0];
                        }
                    }
                    break;
            }
            operation.SetOperation(Utils.CurrentOperation.Ready);
            return outData;       
        }        
    }
}
