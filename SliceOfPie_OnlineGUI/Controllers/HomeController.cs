using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SliceOfPie_OnlineGUI.Controllers {
  public class HomeController : Controller {
    public ActionResult Index() {
      return View();
    }

    public ActionResult Editor() {
      return View();
    }
  }
}
