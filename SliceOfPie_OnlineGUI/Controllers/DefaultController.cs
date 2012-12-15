using System.Data.Entity;
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
      var id = -1;
      var convert = int.TryParse(@ViewBag.Id, out id);
      if (!convert) id = int.Parse(Request.Params.Get("id"));
      var documentToEdit = Models.FileModel.GetDocument(id);
      if (documentToEdit != null) {
        @ViewBag.DocumentTitle = documentToEdit.Title;
        @ViewBag.DocumentContent = documentToEdit.Content;
        @ViewBag.Id = documentToEdit.id;
      } else {
        @ViewBag.DocumentTitle = "N/A";
        @ViewBag.DocumentContent = "Document not found or not a document";
      }
      return View();
    }

    public ActionResult Viewer() {
      var id = @ViewBag.Id ?? Request.Params["id"];
      if (id != null && id != "") {
        FileInstance fileToView = Models.FileModel.GetDocument(int.Parse(id)) ??
                                  Models.FileModel.GetFile(int.Parse(id));
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
      if (collection.Get("DocumentId") != null && collection.Get("DocumentId") != "") {
        try {
          Models.FileModel.ModifyDocument(int.Parse(collection.Get("DocumentId")), collection.Get("DocumentTitle"), collection.Get("DocumentContent"));
          @ViewBag.StatusMessage = "Content saved";
          @ViewBag.MessageType = "status";
          @ViewBag.Id = collection.Get("DocumentId");
        } catch (ConstraintException) {
          @ViewBag.StatusMessage = "Exception during save";
          @ViewBag.MessageType = "error";
        }
      } else {
        @ViewBag.StatusMessage = "Document ID not found. Could not save";
        @ViewBag.MessageType = "error";
      }
      Editor();
      return View("Editor");
    }
  }
}
