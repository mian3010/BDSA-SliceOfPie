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

namespace SliceOfPie_OfflineGUI
{
    public partial class Form3 : Form
    {
        String xml = "<?xml version=\"1.0\"?><folder><file>Example.txt</file></folder>";

        public Form3()
        {
            InitializeComponent();
            //InitializeTree();
        }

        private void InitializeTree()
        {
            XmlDocument dom = new XmlDocument();
            dom.LoadXml(xml);

            treeView1.Nodes.Clear();
            treeView1.Nodes.Add(new TreeNode(dom.DocumentElement.Name));
            TreeNode tNode = new TreeNode();
            tNode = treeView1.Nodes[0];

            AddNode(dom.DocumentElement, tNode);
            treeView1.ExpandAll();


        }

        private void AddNode(XmlNode inXmlNode, TreeNode inTreeNode)
        {
            XmlNode xNode;
            TreeNode tNode;
            XmlNodeList nodeList;
            int i;

            // Loop through the XML nodes until the leaf is reached.
            // Add the nodes to the TreeView during the looping process.
            if (inXmlNode.HasChildNodes)
            {
                nodeList = inXmlNode.ChildNodes;
                for (i = 0; i <= nodeList.Count - 1; i++)
                {
                    xNode = inXmlNode.ChildNodes[i];
                    inTreeNode.Nodes.Add(new TreeNode(xNode.Name));
                    tNode = inTreeNode.Nodes[i];
                    AddNode(xNode, tNode);
                }
            }
            else
            {
                // Here you need to pull the data from the XmlNode based on the
                // type of node, whether attribute values are required, and so forth.
                inTreeNode.Text = (inXmlNode.OuterXml).Trim();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            InitializeTree();
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }
    }
}

