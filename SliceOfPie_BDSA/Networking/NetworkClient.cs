using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using SliceOfPie_Model.Persistence;
using System.Diagnostics;
using SliceOfPie_Model;
using System.Threading;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace SliceOfPie_Model {
  public class NetworkClient : INetClient {
    private bool is_active;
    private int port = 8080;

    public bool SaveFile(File FileToSave) {
      throw new NotImplementedException();
    }

    public long PushFile(File FileToSave) {
      throw new NotImplementedException();
    }

    public FileList SyncServer(FileList list) {
      throw new NotImplementedException();
    }

    /*public FileList SyncServer(FileList list) {
      return SendFileList(list);
    }*/
    public File PullFile(long FileId) {
      WebRequest Request = WebRequest.Create("http://localhost:8080/");
      Request.Method = "GET";
      System.IO.Stream RequestStream = Request.GetRequestStream();
      byte[] FileIdByte = Encoding.ASCII.GetBytes(FileId.ToString());
      RequestStream.Write(FileIdByte, 0, FileIdByte.Length);
      System.IO.Stream ResponseFromServer = Request.GetResponse().GetResponseStream();

      //Read length of the file object data from response
      byte[] FileObjectLength = new byte[4];
      ResponseFromServer.Read(FileObjectLength, 0, FileObjectLength.Length);
      //Read the file instance object from response
      byte[] FileObject = new byte[BitConverter.ToInt32(FileObjectLength, 0)];
      ResponseFromServer.Read(FileObject, 0, FileObject.Length);
      IFormatter Formatter = new BinaryFormatter();
      FileInstance FileObjectRecieved = (FileInstance)Formatter.Deserialize(new System.IO.MemoryStream(FileObject));
      //Read the rest of the response into file with path from instance
      int Len;
      byte[] WriteBuffer = new byte[8 * 1024];
      System.IO.Stream FileStream = new System.IO.FileStream(FileObjectRecieved.path, System.IO.FileMode.Create);
      while ((Len = ResponseFromServer.Read(WriteBuffer, 0, WriteBuffer.Length)) > 0) {
        FileStream.Write(WriteBuffer, 0, Len);
      }

      RequestStream.Close();

      throw new NotImplementedException();
    }

    /*public long PushFile(File file) {
      return SendFile(file);
    }*/
    public void PushFile(FileInstance FileToPush) {
      HttpWebRequest Request = (HttpWebRequest)WebRequest.Create("http://localhost:8080/");
      Request.Method = "POST";
      System.IO.Stream RequestStream = Request.GetRequestStream();
      //Write the length of the file object at start of stream
      byte[] FileObjectLength = new byte[4];
      BinaryFormatter Formatter = new BinaryFormatter();
      System.IO.MemoryStream FileObjectStream = new System.IO.MemoryStream();
      Formatter.Serialize(FileObjectStream, FileToPush);
      FileObjectLength = BitConverter.GetBytes(FileObjectStream.Length);
      //Write file object after this
      byte[] FileObject = new byte[FileObjectStream.Length];
      FileObjectStream.Read(FileObject, 0, FileObject.Length);
      byte[] FileToSend = System.IO.File.ReadAllBytes(FileToPush.path);
      //Copy everything into one byte array for sending
      byte[] AllData = new byte[FileObjectLength.Length + FileObject.Length + FileToSend.Length];
      FileObjectLength.CopyTo(AllData, 0);
      FileObject.CopyTo(AllData, FileObjectLength.Length);
      FileToSend.CopyTo(AllData, FileObjectLength.Length + FileObject.Length);
      //Send byte array to server
      RequestStream.Write(AllData, 0, AllData.Length);
      RequestStream.Close();
    }

    /*
     * http://www.codeproject.com/Articles/32633/Sending-Files-using-TCP
    byte[] SendingBuffer = null;
    TcpClient clientSocket = null;
    NetworkStream netstream = null;
    try {
      clientSocket = new TcpClient(remoteHostIP, remoteHostPort);
      netstream = clientSocket.GetStream();
      FileStream Fs = new FileStream(longFileName, FileMode.Open, FileAccess.Read);
      int NoOfPackets = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(Fs.Length) / Convert.ToDouble(BufferSize)));
      int TotalLength = (int)Fs.Length, CurrentPacketLength, counter = 0;
      for (int i = 0; i < NoOfPackets; i++) {
        if (TotalLength > BufferSize) {
          CurrentPacketLength = BufferSize;
          TotalLength = TotalLength - CurrentPacketLength;
        } else
          CurrentPacketLength = TotalLength;
        SendingBuffer = new byte[CurrentPacketLength];
        Fs.Read(SendingBuffer, 0, CurrentPacketLength);
        netstream.Write(SendingBuffer, 0, (int)SendingBuffer.Length);
      }
      Fs.Close();
    } catch (Exception ex) {
      Console.WriteLine(ex.Message);
    } finally {
      netstream.Close();
      clientSocket.Close();
    }

  }
}*/

    /// <summary>
    /// Send the XML to the server using a HTTP request.
    /// </summary>
    /// <param name="xml">The xml to send</param>
    /// <param name="method">The REST method</param>
    /// <returns>A response from the server to be returned</returns>
    /*
    private HttpWebResponse Send(string xml, string method) {
      HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost:8080/");
      request.Accept = "text/xml,text/html";
      request.Method = method;
      // Creates a byteversion of the XML string
      byte[] byteVersion = Encoding.ASCII.GetBytes(xml);

      request.ContentLength = xml.Length;

      System.IO.Stream stream = request.GetRequestStream();
      stream.Write(byteVersion, 0, byteVersion.Length);
      stream.Close();
      // Waist for the HTTP response from the server
      return (HttpWebResponse)request.GetResponse();
    }

    private SliceOfPie_Model.Persistence.File GetFile(long id) {
      HttpWebResponse response = Send(id.ToString(), "GET");
      return HandleFileResponse(response);
    }

    /// <summary>
    /// Sends HTML using a HTTP protocol.
    /// </summary>
    /// <param name="msg"></param>
    private FileList SendFileList(FileList log) {
      string xml = HtmlMarshalUtil.MarshallFileList(log);
      HttpWebResponse response = Send(xml, "POST");
      return HandleFileListResponse(response);
    }

    private long SendFile(SliceOfPie_Model.Persistence.File file) {
      string xml = HtmlMarshalUtil.MarshallFile(file);
      HttpWebResponse response = Send(xml, "PUT");
      response = Send
      return HandleIdResponse(response);
    }

    /// <summary>
    /// Handles the resultcomming from the server.
    /// </summary>
    /// <param name="response">The response from the server</param>
    private FileList HandleFileListResponse(HttpWebResponse response) {
      Console.Out.WriteLine(response.Server);
      System.IO.StreamReader reader = new System.IO.StreamReader(response.GetResponseStream());
      Console.Out.WriteLine(reader.ReadToEnd());
      return null;
    }

    /// <summary>
    /// Handles the result comming from the server
    /// </summary>
    /// <param name="response">The response from the server</param>
    private SliceOfPie_Model.Persistence.File HandleFileResponse(HttpWebResponse response) {
      Console.Out.WriteLine(response.Server);
      System.IO.StreamReader reader = new System.IO.StreamReader(response.GetResponseStream());
      Console.Out.WriteLine(reader.ReadToEnd());
      return null;
    }

    private long HandleIdResponse(HttpWebResponse response) {
      return 0;
    }


    public static void Main(String[] args) {
      //NetworkServer server = NetworkServer.GetInstance();
      // NetworkClient client = new NetworkClient();
      // Testdata
      // Thread thread = new Thread(() => server.listen());
      // thread.Start();
      //client.SendLog(list);
      // server.Close();
    }
  }
     * */
  }
}
