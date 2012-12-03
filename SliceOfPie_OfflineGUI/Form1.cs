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

        Document d;

        public Form1()
        {
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            d = new Document();

            MetaDataType MetaDataType1 = new MetaDataType();
            MetaDataType1.Type = "Created date";
            FileMetaData FileMetaData1 = new FileMetaData();
            FileMetaData1.MetaDataType = MetaDataType1;
            FileMetaData1.value = "2012-11-27 10:23:11";

            MetaDataType MetaDataType2 = new MetaDataType();
            MetaDataType2.Type = "Owner";
            FileMetaData FileMetaData2 = new FileMetaData();
            FileMetaData2.MetaDataType = MetaDataType2;
            FileMetaData2.value = "Michael Søby Andersen";

            MetaDataType MetaDataType3 = new MetaDataType();
            MetaDataType3.Type = "Type";
            FileMetaData FileMetaData3 = new FileMetaData();
            FileMetaData3.MetaDataType = MetaDataType3;
            FileMetaData3.value = "Document";

            d.FileMetaDatas.Add(FileMetaData1);
            d.FileMetaDatas.Add(FileMetaData2);
            d.FileMetaDatas.Add(FileMetaData3);
            d.Content = new StringBuilder("Awesome text document here!<br /><strong>This should be bold</strong><br />OMG PIE:<br /><img src=\"http://www.seriouseats.com/images/potd_pi-pie.jpg\" />");
            d.Content.Append("<br /><br />Testing wrappingggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggg");
            d.Title = "The awesome title";
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            FileListBox.GetSelected(FileListBox.SelectedIndex);
        }

        private void editButton_Click(object sender, EventArgs e)
        {
           
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

        private void viewButton_Click(object sender, EventArgs e)
        {
            Form2 docform = new Form2();
            docform.Show();
            docform.LoadDocContent(d);
        }
    }
}
