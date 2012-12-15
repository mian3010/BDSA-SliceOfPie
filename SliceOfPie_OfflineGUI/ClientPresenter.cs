using System;
using SliceOfPie_Model;
using System.Windows.Forms;

namespace SliceOfPie_OfflineGUI
{

    /// <summary>
    /// Initiates the Client.
    /// Controls the input from the GUI and redirects the relevant information to the Model.
    /// Binds primarily using delegates and events.
    /// </summary>
    public class ClientPresenter
    {
        private readonly MainWindow _view;
        private readonly OfflineAdministrator _model;

        [STAThread]
        public static void Main(String[] args)
        {

            new ClientPresenter();
          
        }


      private ClientPresenter()
        {
            _model = OfflineAdministrator.GetInstance();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Initiate main window with filepaths from the Model.
            _view = new MainWindow(_model.GetPathsAndIDs());
  
            // Bind update to FileRequested event
            _view.FileRequested += UpdateFileInGui;

            // Bind the closing event of the GUI to the model persisting its data.
            _view.InterfaceClosing += _model.ExitGracefully;

            // Bind the request for a file save in the model.
            _view.FileSaved += _model.SaveFile;

            // Bind a file created request so it add's the file in the model
             _view.FileCreated += _model.AddFile;

           // Bind synchronization requests to the models synchronize with a little logic.
            _view.SynchronizationRequested += SynchronizeFiles;

          _view.TreeRefreshed += GetNewFileStructure;

            Application.Run(_view);

     

        }

        private void GetNewFileStructure(object sender, EventArgs e)
        {
            _view.RefreshFileView(_model.GetPathsAndIDs());
        }

        private void SynchronizeFiles(object sender, EventArgs e)
        {
            _model.Synchronize(_view.CurrentUser);
        }


        /// <summary>
        /// Sets the current document to display in editor from the GUI.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateFileInGui(object sender, FileEventArgs e)
        {
            // TO DO IMPLEMENT DOCUMENT CHECKING
            _view.CurrentDocument = (SliceOfPie_Model.Persistence.Document)_model.GetFile(e.FileId);
        }


    }
}
