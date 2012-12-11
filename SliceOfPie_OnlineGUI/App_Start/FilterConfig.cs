using System.Web.Mvc;

namespace SliceOfPie_OnlineGUI {
  public class FilterConfig {
    public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
      filters.Add(new HandleErrorAttribute());
    }
  }
}