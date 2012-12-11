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

    /// <summary>
    /// Initiates the Client.
    /// Controls the input from the GUI and redirects the relevant information to the Model.
    /// Binds primarily using delegates and events.
    /// </summary>
    public class ClientPresenter
    {
        private readonly MainWindow view;
        private readonly OfflineAdministrator model;

        public static void Main(String[] args)
        {
            new ClientPresenter();
          
        }


      private ClientPresenter()
        {
            model = OfflineAdministrator.GetInstance();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Initiate main window with filepaths from the Model.
            view = new MainWindow(model.GetPathsAndIDs());
  
            // Bind update to FileRequested event
            view.FileRequested += UpdateFileInGui;

            // Bind the closing event of the GUI to the model persisting its data.
            view.InterfaceClosing += model.ExitGracefully;

            // Bind the request for a file save in the model.
            view.FileSaved += model.SaveFile;

            view.SynchronizationRequested += SynchronizeFiles;

            Application.Run(view);

     

        }

        private void SynchronizeFiles(object sender, EventArgs e)
        {
            model.Synchronize();
        }


        /// <summary>
        /// Sets the current document to display in editor from the GUI.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateFileInGui(object sender, FileEventArgs e)
        {
            // TO DO IMPLEMENT DOCUMENT CHECKING
            view.CurrentDocument = model.GetFile(e.FileId);
        }

        public MainWindow MainWindow
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

    }
}
