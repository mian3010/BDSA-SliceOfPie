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
        List<FileInstance> testInput = new List<FileInstance>();
        FileInstance testOne;
        FileInstance testTwo;
        FileInstance testThree;
        FileInstance testFour;
        FileInstance testFive;

        /// <summary>
        /// Test method that creates 5 fileInstances with input
        /// </summary>
        /// <returns> List of test files
        [TestMethod]
        public List<FileInstance> FileInstanceTestInput()
        {
            MetaDataType authorType = new MetaDataType();
            authorType.Type = "Author";


            FileMetaData authorTypeValue = new FileMetaData();
            authorTypeValue.value = "morr";
            authorTypeValue.MetaDataType = authorType;


            //Test input for test file
            testOne = new FileInstance();
            testOne.id = 1;
            testOne.File.name = "Document 1";
            testOne.File.serverpath = "Testclass/HTMLMarshallerTest/testOne";
            testOne.File.Project_id = 1;
            testOne.UserEmail = "morr@itu.com";
            testOne.File.FileMetaDatas.Add(authorTypeValue);

            testTwo = new FileInstance();
            testTwo.id = 0;
            testTwo.File.name = "";
            testTwo.File.serverpath = "";
            testTwo.File.Project_id = 0;
            testTwo.UserEmail = "";
            testTwo.File.FileMetaDatas.Add(authorTypeValue);

            testThree = new FileInstance();
            testThree.id = -3;
            testThree.File.name = "33333333333";
            testThree.File.serverpath = "333333333333333333333333333";
            testThree.File.Project_id = 3;
            testThree.UserEmail = "33333333333";
            testThree.File.FileMetaDatas.Add(authorTypeValue);
            testThree.File.FileMetaDatas.Add(authorTypeValue);

            testFour = new FileInstance();
            testFour.id = 2147483647;
            testFour.File.name = "!#¤%&/()=?+}@£½§€{][";
            testFour.File.serverpath = "!#¤%&/()=?+}@£½§€{][";
            testFour.File.Project_id = 2147483647;
            testFour.UserEmail = "!#¤%&/()=?+}@£½§€{][jio";
            testFour.File.FileMetaDatas.Add(authorTypeValue);

            testFive = new FileInstance();
            testFive.id = -2147483648;
            testFive.File.name = "Document 5";
            testFive.File.serverpath = "Testclass/HTMLMarshallerTest/testFive";
            testFive.File.Project_id = 2147483647;
            testFive.UserEmail = "morr@itu.com";
            testFive.File.FileMetaDatas.Add(authorTypeValue);
            
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

            foreach (FileInstance testFileInstance in testInput)
            {
                String marshalledFile = HtmlMarshalUtil.MarshallFile(testFileInstance);
                FileInstance unmarshalledFile = HtmlMarshalUtil.UnmarshallDocument(marshalledFile);

                Assert.AreEqual(unmarshalledFile.id, testFileInstance.id);
                Assert.AreEqual(unmarshalledFile.File.name, testFileInstance.File.name);
                Assert.AreEqual(unmarshalledFile.File.serverpath, testFileInstance.File.serverpath);
                Assert.AreEqual(unmarshalledFile.File.Project_id, testFileInstance.File.Project_id);
                Assert.AreEqual(unmarshalledFile.UserEmail, testFileInstance.UserEmail);
                Assert.AreEqual(unmarshalledFile.File.FileMetaDatas, testFileInstance.File.FileMetaDatas);

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
