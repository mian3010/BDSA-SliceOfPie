using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using SliceOfPie_Model.Persistence;
using SliceOfPie_Model;


namespace SliceOfPie_OfflineGUI {
  public partial class Form3 : Form {
   
    private Dictionary<String, long> paths;
    private File currentDocument; 

    public event FileRequestHandler FileRequested;
    public event EventHandler InterfaceClosing;
    public delegate void FileRequestHandler(object sender, FileEventArgs args);

    public Form3(Dictionary<String, long> FileTree) {
      InitializeComponent();
      paths = FileTree;
    }

    public void SetCurrentDocument(File doc)
    {
        currentDocument = doc;
    }

    private void InitializeTree() {
        TreeNode root = new TreeNode("Files");
      TreeNode node = root;
      treeView1.Nodes.Add(root);
        List<String> allPaths = paths.Keys.ToList();
        allPaths.Sort();
      foreach (string filePath in allPaths) {
        node = root;
        foreach (string pathBits in filePath.Split('/')) {
          node = AddNode(node, pathBits);
        }
      }
      treeView1.ExpandAll();

    }
    private TreeNode AddNode(TreeNode node, string key) {
      if (node.Nodes.ContainsKey(key)) {
        return node.Nodes[key];
      } else {
        return node.Nodes.Add(key, key);
      }
    }

    private void AddNode(XmlNode inXmlNode, TreeNode inTreeNode) {
      XmlNode xNode;
      TreeNode tNode;
      XmlNodeList nodeList;
      int i;

      // Loop through the XML nodes until the leaf is reached.
      // Add the nodes to the TreeView during the looping process.
      if (inXmlNode.HasChildNodes) {
        nodeList = inXmlNode.ChildNodes;
        for (i = 0; i <= nodeList.Count - 1; i++) {
          xNode = inXmlNode.ChildNodes[i];
          inTreeNode.Nodes.Add(new TreeNode(xNode.Name));
          tNode = inTreeNode.Nodes[i];
          AddNode(xNode, tNode);
        }
      } else {
        // Here you need to pull the data from the XmlNode based on the
        // type of node, whether attribute values are required, and so forth.
        inTreeNode.Text = (inXmlNode.OuterXml).Trim();
      }
    }

    private void button1_Click(object sender, EventArgs e) {
      InitializeTree();
    }

    private void Form3_Load(object sender, EventArgs e) {

    }

    private long IDFromCurrentNode()
    {
        List<String> fullPath = new List<String>();
        TreeNode current = treeView1.SelectedNode;
        fullPath.Add(current.Name);
        while (current.Parent != null)
        {
            fullPath.Add(current.Parent.Name);
            current = current.Parent;
        }
        fullPath.Reverse();
        String cPath = System.IO.Path.Combine(fullPath.ToArray());
        return paths[cPath];
    }

    private void treeView1_AfterSelect(object sender, TreeViewEventArgs e) {

    }

    private void button_load_Click(object sender, EventArgs e)
    {

        FileRequested(this, new FileEventArgs(IDFromCurrentNode()));
        Form2 editWindow = new Form2();
        editWindow.LoadDocContent(currentDocument);
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        if (InterfaceClosing != null)
            InterfaceClosing(this, null);

        base.OnFormClosing(e);
    }

  
  
  }
   
}

