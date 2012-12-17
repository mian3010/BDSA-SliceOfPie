using System;
using SliceOfPie_Model.Persistence;

namespace SliceOfPie_Model {
  public interface IAdministrator {


    FileInstance GetFile(int id);
    
      /// <summary>
    /// Initializes the Synchroization process.
    /// </summary>
    void Synchronize(String userEmail);


    void SaveFile(FileInstance file);

    /// <summary>
    /// Tells the model to persist any information gracefully.
    /// </summary>
    /// <param name="sender">The object which sends the message</param>
    /// <param name="e">Any arguments concerning the exit</param>
    void ExitGracefully(object sender, EventArgs e);
  }
}
