﻿using System;
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
        /// <summary>
        /// Marshall file to xml. 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string MarshallFile(SliceOfPie_Model.File file)
        {
            StringBuilder builder = new StringBuilder();
            using (XmlWriter writer = XmlWriter.Create(builder))
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

            }
            return builder.ToString();
        }



        /// <summary>
        /// Method that Marshalls a list of LogEntrys into XML
        /// </summary>
        /// <param name="loglist"></param>
        /// <returns>A list of Log Entries</returns>
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

        /// <summary>
        /// Unmarshalls the log in XML form
        /// </summary>
        /// <param name="xml"></param>
        /// <returns>A list of LogEntries</returns>
        public static List<LogEntry> UnMarshallLog(string xml)
        {
            StringReader stringReader = new StringReader(xml);
            List<LogEntry> LogList = new List<LogEntry>();
            Console.Out.WriteLine("Starting to parse");
            using (XmlReader reader = XmlReader.Create(stringReader))
            {
                reader.MoveToContent();
                string root = reader.GetAttribute("Logentry");
                string id = reader.GetAttribute("ID");
                string fileName = reader.GetAttribute("fileName");
                string filePath = reader.GetAttribute("filePath");
                string timeStamp = reader.GetAttribute("timestamp");
                string modification = reader.GetAttribute("Modification");

                if (reader.IsEmptyElement) { reader.Read(); return null; }
                bool b = true;
                reader.ReadStartElement();
                while (b)
                {
                    if (reader.NodeType == XmlNodeType.EndElement) break;
                    reader.Read();
                    id = reader.ReadElementContentAsString();
                    Console.Out.WriteLine("id:" + id);
                    //reader.Read();
                    fileName = reader.ReadElementContentAsString();
                    Console.Out.WriteLine("fileName: " + fileName);
                    //reader.Read();
                    filePath = reader.ReadElementContentAsString();
                    Console.Out.WriteLine("filePath: " + filePath);
                    timeStamp = reader.ReadElementContentAsString();
                    Console.Out.WriteLine("timestamp: " + timeStamp);
                    modification = reader.ReadElementContentAsString();
                    Console.Out.WriteLine("modification: " + modification);
                    reader.Read();
                    FileModification f = FileModification.Add;
                    switch (modification) {
                        case "Add": f = FileModification.Add; break;
                        case "Delete": f = FileModification.Delete; break;
                        case "Modify": f = FileModification.Modify; break;
                        case "MergeReady": f = FileModification.MergeReady; break;
                        case "Rename": f = FileModification.Rename; break;
                        case "Move": f = FileModification.Move; break;
                    }

                    LogEntry entry = new LogEntry(int.Parse(id), fileName, filePath, DateTime.Parse(timeStamp) , f);
                    LogList.Add(entry);
                }
                

                Console.Out.WriteLine("UNMARSHALLING RESULT: " + root + id + fileName + filePath + timeStamp + modification);
            }
            return LogList;


        }
    }
}
