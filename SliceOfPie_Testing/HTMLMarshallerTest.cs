using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SliceOfPie_Model;
using SliceOfPie_Network;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using SliceOfPie_Model.Persistence;


namespace SliceOfPie_Testing
{
    [TestClass]
    public class HTMLMarshallerTest
    {
        List<File> testInput = new List<File>();
        File testOne;
        File testTwo;
        File testThree;
        File testFour;
        File testFive;

        /// <summary>
        /// Test method that creates 5 files with input
        /// </summary>
        /// <returns> List of test files
        [TestMethod]
        public List<File> FileTestInput()
        {
            MetaDataType authorType = new MetaDataType();
            authorType.Type = "Author";


            FileMetaData authorTypeValue = new FileMetaData();
            authorTypeValue.value = "morr";
            authorTypeValue.MetaDataType = authorType;


            //Test input for test file
            testOne = new File();
            testOne.id = 1;
            testOne.name = "Document 1";
            testOne.serverpath = "Testclass/HTMLMarshallerTest/testOne";
            testOne.Project_id = 1;
            testOne.UserEmail = "morr@itu.com";
            testOne.FileMetaDatas.Add(authorTypeValue);

            testTwo = new File();
            testTwo.id = 0;
            testTwo.name = "";
            testTwo.serverpath = "";
            testTwo.Project_id = 0;
            testTwo.UserEmail = "";
            testTwo.FileMetaDatas.Add(authorTypeValue);

            testThree = new File();
            testThree.id = -3;
            testThree.name = "33333333333";
            testThree.serverpath = "333333333333333333333333333";
            testThree.Project_id = 3;
            testThree.UserEmail = "33333333333";
            testThree.FileMetaDatas.Add(authorTypeValue);
            testThree.FileMetaDatas.Add(authorTypeValue);

            testFour = new File();
            testFour.id = 2147483647;
            testFour.name = "!#¤%&/()=?+}@£½§€{][";
            testFour.serverpath = "!#¤%&/()=?+}@£½§€{][";
            testFour.Project_id = 2147483647;
            testFour.UserEmail = "!#¤%&/()=?+}@£½§€{][jio";
            testFour.FileMetaDatas.Add(authorTypeValue);

            testFive = new File();
            testFive.id = -2147483648;
            testFive.name = "Document 5";
            testFive.serverpath = "Testclass/HTMLMarshallerTest/testFive";
            testFive.Project_id = 2147483647;
            testFive.UserEmail = "morr@itu.com";
            testFive.FileMetaDatas.Add(authorTypeValue);
            
            //test files added to list.
            testInput.Add(testOne);
            testInput.Add(testTwo);
            testInput.Add(testThree);
            testInput.Add(testFour);
            testInput.Add(testFive);

            return testInput;
        }


        [TestMethod]
        public void TestMarshalling()
        {

            foreach (File testFile in testInput)
            {
                String marshalledFile = HTMLMarshalUtil.MarshallFile(testFile);
                File unmarshalledFile = HTMLMarshalUtil.UnmarshallFile(marshalledFile);

                Assert.AreEqual(unmarshalledFile.id, testFile.id);
                Assert.AreEqual(unmarshalledFile.name, testFile.name);
                Assert.AreEqual(unmarshalledFile.serverpath, testFile.serverpath);
                Assert.AreEqual(unmarshalledFile.Project_id, testFile.Project_id);
                Assert.AreEqual(unmarshalledFile.UserEmail, testFile.UserEmail);
                Assert.AreEqual(unmarshalledFile.FileMetaDatas, testFile.FileMetaDatas);

                Assert.AreNotEqual(unmarshalledFile.id, 99999);
            }
        }

    }
}
