using System;
using SliceOfPie_Model.Persistence;

namespace SliceOfPie_Model {
  public interface IAdministrator {

    /// <summary>
    /// Retrieves a file according to it's id. 
    /// </summary>
    /// <param name="id">The id of the file to get</param>
    /// <returns></returns>
    FileInstance GetFile(int id);
    
      /// <summary>
    /// Initializes the Synchronization process.
    /// </summary>
    void Synchronize(String userEmail);

    /// <summary>
    /// Saves a file to storage. 
    /// </summary>
    /// <param name="file"></param>
    void SaveFile(FileInstance file);

    /// <summary>
    /// Tells the model to persist any information gracefully.
    /// </summary>
    /// <param name="sender">The object which sends the message</param>
    /// <param name="e">Any arguments concerning the exit</param>
    void ExitGracefully(object sender, EventArgs e);
  }
}
