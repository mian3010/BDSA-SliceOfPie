using System;
using SliceOfPie_Model.Persistence;

namespace SliceOfPie_Model {
  public interface IAdministrator {


    File GetFile(long id);
    
      /// <summary>
    /// Initializes the Synchroization process.
    /// </summary>
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
