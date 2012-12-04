using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SliceOfPie_Model;

namespace SliceOfPie_Testing
{
    [TestClass]
    public class FileCommunicatorTests
    {
        /// <summary>
        /// Returns a set of files, built for testing various aspects of the system. Tests regular works, making new folders ...
        /// </summary>
        /// <returns>The test rig to be used in the test methods</returns>
        public List<File> GetTestRig()
        {
            FileMetaData meta = new FileMetaData();
            meta.MetaDataType = new MetaDataType() { Type = "Author" };
            meta.value = "Magnus Stahl";

            FileMetaData beta = new FileMetaData();
            beta.MetaDataType = new MetaDataType() { Type = "Author" };
            beta.value = "Magnus Stahl";

            FileMetaData deta = new FileMetaData();
            deta.MetaDataType = new MetaDataType() { Type = "Author" };
            deta.value = "Magnus Stahl";

            FileMetaData feta = new FileMetaData();
            feta.MetaDataType = new MetaDataType() { Type = "Author" };
            feta.value = "Magnus Stahl";

            
            File one = new File();
            one.serverpath = @"C:\Users\Magnus\Desktop\test\";
            one.name = "Reg1";
            one.id = 1;
            one.FileMetaDatas.Add(deta);

            File two = new File();
            two.serverpath = @"C:\Users\Magnus\Desktop\test\newfolder\";
            two.name = "NewF1";
            two.id = 2;
            two.FileMetaDatas.Add(beta);

            File three = new File();
            three.serverpath = @"IllegalServerPath";
            three.name = "NewF2";
            three.id = 3;
            three.FileMetaDatas.Add(feta);
    
            Document four = new Document();
            four.serverpath = @"C:\Users\Magnus\Desktop\test\";
            four.name = "RegD1";
            four.id = 4;
            four.FileMetaDatas.Add(meta);    

            List<File> rig = new List<File>();
            rig.Add(one); rig.Add(two); rig.Add(three); rig.Add(four);

            return rig;
        }

        /// <summary>
        /// We'll test that the file has been added. However, we'll need to check the files manually on our disk as reading them again
        /// would test other methods aswell.
        /// </summary>
        [TestMethod]
        public void TestAddFile()
        {
            CommunicatorOfflineAdapter ts = new CommunicatorOfflineAdapter(@"C:\Users\Magnus\Desktop\test\");
            List<File> rig = GetTestRig();
            foreach (File file in rig)
            {
                ts.AddFile(file);
            }
        }

        [TestMethod]
        public void TestRemoveFile()
        {
        }

        [TestMethod]
        public void TestLoadFile()
        {
        }
        
    }
}
