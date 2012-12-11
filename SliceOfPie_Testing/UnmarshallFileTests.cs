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
            List<FileInstance> rig = FileCommunicatorTests.GetTestRig();
            List<FileInstance> results = new List<FileInstance>();
            foreach (FileInstance fileInstance in rig)
            {
                /// We assume that the marshalls of file works. TO-DO MAke manual XML to
                /// remove this dependency.
                String xml = HtmlMarshalUtil.MarshallFile(fileInstance);
                FileInstance resfile = HtmlMarshalUtil.UnmarshallDocument(xml);
                results.Add(resfile);
            
                Debug.WriteLine(resfile.id + " " + resfile.File.name + " " + resfile.File.serverpath);
            }
            

        }
    }
}
