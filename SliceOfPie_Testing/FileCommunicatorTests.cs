using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SliceOfPie_Model;
using System.Diagnostics;

namespace SliceOfPie_Testing
{
    [TestClass]
    public class FileCommunicatorTests
    {
        public static string TestPath = @".\Files";

        /// <summary>
        /// Returns a set of files, built for testing various aspects of the system. Tests regular works, making new folders ...
        /// </summary>
        /// <returns>The test rig to be used in the test methods</returns>
        static public List<File> GetTestRig()
        {
            FileMetaData meta = new FileMetaData();
            meta.MetaDataType = new MetaDataType() { Type = "Author" };
            meta.value = "Magnus Stahl";

            FileMetaData beta = new FileMetaData();
            beta.MetaDataType = new MetaDataType() { Type = "Author" };
            beta.value = "Magnus Dahl";

            FileMetaData deta = new FileMetaData();
            deta.MetaDataType = new MetaDataType() { Type = "Author" };
            deta.value = "Agnes Stahl";

            FileMetaData feta = new FileMetaData();
            feta.MetaDataType = new MetaDataType() { Type = "Author" };
            feta.value = "Stor fed Feta Stahl";

            
            File one = new File();
            one.serverpath = TestPath;
            one.name = "Reg1.html";
            one.id = 1;
            one.FileMetaDatas.Add(deta);

            File two = new File();
            two.serverpath = TestPath + "\\newfolder" ;
            two.name = "NewF1.html";
            two.id = 2;
            two.FileMetaDatas.Add(beta);

            File three = new File();
            three.serverpath = @"IllegalServerPath";
            three.name = "NewF2.html";
            three.id = 3;
            three.FileMetaDatas.Add(feta);
    
            Document four = new Document();
            four.serverpath = TestPath;
            four.name = "RegD1.html";
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
            CommunicatorOfflineAdapter ts = new CommunicatorOfflineAdapter();
            List<File> rig = GetTestRig();
       
            foreach (File file in rig)
            {
                ts.AddFile(file);
                Assert.AreEqual(true, ts.FindFile(file));
            }
        }

        [TestMethod]
        public void TestRenameFile()
        {
            CommunicatorOfflineAdapter ts = new CommunicatorOfflineAdapter();
            List<File> rig = GetTestRig();
            AddFileRig(ts, rig);
            foreach (File file in rig)
            {
                String newName = "exexex" + file.name;
                ts.RenameFile(file, newName);
                file.name = newName;
                Assert.AreEqual(true, ts.FindFile(file));
                ts.DeleteFile(file);
            }

        }

        [TestMethod]
        public void TestDeleteFile()
        {
            CommunicatorOfflineAdapter ts = new CommunicatorOfflineAdapter();
            foreach (File file in GetTestRig())
            {
                ts.DeleteFile(file);
                Assert.AreEqual(false, ts.FindFile(file));
            }
            
        }

        [TestMethod]
        public void TestMoveFile()
        {
            CommunicatorOfflineAdapter ts = new CommunicatorOfflineAdapter();
            List<File> rig = GetTestRig();
            AddFileRig(ts, rig);
            foreach (File file in rig)
            {
                String newPath = TestPath + "\\testMove";
                Debug.WriteLine("!!!!! WTF : " + newPath);
                ts.MoveFile(file, newPath);
                Assert.AreEqual(true, ts.FindFile(file));
                file.serverpath = newPath;
                ts.DeleteFile(file);
            }
            

        }

        private static void AddFileRig(CommunicatorOfflineAdapter ts, List<File> rig)
        {
            foreach (File file in rig)
            {
                ts.AddFile(file);
            }
        }

        [TestMethod]
        public void TestLoadFile()
        {
            
        }
        
    }
}
