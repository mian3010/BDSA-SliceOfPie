using System;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;
using SliceOfPie_OnlineGUI.Models;

namespace SliceOfPie_OnlineGUI.Controllers {
  [Authorize]
  public class AccountController : Controller {
    //
    // GET: /Account/Login

    [AllowAnonymous]
    public ActionResult Login() {
      return View();
    }

    //
    // POST: /Account/Login

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public ActionResult Login(LoginModel model) {
      if (ModelState.IsValid && LoginModel.LoginUser(model.UserEmail)) {
        return RedirectToAction("Index", "Default");
      }

      // If we got this far, something failed, redisplay form
      @ViewBag.StatusMessage = "The email provided does not exist";
      return View(model);
    }

    //
    // POST: /Account/LogOff

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult LogOff() {
      LoginModel.LogOut();

      return RedirectToAction("Login", "Account");
    }

    //
    // GET: /Account/Register

    [AllowAnonymous]
    public ActionResult Register() {
      return View();
    }

    //
    // POST: /Account/Register

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public ActionResult Register(RegisterModel model) {
      if (ModelState.IsValid) {
        // Attempt to register the user
        try {
          RegisterModel.CreateUser(model.UserEmail);
          LoginModel.LoginUser(model.UserEmail);
          return RedirectToAction("Index", "Default");
        } catch (MembershipCreateUserException e) {
          @ViewBag.StatusMessage = e.ToString();
        } catch (MemberAccessException e) {
          @ViewBag.StatusMessage = e.ToString();
        }
      }

      // If we got this far, something failed, redisplay form
      return View(model);
    }

  }
}
