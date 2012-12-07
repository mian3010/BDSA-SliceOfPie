using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SliceOfPie_Model
{
    interface INetClient
    {
        FileList SyncServer(FileList list);

        File PullFile(long fileID);

        // Returns new ID of the File you want to push
        long PushFile(File file);

    }
}
