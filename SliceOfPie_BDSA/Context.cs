using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SliceOfPie_Model {
  /// <summary>
  /// Context has methods for operating on the database (in this case ITU MySQL).
  /// </summary>
  public static class Context {

    public static SliceOfPie_OnlineGUI.User[] GetUsers() {
      var DbContext = new SliceOfPie_OnlineGUI.SliceOfLifeEntities();
      var query = from u in DbContext.Users
                  select u;
      return query.ToArray<SliceOfPie_OnlineGUI.User>();
    }

    public static SliceOfPie_OnlineGUI.User GetUsers(string email) {
      var DbContext = new SliceOfPie_OnlineGUI.SliceOfLifeEntities();
      var query = from u in DbContext.Users
                  where u.email == email
                  select u;
      return (SliceOfPie_OnlineGUI.User)query;
    }

    // Does this return object hold a list of MetaDataTypes?
    public static SliceOfPie_OnlineGUI.FileMetaData[] GetMetaData(long FileId) {
      var DbContext = new SliceOfPie_OnlineGUI.SliceOfLifeEntities();
      var query = from meta in DbContext.FileMetaDatas
                  where meta.File_id == FileId
                  select meta;
      return query.ToArray<SliceOfPie_OnlineGUI.FileMetaData>();
    }

    public static void Main(string args[]) {

    }
  }
}
