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
        bool AddFile(File file);

        bool ModifyFile(File file);

        bool DeleteFile(File file);

        void RenameFile(File file, string newName);

        void MoveFile(File file, string newPath);
    }
}
