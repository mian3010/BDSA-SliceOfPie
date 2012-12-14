using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using SliceOfPie_Model.Persistence;



namespace SliceOfPie_Model
{
    public static class HtmlMarshalUtil
    {

        public static string MarshallFile(FileInstance fileInstance)
        {
            var set = new XmlWriterSettings {Indent = true};
            var builder = new StringBuilder();
            using(XmlWriter writer = XmlWriter.Create(builder, set))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("html");

                // Write filemetadata 
                foreach (FileMetaData types in fileInstance.File.FileMetaDatas)
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
                writer.WriteString(fileInstance.id.ToString(CultureInfo.InvariantCulture));
                writer.WriteEndElement();

                // Write body, notice we can't somehow write < and > properly when passed as strings.. :/
                writer.WriteStartElement("body");
                string s = fileInstance.GetContent();
                writer.WriteString(s);
                writer.WriteEndElement();

                // writer.WriteStartElement("

                writer.WriteEndElement();
                writer.WriteEndDocument();
  
            }
            return builder.ToString();
        }
        /// <summary>
        /// Unmarshals a Document according to the loosely defined XML format used in Slice of Pie.
        /// </summary>
        /// <param name="xml">The xml to unmarshal</param>
        /// <returns>A new FileInstance object</returns>
        public static Document UnmarshallDocument(String xml)
        {
            var file = new Document();
            XElement doc = XElement.Parse(xml);
            file.File = new File();
            IEnumerable<XElement> metaData = doc.Elements("meta");
            foreach (XElement meta in metaData)
            {
                var fmd = new FileMetaData
                  {
                    MetaDataType_Type = meta.Attribute("name").Value,
                    value = meta.Attribute("content").Value
                  };
              file.File.FileMetaDatas.Add(fmd);
            }

            XElement id = doc.Element("ID");
          if (id != null) file.id = int.Parse(id.Value);

          XElement body = doc.Element("body");
          if (body != null) file.Content = body.Value;

          return file;
        }



        public static string MarshallFileList(FileList fileList)
        {
            List<FileListEntry> fList = fileList.List.Values.ToList();
            var doc = new XElement("logInfo", 
                                new XElement("fileList",
                                                 from a in fList
                                                 select
                                                     new XElement("listEntry",
                                                     new XElement("ID", a.Id.ToString(CultureInfo.InvariantCulture)),
                                                     new XElement("fileName", a.Name),
                                                     new XElement("filePath", a.Path.ToString(CultureInfo.InvariantCulture)),
                                                     new XElement("version", a.Version.ToString(CultureInfo.InvariantCulture)),
                                                     new XElement("type", a.Type.ToString()),
                                                     new XElement("isDeleted", a.IsDeleted.ToString())))
                               ,new XElement("incrementCounter", fileList.IncrementCounter.ToString(CultureInfo.InvariantCulture)) );
                                                 
            return doc.ToString();
        }

        public static FileList UnMarshallFileList(string xml)
        {

            var fileList = new Dictionary<int, FileListEntry>();
            XElement doc = XElement.Parse(xml);

            XElement root = doc.Element("fileList");
          if (root != null)
          {
            XElement m = root.Element("listEntry");
            while (m != null)
            {
              var xElement = m.Element("ID");
              if (xElement != null)
              {
                var element = m.Element("fileName");
                if (element != null)
                {
                  var xElement1 = m.Element("filePath");
                  if (xElement1 != null)
                  {
                    var element1 = m.Element("version");
                    if (element1 != null)
                    {
                      var xElement2 = m.Element("isDeleted");
                      var entry = new FileListEntry
                        {
                          Id = Int32.Parse(xElement.Value),
                          Name = element.Value,
                          Path = xElement1.Value,
                          Version = decimal.Parse(element1.Value),
                          IsDeleted = xElement2 != null && bool.Parse(xElement2.Value)
                        };
                      var element2 = m.Element("type");
                      if (element2 != null)
                        switch (element2.Value)
                        {
                          case "Push": entry.Type = FileListType.Push; break;
                          case "Pull": entry.Type = FileListType.Pull; break;
                          case "Conflict": entry.Type = FileListType.Conflict; break;
                        }

                      fileList.Add(entry.Id, entry);
                    }
                  }
                }
              }
              m = m.ElementsAfterSelf().FirstOrDefault();
            }
          }

          XElement e = doc.Element("incrementCounter");
          if (e != null)
          {
            String inc = e.Value;
            int incCounter = Int32.Parse(inc);
            return new FileList { List = fileList, IncrementCounter = incCounter };
          }
          return null;
        }

        /// <summary>
        /// Responsible for marshalling id to XML
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static String MarshallId(int id)
        {
            var doc = new XElement("IDResponse",
                                                  new XElement("FileID", id)
                                        );
            return doc.ToString();
        }

        /// <summary>
        /// Responsible for unmarshalling xml to an ID
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
          public static int UnMarshallId(String xml)
        {
            XElement doc = XElement.Parse(xml);
            XElement root = doc.Element("FileID");
          if (root != null)
          {
            int id = int.Parse(root.Value);
            return id;
          }
          return 0;
        }

        /// <summary>
        /// Returns a dictionary that maps each file_id to a list of changes. 
        /// Must conform to the loosely specified XML format as defined by the MarshallChangeList method.
        /// </summary>
        /// <param name="logXml"></param>
        /// <returns></returns>
        public static Dictionary<int, List<Change>> UnMarshallChangeList(string logXml)
        {
            var changeLog = new Dictionary<int, List<Change>>();
            XElement Node = XElement.Parse(logXml).Element("Change");
            // Default node if any element is null
            XElement def = new XElement("def", "no_entry1");
            while (Node != null)
            {
                Change c = new Change();
                int fileId = Int32.Parse((Node.Element("file_id") ?? def).Value);
                c.id = Int32.Parse((Node.Element("id") ?? def).Value);
                c.File_id = fileId;
                c.User_email = (Node.Element("user_email") ?? def).Value;
                c.change1 = (Node.Element("change") ?? def).Value;
                c.timestamp = long.Parse((Node.Element("timeStamp") ?? def).Value);

                List<Change> list = null;
                if (changeLog.TryGetValue(fileId,out list))
                {
                    list.Add(c);
                    changeLog[fileId] = list;
                }
                else
                {
                    list = new List<Change>();
                    changeLog[fileId] = list;
                }
            Node = Node.ElementsAfterSelf().FirstOrDefault();
            }
                return changeLog;
        }
            
       

        public static String MarshallChangeList(Dictionary<int, List<Change>> changeList)
        {
            List<Change> bigList = new List<Change>();
            foreach(List<Change> list in changeList.Values)
            {
                bigList.AddRange(list);
            }
            var doc = new XElement("Changes",
                                 from a in bigList
                                 select new XElement("Change",
                                                     new XElement("file_id", a.File_id),
                                                     new XElement("user_email", a.User_email),
                                                     new XElement("timeStamp", a.timestamp),
                                                     new XElement("change", a.change1),
                                                     new XElement("id", a.id)));
            return doc.ToString();
        }
        
    }
}
