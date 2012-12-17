using SliceOfPie_Model;
using SliceOfPie_Model.Persistence;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SliceOfPie_Testing
{
    [TestClass]
    public class SimpleMergePolicyTest
    {
        private const char SplitValue = '.';

        [TestMethod]
        public void TestArrayLengths()
        {
            const string test = "This is a test";
            const string test1 = "This is another test.";

            const string test2 = "We are now testing a reguarly sized array. With. More. Than. One. Dot!!!!!!!";

            Assert.AreEqual(test.Split(SplitValue).Length, 1);
            Assert.AreEqual(test1.Split(SplitValue).Length, 2);
            Assert.AreEqual(test2.Split(SplitValue).Length, 6);
        }

        [TestMethod]
        public void SimpleMergePolicyEquals()
        {
            const string original = "This test should have a simple merged output.";
            const string latest = "This test should have a simple merged output.";

            Document result = MergePolicy.Merge(CreateTestDocument(original), CreateTestDocument(latest));
            Assert.AreEqual(result.Content, latest);
        }

        [TestMethod]
        public void MergeWithoutDot()
        {
            const string original = "We are testing content without dots";
            const string latest = "Against something without dots";

            Assert.AreEqual(latest, MergePolicy.Merge(CreateTestDocument(original), CreateTestDocument(latest)).Content);
        }

        [TestMethod]
        public void MergeWithAddToLatest()
        {
            const string original = "We are testing to see if our merger can add lines. It's gonna be exciting.";
            const string latest = "We are testing to see if our merger can add lines. It's gonna be exciting. And we should" +
                                  "throw a party afterwards. Yay!";

            Assert.AreEqual(latest, MergePolicy.Merge(CreateTestDocument(original), CreateTestDocument(latest)).Content);
        }

        [TestMethod]
        public void MergeWithDeletedLatest()
        {
            const string original = "Something has to be deleted. Something has to go from here. And it's youuuuuuuuuuuuuuuuu";
            const string latest = "Something has to be deleted. Something has to go from here.";

            Assert.AreEqual(latest, MergePolicy.Merge(CreateTestDocument(original), CreateTestDocument(latest)).Content);
        }

        [TestMethod]
        public void MergeWithDifferencesOne()
        {
            const string original = "We are testing. That some sentence is removed. From. ORIGINAL. And is now not in the latest";
            const string latest = "We are testing. That some sentence is removed. From. And is now not in the latest";

            Assert.AreEqual(latest, MergePolicy.Merge(CreateTestDocument(original), CreateTestDocument(latest)).Content);
        }

        [TestMethod]
        public void MergeWithDifferencesTwo()
        {
            const string original = "We rode a nice little ride. Then we saw the sunshine. \"Hey, fuck you\", he yelled. And it was good.";
            const string latest = "We rode a nice little ride. Then we saw the sunshine. We ate some ice cream. And drove out to the mountains."
                                  + "\"Hey, fuck you\", he yelled. Fuck you too. And it was good.";

            Assert.AreEqual(latest, MergePolicy.Merge(CreateTestDocument(original), CreateTestDocument(latest)).Content);
        }

        private Document CreateTestDocument(string content)
        {
            var file = new File { };
            return new Document { File = file, Content = content };
        }
    }
}
