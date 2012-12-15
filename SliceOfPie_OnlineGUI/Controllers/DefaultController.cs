using System.Web;
using System.Web.Mvc;
using SliceOfPie_Model;
using SliceOfPie_Model.Persistence;

namespace SliceOfPie_OnlineGUI.Controllers {
  public class DefaultController : Controller {
    public ActionResult Index() {
      string email = System.Web.HttpContext.Current.User.Identity.Name;
      if (!string.IsNullOrEmpty(email)) {
        //Context2.CleanUp("VerySecretPasswordYoureNeverGonnaGuess");
        @ViewBag.FileList = Models.FileModel.FileListToTree(Context2.GetFiles(email));
        return View();
      }
      return RedirectToAction("Login", "Account");
    }

    public ActionResult Creator() {
      string email = System.Web.HttpContext.Current.User.Identity.Name;
      if (!string.IsNullOrEmpty(email)) {
        return View();
      }
      return RedirectToAction("Login", "Account");
    }
    public ActionResult Editor() {
      string email = System.Web.HttpContext.Current.User.Identity.Name;
      if (!string.IsNullOrEmpty(email)) {
        int id;
        var convert = int.TryParse((string)RouteData.Values["id"], out id);
        if (convert || Request.Params.Get("id") != null) {
          if (!convert) id = int.Parse(Request.Params.Get("id"));
          var documentToEdit = Models.FileModel.GetDocument(id);
          if (documentToEdit != null) {
            @ViewBag.DocumentTitle = documentToEdit.Title;
            @ViewBag.DocumentContent = documentToEdit.Content;
            @ViewBag.Id = documentToEdit.id;
            return View();
          }
        }
        @ViewBag.DocumentTitle = "N/A";
        @ViewBag.DocumentContent = "Document not found or not a document";
        return View();
      }
      return RedirectToAction("Login", "Account");
    }

    public ActionResult Viewer() {
      string email = System.Web.HttpContext.Current.User.Identity.Name;
      if (!string.IsNullOrEmpty(email)) {
        var id = @ViewBag.Id ?? Request.Params["id"];
        if (id != null && id != "") {
          FileInstance fileToView = Models.FileModel.GetDocument(int.Parse(id)) ??
                                    Models.FileModel.GetFile(int.Parse(id));
          @ViewBag.Document = fileToView.ToString();
          @ViewBag.Id = fileToView.id;
        }
        return View();
      }
      return RedirectToAction("Login", "Account");
    }

    public ActionResult History() {
      string email = System.Web.HttpContext.Current.User.Identity.Name;
      if (!string.IsNullOrEmpty(email)) {
        if (Request.Params["id"] != null && Request.Params["id"] != "") {
          FileInstance fileToView = Models.FileModel.GetDocument(int.Parse(Request.Params["id"])) ??
                                    Models.FileModel.GetFile(int.Parse(Request.Params["id"]));
          @ViewBag.Title = "File: " + fileToView.File.name;
          @ViewBag.DocumentHistory = fileToView.HistoryToString();
        }
        return View();
      }
      return RedirectToAction("Login", "Account");
    }

    public ActionResult Sharer() {
      string email = System.Web.HttpContext.Current.User.Identity.Name;
      if (!string.IsNullOrEmpty(email)) {
        int id;
        var convert = int.TryParse((string)RouteData.Values["id"], out id);
        if (convert || Request.Params.Get("id") != null) {
          if (!convert) id = int.Parse(Request.Params.Get("id"));
          @ViewBag.Id = id;
          @ViewBag.Authors = Models.FileModel.GetAuthors(id);
        }
        return View();
      }
      return RedirectToAction("Login", "Account");
    }

    [HttpPost]
    [ValidateInput(false)]
    public ActionResult SaveDocument(FormCollection collection) {
      string email = System.Web.HttpContext.Current.User.Identity.Name;
      if (!string.IsNullOrEmpty(email)) {
        if (collection.Get("DocumentId") != null && collection.Get("DocumentId") != "") {
          try {
            Models.FileModel.ModifyDocument(int.Parse(collection.Get("DocumentId")), collection.Get("DocumentTitle"),
                                            collection.Get("DocumentContent"));
            @ViewBag.StatusMessage = "Content saved";
            @ViewBag.MessageType = "status";
          } catch (ConstraintException) {
            @ViewBag.StatusMessage = "Exception during save";
            @ViewBag.MessageType = "error";
          }
        } else {
          @ViewBag.StatusMessage = "Document ID not found. Could not save";
          @ViewBag.MessageType = "error";
        }
        return RedirectToAction("Editor", "Default", new { id = collection.Get("DocumentId") });
      }
      return RedirectToAction("Login", "Account");
    }

    [HttpPost]
    [ValidateInput(false)]
    public ActionResult CreateDocument(FormCollection collection) {
      string email = System.Web.HttpContext.Current.User.Identity.Name;
      if (!string.IsNullOrEmpty(email)) {
        try {
          var instance = Models.FileModel.CreateDocument(email, collection.Get("FileName"), decimal.Parse(collection.Get("FileVersion")), collection.Get("DocumentPath"), collection.Get("DocumentTitle"), collection.Get("DocumentContent"));
          @ViewBag.StatusMessage = "Content saved";
          @ViewBag.MessageType = "status";
          @ViewBag.Id = collection.Get("DocumentId");
          return RedirectToAction("Editor", "Default", new { id = instance.id });
        } catch (ConstraintException) {
          @ViewBag.StatusMessage = "Exception during save";
          @ViewBag.MessageType = "error";
        }
        return RedirectToAction("Creator", "Default");
      }
      return RedirectToAction("Login", "Account");
    }

    [HttpPost]
    [ValidateInput(false)]
    public ActionResult AddAuthor(FormCollection collection) {
      string email = System.Web.HttpContext.Current.User.Identity.Name;
      if (!string.IsNullOrEmpty(email))
      {
        Models.FileModel.AddAuthor(collection.Get("AuthorEmail"), int.Parse(collection.Get("DocumentId")));
        return RedirectToAction("Sharer", "Default", new { id = collection.Get("DocumentId") });
      }
      return RedirectToAction("Login", "Account");
    }
  }
}
