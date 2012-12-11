using System;
using System.Globalization;
using System.Windows.Forms;
using SliceOfPie_Model;
using SliceOfPie_Model.Persistence;

namespace SliceOfPie_OfflineGUI
{
    public partial class EditorWindow : Form
    {
        public event DocumentHandler DocumentSaved;
       
 
        public EditorWindow()
        {
            InitializeComponent();

            //Makes the html editable
            webBrowser1.Navigate("about:blank");
            Application.DoEvents();
          if (webBrowser1.Document != null)
          {
            webBrowser1.Document.OpenNew(false).Write("<html><body><div id=\"editable\">Edit this text</div></body></html>");

            foreach (HtmlElement el in webBrowser1.Document.All)
            {
              el.SetAttribute("unselectable", "on");
              el.SetAttribute("contenteditable", "false");
            }

            if (webBrowser1.Document.Body != null)
            {
              webBrowser1.Document.Body.SetAttribute("width", Width.ToString(CultureInfo.InvariantCulture) + "px");
              webBrowser1.Document.Body.SetAttribute("height", "100%");
              webBrowser1.Document.Body.SetAttribute("contenteditable", "true");
            }
            webBrowser1.Document.DomDocument.GetType().GetProperty("designMode").SetValue(webBrowser1.Document.DomDocument, "On", null);
          }
          webBrowser1.IsWebBrowserContextMenuEnabled = false;  
        }

        public void LoadDocContent(File doc)
        {
            if (doc != null)
            {
              if (webBrowser1.Document != null) webBrowser1.Document.Write(doc.ToString());
            }
            else
                MessageBox.Show("No document selected");
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }

        private void saveButton_Click(object sender, EventArgs e)
        {
          if (webBrowser1.Document != null)
          {
            String text = webBrowser1.Document.Body.InnerHtml;
            if (DocumentSaved != null)
            {
              DocumentSaved(this, text);
            }
          }
        }
    }
}