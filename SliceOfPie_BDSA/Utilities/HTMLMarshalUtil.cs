using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SliceOfPie_Model;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Data.Entity;


namespace SliceOfPie_Model
{
    public static class HTMLMarshalUtil
    {

        public static string MarshallFile(SliceOfPie_Model.File file)
        {
            StringBuilder builder = new StringBuilder();
            using(XmlWriter writer = XmlWriter.Create(builder))
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

                // writer.WriteStartElement("

                writer.WriteEndElement();
                writer.WriteEndDocument();
  
            }
            return builder.ToString();
        }
        /// <summary>
        /// Unmarshals a File according to the loosely defined XML format used in Slice of Pie.
        /// </summary>
        /// <param name="XML">The xml to unmarshal</param>
        /// <returns>A new FileInstance object</returns>
        public static File UnmarshallFile(String XML)
        {
            File file = new File();
            XDocument doc = XDocument.Parse(XML);

            IEnumerable<XElement> elements = doc.Elements("html");

            foreach (var m in elements)
            {
                FileMetaData fmd = new FileMetaData();
                fmd.value = m.Element("meta").Attribute("content").Value;
                fmd.MetaDataType_Type = m.Element("meta").Attribute("name").Value;
                file.FileMetaDatas.Add(fmd);
                file.Content.Append(m.Element("body").Value);    
            }   
            

            return file;
        }



        public static string MarshallFileList(FileList fileList)
        { 
            StringBuilder builder = new StringBuilder();
            using (XmlWriter writer = XmlWriter.Create(builder))
	        {
	            writer.WriteStartDocument();
	            writer.WriteStartElement("fileList");

	            foreach (FileListEntry entry in fileList.List.Values)
	            {
		            writer.WriteStartElement("listEntry");
		            writer.WriteElementString("ID", entry.Id.ToString());
		            writer.WriteElementString("fileName", entry.Name);
		            writer.WriteElementString("filePath", entry.Path);
		            writer.WriteElementString("version", entry.Version.ToString());
                    writer.WriteElementString("type", entry.Type.ToString());
		            writer.WriteElementString("isDeleted", entry.IsDeleted.ToString());
                    writer.WriteEndElement();
	            }
                writer.WriteStartElement("incrementCounter", fileList.incrementCounter.ToString());
                writer.WriteEndElement();

	            writer.WriteEndElement();
	            writer.WriteEndDocument();
	        }
            return builder.ToString();
        }

        public static FileList UnMarshallFileList(string xml)
        {
            Dictionary<long, FileListEntry> fileList = new Dictionary<long, FileListEntry>();
            XDocument doc = XDocument.Parse(xml);
            IEnumerable<XElement> elements = doc.Elements("fileList");
            foreach (var m in elements)
            {
                FileListEntry entry = new FileListEntry();
                entry.Id = Int64.Parse(m.Element("ID").Value);
                entry.Name = m.Element("fileName").Value;
                entry.Path = m.Element("filePath").Value;
                entry.Version = float.Parse(m.Element("version").Value);
                entry.IsDeleted = bool.Parse(m.Element("isDeleted").Value);
                switch(m.Element("type").Value)
                {
                    case "Push": entry.Type = FileListType.Push; break;
                    case "Pull": entry.Type = FileListType.Pull; break;
                    case "Conflict": entry.Type = FileListType.Conflict; break;
                }
                
                fileList.Add(entry.Id, entry);
            }
            long incCounter = Int64.Parse(doc.Element("incrementCounter").Value);
            return new FileList() { List = fileList, incrementCounter = incCounter };
        }
    }
}

