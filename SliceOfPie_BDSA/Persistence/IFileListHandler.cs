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

        /// <summary>
        /// Returns a dictionary containing paths of all file in the system as key and their ID's as value.
        /// </summary>
        /// <returns>The dictionary mentioned in the Summary</returns>
        Dictionary<String, long> GetPathsWithId();
    }
}
