using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SliceOfPie_Model
{
    /// <summary>
    /// The interface resposinsible for communicating with the persistence layer.
    /// The persistence layer could be a database or a file.
    /// </summary>
    public interface ICommunicator
    {
        int AddFile(File file);
        Boolean SaveFile(File file);
        


        File ChangePath(File old, String newPath);

        List<LogEntry> GetLog();

        void SaveLog();

    }
}
