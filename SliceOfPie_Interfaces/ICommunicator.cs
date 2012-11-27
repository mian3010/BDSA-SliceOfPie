using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SliceOfPie_Interfaces
{
    /// <summary>
    /// The interface resposinsible for communicating with the persistence layer.
    /// The persistence layer could be a database or a file.
    /// </summary>
    public interface ICommunicator
    {
        public List<File> UpdateFiles();

        public String GetContent(int id);

        public int SaveContent(int id, String content);
    }
}
