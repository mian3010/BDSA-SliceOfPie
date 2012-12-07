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

namespace SliceOfPie_OfflineGUI {
  public partial class Form3 : Form {
    List<String> paths = new List<String>();

    /// <summary>
    /// To-Do delete
    /// </summary>
    private void TestPaths() {
      paths.Add("C:/File");
      paths.Add("C:/John");
      paths.Add("C:/Fail");
      paths.Add("C:/Fail/Example");
      paths.Add("C:/Fail.txt");
      paths.Add("C:/Fail/noob.txt");
      paths.Add("D:/Fail/noob.txt");
    }

    public Form3() {
      InitializeComponent();
      //InitializeTree();
    }

    private void InitializeTree() {
      TestPaths();
      TreeNode root = new TreeNode("Files");
      TreeNode node = root;
      treeView1.Nodes.Add(root);

      foreach (string filePath in paths) {
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

    private void treeView1_AfterSelect(object sender, TreeViewEventArgs e) {

    }
  }
}

