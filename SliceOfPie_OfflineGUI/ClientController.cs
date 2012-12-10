using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SliceOfPie_Model;
using SliceOfPie_OfflineGUI;
using System.Windows.Forms;
using SliceOfPie_Model.Persistence;

namespace SliceOfPie_OfflineGUI
{
    public class ClientController
    {
        private Form3 view;
        private OfflineAdministrator model;

        public static void Main(String[] args)
        {
            new ClientController();
          
        }

        public ClientController()
        {
            model = OfflineAdministrator.GetInstance();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            view = new Form3(model.GetPathsAndIDs());
            view.FileRequested += UpdateFileInGUI;
            view.InterfaceClosing += model.ExitGracefully;
            Application.Run(view);

            File one = new File();
            one.serverpath = @"C:\test\";
            one.name = "add.html";

            model.AddFile(one);

        }



        public void UpdateFileInGUI(object sender, FileEventArgs e)
        {
            // TO DO IMPLEMENT DOCUMENT CHECKING
            view.SetCurrentDocument((Document) model.GetFile(e.FileID));
        }

    }
}
