using System;
using System.Web.Mvc;
using SliceOfPie_Model.Persistence;

namespace SliceOfPie_OnlineGUI.Controllers {
  public class DefaultController : Controller {
    public ActionResult Index()
    {
      @ViewBag.FileList = Models.FileModel.FileListToTree(Context2.GetFiles("test@example.com"));
      return View();
    }

    public ActionResult Editor() {
      Document documentToEdit = Models.FileModel.GetFile(0);
      @ViewBag.DocumentTitle = documentToEdit.Title;
      @ViewBag.DocumentContent = documentToEdit.Content;
      return View();
    }

    public ActionResult Viewer() {
      FileInstance fileToView = Models.FileModel.GetFile(0);
      @ViewBag.Document = fileToView.ToString();
      return View();
    }

    public ActionResult History() {
      FileInstance fileToView = Models.FileModel.GetFile(0);
      try {
        var documentToView = (Document)fileToView;
        @ViewBag.Title = documentToView.Title;
      } catch (InvalidCastException) {
        @ViewBag.Title = "File: "+fileToView.File.name;
      }
      @ViewBag.DocumentHistory = fileToView.HistoryToString();
      return View();
    }
  }
}
