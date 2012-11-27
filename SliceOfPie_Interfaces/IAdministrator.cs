using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SliceOfPie_Interfaces
{
    public interface IAdministrator
    {
        public delegate void EventHandler(object sender, EventArgs e);

        public event EventHandler FilesUpdated, ContentAdded, FileSaved;

        public void GetAllFiles();

        public void OpenFile(int id);

        public void SaveFile(int id, String content);
    }
}
