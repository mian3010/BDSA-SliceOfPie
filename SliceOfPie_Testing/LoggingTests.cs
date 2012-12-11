using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using SliceOfPie_Model.Persistence;
using SliceOfPie_Model;
using System.Diagnostics;
namespace SliceOfPie_Testing
{
    [TestClass]
    public class LoggingTests
    {
        HTMLMarshallerTest marshallerTest;
        List<File> fileInput;
        CommunicatorOfflineAdapter communicatorOfflineAdaptor = new CommunicatorOfflineAdapter();

        private void GetTestFiles1()
        {
            //instans of HTMLMarshallerTest clas to get test files.
            marshallerTest = new HTMLMarshallerTest();
            fileInput = marshallerTest.FileTestInput();
        }


        [TestMethod]
        public void TestAddOffLineCreatedFiles1(List<File> fileInput)
        {
            // Add all test files to local disc.
            foreach (File testFile in fileInput)
            {
                communicatorOfflineAdaptor.AddOfflineCreatedFile(testFile);
            }
        }
        

        [TestMethod]
        public void TestRenameFiles1(List<File> fileInput)
        {
            int i = 1;
            foreach (File testFile in fileInput)
            {
                communicatorOfflineAdaptor.RenameFile(testFile, "Document" + i);
                i++;
            }
        }

        [TestMethod]
        public void TestMoveFiles1(List<File> fileInput)
        {
            foreach (File testFile in fileInput)
            {
                communicatorOfflineAdaptor.MoveFile(testFile, testFile.serverpath + "/newTestFolder");
            }
        }


        [TestMethod]
        public void CheckEntries1(List<File> fileInput)
        {
            int i = 0;
            IFileListHandler fileListHandler = communicatorOfflineAdaptor.FileListHandler;
            FileList fileList = fileListHandler.FileList;

            Debug.WriteLine("Document" + i + "'s path is" + fileList.List[i].Path);

            TestAddOffLineCreatedFiles1(fileInput);

            foreach (File testFile in fileInput)
            {
                Assert.AreEqual(fileList.List[i].Id, testFile.id);
                Assert.AreEqual(fileList.List[i].Version, testFile.version);
                Assert.AreEqual(fileList.List[i].Name, testFile.name);
                Assert.AreEqual(fileList.List[i].Path, testFile.serverpath);
                i++;
            }

            
        }








    //    private FileList BasicFileList(bool saveLog)
    //    {
    //        List<File> rig = FileCommunicatorTests.GetTestRig();
    //        CommunicatorOfflineAdapter ts = new CommunicatorOfflineAdapter();
    //        OfflineFileList l = new OfflineFileList(ts);
    //        foreach (File file in rig)
    //        {
    //            ts.AddFile(file);
    //        }
    //        ts.DeleteFile(rig[0]);
    //        ts.RenameFile(rig[1], "renamefile.html");
    //        Document ds = (Document) rig[3];
    //        ds.Content.Append("Changing something");
    //        ts.ModifyFile(rig[3]);
    //        if (saveLog) {
    //            l.PersistLogOnDisk(); 
    //        }
    //        return l.FileList;

    //    }



    }
}
