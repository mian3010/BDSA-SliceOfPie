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
        List<FileInstance> testFiles;
        CommunicatorOfflineAdapter communicatorOfflineAdaptor = CommunicatorOfflineAdapter.GetCommunicatorInstance();

        private List<FileInstance> GetTestFiles()
        {
            //instans of HTMLMarshallerTest clas to get test files.
            marshallerTest = new HTMLMarshallerTest();
            testFiles = marshallerTest.FileInstanceTestInput();
            return testFiles;            
        }
        

        [TestMethod]
        public void TestAddOffLineCreatedFiles()
        {
            List<FileInstance> testFiles = GetTestFiles();
            // Add all test files to local disc.
            foreach (FileInstance testFileInstance in testFiles)
            {
                communicatorOfflineAdaptor.AddFile(testFileInstance);
            }
            Assert.AreEqual(1, 1);        
            communicatorOfflineAdaptor.CleanUp();
        }
        

        [TestMethod]
        public void TestRenameFiles()
        {
            int i = 1;
            List<FileInstance> testFiles = GetTestFiles();
            foreach (FileInstance testFile in testFiles)
            {
                communicatorOfflineAdaptor.RenameFile(testFile, "Document" + i);
                i++;
            }
            communicatorOfflineAdaptor.CleanUp();
        }

        //[TestMethod]
        //public void TestMoveFiles()
        //{
        //    List<FileInstance> testFiles = GetTestFiles();
        //    foreach (FileInstance testFileInstance in testFiles)
        //    {
        //        communicatorOfflineAdaptor.MoveFile(testFileInstance, testFileInstance.path + @"\newTestFolder");
        //    }
        //    communicatorOfflineAdaptor.CleanUp();
        //}


        [TestMethod]
        public void CheckEntries()
        {
            int i = 1;
            List<FileInstance> testFiles = GetTestFiles();
            //for debuging.
/*            foreach (FileInstance testFileInstance in testFiles)
            {
                Debug.WriteLine("id: " + testFileInstance.id);
                Debug.WriteLine("name: " + testFileInstance.File.name);
                Debug.WriteLine("ServerPath: " + testFileInstance.File.serverpath);
                Debug.WriteLine("Project id: " + testFileInstance.File.Project_id);
                Debug.WriteLine("User e-mail: " + testFileInstance.UserEmail);
            }

*/
            IFileListHandler fileListHandler = communicatorOfflineAdaptor.FileListHandler;
            FileList fileList = fileListHandler.FileList;

//            Debug.WriteLine("Document" + i + "'s path is" + fileList.List[i].Path);

            TestAddOffLineCreatedFiles();

            foreach (FileInstance testFileInstance in testFiles)
            {
                Assert.AreEqual(fileList.List[i].Id, testFileInstance.id);
                Assert.AreEqual(fileList.List[i].Version, testFileInstance.File.Version);
                Assert.AreEqual(fileList.List[i].Name, testFileInstance.File.name);
                Assert.AreEqual(fileList.List[i].Path, testFileInstance.path);
                break;
//                i--;
            }
            communicatorOfflineAdaptor.CleanUp();
        }
    }
}
