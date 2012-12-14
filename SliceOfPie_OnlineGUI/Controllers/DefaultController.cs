using System.Web.Mvc;
using SliceOfPie_Model;
using SliceOfPie_Model.Persistence;

namespace SliceOfPie_OnlineGUI.Controllers {
  public class DefaultController : Controller {
    public ActionResult Index() {
      //Context2.CleanUp("VerySecretPasswordYoureNeverGonnaGuess");
      @ViewBag.FileList = Models.FileModel.FileListToTree(Context2.GetFiles("testuser0@example.com"));
      return View();
    }

    public ActionResult Editor() {
      if (Request.Params["id"] != null && Request.Params["id"] != "") {
        Document documentToEdit = Models.FileModel.GetDocument(int.Parse(Request.Params["id"]));
        if (documentToEdit != null) {
          @ViewBag.DocumentTitle = documentToEdit.Title;
          @ViewBag.DocumentContent = documentToEdit.Content;
          @ViewBag.Id = documentToEdit.id;
        } else {
          @ViewBag.DocumentTitle = "N/A";
          @ViewBag.DocumentContent = "Document not found or not a document";
        }
      }
      return View();
    }

    public ActionResult Viewer() {
      if (Request.Params["id"] != null && Request.Params["id"] != "") {
        FileInstance fileToView = Models.FileModel.GetDocument(int.Parse(Request.Params["id"])) ??
                                  Models.FileModel.GetFile(int.Parse(Request.Params["id"]));
        @ViewBag.Document = fileToView.ToString();
        @ViewBag.Id = fileToView.id;
      }
      return View();
    }

    public ActionResult History() {
      if (Request.Params["id"] != null && Request.Params["id"] != "") {
        FileInstance fileToView = Models.FileModel.GetDocument(int.Parse(Request.Params["id"])) ??
                                  Models.FileModel.GetFile(int.Parse(Request.Params["id"]));
        @ViewBag.Title = "File: " + fileToView.File.name;
        @ViewBag.DocumentHistory = fileToView.HistoryToString();
      }
      return View();
    }

    [HttpPost]
    [ValidateInput(false)]
    public ActionResult SaveDocument(FormCollection collection) {
      if (Request.Params["id"] != null && Request.Params["id"] != "") {
        Document fileToEdit = Models.FileModel.GetDocument(int.Parse(Request.Params["id"]));
        if (fileToEdit != null) {
          fileToEdit.Title = collection.Get("DocumentTitle");
          fileToEdit.Content = collection.Get("DocumentContent");
          try {
            Context2.ModifyFileInstance(fileToEdit);
            @ViewBag.StatusMessage = "Content saved";
            @ViewBag.MessageType = "status";
          } catch (ConstraintException) {
            @ViewBag.StatusMessage = "Exception during save";
            @ViewBag.MessageType = "error";
          }
        }
      } else {
        @ViewBag.StatusMessage = "Document ID not found. Could not save";
        @ViewBag.MessageType = "error";
      }
      return View("Editor");
    }
  }
}
