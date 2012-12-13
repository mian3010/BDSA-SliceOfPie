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
        //List to hold the test input from file instances
        List<FileInstance> TestInput = new List<FileInstance>();


        //New File for test input.
        File FileOne = new File();
/*        File FileThree = new File();
        File FileFour = new File();
        File FileFive = new File();
*/        
        
        //File Instances variabels for test input.
        FileInstance TestOne;
/*        FileInstance TestTwo;
        FileInstance TestThree;
        FileInstance TestFour;
        FileInstance TestFive;
*/
        /// <summary>
        /// Test method that creates 5 fileInstances with input
        /// </summary>
        /// <returns> List of test files
        [TestMethod]
        public List<FileInstance> FileInstanceTestInput()
        {
            //Creates a new author meta data type
            MetaDataType authorType = new MetaDataType();
            authorType.Type = "Author";

            //Creates a value for the author type
            FileMetaData authorTypeValue = new FileMetaData();
            authorTypeValue.value = "morr";
            authorTypeValue.MetaDataType = authorType;

            //Test input for test file instance
            TestOne = new FileInstance();
            TestOne.File = FileOne;
            TestOne.id = 1;
            TestOne.File.name = "Document 1";
            TestOne.File.serverpath = "Testclass/HTMLMarshallerTest/testOne";
            TestOne.File.Project_id = 1;
            TestOne.UserEmail = "morr@itu.com";
            TestOne.File.FileMetaDatas.Add(authorTypeValue);
/*
            TestTwo = new FileInstance();
            TestTwo.File = FileTwo;
            TestTwo.id = 0;
            TestTwo.File.name = "";
            TestTwo.File.serverpath = "";
            TestTwo.File.Project_id = 0;
            TestTwo.UserEmail = "";
            TestTwo.File.FileMetaDatas.Add(authorTypeValue);

            TestThree = new FileInstance();
            TestThree.File = FileThree;
            TestThree.id = -3;
            TestThree.File.name = "33333333333";
            TestThree.File.serverpath = "333333333333333333333333333";
            TestThree.File.Project_id = 3;
            TestThree.UserEmail = "33333333333";
            TestThree.File.FileMetaDatas.Add(authorTypeValue);
            TestThree.File.FileMetaDatas.Add(authorTypeValue);

            TestFour = new FileInstance();
            TestFour.File = FileFour;
            TestFour.id = 2147483647;
            TestFour.File.name = "!#¤%&/()=?+}@£½§€{][";
            TestFour.File.serverpath = "!#¤%&/()=?+}@£½§€{][";
            TestFour.File.Project_id = 2147483647;
            TestFour.UserEmail = "!#¤%&/()=?+}@£½§€{][jio";
            TestFour.File.FileMetaDatas.Add(authorTypeValue);

            TestFive = new FileInstance();
            TestFive.File = FileFive;
            TestFive.id = -2147483648; 
            TestFive.File.name = "Document 5";
            TestFive.File.serverpath = @"Testclass\HTMLMarshallerTest\testFive";
            TestFive.File.Project_id = 2147483647;
            TestFive.UserEmail = "morr@itu.com";
            TestFive.File.FileMetaDatas.Add(authorTypeValue);
*/            
            //test files added to list.
            TestInput.Add(TestOne);
/*            TestInput.Add(TestTwo);
            TestInput.Add(TestThree);
            TestInput.Add(TestFour);
            TestInput.Add(TestFive);
*/
            return TestInput;
        }

        [TestMethod]
        public void TestMarshalling()
        {

            foreach (FileInstance testFileInstance in TestInput)
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
            int id = 1;
            string marshalledFile = HtmlMarshalUtil.MarshallId(id);
            int UnmarshallFile = HtmlMarshalUtil.UnMarshallId(marshalledFile);
            Assert.AreEqual(id, UnmarshallFile);
        }

    }
}
