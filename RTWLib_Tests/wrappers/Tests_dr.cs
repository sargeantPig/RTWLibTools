﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using RTWLibPlus.dataWrappers;
using RTWLibPlus.helpers;
using RTWLibPlus.parsers.objects;
using RTWLibPlus.parsers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography.X509Certificates;

namespace RTWLib_Tests.wrappers
{
    [TestClass]
    public class Tests_dr
    {
        [TestMethod]
        public void DRGetRegionDataLocusGepidae()
        {
            var smf = TokenParse.ReadFile(RFH.CurrDirPath("resources", "descr_regions.txt"), false);
            var smfParse = DepthParse.Parse(smf, Creator.DRcreator, '\t');
            var parsedsmf = new DR(smfParse);
            var result = parsedsmf.GetNumberOfItems(8, "Locus_Gepidae");
            var expected = 8;
            Assert.AreEqual("Locus_Gepidae", ((baseObj)result[0]).Tag);
            Assert.AreEqual(expected, result.Count);
        }

        [TestMethod]
        public void DRGetRegionDataHibernia()
        {
            var smf = TokenParse.ReadFile(RFH.CurrDirPath("resources", "descr_regions.txt"), false);
            var smfParse = DepthParse.Parse(smf, Creator.DRcreator, '\t');
            var parsedsmf = new DR(smfParse);
            var result = parsedsmf.GetNumberOfItems(8, "Hibernia");
            var expected = 8;
            Assert.AreEqual("Hibernia", ((baseObj)result[0]).Tag);
            Assert.AreEqual(expected, result.Count);
        }
        [TestMethod]
        public void DRGetRegionDataThebais()
        {
            var smf = TokenParse.ReadFile(RFH.CurrDirPath("resources", "descr_regions.txt"), false);
            var smfParse = DepthParse.Parse(smf, Creator.DRcreator, '\t');
            var parsedsmf = new DR(smfParse);
            var result = parsedsmf.GetNumberOfItems(8, "Thebais");
            var expected = 8;
            Assert.AreEqual("Thebais", ((baseObj)result[0]).Tag);
            Assert.AreEqual(expected, result.Count);
        }

    }
}
