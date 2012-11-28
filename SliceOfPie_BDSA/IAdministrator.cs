using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SliceOfPie_Model
{
    interface IAdministrator
    {
        IAdministrator GetInstance();

        void GetAllFiles();

        void OpenFile(File file);

        void SaveFile(File file);
    }
}
