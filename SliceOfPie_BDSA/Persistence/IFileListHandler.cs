using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SliceOfPie_Model
{
    public interface IFileListHandler
    {
        /// <summary>
        /// A property that contains the FileList itself. 
        /// </summary>
        FileList FileList
        {
            get;
        }

        /// <summary>
        /// Saves the FileList property in storage
        /// </summary>
        void PersistFileList();


        Dictionary<String, long> GetPathsWithID();
    }
}
