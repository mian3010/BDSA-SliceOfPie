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

      private List<LogEntry> offLineLog;

      public CommunicatorOfflineAdapter(String rootpath)
      {
          this.rootpath = rootpath;
          offLineLog = new List<LogEntry>();
      }

    private bool AddNewFile(File file) {
        if (!file.serverpath.Contains(rootpath))
        {
            file.serverpath = rootpath;
        }
        
        if(!Directory.Exists(file.serverpath)) {
            Directory.CreateDirectory(file.serverpath);
        }
        string fullpath = System.IO.Path.Combine(file.serverpath, file.name);
        using (XmlWriter writer = XmlWriter.Create(fullpath + ".html"))
        {
            writer.WriteStartDocument();
            writer.WriteStartElement("html");
            
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

            // Save to our log
            return true;
        }
    }

      /// <summary>
      /// Add a new file to the disk with either the serverpath specified in the file or the default rootpath.
      /// Will overwrite existing files. Must be run with administrator rights. Also saves a LogEntry describing the action
      /// </summary>
      /// <param name="file">The file to be added. Does not need a id or a path</param>
      /// <returns>Boolean indicating whether the creation was succesful</returns>
    public bool AddFile(File file)
    {
        if (AddNewFile(file))
        {
            SaveToLog(file.id, file.name, file.serverpath, DateTime.Now, FileModification.Add);
            return true;
        }
        else return false;    

    }

    /// <summary>
    /// Tries to modify the existing file given as param. If the file does not exist, it will create a new file
    /// with the specified path or at the rootpath.
    /// </summary>
    /// <param name="file">The file to modify</param>
    /// <returns></returns>
    public bool ModifyFile(File file) {

        if(file == null){
            throw new ArgumentException();
        }

        if (AddNewFile(file))
        {
            offLineLog.Add(new LogEntry(file.id, file.name, file.serverpath, DateTime.Now, FileModification.Modify));
            return true;
        }
        return false;
    
    }

    /// <summary>
    /// Tries to delete a file. Throws exception if the file is already flagged for deletion.
    /// 
    /// </summary>
    /// <param name="file">The file to be deleted. Need a full path to work</param>
    /// <returns>True if sucessful, failure in any exception case</returns>
    public bool DeleteFile(File file)
    {
        //Is metadata availible.
        if (file.deleted <= 0)
        {
            throw new ArgumentException();
        }
        try
        {
           
            //Delete the file through System.IO.File class, not Slice Of Pie File class.
            System.IO.File.Delete(file.serverpath);

            offLineLog.Add(new LogEntry(file.id, file.name, file.serverpath, DateTime.Now, FileModification.Delete));
            return true;
        }
        catch (IOException e)
        {
            Console.Out.WriteLine("File is not deleted: " + e.Message);
            return false;
        }
      
    }

      /// <summary>
      /// Tries to rename the file specified in the param with the newName param. 
      /// Overwrites any file existing at the new name.
      /// </summary>
      /// <param name="file">The old file path and name</param>
      /// <param name="newName">The new name of the file. NB: THIS IS ONLY THE FILENAME NOT PATH</param>
    public void RenameFile(File file, string newName)
    {
        // Should be less than zero, flag for deleted. If zero might not be initialized.
        if (file.deleted < 0)
        {
            throw new ArgumentException();
        }
        
     
        string fullPath = System.IO.Path.Combine(file.serverpath, file.name);
        string newFullPath = System.IO.Path.Combine(file.serverpath, newName);

        MoveFile(fullPath, newFullPath);
        file.name = newName;

        offLineLog.Add(new LogEntry(file.id, file.name, file.serverpath, DateTime.Now, FileModification.Rename));
    }

    private void MoveFile(String fullOldPath, String fullNewPath)
    {
        if (!System.IO.Directory.Exists(fullNewPath))
        {
            System.IO.Directory.CreateDirectory(fullNewPath);
        }
        System.IO.File.Move(fullOldPath, fullNewPath);
    }

    /// <summary>
    /// Moves a file from the files old path to the newPath.
    /// </summary>
    /// <param name="file">The file to move and it's old path</param>
    /// <param name="newPath">The new path</param>
    public void MoveFile(File file, string newPath) {
        if (file.deleted < 0)
        {
            throw new ArgumentException();
        }
     
        string fullPath = System.IO.Path.Combine(file.serverpath, file.name);
        string newFullPath = System.IO.Path.Combine(newPath, file.name);

        MoveFile(fullPath, newFullPath);

        // Change the file path... in the old file [good idea yet?]
        file.serverpath = newPath;
        offLineLog.Add(new LogEntry(file.id, file.name, file.serverpath, DateTime.Now, FileModification.Move));
    }

     /// <summary>
     /// Returns a log of all the modifications made to files.
     /// </summary>
     /// <returns></returns>
    public List<LogEntry> GetLog() {
        if (offLineLog.Count == 0)
        {
            Console.Out.WriteLine("Off-line log is empty");
        }
        return offLineLog;
    }

    private void SaveToLog(long id, string filename, string filepath, DateTime timeStamp, FileModification modification) {
        offLineLog.Add(new LogEntry(id, filename, filepath, timeStamp, modification));
    }

    public bool SaveFile(File file)
    {
        return true;
    }

    public void PersistLog()
    {
    }

  }
}
