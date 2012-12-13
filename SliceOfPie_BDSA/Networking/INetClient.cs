using SliceOfPie_Model.Persistence;

namespace SliceOfPie_Model
{
    interface INetClient
    {
        FileList SyncServer(FileList list);

        FileInstance PullFile(int fileId);

        // Returns new ID of the File you want to push
        FileInstance PushFile(FileInstance file);

    }
}
