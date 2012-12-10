using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SliceOfPie_Model;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Linq;
using System.Data.Entity;
using SliceOfPie_Model.Persistence;


namespace SliceOfPie_Model
{
    public static class HTMLMarshalUtil
    {

        public static string MarshallFile(File file)
        {
            XmlWriterSettings set = new XmlWriterSettings();
            set.Indent = true;
            StringBuilder builder = new StringBuilder();
            using(XmlWriter writer = XmlWriter.Create(builder, set))
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
                writer.WriteStartElement("ID");
                writer.WriteString(file.id.ToString());
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
            XElement doc = XElement.Parse(XML);

            IEnumerable<XElement> metaData = doc.Elements("meta");
            foreach (XElement meta in metaData)
            {
                FileMetaData fmd = new FileMetaData();
                fmd.MetaDataType_Type = meta.Attribute("name").Value;
                fmd.value = meta.Attribute("content").Value;
            }

            XElement id = doc.Element("ID");
            file.id = long.Parse(id.Value);

            XElement body = doc.Element("body");
            file.Content.Append(body.Value);
            
            return file;
        }



        public static string MarshallFileList(FileList fileList)
        {
            List<FileListEntry> fList = fileList.List.Values.ToList();
            XElement doc = new XElement("logInfo", 
                                new XElement("fileList",
                                                 from a in fList
                                                 select
                                                     new XElement("listEntry",
                                                     new XElement("ID", a.Id.ToString()),
                                                     new XElement("fileName", a.Name),
                                                     new XElement("filePath", a.Path.ToString()),
                                                     new XElement("version", a.Version.ToString()),
                                                     new XElement("type", a.Type.ToString()),
                                                     new XElement("isDeleted", a.IsDeleted.ToString())))
                               ,new XElement("incrementCounter", fileList.incrementCounter.ToString()) );
                                                 
            return doc.ToString();
        }

        public static FileList UnMarshallFileList(string xml)
        {
            Dictionary<long, FileListEntry> fileList = new Dictionary<long, FileListEntry>();
            XElement doc = XElement.Parse(xml);

            XElement root = doc.Element("fileList");
            XElement m = root.Element("listEntry");
            while (m != null)
            {
                FileListEntry entry = new FileListEntry();
                entry.Id = Int64.Parse(m.Element("ID").Value);
                entry.Name = m.Element("fileName").Value;
                entry.Path = m.Element("filePath").Value;
                entry.Version = float.Parse(m.Element("version").Value);
                entry.IsDeleted = bool.Parse(m.Element("isDeleted").Value);
                switch (m.Element("type").Value)
                {
                    case "Push": entry.Type = FileListType.Push; break;
                    case "Pull": entry.Type = FileListType.Pull; break;
                    case "Conflict": entry.Type = FileListType.Conflict; break;
                }

                fileList.Add(entry.Id, entry);
                m = m.ElementsAfterSelf().FirstOrDefault();
            }

            XElement e = doc.Element("incrementCounter");
            String inc = e.Value;
            long incCounter = Int64.Parse(inc);
            return new FileList() { List = fileList, incrementCounter = incCounter };
 
        }

        public static String MarshallId(long id)
        {
            throw new NotImplementedException();
        }

          public static long UnMarshallId(String id)
        {
            throw new NotImplementedException();
        }
    }
}

