using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SliceOfPie_Model {
  interface IAdministrator {

    void GetAllFiles();

    void OpenFile(File file);

    void Synchronize();

    void SaveFile(File file);
  }
}
