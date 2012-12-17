using SliceOfPie_Model;
using SliceOfPie_Model.Persistence;

namespace SliceOfPie_Server {
  /// <summary>
  /// This class handles file receives.
  /// This will usually be started in a seperate thread.
  /// Author: Claus35-DK - clih@itu.dk
  /// Author: mian3010 - msoa@itu.dk
  /// </summary>
  class FileReceiver {
    private HttpProcessor _hp;
    private FileInstance _file;

    /// <summary>
    /// Constructor. 
    /// This will call a method upon the HttpProcessor, to tell wether this was succesful
    /// </summary>
    /// <param name="file"></param>
    /// <param name="hp"></param>
    public FileReceiver(FileInstance file, HttpProcessor hp) {
      _file = file;
      _hp = hp;
    }

    public void Receive() {
      FileInstance success = null; // will be returned
      // success == id if succes
      // -1 if failed
      // -2 if reject

      // Determin new or mod
      // If new file
      if (RequestHandler.Instance.PendingNewFileList.Contains(_file.id)) {
        success = Context2.AddFileInstance(_file);
        RequestHandler.Instance.PendingNewFileList.Remove(_file.id);

        // else if mod file
      } else if (RequestHandler.Instance.PendingModFileList.ContainsKey(_file.id)) {
        try {
          var documentFromFile = (Document)_file;
          var documentFromDb = Context2.GetDocument(_file.id);
          _file = MergePolicy.Merge(documentFromDb, documentFromFile);
        } catch (NotADocumentException) { } catch (MergeImpossibleException) { }
        if (_file.File != null && _file.File.id >= 0) _file.File_id = _file.File.id; //Dont add if id is already set
        if (_file.User != null && _file.User.email != null && _file.User.email != "") _file.User_email = _file.User.email; //Dont add if id is already set
        success = Context2.AddFileInstance(_file);
        RequestHandler.Instance.PendingModFileList.Remove(_file.id);
        // else reject
      }
      _hp.RecieveConfirmation(success);
    }
  }
}
