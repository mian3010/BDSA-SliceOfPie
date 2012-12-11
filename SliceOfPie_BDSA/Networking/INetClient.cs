using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SliceOfPie_Model.Persistence;

namespace SliceOfPie_Model
{
    interface INetClient
    {
        FileList SyncServer(FileList list);

        File PullFile(long fileId);

        // Returns new ID of the File you want to push
        long PushFile(File file);

    }
}
