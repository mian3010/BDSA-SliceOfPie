using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using SliceOfPie_Model.Persistence;
using SliceOfPie_Model;
using SliceOfPie_Model.Exceptions;


namespace SliceOfPie_OfflineGUI {
  public partial class MainWindow : Form
  {
      public const char Separator = '\\';

      private readonly Dictionary<String, int> _pathsToId;

      public Document CurrentDocument {
      private get;
      set;
    }

    private TreeNode _root;

    public event FileInstanceRequestHandler FileRequested;
    public event FileInstanceEventHandler FileSaved, FileCreated;
    public event EventHandler InterfaceClosing, SynchronizationRequested;

    private EditorWindow _editWindow;

    public MainWindow(Dictionary<String, int> fileTree) {
      InitializeComponent();
      _pathsToId = fileTree;
      InitializeTree();
     
    }

    private void DocumentSavedInEditor(Document doc)
    {
       CurrentDocument = doc;

       FileSaved(doc);
    }

      private void InitializeTree()
      {
          _root = new TreeNode("Files");
          TreeNode node = _root;
          treeView1.Nodes.Add(_root);

          foreach (string filePath in _pathsToId.Keys.ToList()) 
          {
              node = _root;
              foreach (string pathBits in filePath.Split(Separator))
              {
                  node = AddNode(node, pathBits);
              }
          }
      }




      private TreeNode AddNode(TreeNode node, string key)
        {
    if (node.Nodes.ContainsKey(key))
    {
        return node.Nodes[key];
    }
    else
    {
        return node.Nodes.Add(key, key);
    }
        }

    private void button1_Click(object sender, EventArgs e) {
      InitializeTree();
    }

    private void Form3_Load(object sender, EventArgs e) {

    }

    /// <summary>
    /// Returns the ID of file located in the NOde that's currently
    /// selected in the tree
    /// </summary>
    /// <returns>ID of the file connected to the node</returns>
    private int IdFromCurrentNode() {
      
      TreeNode current = treeView1.SelectedNode;
      if (current == null) throw new NoNodeSelectedException("No node selected in TreeView");
      
        try
        {
            return _pathsToId[PathOfNode(current)];

        }
        catch (Exception e)
        {
            throw new NotADocumentException();
        }
    }

    private String PathOfNode(TreeNode node)
    {
        var fullPath = new List<String>();
        fullPath.Add(node.Name);

        while (node.Parent != null && node.Parent != _root)
        {
            fullPath.Add(node.Parent.Name);
            node = node.Parent;
        }
        fullPath.Reverse();
        String s = null;
        foreach (String ss in fullPath)
        {
            s += ss + Separator;
        }
        return s.Substring(0,s.Length -1);
    }

    private void treeView1_AfterSelect(object sender, TreeViewEventArgs e) {

    }

      /// <summary>
      /// Calls the FileRequested event which notifies the Controller that a 
      /// File has been requested by the user. Also includes the ID of the file that's requested.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e">Not used, instead uses FileEventArgs which contains a int</param>
    private void button_load_Click(object sender, EventArgs e)
    {
        TryCreateEditor();
          try
        {
            _editWindow.NewDocument = false;
            FileRequested(this, new FileEventArgs(IdFromCurrentNode()));
            _editWindow.LoadDocContent(CurrentDocument);
            _editWindow.Show();
        }
        catch (NoNodeSelectedException ex)
        {
            Console.Out.WriteLine(ex);
        }

    }


    private void RefreshTree()
    {
        treeView1.Nodes.Clear();
        InitializeTree();
    }

      private void TryCreateEditor()
      {
          if (_editWindow == null)
          {
              // We only use 1 instance of our Editor.
              _editWindow = new EditorWindow();
              _editWindow.Hide();
              _editWindow.DocumentSaved += DocumentSavedInEditor;
              _editWindow.DocumentCreated += FileCreatedInEditor;
          }
        
      }

      private void FileCreatedInEditor(Document doc)
      {
          FileCreated(doc);
      }



    /// <summary>
    /// A method that gracefully exists the program. For now just persists the FileLog. Maybe it should also 
    /// save the current file in the editor (TODO).
    /// </summary>
    /// <param name="e">FormClosingEventArgs (not used)</param>
    protected override void OnFormClosing(FormClosingEventArgs e) {
      if (InterfaceClosing != null)
        InterfaceClosing(this, null);

      base.OnFormClosing(e);
    }

    private void button_synchronize_Click(object sender, EventArgs e) {
      SynchronizationRequested(this, null);
    }

    private void button_Create_Click(object sender, EventArgs e)
    {
        CurrentDocument = new Document();
        CurrentDocument.File = new File();
        String path = "";
        TreeNode current = treeView1.SelectedNode.Parent ?? _root;
        if (current == _root)
        {
            path = current.FirstNode.Text;
        }
        else
        {
            try
            {
                IdFromCurrentNode();
                path = PathOfNode(current);
            }
            catch (NotADocumentException ex)
            {
                if (current.Parent != null)
                    path = PathOfNode(treeView1.SelectedNode.Parent);

            }
        }
        CurrentDocument.path = path;

        _editWindow.NewDocument = true;
         
        TryCreateEditor();
        _editWindow.LoadDocContent(CurrentDocument);
        _editWindow.Show();
        
    }
  }

}

