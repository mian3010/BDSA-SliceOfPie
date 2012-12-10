using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SliceOfPie_Model.Persistence;

namespace SliceOfPie_Model {
  public interface IAdministrator {

    void GetAllFiles();

    File GetFile(long id);

    void Synchronize();

    void SaveFile(File file);

    /// <summary>
    /// Tells the model to persist any information gracefully.
    /// </summary>
    /// <param name="sender">The object which sends the message</param>
    /// <param name="e">Any arguments concerning the exit</param>
    void ExitGracefully(object sender, EventArgs e);
  }
}
