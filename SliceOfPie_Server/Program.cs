using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SliceOfPie_Model;
using SliceOfPie_Network;

namespace SliceOfPie_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            File file = File.CreateFile(1, "Hejsa.txt", "Here", 0);
            SaveFile(file);
        }

        public static void SaveFile(File File)
        {
            CommunicatorOfflineAdapter adapter = new CommunicatorOfflineAdapter(@".\Files");
            if (adapter.FindFile(File)) //File exists
            {
                adapter.ModifyFile(File);
            }
            else //New file
            {
                adapter.AddFile(File);
            }

            /*
            string path = @".\Files\" + File.serverpath;
            string name = File.name;
            string FileAsHtml = HTMLMarshaller.MarshallFile(File);

            string fullPath = path + @"\" + name;
            if(!System.IO.Directory.Exists(path)){
                System.IO.Directory.CreateDirectory(path);
            }
            if (!System.IO.File.Exists(fullPath))
            {
                System.IO.File.Create(fullPath).Close();
            }
            System.IO.File.WriteAllText(fullPath, FileAsHtml);
            */
        }
    }
}
