using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using SliceOfPie_Model.Persistence;
using SliceOfPie_Model;
using SliceOfPie_Model.Exceptions;


namespace SliceOfPie_OfflineGUI {
  public partial class MainWindow : Form {
   
    private readonly Dictionary<String, long> _pathsToId;

    public File CurrentDocument
    { private get;
        set;
    }

    public event FileRequestHandler FileRequested;
    public event FileEventHandler FileSaved;
    public event EventHandler InterfaceClosing, SynchronizationRequested;

    private readonly EditorWindow _editWindow;

    public MainWindow(Dictionary<String, long> fileTree) {
      InitializeComponent();
      _pathsToId = fileTree;

      // We only use 1 instance of our Editor.
      _editWindow = new EditorWindow();
      _editWindow.Hide();
      _editWindow.DocumentSaved += DocumentSavedInEditor;
    }

    private void DocumentSavedInEditor(object sender, string newContent)
    {
        CurrentDocument.Content.Clear();
        CurrentDocument.Content.Append(newContent);

        FileSaved(CurrentDocument);
    }
    
      /// <summary>
      /// Builds a tree structure from the full file paths of all files located in the
      /// FileListHandler. 
      /// </summary>
    private void InitializeTree() {
        var root = new TreeNode("Files");
      TreeNode node = root;
      treeView1.Nodes.Add(root);
        List<String> allPaths = _pathsToId.Keys.ToList();
        allPaths.Sort();
      foreach (string filePath in allPaths) {
        filePath.Split('/').Aggregate(root, AddNode);
      }
      treeView1.ExpandAll();

    }
    private TreeNode AddNode(TreeNode node, string key)
    {
      if (node.Nodes.ContainsKey(key)) {
        return node.Nodes[key];
      }
      return node.Nodes.Add(key, key);
    }

    private void AddNode(XmlNode inXmlNode, TreeNode inTreeNode) {
      // Loop through the XML nodes until the leaf is reached.
      // Add the nodes to the TreeView during the looping process.
      if (inXmlNode.HasChildNodes)
      {
        XmlNodeList nodeList = inXmlNode.ChildNodes;
        int i;
        for (i = 0; i <= nodeList.Count - 1; i++) {
          XmlNode xNode = inXmlNode.ChildNodes[i];
          inTreeNode.Nodes.Add(new TreeNode(xNode.Name));
          TreeNode tNode = inTreeNode.Nodes[i];
          AddNode(xNode, tNode);
        }
      }
      else {
        // Just add our outer text for now
        inTreeNode.Text = (inXmlNode.OuterXml).Trim();
      }
    }

    private void button1_Click(object sender, EventArgs e) {
      InitializeTree();
    }

    private void Form3_Load(object sender, EventArgs e) 
    {

    }

      /// <summary>
      /// Returns the ID of file located in the NOde that's currently
      /// selected in the tree
      /// </summary>
      /// <returns>ID of the file connected to the node</returns>
    private long IdFromCurrentNode()
    {
        var fullPath = new List<String>();
        TreeNode current = treeView1.SelectedNode;
        if (current == null) throw new NoNodeSelectedException("No node selected in TreeView");
        fullPath.Add(current.Name);

        while (current.Parent != null)
        {
            fullPath.Add(current.Parent.Name);
            current = current.Parent;
        }
        fullPath.Reverse();
        String cPath = System.IO.Path.Combine(fullPath.ToArray());
        return _pathsToId[cPath];
    }

    private void treeView1_AfterSelect(object sender, TreeViewEventArgs e) {

    }

      /// <summary>
      /// Calls the FileRequested event which notifies the Controller that a 
      /// File has been requested by the user. Also includes the ID of the file that's requested.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e">Not used, instead uses FileEventArgs which contains a long</param>
    private void button_load_Click(object sender, EventArgs e)
    {
        try
        {
            FileRequested(this, new FileEventArgs(IdFromCurrentNode()));

            _editWindow.LoadDocContent(CurrentDocument);
            _editWindow.Show();
        }
        catch (NoNodeSelectedException ex)
        {
            Console.Out.WriteLine(ex);
        }
    }

    /// <summary>
    /// A method that gracefully exists the program. For now just persists the FileLog. Maybe it should also 
    /// save the current file in the editor (TODO).
    /// </summary>
    /// <param name="e">FormClosingEventArgs (not used)</param>
    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        if (InterfaceClosing != null)
            InterfaceClosing(this, null);

        base.OnFormClosing(e);
    }

    private void button_synchronize_Click(object sender, EventArgs e)
    {
        SynchronizationRequested(this, null);
    }

    public EditorWindow EditorWindow
    {
      get
        {
            throw new NotImplementedException();
        }
      set { throw new NotImplementedException(); }
    }
  }
   
}

