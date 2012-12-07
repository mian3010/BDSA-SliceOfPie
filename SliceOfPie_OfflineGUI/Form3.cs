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
        List<String> paths = new List<String>();

        /// <summary>
        /// To-Do delete
        /// </summary>
        private void TestPaths()
        {
            paths.Add("C:/");
            paths.Add("C:/John");
            paths.Add("C:/Fail");
            paths.Add("C:/Fail/Example");
            paths.Add("C:/Fail.txt");
            paths.Add("C:/Fail/noob.txt");
        }

        public Form3()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Builds a tree from a list of strings.
        /// Borrowed from Stackoverflow. Thansk PaulB!
        /// </summary>
        private void InitializeTree()
        {
            TestPaths();
            TreeNode root = new TreeNode();
            TreeNode node = root;
            treeView1.Nodes.Add(root);

            foreach (string filePath in paths) 
            {
                node = root;
                foreach (string pathBits in filePath.Split('/'))
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


        private void button1_Click(object sender, EventArgs e)
        {
            InitializeTree();
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }
    }
}

