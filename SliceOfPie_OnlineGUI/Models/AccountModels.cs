using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Security;
using SliceOfPie_Model.Persistence;

namespace SliceOfPie_OnlineGUI.Models {
  public class LoginModel {
    [Required]
    [Display(Name = "Email")]
    public string UserEmail { get; set; }

    public static bool LoginUser(string email) {
      if (Context.GetUser(email) != null) {
        FormsAuthentication.SetAuthCookie(email, true);
        return true;
      }
      throw new MemberAccessException("Could not login");
    }
    public static void LogOut() {
      FormsAuthentication.SignOut();
    }
  }

  public class RegisterModel {
    [Required]
    [Display(Name = "Email")]
    public string UserEmail { get; set; }
    public static int CreateUser(string email) {
      return Context.AddUser(new User { email = email });
    }


  }
}
