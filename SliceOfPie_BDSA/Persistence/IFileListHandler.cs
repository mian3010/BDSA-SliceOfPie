using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SliceOfPie_Model
{
    interface IFileListHandler
    {
        FileList FileList
        {
            get;
        }

        void PersistFileList();

        String FilesAsXML();
    }
}
