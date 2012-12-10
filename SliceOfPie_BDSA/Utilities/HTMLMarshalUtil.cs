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
using System.IO;


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
                                                 
            String hmm = doc.ToString(SaveOptions.DisableFormatting);
            return doc.ToString(SaveOptions.DisableFormatting);
        }

        public static FileList UnMarshallFileList(string xml)
        {
            StringBuilder builder = new StringBuilder();
             using (XmlReader reader = XmlReader.Create())
             {
                
             }
  {
 
        }
    }
}

