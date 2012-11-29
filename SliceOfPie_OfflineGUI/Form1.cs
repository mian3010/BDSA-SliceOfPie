using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SliceOfPie_Model;
using System.Data.Entity;


namespace SliceOfPie_OfflineGUI
{
    public partial class Form1 : Form, IController
    {

        Document doc = new Document();

        public Form1()
        {
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            doc.Title = "Toke Jensen";
            doc.name = "SUPER DOCUMENT!";
            doc.Content.Append("SLICEOFOIMWEFINMOWIEMNFGPOWEFMPWEMFPOWEFMPWEOMFPOWEMFPOEWF");
            FileListBox.Items.Add(doc.Title);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            FileListBox.GetSelected(FileListBox.SelectedIndex);
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            Form2 docform = new Form2();
            docform.Show();
            docform.LoadDocContent(doc);
        }

        private void addButton_Click(object sender, EventArgs e)
        {

        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Are you sure you want to delete file?", "Delete", MessageBoxButtons.YesNo);
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {
            progressBar1.BackColor = Color.Aqua;
        }
    }
}
