using System;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using SliceOfPie_Model;
using SliceOfPie_Model.Persistence;

namespace SliceOfPie_OfflineGUI
{
    public partial class EditorWindow : Form
    {
        public event DocumentHandler DocumentSaved;
        private Document currentDocument;

        public bool KeepAlive { get; set; }

        public EditorWindow()
        {
            InitializeComponent();
            KeepAlive = true;
        }

        public void LoadDocContent(Document doc)
        {
            if (doc != null)
            {
                currentDocument = doc;
                EditorBox.Text = doc.GetContent();
                if (doc.File.name != null)
                    docnameBox.Text = doc.File.name;
            }
            else
                MessageBox.Show("No document selected");
        }


        private void saveButton_Click(object sender, EventArgs e)
        {
            KeepAlive = true;
            currentDocument.Content = EditorBox.Text;
            DocumentSaved(this, currentDocument);
            this.Hide();
        }

        private void docnameBox_TextChanged(object sender, EventArgs e)
        {
            currentDocument.Content = EditorBox.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (currentDocument.File.Changes.Count == 0)
            {
                MessageBox.Show("This document have no history.");
            }

            else
            {
                StringBuilder history = new StringBuilder();
                foreach (Change change in currentDocument.File.Changes)
                {
                    history.Append("User : " + change.User_email + "made a change at :" + change.timestamp + "\n");
                }
                MessageBox.Show(history.ToString());
            }

        }

        private void EditorWindow_Load(object sender, EventArgs e)
        {

        }


        /// <summary>
        /// A method that gracefully exists the program. For now just persists the FileLog. Maybe it should also 
        /// save the current file in the editor (TODO).
        /// </summary>
        /// <param name="e">FormClosingEventArgs (not used)</param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            e.Cancel = KeepAlive;
           // base.OnFormClosing(e);
        }

    }
}