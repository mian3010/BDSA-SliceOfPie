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
        List<FileInstance> fileInstanceInput;
        CommunicatorOfflineAdapter communicatorOfflineAdaptor = CommunicatorOfflineAdapter.GetCommunicatorInstance();

        private void GetTestFiles1()
        {
            //instans of HTMLMarshallerTest clas to get test files.
            marshallerTest = new HTMLMarshallerTest();
            fileInstanceInput = marshallerTest.FileInstanceTestInput();
        }
        

        [TestMethod]
        public void TestAddOffLineCreatedFiles1(List<FileInstance> fileInstanceInput)
        {
            // Add all test files to local disc.
            foreach (FileInstance testFileInstance in fileInstanceInput)
            {
                communicatorOfflineAdaptor.AddOfflineCreatedFile(testFileInstance);
            }
        }
        

        [TestMethod]
        public void TestRenameFiles1(List<FileInstance> fileInstanceInput)
        {
            int i = 1;
            foreach (FileInstance testFile in fileInstanceInput)
            {
                communicatorOfflineAdaptor.RenameFile(testFile, "Document" + i);
                i++;
            }
        }

        [TestMethod]
        public void TestMoveFiles1(List<FileInstance> fileInstanceInput)
        {
            foreach (FileInstance testFileInstance in fileInstanceInput)
            {
                communicatorOfflineAdaptor.MoveFile(testFileInstance, testFileInstance.File.serverpath + "/newTestFolder");
            }
        }


        [TestMethod]
        public void CheckEntries1(List<FileInstance> fileInstanceInput)
        {
            int i = 0;
            IFileListHandler fileListHandler = communicatorOfflineAdaptor.FileListHandler;
            FileList fileList = fileListHandler.FileList;

            Debug.WriteLine("Document" + i + "'s path is" + fileList.List[i].Path);

            TestAddOffLineCreatedFiles1(fileInstanceInput);

            foreach (FileInstance testFileInstance in fileInstanceInput)
            {
                Assert.AreEqual(fileList.List[i].Id, testFileInstance.id);
                Assert.AreEqual(fileList.List[i].Version, testFileInstance.File.Version);
                Assert.AreEqual(fileList.List[i].Name, testFileInstance.File.name);
                Assert.AreEqual(fileList.List[i].Path, testFileInstance.File.serverpath);
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
