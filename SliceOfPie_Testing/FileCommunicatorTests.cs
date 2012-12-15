using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SliceOfPie_Model;
using System.Diagnostics;
using SliceOfPie_Model.Persistence;

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
        static public List<FileInstance> GetTestRig()
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


            FileInstance one = new FileInstance();
            one.File.serverpath = TestPath;
            one.File.name = "Reg1.html";
            one.File.id = 1;
            one.File.FileMetaDatas.Add(deta);

            FileInstance two = new FileInstance();
            two.File.serverpath = TestPath + "\\newfolder" ;
            two.File.name = "NewF1.html";
            two.id = 2;
            two.File.FileMetaDatas.Add(beta);

            FileInstance three = new FileInstance();
            three.File.serverpath = @"IllegalServerPath";
            three.File.name = "NewF2.html";
            three.File.id = 3;
            three.File.FileMetaDatas.Add(feta);
    
            Document four = new Document();
            four.File.serverpath = TestPath;
            four.File.name = "RegD1.html";
            four.id = 4;
            four.File.FileMetaDatas.Add(meta);

            List<FileInstance> rig = new List<FileInstance>();
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
            CommunicatorOfflineAdapter ts = CommunicatorOfflineAdapter.GetCommunicatorInstance();
            List<FileInstance> rig = GetTestRig();

            foreach (FileInstance fileInstance in rig)
            {
                ts.AddFile(fileInstance);
                Assert.AreEqual(true, ts.FindFile(fileInstance));
            }
        }

        [TestMethod]
        public void TestRenameFile()
        {
            CommunicatorOfflineAdapter ts = CommunicatorOfflineAdapter.GetCommunicatorInstance();
            List<FileInstance> rig = GetTestRig();
            AddFileRig(ts, rig);
            foreach (FileInstance fileInstance in rig)
            {
                String newName = "exexex" + fileInstance.File.name;
                ts.RenameFile(fileInstance, newName);
                fileInstance.File.name = newName;
                Assert.AreEqual(true, ts.FindFile(fileInstance));
                ts.DeleteFile(fileInstance);
            }

        }

        [TestMethod]
        public void TestDeleteFile()
        {
            CommunicatorOfflineAdapter ts = CommunicatorOfflineAdapter.GetCommunicatorInstance();
            foreach (FileInstance fileInstance in GetTestRig())
            {
                ts.DeleteFile(fileInstance);
                Assert.AreEqual(false, ts.FindFile(fileInstance));
            }
            
        }

        [TestMethod]
        public void TestMoveFile()
        {
            CommunicatorOfflineAdapter ts = CommunicatorOfflineAdapter.GetCommunicatorInstance();
            List<FileInstance> rig = GetTestRig();
            AddFileRig(ts, rig);
            foreach (FileInstance fileInstance in rig)
            {
                String newPath = TestPath + "\\testMove";
                Debug.WriteLine("!!!!! WTF : " + newPath);
                ts.MoveFile(fileInstance, newPath);
                Assert.AreEqual(true, ts.FindFile(fileInstance));
                fileInstance.File.serverpath = newPath;
                ts.DeleteFile(fileInstance);
            }
            

        }

        private static void AddFileRig(CommunicatorOfflineAdapter ts, List<FileInstance> rig)
        {
            foreach (FileInstance fileInstance in rig)
            {
                ts.AddFile(fileInstance);
            }
        }

        [TestMethod]
        public void TestLoadFile()
        {

        }
        
    }
}
