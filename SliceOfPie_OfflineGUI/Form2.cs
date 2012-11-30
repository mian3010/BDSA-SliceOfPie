using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SliceOfPie_Model;

namespace SliceOfPie_OfflineGUI
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();

            //Makes the html editable
            webBrowser1.Navigate("about:blank");
            Application.DoEvents();
            webBrowser1.Document.OpenNew(false).Write("<html><body><div id=\"editable\">Edit this text</div></body></html>");

            foreach (HtmlElement el in webBrowser1.Document.All)
            {
                el.SetAttribute("unselectable", "on");
                el.SetAttribute("contenteditable", "false");
            }

            webBrowser1.Document.Body.SetAttribute("width", this.Width.ToString() + "px");
            webBrowser1.Document.Body.SetAttribute("height", "100%");
            webBrowser1.Document.Body.SetAttribute("contenteditable", "true");
            webBrowser1.Document.DomDocument.GetType().GetProperty("designMode").SetValue(webBrowser1.Document.DomDocument, "On", null);
            webBrowser1.IsWebBrowserContextMenuEnabled = false;  
        }

        public void LoadDocContent(Document doc)
        {
            if (doc != null)
                webBrowser1.Document.Write(doc.ToString());
            else
                MessageBox.Show("No document selected");
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }
    }
}
