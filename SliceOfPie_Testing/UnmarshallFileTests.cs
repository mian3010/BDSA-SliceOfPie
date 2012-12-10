using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SliceOfPie_Model;
using System.Collections.Generic;
using System.Diagnostics;
using SliceOfPie_Model.Persistence;


namespace SliceOfPie_Testing
{
    [TestClass]
    public class UnmarshallFileTests
    {
        [TestMethod]
        public void TestUnmarshallFile()
        {
            List<File> rig = FileCommunicatorTests.GetTestRig();
            List<File> results = new List<File>();
            foreach (File file in rig)
            {
                /// We assume that the marshalls of file works. TO-DO MAke manual XML to
                /// remove this dependency.
                String xml = HTMLMarshalUtil.MarshallFile(file);
                File resfile = HTMLMarshalUtil.UnmarshallFile(xml);
                results.Add(resfile);
            
                Debug.WriteLine(resfile.id + " " + resfile.name + " " + resfile.serverpath);
            }
            

        }
    }
}
