using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SliceOfPie_Model;
using SliceOfPie_Model.Persistence;
using System.Collections.Generic;


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
            testTwo.id = 2;
            testTwo.name = "Document 2";
            testTwo.serverpath = "Testclass/HTMLMarshallerTest/testTwo";
            testTwo.Project_id = 2;
            testTwo.UserEmail = "morr@itu.com";
            testTwo.FileMetaDatas.Add(authorTypeValue);

            testThree = new File();
            testThree.id = 3;
            testThree.name = "Document 3";
            testThree.serverpath = "Testclass/HTMLMarshallerTest/testthree";
            testThree.Project_id = 3;
            testThree.UserEmail = "morr@itu.com";
            testThree.FileMetaDatas.Add(authorTypeValue);
            testThree.FileMetaDatas.Add(authorTypeValue);

            testFour = new File();
            testFour.id = 4;
            testFour.name = "Document 4";
            testFour.serverpath = "Testclass/HTMLMarshallerTest/testFour";
            testFour.Project_id = 4;
            testFour.UserEmail = "morr@itu.com";
            testFour.FileMetaDatas.Add(authorTypeValue);

            testFive = new File();
            testFive.id = 5;
            testFive.name = "Document 5";
            testFive.serverpath = "Testclass/HTMLMarshallerTest/testFive";
            testFive.Project_id = 5;
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
                String marshalledFile = HtmlMarshalUtil.MarshallFile(testFile);
                File unmarshalledFile = HtmlMarshalUtil.UnmarshallFile(marshalledFile);

                Assert.AreEqual(unmarshalledFile.id, testFile.id);
                Assert.AreEqual(unmarshalledFile.name, testFile.name);
                Assert.AreEqual(unmarshalledFile.serverpath, testFile.serverpath);
                Assert.AreEqual(unmarshalledFile.Project_id, testFile.Project_id);
                Assert.AreEqual(unmarshalledFile.UserEmail, testFile.UserEmail);
                Assert.AreEqual(unmarshalledFile.FileMetaDatas, testFile.FileMetaDatas);

                Assert.AreNotEqual(unmarshalledFile.id, 99999);
            }
        }

        [TestMethod]
        public void TestMarshallId()
        {
            long id = 1;
            string marshalledFile = HtmlMarshalUtil.MarshallId(id);
            long UnmarshallFile = HtmlMarshalUtil.UnMarshallId(marshalledFile);
            Assert.AreEqual(id, UnmarshallFile);
        }

    }
}
