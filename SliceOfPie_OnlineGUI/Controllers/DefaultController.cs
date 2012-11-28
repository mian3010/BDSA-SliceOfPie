using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SliceOfPie_OnlineGUI.Controllers {
  public class DefaultController : Controller {
    public ActionResult Index() {
      return View();
    }

    public ActionResult Editor() {
      return View();
    }

    public ActionResult Viewer() {
      File FileToView = Models.FileModel.getFile(0);
      @ViewBag.Document = FileToView.ToString();
      return View();
    }
  }
}
