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

namespace SliceOfPie_Model {
  public static class HtmlMarshalUtil {
    public static string MarshallDocument(Document DocumentToMarshall, string Content) {
      StringBuilder builder = new StringBuilder();
      using (XmlWriter writer = XmlWriter.Create(builder)) {
        writer.WriteStartDocument();
        writer.WriteStartElement("html");

        // Write a custom ID tag which we can use later for database purposes.
        writer.WriteStartElement("meta");
        writer.WriteAttributeString("name", "id");
        writer.WriteAttributeString("content", DocumentToMarshall.id.ToString());
        writer.WriteEndElement();

        // Write body
        writer.WriteStartElement("body");
        writer.WriteRaw(DocumentToMarshall.ToString());
        writer.WriteEndElement();

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
    public static Document UnmarshallDocument(String Xml) {
      System.IO.MemoryStream stream = new System.IO.MemoryStream();
      System.IO.StreamWriter writer = new System.IO.StreamWriter(stream);
      writer.Write(Xml);
      writer.Flush();
      stream.Position = 0;
      return UnmarshallDocument(stream);
    }

    public static Document UnmarshallDocument(File FileToUnmarshall) {
      return UnmarshallDocument(new System.IO.FileStream(FileToUnmarshall.serverpath, System.IO.FileMode.Open));
    }

    private static Document UnmarshallDocument(System.IO.Stream Stream) {
      if (!Document.FileIsDocument(Stream)) throw new NotADocumentException("No document root element found");
      Document DocumentCreated = new Document();
      XDocument Doc = XDocument.Load(Stream);

      //First find the document id
      DocumentCreated.id =
          long.Parse((from el in Doc.Descendants("meta")
                      where (string)el.Attribute("name") == "id"
                      select el).First<XElement>().Attribute("content").Value);

      //First find the document root element
      IEnumerable<XElement> DocumentElement =
          from el in Doc.Descendants("div")
          where (string)el.Attribute("class") == "document"
          select el;

      //Find the document title
      DocumentCreated.Title =
          (from el in DocumentElement.Elements("h2")
           where (string)el.Attribute("class") == "document-title"
           select el).First<XElement>().Value;

      //Find the document view containing metadata and content
      IEnumerable<XElement> DocumentView =
          from el in DocumentElement.Elements("div")
          where (string)el.Attribute("class") == "document-view"
          select el;

      //Find the metadata element
      XElement MetadataElement =
          (from el in DocumentView.Elements("ul")
           where (string)el.Attribute("class") == "metadata-view"
           select el).First<XElement>();

      //Find all metadata elements
      IEnumerable<XElement> Metadatas =
          from el in MetadataElement.Elements("li")
          select el;

      foreach (XElement Metadata in Metadatas) {
        FileMetaData MetadataFound = new FileMetaData();
        //Get the type of the metadata
        MetadataFound.MetaDataType_Type =
          (from el in Metadata.Elements("span")
           where (string)el.Attribute("class") == "metadata-type"
           select el).First<XElement>().Value;

        //Get the value of the metadata
        MetadataFound.value =
          (from el in Metadata.Elements("span")
           where (string)el.Attribute("class") == "metadata-value"
           select el).First<XElement>().Value;
        DocumentCreated.FileMetaData.Add(MetadataFound);
      }

      return DocumentCreated;
    }

    public static string MarshallFileList(FileList fileList) {
      StringBuilder builder = new StringBuilder();
      using (XmlWriter writer = XmlWriter.Create(builder)) {
        writer.WriteStartDocument();
        writer.WriteStartElement("fileList");

        foreach (FileListEntry entry in fileList.List.Values) {
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

    public static FileList UnmarshallFileList(string xml) {
      Dictionary<long, FileListEntry> fileList = new Dictionary<long, FileListEntry>();
      XDocument doc = XDocument.Parse(xml);
      IEnumerable<XElement> elements = doc.Elements("fileList");
      foreach (var m in elements) {
        FileListEntry entry = new FileListEntry();
        entry.Id = Int64.Parse(m.Element("ID").Value);
        entry.Name = m.Element("fileName").Value;
        entry.Path = m.Element("filePath").Value;
        entry.Version = decimal.Parse(m.Element("version").Value);
        entry.IsDeleted = bool.Parse(m.Element("isDeleted").Value);
        switch (m.Element("type").Value) {
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