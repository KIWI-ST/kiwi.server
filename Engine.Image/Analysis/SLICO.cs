// SLICO.cs: implementation of the SLICO class.
//===========================================================================
// This code is a CSharp implemention of the zero parameter superpixel segmentation 
// (SLICO) technique described in: 
//
//
// "SLIC Superpixels Compared to State-of-the-art Superpixel Methods"
//
// Radhakrishna Achanta, Appu Shaji, Kevin Smith, Aurelien Lucchi, Pascal Fua,
// and Sabine Susstrunk,
//
// IEEE TPAMI, Volume 34, Issue 11, Pages 2274-2282, November 2012.
//
//
// This code is distributed in the hope that it will be useful, but Without
// ANY WARRANTY.
//
//===========================================================================
// Copyright (c) 2015 Junjie (Jason) Zhang.
// Reference: https://github.com/junjiez/SLICOSharp/blob/master/SLICOSuperpixels/SLICO.cs
// Email: junjiezhung@gmail.com
//===========================================================================

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;

namespace Engine.Image.Analysis
{
    public class SLICO
    {
        /// <summary>
        /// PerformSLICO_ForGivenK
        /// 
        /// Overloaded. Runs SLICO on an input Bitmap directed, and returns a bitmap
        /// with segment contour drawed with specified color.
        /// </summary>
        /// <param name="imgBitmap">Input Bitmap</param>
        /// <param name="klabels">SLICO segment label vectors of all pixels</param>
        /// <param name="numlabels">number of labels</param>
        /// <param name="K">required number of superpixels</param>
        /// <param name="color">Color to draw the contour</param>
        /// <param name="m">number of kmeans iterations</param>
        /// <returns>bitmap with segment contour drawed with specified color</returns>
        public Bitmap PerformSLICO_ForGivenK(ref Bitmap imgBitmap,
                                                out int[] klabels,
                                                out int numlabels,
                                                int K,
                                                Color color,
                                                int m = 10)
        {
            PerformSLICO_ForGivenK(ref imgBitmap, out klabels, out numlabels, K, m);
            Bitmap imgBitmapDes = DrawContourToBitmap(imgBitmap, ref klabels, color);
            return imgBitmapDes;
        }

        /// <summary>
        /// PerformSLICO_ForGivenK
        /// 
        /// Overloaded. Runs SLICO on an input Bitmap directed.
        /// </summary>
        /// <param name="imgBitmap">Input Bitmap</param>
        /// <param name="klabels">SLICO segment label vectors of all pixels</param>
        /// <param name="numlabels">number of labels</param>
        /// <param name="K">required number of superpixels</param>
        /// <param name="m">number of kmeans iterations</param>
        public void PerformSLICO_ForGivenK(ref Bitmap imgBitmap,
                                            out int[] klabels,
                                            out int numlabels,
                                            int K,
                                            int m = 10)
        {
            byte[] imgBuff = InitializeImageBuffer(imgBitmap);
            PerformSLICO_ForGivenK(ref imgBuff, m_width, m_height, m_depth, out klabels, out numlabels, K, m);
        }

        /// <summary>
        /// PerformSLICO_ForGivenK
        /// 
        /// Zero parameter SLIC algorithm for a given number L of superpixels
        /// 
        /// This overload required input a Bitmap = byte[] imgBuff + width + height + depth. 
        /// </summary>
        /// <param name="imgBuff">image buffer in byte[], get from BitmapData</param>
        /// <param name="width">width of the image</param>
        /// <param name="height">height of the image</param>
        /// <param name="depth">depth of the image</param>
        /// <param name="klabels">label vectors of all pixels</param>
        /// <param name="numlabels">number of labels</param>
        /// <param name="K">required number of superpixels</param>
        /// <param name="m">number of kmeans iterations</param>
        public void PerformSLICO_ForGivenK(ref byte[] imgBuff,
                                            int width,
                                            int height,
                                            int depth,
                                            out int[] klabels,
                                            out int numlabels,
                                            int K, // required number of superpixel
                                            int m = 10) //number of kmeans iterations, default to be 10
        {
            List<double> kseedsl = new List<double>();
            List<double> kseedsa = new List<double>();
            List<double> kseedsb = new List<double>();
            List<double> kseedsx = new List<double>();
            List<double> kseedsy = new List<double>();

            // ---------------------------------------------------------
            m_width = width;
            m_height = height;
            m_depth = depth;

            int sz = m_width * m_height;
            // ---------------------------------------------------------
            klabels = new int[sz];
            for (int s = 0; s < sz; s++) klabels[s] = -1;

            if (!_bInitialized) // RGB
            {
                DoRGB2LABConversion(ref imgBuff, out m_lvec, out m_avec, out m_bvec);
                _bLABUpdated = true;
                _bInitialized = false;
            }
            else if (_bInitialized) // image reloaded
            {
                DoRGB2LABConversion(ref imgBuff, out m_lvec, out m_avec, out m_bvec);
                _bLABUpdated = true;
            }

            bool perturbseeds = true;
            List<double> edgemag = new List<double>(new double[sz]);
            if (perturbseeds)
                DetectLabEdges(ref m_lvec, ref m_avec, ref m_bvec, m_width, m_height, ref edgemag);
            GetLABXYSeeds_ForGivenK(ref kseedsl, ref kseedsa, ref kseedsb, ref kseedsx, ref kseedsy, K, perturbseeds, ref edgemag);

            int STEP = (int)(Math.Sqrt((double)sz / (double)K) + 2.0);
            PerformSuperpixelSegmentation_VariableSandM(ref kseedsl, ref kseedsa, ref kseedsb, ref kseedsx, ref kseedsy,
                ref klabels, STEP, m);
            numlabels = kseedsl.Count();

            int[] nlabels = new int[sz];
            EnforceLabelConnectivity(ref klabels, m_width, m_height, ref nlabels, numlabels, K);
            for (int i = 0; i < sz; i++) klabels[i] = nlabels[i];

        }

        /// <summary>
        /// DrawContourToBitmap
        /// 
        /// Draw segmentation contour to a Bitmap identical to the original Bitmap
        /// </summary>
        /// <param name="imgBitmapOri">Original Bitmap</param>
        /// <param name="klabels">SLICO segment label vector of all pixels</param>
        /// <param name="color">Color to draw the contour</param>
        /// <returns></returns>
        public Bitmap DrawContourToBitmap(
                    Bitmap imgBitmapOri,
                    ref int[] klabels,
                    Color color)
        {
            bool[] contlabels;
            this.DrawContoursAroundSegments(ref klabels, m_width, m_height, out contlabels);

            Bitmap imgBitmapDes = DrawContourToBitmapFormat24bppRgb(imgBitmapOri, ref contlabels, color);
            return imgBitmapDes;
        }

        // 01 - Color conversion methods
        // Provide methods for converting RGB to CIELAB color space
        //
        #region COLOR CONVERSION METHODS
        /// <summary>
        /// RGB2XYZ
        /// 
        /// sRGB (D65 illuninant assumption) to XYZ conversion
        /// </summary>
        /// <param name="sR">sR (0-255)</param>
        /// <param name="sG">sG (0-255)</param>
        /// <param name="sB">sB (0-255)</param>
        /// <param name="X">X</param>
        /// <param name="Y">Y</param>
        /// <param name="Z">Z</param>
        private void RGB2XYZ(ref int sR, ref int sG, ref int sB, ref double X, ref double Y, ref double Z)
        {
            double R = sR / 255.0;
            double G = sG / 255.0;
            double B = sB / 255.0;

            double r, g, b;

            if (R <= 0.04045) r = R / 12.92;
            else r = Math.Pow((R + 0.055) / 1.055, 2.4);
            if (G <= 0.04045) g = G / 12.92;
            else g = Math.Pow((G + 0.055) / 1.055, 2.4);
            if (B <= 0.04045) b = B / 12.92;
            else b = Math.Pow((B + 0.055) / 1.055, 2.4);

            X = r * 0.4124564 + g * 0.3575761 + b * 0.1804375;
            Y = r * 0.2126729 + g * 0.7151522 + b * 0.0721750;
            Z = r * 0.0193339 + g * 0.1191920 + b * 0.9503041;
        }

        /// <summary>
        /// RGB2LAB
        /// 
        /// RGB -> XYZ -> LAB
        /// </summary>
        /// <param name="sR">sR (0-255)</param>
        /// <param name="sG">sG (0-255)</param>
        /// <param name="sB">sB (0-255)</param>
        /// <param name="lval">L value</param>
        /// <param name="aval">a value</param>
        /// <param name="baval">b value</param>
        private void RGB2LAB(ref int sR, ref int sG, ref int sB, ref double lval, ref double aval, ref double bval)
        {
            // sRGB to XYZ conversion
            double X, Y, Z;
            X = Y = Z = 0;
            RGB2XYZ(ref sR, ref sG, ref sB, ref X, ref Y, ref Z);

            // XYZ to LAB conversion
            double epsilon = 0.008856;	//actual CIE standard
            double kappa = 903.3;		//actual CIE standard

            double Xr = 0.950456;	//reference white
            double Yr = 1.0;		//reference white
            double Zr = 1.088754;	//reference white

            double xr = X / Xr;
            double yr = Y / Yr;
            double zr = Z / Zr;

            double fx, fy, fz;
            if (xr > epsilon) fx = Math.Pow(xr, 1.0 / 3.0);
            else fx = (kappa * xr + 16.0) / 116.0;
            if (yr > epsilon) fy = Math.Pow(yr, 1.0 / 3.0);
            else fy = (kappa * yr + 16.0) / 116.0;
            if (zr > epsilon) fz = Math.Pow(zr, 1.0 / 3.0);
            else fz = (kappa * zr + 16.0) / 116.0;

            lval = 116.0 * fy - 16.0;
            aval = 500.0 * (fx - fy);
            bval = 200.0 * (fy - fz);
        }

        /// <summary>
        /// DoRGB2LABConversion
        /// 
        /// For pixels in whole image and store them in L, A, B vectors
        /// </summary>
        /// <param name="imgBuff">image buffer in byte[], get from BitmapData</param>
        /// <param name="lvec">L vector</param>
        /// <param name="avec">A vector</param>
        /// <param name="bvec">B vector</param>
        private void DoRGB2LABConversion(ref byte[] imgBuff, out double[] lvec, out double[] avec, out double[] bvec)
        {
            // initialize LAB vector with repect to pixelCount
            int pixelCount = m_width * m_height;
            lvec = new double[pixelCount];
            avec = new double[pixelCount];
            bvec = new double[pixelCount];

            // fether RGB from imgBuff and convert
            for (int j = 0; j < pixelCount; j++)
            {
                int pixelPosIndex = m_BytePerPixel * j;
                int r, g, b;

                if (m_depth == 8)
                {
                    r = g = b = imgBuff[pixelPosIndex];
                }
                else if (m_depth == 24 || m_depth == 32)
                {
                    r = imgBuff[pixelPosIndex + 2];
                    g = imgBuff[pixelPosIndex + 1];
                    b = imgBuff[pixelPosIndex];
                }
                else
                {
                    throw new ArgumentException("The bbp of the image should be 8, 24 or 32!");
                }

                // convert RGB of the pixel to lab
                RGB2LAB(ref r, ref g, ref b, ref lvec[j], ref avec[j], ref bvec[j]);
            }
        }
        #endregion

        // 02 - Core SLICO Functions
        // 
        #region SLICO FUNCTIONS
        private void DetectLabEdges(ref double[] lvec, ref double[] avec, ref double[] bvec, int width, int height, ref List<double> edgemag)
        {
            int sz = width * height;

            //edgemag = new List<double>(sz);

            for (int j = 1; j < height - 1; j++)
            {
                for (int k = 1; k < width - 1; k++)
                {
                    int i = j * width + k;

                    double dx = (lvec[i - 1] - lvec[i + 1]) * (lvec[i - 1] - lvec[i + 1]) +
                                (avec[i - 1] - avec[i + 1]) * (avec[i - 1] - avec[i + 1]) +
                                (bvec[i - 1] - bvec[i + 1]) * (bvec[i - 1] - bvec[i + 1]);

                    double dy = (lvec[i - width] - lvec[i + width]) * (lvec[i - width] - lvec[i + width]) +
                                (avec[i - width] - avec[i + width]) * (avec[i - width] - avec[i + width]) +
                                (bvec[i - width] - bvec[i + width]) * (bvec[i - width] - bvec[i + width]);

                    //edges[i] = sqrt(dx) + sqrt(dy);
                    edgemag[i] = dx + dy;

                }
            }

        }

        private void GetLABXYSeeds_ForGivenK(ref List<double> kseedsl,
                                             ref List<double> kseedsa,
                                             ref List<double> kseedsb,
                                             ref List<double> kseedsx,
                                             ref List<double> kseedsy,
                                             int K,
                                             bool perturbseeds,
                                             ref List<double> edgemag)
        {
            int sz = m_width * m_height;
            double step = Math.Sqrt((double)sz / (double)K);
            int T = (int)step;
            int xoff = (int)(step / 2);
            int yoff = (int)(step / 2);

            int n = 0;
            int r = 0;
            for (int y = 0; y < m_height; y++)
            {
                int Y = (int)(y * step + yoff);
                if (Y > m_height - 1) break;

                for (int x = 0; x < m_width; x++)
                {
                    //int X = x*step + xoff; //square grid
                    int X = (int)(x * step + (xoff << (r & 0x1))); //hex grid
                    if (X > m_width - 1) break;

                    int i = Y * m_width + X;

                    kseedsl.Add(m_lvec[i]);
                    kseedsa.Add(m_avec[i]);
                    kseedsb.Add(m_bvec[i]);
                    kseedsx.Add(X);
                    kseedsy.Add(Y);

                    n++;
                }
                r++;
            }

            if (perturbseeds)
            {
                PerturbSeeds(ref kseedsl, ref kseedsa, ref kseedsb, ref kseedsx, ref kseedsy, ref edgemag);
            }
        }

        private void PerturbSeeds(ref List<double> kseedsl,
                                  ref List<double> kseedsa,
                                  ref List<double> kseedsb,
                                  ref List<double> kseedsx,
                                  ref List<double> kseedsy,
                                  ref List<double> edgemag)
        {
            int[] dx8 = { -1, -1, 0, 1, 1, 1, 0, -1 };
            int[] dy8 = { 0, -1, -1, -1, 0, 1, 1, 1 };

            int numseeds = kseedsl.Count();

            for (int n = 0; n < numseeds; n++)
            {
                int ox = (int)kseedsx[n]; // original x
                int oy = (int)kseedsy[n]; // original y
                int oind = oy * m_width + ox;

                int storeind = oind;
                for (int i = 0; i < 8; i++)
                {
                    int nx = ox + dx8[i]; //new x
                    int ny = oy + dy8[i]; //new y

                    if (nx >= 0 && nx < m_width && ny >= 0 && ny < m_height)
                    {
                        int nind = ny * m_width + nx;
                        if (edgemag[nind] < edgemag[storeind])
                        {
                            storeind = nind;
                        }
                    }
                }
                if (storeind != oind)
                {
                    kseedsx[n] = storeind % m_width;
                    kseedsy[n] = (int)(storeind / m_width);
                    kseedsl[n] = m_lvec[storeind];
                    kseedsa[n] = m_avec[storeind];
                    kseedsb[n] = m_bvec[storeind];
                }
            }
        }

        /// <summary>
        /// PerformSuperpixelSegmentation_VariableSandM
        /// 
        /// SLICO - Magic SLIC, no parameters
        /// 
        /// Performs k means segmentation. It is fast because it looks locally, 
        /// not over the entire image.
        /// This function picks the maximum value of color distance as compact factor M
        /// and maximum pixel distance as grid step size S from each cluster.
        /// So no need to input a constant value of M and S.
        /// 
        /// There are two clear advantages:
        /// 1. The algorithm now better handles both textured and non-textured regions
        /// 2. There is no need to set any parameters!!
        /// 
        /// SLICO dynamically varies only the on the compactness factor M, not the step size S.
        /// </summary>
        /// <param name="kseedsl">seed vector of L. Passed by reference.</param>
        /// <param name="kseedsa">seed vector of A. Passed by reference.</param>
        /// <param name="kseedsb">seed vector of B. Passed by reference.</param>
        /// <param name="kseedsx">seed vector of X. Passed by reference.</param>
        /// <param name="kseedsy">seed vector of Y. Passed by reference.</param>
        /// <param name="klabels">The SLICO produced label vector.</param>
        /// <param name="STEP">K-means local searching step size. Calcuated based on size of the image and number of desired superpixels.</param>
        /// <param name="NUMITR">Number of iterations, excuting the k-means. Suggested as 10.</param>
        private void PerformSuperpixelSegmentation_VariableSandM(
            ref List<double> kseedsl,
            ref List<double> kseedsa,
            ref List<double> kseedsb,
            ref List<double> kseedsx,
            ref List<double> kseedsy,
            ref int[] klabels,
            int STEP,
            int NUMITR)
        {
            int sz = m_width * m_height;
            int numk = kseedsl.Count();

            int numitr = 0;

            //---------------
            int offset = STEP;
            if (STEP < 10) offset = (int)(STEP * 1.5);
            //---------------

            List<double> sigmal = new List<double>(new double[numk]);
            List<double> sigmaa = new List<double>(new double[numk]);
            List<double> sigmab = new List<double>(new double[numk]);
            List<double> sigmax = new List<double>(new double[numk]);
            List<double> sigmay = new List<double>(new double[numk]);
            List<int> clustersize = new List<int>(new int[numk]);
            List<double> inv = new List<double>(new double[numk]); // to store 1/clustersize[k] values

            double[] DBL_MAX_ARR = Enumerable.Repeat(DBL_MAX, sz).ToArray();
            List<double> distxy = new List<double>(DBL_MAX_ARR);
            List<double> distlab = new List<double>(DBL_MAX_ARR);
            List<double> distvec = new List<double>(DBL_MAX_ARR);

            double[] MAX_LAB = Enumerable.Repeat((double)10 * 10, numk).ToArray();
            List<double> maxlab = new List<double>(MAX_LAB); // This is the variable value of M, just start with 10

            double[] MAX_XY = Enumerable.Repeat((double)STEP * STEP, numk).ToArray();
            List<double> maxxy = new List<double>(MAX_XY); // This is the variable value of M, just start with STEP

            double invxywt = 1.0 / (STEP * STEP); // Note: this is different from how ususal SLIC/LKM works

            while (numitr < NUMITR)
            {
                //-------
                //cumerr = 0;
                numitr++;
                //-------

                distvec = new List<double>(DBL_MAX_ARR);
                for (int n = 0; n < numk; n++)
                {
                    int y1 = (int)Math.Max(0, kseedsy[n] - offset);
                    int y2 = (int)Math.Min(m_height, kseedsy[n] + offset);
                    int x1 = (int)Math.Max(0, kseedsx[n] - offset);
                    int x2 = (int)Math.Min(m_width, kseedsx[n] + offset);

                    for (int y = y1; y < y2; y++)
                    {
                        for (int x = x1; x < x2; x++)
                        {
                            int i = y * m_width + x;

                            double l = m_lvec[i];
                            double a = m_avec[i];
                            double b = m_bvec[i];

                            distlab[i] = (l - kseedsl[n]) * (l - kseedsl[n]) +
                                         (a - kseedsa[n]) * (a - kseedsa[n]) +
                                         (b - kseedsb[n]) * (b - kseedsb[n]);

                            distxy[i] = (x - kseedsx[n]) * (x - kseedsx[n]) +
                                        (y - kseedsy[n]) * (y - kseedsy[n]);

                            //------------------------------------------------------
                            double dist = distlab[i] / maxlab[n] + distxy[i] * invxywt; // only varying m, prettier superpixels
                            // double dist = distlab[i]/maxlab[n] + distxy[i]/maxxy[n]; //varying both m and s
                            //------------------------------------------------------

                            if (dist < distvec[i])
                            {
                                distvec[i] = dist;
                                klabels[i] = n;
                            }
                        }

                    }
                }
                //----------------------------------------------------
                // Assign the max color distance for a cluster
                //----------------------------------------------------
                if (0 == numitr)
                {
                    double[] MAX_LAB_TMP = Enumerable.Repeat((double)1, numk).ToArray();
                    maxlab = new List<double>(MAX_LAB_TMP);

                    double[] MAX_XY_TMP = Enumerable.Repeat((double)1, numk).ToArray();
                    maxxy = new List<double>(MAX_XY_TMP);
                }
                //-----------------------------------------------------
                // Recaculate the centroid and store in the seed values
                //-----------------------------------------------------
                sigmal = new List<double>(new double[numk]);
                sigmaa = new List<double>(new double[numk]);
                sigmab = new List<double>(new double[numk]);
                sigmax = new List<double>(new double[numk]);
                sigmay = new List<double>(new double[numk]);
                clustersize = new List<int>(new int[numk]);

                for (int j = 0; j < sz; j++)
                {
                    int temp = klabels[j];

                    sigmal[klabels[j]] += m_lvec[j];
                    sigmaa[klabels[j]] += m_avec[j];
                    sigmab[klabels[j]] += m_bvec[j];
                    sigmax[klabels[j]] += (j % m_width);
                    sigmay[klabels[j]] += (j / m_width);

                    clustersize[klabels[j]]++;
                }

                for (int k = 0; k < numk; k++)
                {
                    if (clustersize[k] <= 0) clustersize[k] = 1;
                    inv[k] = 1.0 / (double)(clustersize[k]); //computing inverse now to multiply, then divide later
                }

                for (int k = 0; k < numk; k++)
                {
                    kseedsl[k] = sigmal[k] * inv[k];
                    kseedsa[k] = sigmaa[k] * inv[k];
                    kseedsb[k] = sigmab[k] * inv[k];
                    kseedsx[k] = sigmax[k] * inv[k];
                    kseedsy[k] = sigmay[k] * inv[k];
                }

            }

        }

        /// <summary>
        /// EnforceLabelConnectivity
        /// 
        /// 1. finding an adjacent label for each new component at the start
        /// 2. if a certain component is too small, assigning the previously found 
        ///    adjacent label to this component, and not incrementing the label.
        /// </summary>
        /// <param name="klabels">input labels that need to be corrected to remove stray labels</param>
        /// <param name="m_width">width of the image</param>
        /// <param name="m_height">height of the image</param>
        /// <param name="nlabels">new labels</param>
        /// <param name="numlabels">the number of labels changes in the end if segments are removed</param>
        /// <param name="K">the number of superpixel desired by the user</param>
        private void EnforceLabelConnectivity(
            ref int[] klabels,
            int width,
            int height,
            ref int[] nlabels,
            int numlabels,
            int K)
        {
            int[] dx4 = new int[] { -1, 0, 1, 0 };
            int[] dy4 = new int[] { 0, -1, 0, 1 };

            int sz = width * height;
            int SUPSZ = sz / K;

            for (int i = 0; i < sz; i++) nlabels[i] = -1;

            int label = 0;
            int[] xvec = new int[sz];
            int[] yvec = new int[sz];
            int oindex = 0;
            int adjlabel = 0; //adjacent label
            for (int j = 0; j < height; j++)
            {
                for (int k = 0; k < width; k++)
                {
                    if (0 > nlabels[oindex])
                    {
                        nlabels[oindex] = label;
                        //----------------------
                        // Start a new segment
                        //----------------------
                        xvec[0] = k;
                        yvec[0] = j;
                        //------------------------------------------------------
                        // Quickly find an adjacent label for use afer if needed
                        //------------------------------------------------------
                        {
                            for (int n = 0; n < 4; n++)
                            {
                                int x = xvec[0] + dx4[n];
                                int y = yvec[0] + dy4[n];
                                if ((x >= 0 && x < width) && (y >= 0 && y < height))
                                {
                                    int nindex = y * width + x;
                                    if (nlabels[nindex] >= 0) adjlabel = nlabels[nindex];
                                }
                            }
                        }

                        int count = 1;
                        for (int c = 0; c < count; c++)
                        {
                            for (int n = 0; n < 4; n++)
                            {
                                int x = xvec[c] + dx4[n];
                                int y = yvec[c] + dy4[n];

                                if ((x >= 0 && x < width) && (y >= 0 && y < height))
                                {
                                    int nindex = y * width + x;

                                    if (0 > nlabels[nindex] && klabels[oindex] == klabels[nindex])
                                    {
                                        xvec[count] = x;
                                        yvec[count] = y;
                                        nlabels[nindex] = label;
                                        count++;
                                    }
                                }
                            }
                        }
                        //------------------------------------------------------
                        // If segment size is less than a limit, assign an 
                        // adjacent label found before, and decerent label count.
                        //------------------------------------------------------
                        if (count <= SUPSZ >> 2)
                        {
                            for (int c = 0; c < count; c++)
                            {
                                int ind = yvec[c] * width + xvec[c];
                                nlabels[ind] = adjlabel;
                            }
                            label--;
                        }
                        label++;
                    }
                    oindex++;
                }
            }
            numlabels = label;

        }

        #endregion

        // 03 - Segmentation post-processing & Visualization functions
        //
        #region SEGMENTATION POST-PROCESSING & VISUALIZATION

        private void DrawContoursAroundSegments(
            ref int[] klabels,
            int width,
            int height,
            out bool[] contlabels)
        {
            int[] dx8 = new int[] { -1, -1, 0, 1, 1, 1, 0, -1 };
            int[] dy8 = new int[] { 0, -1, -1, -1, 0, 1, 1, 1 };

            int sz = width * height;

            List<bool> istaken = new List<bool>(new bool[sz]);
            contlabels = new bool[sz];

            int mainindex = 0;
            for (int j = 0; j < height; j++)
            {
                for (int k = 0; k < width; k++)
                {
                    int np = 0;
                    for (int i = 0; i < 8; i++)
                    {
                        int x = k + dx8[i];
                        int y = j + dy8[i];

                        if ((x >= 0 && x < width) && (y >= 0 && y < height))
                        {
                            int index = y * width + x;

                            if (false == istaken[index]) // comment this to obtain internal contours
                            {
                                if (klabels[mainindex] != klabels[index]) np++;
                            }
                        }
                    }
                    if (np > 1) // change to 2 or 3 for thinner lines
                    {
                        contlabels[mainindex] = true;
                        istaken[mainindex] = true;
                    }
                    mainindex++;
                }
            }
        }

        private Bitmap DrawContourToBitmapFormat24bppRgb(
            Bitmap imgBitmapOri,
            ref bool[] contlabels,
            Color color)
        {
            int width = imgBitmapOri.Width;
            int height = imgBitmapOri.Height;
            //Bitmap imgBitmapDes = new Bitmap(width, height, PixelFormat.Format24bppRgb);

            //using (Graphics g = Graphics.FromImage(imgBitmapDes))
            //{
            //    // Prevent DPI conversion
            //    g.PageUnit = GraphicsUnit.Pixel;
            //    // Draw the image
            //    g.DrawImage(imgBitmapOri, 0, 0);
            //}

            Bitmap imgBitmapDes = imgBitmapOri.Clone(new Rectangle(0, 0, width, height), PixelFormat.Format24bppRgb);

            // Lock the bitmap's bits
            Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData imgBitmapData = imgBitmapDes.LockBits(rect,
                                                          ImageLockMode.ReadWrite,
                                                          imgBitmapDes.PixelFormat);

            // Get the address of the first line
            IntPtr ptr = imgBitmapData.Scan0;

            // Allocate size of the array to hold the bytes of the bitmap
            // in byteCount = imgBitmapData.Stride * imgBitmapData.Height;
            int pixelCount = width * height;
            int byteCount = width * height * 24 / 8;
            byte[] imgBuff = new byte[byteCount];

            // copy the RGB value into the array
            Marshal.Copy(ptr, imgBuff, 0, byteCount);

            // operate on pixel value
            for (int j = 0; j < pixelCount; j++)
            {
                if (true == contlabels[j])
                {
                    int pixelPosIndex = j * 24 / 8;

                    imgBuff[pixelPosIndex + 2] = color.R;
                    imgBuff[pixelPosIndex + 1] = color.G;
                    imgBuff[pixelPosIndex] = color.B;
                }
            }

            // draw imgBuff back to imgBitmapDes
            Marshal.Copy(imgBuff, 0, ptr, byteCount);

            imgBitmapDes.UnlockBits(imgBitmapData);

            return imgBitmapDes;
        }

        #endregion

        // 04 - Image importing functions
        // Supplementary image importing functions
        // 
        #region IMAGE OPERATION FUNCTIONS

        /// <summary>
        /// InitializeImageBuffer
        /// 
        /// Get image buffer from Bitmap object, and initialize image members in the class
        /// </summary>
        /// <param name="imgBitmaph">Bitmap class</param>
        /// <returns>btye[] imgBuff. Image buffer in byte[] format.</returns>
        public byte[] InitializeImageBuffer(Bitmap imgBitmap)
        {
            byte[] imgBuff;
            GetImageBuffer(imgBitmap, out imgBuff, ref m_width, ref m_height, ref m_depth);

            _bInitialized = true;
            return imgBuff;
        }

        /// <summary>
        /// GetImageBuffer
        /// </summary>
        /// <param name="imgBitmap"></param>
        /// <param name="imgBuff"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="depth"></param>
        private void GetImageBuffer(Bitmap imgBitmap, out byte[] imgBuff, ref int width, ref int height, ref int depth)
        {
            width = imgBitmap.Width;
            height = imgBitmap.Height;

            // Get total locked pixel count
            // int pixelCount = width*height;

            // Get source bitmap pixel format size
            depth = Bitmap.GetPixelFormatSize(imgBitmap.PixelFormat);
            //int bytesPerPixel = depth/8;

            // Lock the bitmap's bits
            Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData imgBitmapData = imgBitmap.LockBits(rect,
                                                            ImageLockMode.ReadOnly,
                                                            imgBitmap.PixelFormat);

            // Get the address of the first line
            IntPtr ptr = imgBitmapData.Scan0;

            // Allocate size of the array to hold the bytes of the bitmap
            // in byteCount = imgBitmapData.Stride * imgBitmapData.Height;
            int byteCount = width * height * depth / 8;
            imgBuff = new byte[byteCount];

            // copy the RGB value into the array
            Marshal.Copy(ptr, imgBuff, 0, byteCount);

            imgBitmap.UnlockBits(imgBitmapData);

        }
        #endregion

        // image properties fields
        //
        private int m_width;
        private int m_height;
        private int m_depth;

        // image LAB vectors converted from RGB values
        //
        private double[] m_lvec;
        private double[] m_avec;
        private double[] m_bvec;

        // Maximum double value
        //
        private const double DBL_MAX = Double.MaxValue;

        // State related flags, may need to be removed in formal release of the class
        //
        private bool _bInitialized = false;  // flag whether SLIC is initialized
        private bool _bLABUpdated = false;   // flag whether LAB is updated


        // Property m_BytePerPixel helps to check the depth of input image
        // may not needed in formal release of the class
        //
        private int m_BytePerPixel
        {
            get
            {
                if (m_depth == 8 || m_depth == 24 || m_depth == 32)
                {
                    return m_depth / 8;
                }
                else
                {
                    throw new ArgumentException("The bbp of the image should be 8, 24 or 32!");
                }
            }
        }
    }
}
