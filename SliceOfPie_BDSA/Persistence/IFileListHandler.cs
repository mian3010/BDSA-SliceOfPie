using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SliceOfPie_Model
{
    interface IFileListHandler
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
        /// Supplies a list of the files in XML format.
        /// </summary>
        /// <returns>A string containing the XML</returns>
        String FilesAsXML();
    }
}
