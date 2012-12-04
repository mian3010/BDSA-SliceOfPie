using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Diagnostics;

namespace SliceOfPie_Model {
  public class CommunicatorOfflineAdapter : ICommunicator {

      // The object takes a path for the root folder of the SoP documents. Each document will be automatically saved from there.

      public readonly String rootpath;

      public CommunicatorOfflineAdapter(String rootpath)
      {
          this.rootpath = rootpath;
      }

    public bool AddFile(File file) {
        if (!file.serverpath.Contains(rootpath))
        {
            file.serverpath = rootpath;
        }
        
        if(!Directory.Exists(file.serverpath)) {
            Directory.CreateDirectory(file.serverpath);
        }
        using (XmlWriter writer = XmlWriter.Create(file.serverpath + file.name + ".html"))
        {
            writer.WriteStartDocument();
            writer.WriteStartElement("html");
            Debug.WriteLine("FILE : " + file.serverpath + file.name + "has : " + file.FileMetaDatas.Count + " files in filemetadat");
            
            // Write filemetadata 
            foreach (FileMetaData types in file.FileMetaDatas)
            {
                writer.WriteStartElement("meta");
                writer.WriteAttributeString("name", types.MetaDataType.Type);
                //writer.WriteEndAttribute(); 
                writer.WriteAttributeString("content", types.value);
                // writer.WriteEndAttribute();
                writer.WriteEndElement();
            }

            // Write a custom ID tag which we can use later for database purposes.
            writer.WriteStartElement("meta");
            writer.WriteAttributeString("id", file.id.ToString());
            writer.WriteEndElement();

            // Write body, notice we can't somehow write < and > properly when passed as strings.. :/
            writer.WriteStartElement("body");
            writer.WriteString(file.ToString());
            writer.WriteEndElement();
            
            
            writer.WriteEndElement();
            writer.WriteEndDocument();
            return true;
        }
    }

    public bool SaveFile(File file) {
      throw new NotImplementedException();
    }

    public File ChangePath(File old, string newPath) {
      throw new NotImplementedException();
    }

    public List<LogEntry> GetLog() {
      throw new NotImplementedException();
    }

    public void SaveLog() {
      throw new NotImplementedException();
    }
  }
}
