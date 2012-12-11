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
    private readonly File _file;

    /// <summary>
    /// Constructor. 
    /// This will call a method upon the HttpProcessor, to tell wether this was succesful
    /// </summary>
    /// <param name="file"></param>
    /// <param name="hp"></param>
    public FileReceiver(File file, HttpProcessor hp) {
      _file = file;
      _hp = hp;
    }

    public void Receive() {
      long success = -2; // will be returned
      // success == id if succes
      // -1 if failed
      // -2 if reject

      // Determin new or mod
      // If new file
      if (RequestHandler.Instance.PendingNewFileList.Contains(_file.id)) {
        Context.SaveFile(_file);

        // else if mod file
      } else if (RequestHandler.Instance.PendingModFileList.ContainsKey(_file.id)) {
        try {
          //Document DocumentFromFile = Document.CreateDocument(file);
          //Document DocumentFromDb = Document.CreateDocument(Context.GetFile(file.id));
          //Document DocumentMerged = MergePolicy.Merge(DocumentFromFile, DocumentFromDb);
        } catch (NotADocumentException e) { } catch (MergeImpossibleException e) { }
        Context.UpdateFile(_file);

        // else reject
      }
      // hp.something(succes);
    }
  }
}
