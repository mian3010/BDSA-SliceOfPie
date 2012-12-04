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

      private List<LogEntry> log = new List<LogEntry>();

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

            SaveLog(file.id, file.name, file.serverpath, DateTime.Now, FileModification.Add);

            return true;
        }
    }

    public bool ModifyFile(File file) {

      throw new NotImplementedException();
    }

    public void DeleteFile(File file)
    {

    }

    public void RenameFile(File file)
    {

    }



    public File MoveFile(File old, string newPath) {
      throw new NotImplementedException();
    }

    public List<LogEntry> GetLog() {
        return log;
    }

    public void SaveLog(long id, string filename, string filepath, DateTime timeStamp, FileModification modification) {
        log.Add(new LogEntry(id, filename, filepath, timeStamp, modification));
    }
  }
}
