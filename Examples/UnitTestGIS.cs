﻿using System;
using System.IO;
using Engine.GIS.GLayer.GRasterLayer;
using Engine.GIS.GOperation.Arithmetic;
using Engine.GIS.GOperation.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Examples
{
    [TestClass]
    public class TestGIS
    {
        /// <summary>
        /// a raster image file for testing
        /// </summary>
        string fullFilename = Directory.GetCurrentDirectory() + @"\Datasets\Band18.tif";

        [TestMethod]
        public void ReadRasterLayer()
        {
            GRasterLayer rasterLayer = new GRasterLayer(fullFilename);
            Assert.AreEqual(rasterLayer.BandCollection.Count, 18);
        }

        [TestMethod]
        public void BandExport()
        {
            string filename = @"C:\Users\81596\Desktop\kiwi.literature\revise\experiment A\A_Band18.tif";
            GRasterLayer rasterLayer = new GRasterLayer(filename);
            rasterLayer.ExprotNormalizedLayer(@"C:\Users\81596\Desktop\kiwi.literature\revise\experiment A\A_Normalized_Band18.tif");
        }

        [TestMethod]
        public void RasterBandPickValueTool()
        {
            GRasterLayer rasterLayer = new GRasterLayer(fullFilename);
            IRasterBandCursorTool pRasterBandCursorTool = new GRasterBandCursorTool();
            pRasterBandCursorTool.Visit(rasterLayer.BandCollection[0]);
            double normalValue = pRasterBandCursorTool.PickNormalValue(100, 100);
            //the col and row should be odd number
            float[] rangeNormalValue = pRasterBandCursorTool.PickRangeNormalValue(100, 100, 3, 3);
            float rawValue = pRasterBandCursorTool.PickRawValue(100, 100);
            //the col and row should be odd number
            float[] rangeRawValue = pRasterBandCursorTool.PickRangeRawValue(100, 100, 3, 3);
            //
            Assert.AreEqual(normalValue, 0.62204724409448819);
            Assert.AreEqual(string.Join(",", rangeNormalValue), "0.645669291338583,0.637795275590551,0.661417322834646,0.614173228346457,0.622047244094488,0.645669291338583,0.614173228346457,0.618110236220472,0.622047244094488");
            Assert.AreEqual(rawValue, 159);
            Assert.AreEqual(string.Join(",", rangeRawValue), "165,163,169,157,159,165,157,158,159");
        }

        [TestMethod]
        public void RasterBandStatisticTool()
        {
            GRasterLayer rasterLayer = new GRasterLayer(fullFilename);
            IRasterBandStatisticTool pRasterBandStasticTool = new GRasterBandStatisticTool();
            pRasterBandStasticTool.Visit(rasterLayer.BandCollection[0]);
            var rawGraph = pRasterBandStasticTool.StaisticalRawGraph;
            var rawTable = pRasterBandStasticTool.StatisticalRawQueryTable;
            Assert.AreEqual(rawGraph.Count, 255);
            Assert.AreEqual(rawTable.Length, 768000);
        }

        [TestMethod]
        public void RasterLayerPickValueTool()
        {
            GRasterLayer rasterLayer = new GRasterLayer(fullFilename);
            IRasterLayerCursorTool pRasterLayerCursorTool = new GRasterLayerCursorTool();
            pRasterLayerCursorTool.Visit(rasterLayer);
            //returen the nomalized values form each layer at given point (100,100)
            float[] normalValue = pRasterLayerCursorTool.PickNormalValue(100, 100);
            //the col and row should be odd number
            float[] rangeNormalValue = pRasterLayerCursorTool.PickRagneNormalValue(100, 100, 3, 3);
            Assert.AreEqual(string.Join(",", normalValue), "0.622047244094488,0.574803149606299,0.322834645669291,0.12992125984252,0.145669291338583,0.338582677165354,0.192913385826772,0.192913385826772,0.354330708661417,0.488188976377953,0.165354330708661,0.181102362204724,0.12992125984252,0.366141732283465,0.133858267716535,0.102362204724409,0.381889763779528,0.133858267716535");
            Assert.AreEqual(string.Join(",", rangeNormalValue), "0.645669291338583,0.637795275590551,0.661417322834646,0.614173228346457,0.622047244094488,0.645669291338583,0.614173228346457,0.618110236220472,0.622047244094488,0.622047244094488,0.598425196850394,0.610236220472441,0.574803149606299,0.574803149606299,0.586614173228346,0.570866141732283,0.566929133858268,0.566929133858268,0.385826771653543,0.354330708661417,0.354330708661417,0.330708661417323,0.322834645669291,0.326771653543307,0.322834645669291,0.31496062992126,0.311023622047244,0.12992125984252,0.137795275590551,0.153543307086614,0.125984251968504,0.12992125984252,0.145669291338583,0.133858267716535,0.137795275590551,0.141732283464567,0.165354330708661,0.15748031496063,0.18503937007874,0.133858267716535,0.145669291338583,0.177165354330709,0.125984251968504,0.133858267716535,0.137795275590551,0.409448818897638,0.374015748031496,0.362204724409449,0.346456692913386,0.338582677165354,0.334645669291339,0.338582677165354,0.326771653543307,0.322834645669291,0.196850393700787,0.196850393700787,0.228346456692913,0.18503937007874,0.192913385826772,0.216535433070866,0.196850393700787,0.200787401574803,0.208661417322835,0.196850393700787,0.196850393700787,0.228346456692913,0.18503937007874,0.192913385826772,0.216535433070866,0.196850393700787,0.200787401574803,0.208661417322835,0.413385826771654,0.385826771653543,0.385826771653543,0.358267716535433,0.354330708661417,0.358267716535433,0.358267716535433,0.346456692913386,0.338582677165354,0.409448818897638,0.425196850393701,0.551181102362205,0.409448818897638,0.488188976377953,0.362204724409449,0.374015748031496,0.437007874015748,0.275590551181102,0.106299212598425,0.153543307086614,0.141732283464567,0.12992125984252,0.165354330708661,0.125984251968504,0.141732283464567,0.114173228346457,0.137795275590551,0.122047244094488,0.188976377952756,0.177165354330709,0.145669291338583,0.181102362204724,0.133858267716535,0.133858267716535,0.0984251968503937,0.118110236220472,0.12992125984252,0.133858267716535,0.153543307086614,0.125984251968504,0.12992125984252,0.145669291338583,0.133858267716535,0.137795275590551,0.141732283464567,0.437007874015748,0.401574803149606,0.397637795275591,0.374015748031496,0.366141732283465,0.37007874015748,0.366141732283465,0.358267716535433,0.354330708661417,0.153543307086614,0.145669291338583,0.169291338582677,0.125984251968504,0.133858267716535,0.161417322834646,0.118110236220472,0.125984251968504,0.125984251968504,0.094488188976378,0.102362204724409,0.118110236220472,0.0984251968503937,0.102362204724409,0.110236220472441,0.118110236220472,0.118110236220472,0.122047244094488,0.452755905511811,0.417322834645669,0.405511811023622,0.389763779527559,0.381889763779528,0.374015748031496,0.389763779527559,0.374015748031496,0.366141732283465,0.153543307086614,0.145669291338583,0.169291338582677,0.125984251968504,0.133858267716535,0.161417322834646,0.118110236220472,0.125984251968504,0.125984251968504");
        }

        [TestMethod]
        public void RasterKappaIndexCalcute()
        {
            string rootFilename = @"C:\Users\81596\Desktop\kiwi.literature\revise\experiment A\";
            string smapleSize = "300";
            //"RF", "DQN", "CNN", "SVML2",
            string[] suffixs = new string[] { "RF", "DQN", "CNN", "SVML2", "SVM" };
            string lableFilename = "Test.tif";
            GRasterLayer truthLayer = new GRasterLayer(rootFilename + lableFilename);
            //string kappaText = "";
            //string oaText = "";
            string text = "";
            for (int i = 1; i <= 10; i++)
            {
                Array.ForEach(suffixs, suffix => {
                    string predFilename = string.Format(@"{0}{1}\{2}", rootFilename, smapleSize, suffix + "_" + i + ".tif");
                    GRasterLayer predLayer = new GRasterLayer(predFilename);
                    var (matrix, kappa, actionsNumber, oa) = KappaIndex.Calcute(truthLayer, predLayer);
                    text += string.Format("{0:P}\t", kappa).Replace("%", "") + string.Format("{0:P}\t", oa).Replace("%", "");
                    //kappaText += string.Format("{0:P}\r\n", kappa).Replace("%", "");
                    //oaText += string.Format("{0:P}\r\n", oa).Replace("%", "");
                });
                text += "\r\n";
            }
            Assert.AreSame(1, 1);
        }

        [TestMethod]
        public void VectorPyramidOutput()
        {

        }

        [TestMethod]
        public void RasterSuperPixel()
        {

        }
    }
}
