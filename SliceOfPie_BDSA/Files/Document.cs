using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using System.Linq;

namespace SliceOfPie_Model.Persistence {
  /// <summary>
  /// Document class. Emulates a PARTIAL html document. 
  /// Needs enclosing HTML tags when saved and displayed in system.
  /// Author morr&msta.
  /// </summary>
  public class Document : File {
    public String Title { get; set; }
    public IList<FileMetaData> MetaData { get; set; }
    public string Content {
      get {
        return getContentFromFile();
      }
    }

    public Document() { }

    public override string ToString() {
      StringBuilder output = new StringBuilder();
      output.Append(
        "<div class=\"document\">" +
        "  <h2 class=\"document-title\">" + Title + "</h2>" +
        "  <div class=\"document-view\">" +
        "    <ul class=\"metadata-view\">");
      
      foreach (FileMetaData Meta in MetaData) {
        output.Append("      <li><span class=\"metadata-type\">" + Meta.MetaDataType + "</span>: <span class=\"metadata-value\">" + Meta + "</span></li>");
      }
      output.Append(
        "    </ul>" +
        "    <div class=\"document-content\">" +
               Content +
        "    </div>" +
        "  </div>" +
        "</div>");
      return output.ToString();
    }

    public string HistoryToString() {
      StringBuilder output = new StringBuilder();
      output.Append("<ol>");
      output.Append("<li>Document created</li>");
      output.Append("<li>Document saved</li>");
      output.Append("</ol>");
      return output.ToString();
    }

    private string getContentFromFile() {
      XDocument Doc = XDocument.Load(new System.IO.FileStream(this.serverpath, System.IO.FileMode.Open));
      IEnumerable<XElement> DocumentElement =
          from el in Doc.Descendants("div")
          where (string)el.Attribute("class") == "document"
          select el;
      if (DocumentElement.Count() != 1) throw new NotADocumentException("No document-content tag found");

      return DocumentElement.First<XElement>().Value;
    }

    public static bool FileIsDocument(System.IO.Stream Stream) {
      XDocument Doc = XDocument.Load(Stream);
      IEnumerable<XElement> DocumentId =
          from el in Doc.Descendants("meta")
          where (string)el.Attribute("name") == "id"
          select el;
      if (DocumentId.Count() != 1) return false;
      IEnumerable<XElement> DocumentElement =
          from el in Doc.Descendants("div")
          where (string)el.Attribute("class") == "document"
          select el;
      if (DocumentElement.Count() != 1) return false;
      IEnumerable<XElement> DocumentViewElement =
          from el in DocumentElement.Descendants("div")
          where (string)el.Attribute("class") == "document-view"
          select el;
      if (DocumentViewElement.Count() != 1) return false;
      IEnumerable<XElement> DocumentMetadataElement =
          from el in DocumentViewElement.Descendants("ul")
          where (string)el.Attribute("class") == "metadata-view"
          select el;
      if (DocumentMetadataElement.Count() != 1) return false;
      IEnumerable<XElement> DocumentContentElement =
          from el in DocumentViewElement.Descendants("div")
          where (string)el.Attribute("class") == "document-content"
          select el;
      if (DocumentContentElement.Count() != 1) return false;
      return true;
    }

    public static bool FileIsDocument(File FileToCheck) {
      //TODO: What if serverpath is invalid?
      return FileIsDocument(new System.IO.FileStream(FileToCheck.serverpath, System.IO.FileMode.Open));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="FromFile"></param>
    /// <returns></returns>
    /// <exception cref="NotADocumentException"></exception>
    public Document CreateDocument(File FromFile) {
      return HtmlMarshalUtil.UnmarshallDocument(FromFile);
    }
  }
}