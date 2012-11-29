using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SliceOfPie_Model;

namespace SliceOfPie_OnlineGUI.Controllers {
  public class DefaultController : Controller {
    public ActionResult Index() {
      return View();
    }

    public ActionResult Editor() {
      return View();
    }

    public ActionResult Viewer() {
      SliceOfPie_Model.File FileToView = Models.FileModel.getFile(0);
      @ViewBag.Document = FileToView.ToString();
      return View();
    }

    public ActionResult History() {
      File FileToView = Models.FileModel.getFile(0);
      try {
        Document DocumentToView = (Document)FileToView;
        @ViewBag.Title = DocumentToView.Title;
      } catch (InvalidCastException e) {
        @ViewBag.Title = "File: "+FileToView.name;
      }
      @ViewBag.DocumentHistory = FileToView.HistoryToString();
      return View();
    }
  }
}
