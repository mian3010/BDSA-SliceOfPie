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
                try
                {
                    FileMetaData fmd = new FileMetaData();
                    fmd.value = m.Element("meta").Attribute("content").Value;
                    fmd.MetaDataType_Type = m.Element("meta").Attribute("name").Value;
                    file.FileMetaDatas.Add(fmd);
                    file.Content.Append(m.Element("body").Value);
                }
                catch (NullReferenceException e)
                {
                    continue;
                }
            }   
            

            return file;
        }



        public static string MarshallFileList(FileList fileList)
        {
            //XmlWriterSettings set = new XmlWriterSettings();
            //set.Indent = true;
            //StringBuilder builder = new StringBuilder();
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
                               ,new XElement("incrementCoutner", fileList.incrementCounter.ToString()) );
                                                 
            //using (XmlWriter writer = XmlWriter.Create(builder,set))
            //{
            //    writer.WriteStartDocument();
            //    writer.WriteStartElement("loginfo");
            //    writer.WriteStartElement("fileList");

            //    foreach (FileListEntry entry in fileList.List.Values)
            //    {
            //        writer.WriteStartElement("listEntry");
            //        writer.WriteElementString("ID", entry.Id.ToString());
            //        writer.WriteElementString("fileName", entry.Name);
            //        writer.WriteElementString("filePath", entry.Path);
            //        writer.WriteElementString("version", entry.Version.ToString());
            //        writer.WriteElementString("type", entry.Type.ToString());
            //        writer.WriteElementString("isDeleted", entry.IsDeleted.ToString());
            //        writer.WriteEndElement();
            //    }
             
            //    writer.WriteEndElement();
            //    writer.WriteStartElement("incrementCounter");
            //    writer.WriteString(fileList.incrementCounter.ToString());
            //    writer.WriteEndElement();
              
            //    writer.WriteEndElement();
                
            //    writer.WriteEndDocument();
            //}
            //return builder.ToString();
            String hmm = doc.ToString(SaveOptions.DisableFormatting);
            return doc.ToString(SaveOptions.DisableFormatting);
        }

        public static FileList UnMarshallFileList(string xml)
        {
            Dictionary<long, FileListEntry> fileList = new Dictionary<long, FileListEntry>();
            XDocument doc = XDocument.Parse(xml);
            
            var fileentries = from res in doc.Descendants("fileList")
                               select new {
                        lol = res.Element("ID").Value };
       
          //FileListEntry entry = new FileListEntry();
          //          entry.Id = Int64.Parse(m.Element("ID").Value);
          //          entry.Name = m.Element("fileName").Value;
          //          entry.Path = m.Element("filePath").Value;
          //          entry.Version = float.Parse(m.Element("version").Value);
          //          entry.IsDeleted = bool.Parse(m.Element("isDeleted").Value);
          //          switch (m.Element("type").Value)
          //          {
          //              case "Push": entry.Type = FileListType.Push; break;
          //              case "Pull": entry.Type = FileListType.Pull; break;
          //              case "Conflict": entry.Type = FileListType.Conflict; break;
          //          }

          //          fileList.Add(entry.Id, entry);

            //foreach(var m in fileentries)
            //{
                   
             
            //}
            
            //XElement e = doc.Element("incrementCounter");
            //String inc = e.Value;
            //long incCounter = Int64.Parse(inc);
            //return new FileList() { List = fileList, incrementCounter = incCounter };
            return null;
        }

        /// <summary>
        /// Makes the ID into xml
        /// </summary>
        /// <param name="id">long</param>
        /// <returns>XmlString</returns>
        public static string MarshallId(long id)
        { 
            StringBuilder builder = new StringBuilder();
            using (XmlWriter writer = XmlWriter.Create(builder))
            {
                writer.WriteStartDocument();
                writer.WriteElementString("FileID", id.ToString());
                writer.WriteEndDocument();
            }
            return builder.ToString();
        }

        /// <summary>
        /// Returns the ID as a long value
        /// </summary>
        /// <param name="xml">string</param>
        /// <returns>long</returns>
        public static long UnMarshallId(string xml)
        {
            XDocument doc = XDocument.Parse(xml);
            long id = long.Parse(doc.Element("FileID").Value);
            return id;
        }
    }
}

