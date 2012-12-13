using SliceOfPie_Model.Persistence;

namespace SliceOfPie_Model
{
    /// <summary>
    /// The interface resposinsible for communicating with the persistence layer.
    /// The persistence layer could be a database or a file.
    /// </summary>
    public interface ICommunicator
    {
        IFileListHandler FileListHandler
        {
            get;
        }

        /// <summary>
        /// Adds a file to storage
        /// </summary>
        /// <param name="file">The file object to add</param>
        /// <returns></returns>
        bool AddOfflineCreatedFile(FileInstance file);

        /// <summary>
        /// Adds a file from remote storage. Should be used during synchronization.
        /// </summary>
        /// <param name="file">The file to add from a remote location</param>
        /// <returns>True if successful, false otherwise</returns>
        bool AddFileFromRemote(FileInstance file);

        /// <summary>
        /// Modifies a file already in storage
        /// </summary>
        /// <param name="file">The file with the new info</param>
        /// <returns></returns>
        bool ModifyFile(FileInstance file);

        /// <summary>
        /// Deletes the specified file from storage
        /// </summary>
        /// <param name="file">The file to delete</param>
        /// <returns></returns>
        bool DeleteFile(FileInstance file);

        /// <summary>
        /// Renames a file in storage
        /// </summary>
        /// <param name="file">The file to rename</param>
        /// <param name="newName">The new name of the file</param>
        void RenameFile(FileInstance file, string newName);

        /// <summary>
        /// Gives a file a new ID in storage
        /// </summary>
        /// <param name="file">The file which id should be updated ( should contain original id )</param>
        /// <param name="newId">The new id of the file</param>
        void UpdateFileId(FileInstance file, int newId);

        /// <summary>
        /// Loads a file from storage
        /// </summary>
        /// <param name="id">The identifier of the file</param>
        /// <returns></returns>
        FileInstance GetFile(int id);

        /// <summary>
        /// Moves a file from its old path to a new path.
        /// </summary>
        /// <param name="file">The file to move (should include original path) </param>
        /// <param name="newPath">The new path of the file</param>
        void MoveFile(FileInstance file, string newPath);

        /// <summary>
        /// Events that the communicator should fire whenever the methods are called. 
        /// </summary>
        event FileEventHandler FileAdded, FileChanged, FileDeleted, FileMoved, FileRenamed, FilePulled;
    }

   
}
