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
using System.Data.Entity;


namespace SliceOfPie_Network
{
    public static class HTMLMarshaller
    {

        public static string MarshallFile(SliceOfPie_Model.File file)
        {
            StringBuilder builder = new StringBuilder();
            using(XmlWriter writer = XmlWriter.Create(builder))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("File");
                writer.WriteElementString("id", file.id.ToString());
                writer.WriteElementString("name", file.name);
                writer.WriteElementString("path", file.serverpath);     
            }
            return builder.ToString();
        }

        public static string MarshallLog(List<LogEntry> loglist)
        { 
            StringBuilder builder = new StringBuilder();
            using (XmlWriter writer = XmlWriter.Create(builder))
	        {
	            writer.WriteStartDocument();
	            writer.WriteStartElement("Log");

	            foreach (LogEntry log in loglist)
	            {
		            writer.WriteStartElement("Logentry");
		            writer.WriteElementString("ID", log.id.ToString());
		            writer.WriteElementString("fileName", log.fileName);
		            writer.WriteElementString("filePath", log.filePath);
		            writer.WriteElementString("timeStamp", log.timeStamp.ToString());
                    writer.WriteElementString("Modification", log.modification.ToString());
		            writer.WriteEndElement();
	            }

	            writer.WriteEndElement();
	            writer.WriteEndDocument();
	        }
            return builder.ToString();
        }
    }
}

