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

        // else if mod file
      } else if (RequestHandler.Instance.PendingModFileList.ContainsKey(_file.id)) {
        try {
          var documentFromFile = Document.CreateDocument(_file);
          var documentFromDb = Document.CreateDocument(Context2.GetFileInstance(_file.id));
          _file = MergePolicy.Merge(documentFromFile, documentFromDb);
        } catch (NotADocumentException) { } catch (MergeImpossibleException) { }
        success = Context2.AddFileInstance(_file);

        // else reject
      }
      _hp.RecieveConfirmation(success);
    }
  }
}
