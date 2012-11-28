using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SliceOfPie_BDSA
{
    /// <summary>
    /// Document class. Emulates a simple document.
    /// Author morr&msta.
    /// </summary>
    public class Document
    {

        public String Name
        {
            get;
            set;
        }

        public StringBuilder Content
        {
            get;
            set;
        }

        public List<String> UserIdList
        {
            get;
            set;
        }

        // VersionHistory version.


        static internal Document createTestDocument(String s)
        {
            Document d = new Document();
            d.Content = new StringBuilder(s);
            return d;
        }

    }
}
