using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SliceOfPie_Model.Persistence;

namespace SliceOfPie_OnlineGUI.Controllers {
  public class DefaultController : Controller {
    public ActionResult Index() {
      return View();
    }

    public ActionResult Editor() {
      Document documentToEdit = Models.FileModel.GetFile(0);
      @ViewBag.DocumentTitle = documentToEdit.Title;
      @ViewBag.DocumentContent = documentToEdit.Content;
      return View();
    }

    public ActionResult Viewer() {
      File fileToView = Models.FileModel.GetFile(0);
      @ViewBag.Document = fileToView.ToString();
      return View();
    }

    public ActionResult History() {
      File fileToView = Models.FileModel.GetFile(0);
      try {
        Document documentToView = (Document)fileToView;
        @ViewBag.Title = documentToView.Title;
      } catch (InvalidCastException e) {
        @ViewBag.Title = "File: "+fileToView.name;
      }
      @ViewBag.DocumentHistory = fileToView.HistoryToString();
      return View();
    }
  }
}
