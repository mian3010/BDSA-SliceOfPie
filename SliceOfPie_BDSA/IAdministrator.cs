using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SliceOfPie_Model
{
    public interface IAdministrator
    {
        IAdministrator GetInstance();

        event EventHandler FilesUpdated, ContentAdded, FileSaved;

        void GetAllFiles();

        void OpenFile(int id);

        void SaveFile(int id, String content);
    }
}
