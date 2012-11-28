using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SliceOfPie_Model
{
    class AdministratorOfflineAdapter : IAdministrator
    {

        public IAdministrator GetInstance()
        {
            throw new NotImplementedException();
        }

        public event EventHandler FilesUpdated;

        public event EventHandler ContentAdded;

        public event EventHandler FileSaved;

        public void GetAllFiles()
        {
            throw new NotImplementedException();
        }

        public void OpenFile(int id)
        {
            throw new NotImplementedException();
        }

        public void SaveFile(int id, string content)
        {
            throw new NotImplementedException();
        }
    }
}
