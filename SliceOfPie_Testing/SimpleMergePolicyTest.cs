using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using SliceOfPie_Model;
using SliceOfPie_Model.Persistence;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SliceOfPie_Testing
{
    [TestClass]
    public class SimpleMergePolicyTest
    {
        static char splitValue = '.';

        [TestMethod]
        public void TestArrayLengths()
        {
            String test = "This is a test";
            String test1 = "This is another test.";

            String test2 = "We are now testing a reguarly sized array. With. More. Than. One. Dot!!!!!!!";

            Assert.AreEqual(test.Split(splitValue).Length,1);
            Assert.AreEqual(test1.Split(splitValue).Length, 2);
            Assert.AreEqual(test2.Split(splitValue).Length, 6);
        }

        [TestMethod]
        public void SimpleMergePolicyEquals()
        {
            String original = "This test should have a simple merged output.";
            String latest = "This test should have a simple merged output.";

            Document result = SimpleMergePolicy.Merge(Document.CreateTestDocument(original), Document.CreateTestDocument(latest));
            Assert.AreEqual(result.Content.ToString(), latest);
        }

        [TestMethod]
        public void MergeWithAddToLatest()
        {
            String original = "We are testing to see if our merger can add lines. It's gonna be exciting.";
            String latest = "We are testing to see if our merger can add lines. It's gonna be exciting. And we should" +
                            "throw a party afterwards. Yay!";

            Assert.AreEqual(SimpleMergePolicy.Merge(Document.CreateTestDocument(original), Document.CreateTestDocument(latest)).Content
                .ToString(), latest);
        }

        [TestMethod]
        public void MergeWithDeletedLatest()
        {
            String original = "Something has to be deleted. Something has to go from here. And it's youuuuuuuuuuuuuuuuu";
            String latest = "Something has to be deleted. Something has to go from here.";

            Assert.AreEqual(SimpleMergePolicy.Merge(Document.CreateTestDocument(original), Document.CreateTestDocument(latest)).Content
                .ToString(), latest);
        }

        [TestMethod]
        public void MergeWithDifferencesOne()
        {
            String original = "We are testing. That some sentence is removed. From. ORIGINAL. And is now not in the latest";
            String latest = "We are testing. That some sentence is removed. From. And is now not in the latest";

            Assert.AreEqual(SimpleMergePolicy.Merge(Document.CreateTestDocument(original), Document.CreateTestDocument(latest)).Content
                .ToString(), latest);
        }

        [TestMethod]
        public void MergeWithDifferencesTwo()
        {
            String original = "We rode a nice little ride. Then we saw the sunshine. \"Hey, fuck you\", he yelled. And it was good.";
            String latest = "We rode a nice little ride. Then we saw the sunshine. We ate some ice cream. And drove out to the mountains."
                               + "\"Hey, fuck you\", he yelled. Fuck you too. And it was good.";

            Assert.AreEqual(SimpleMergePolicy.Merge(Document.CreateTestDocument(original), Document.CreateTestDocument(latest)).Content
                    .ToString(), latest);
        }
    }
}
