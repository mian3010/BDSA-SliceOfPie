using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SliceOfPie_Model.Persistence;

namespace SliceOfPie_Model {
  interface IAdministrator {

    void GetAllFiles();

    void OpenFile(File file);

    void Synchronize();

    void SaveFile(File file);
  }
}
