using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SliceOfPie_Model.Persistence;

namespace SliceOfPie_Model
{
    /// <summary>
    /// The interface resposinsible for communicating with the persistence layer.
    /// The persistence layer could be a database or a file.
    /// </summary>
    public interface ICommunicator
    {
        /// <summary>
        /// Adds a file to storage
        /// </summary>
        /// <param name="file">The file object to add</param>
        /// <returns></returns>
        bool AddFile(File file);

        /// <summary>
        /// Modifies a file already in storage
        /// </summary>
        /// <param name="file">The file with the new info</param>
        /// <returns></returns>
        bool ModifyFile(File file);

        /// <summary>
        /// Deletes the specified file from storage
        /// </summary>
        /// <param name="file">The file to delete</param>
        /// <returns></returns>
        bool DeleteFile(File file);

        /// <summary>
        /// Renames a file in storage
        /// </summary>
        /// <param name="file">The file to rename</param>
        /// <param name="newName">The new name of the file</param>
        void RenameFile(File file, string newName);

        /// <summary>
        /// Gives a file a new ID in storage
        /// </summary>
        /// <param name="file">The file which id should be updated ( should contain original id )</param>
        /// <param name="newID">The new id of the file</param>
        void UpdateFileID(File file, long newID);

        /// <summary>
        /// Loads a file from storage
        /// </summary>
        /// <param name="id">The identifier of the file</param>
        /// <returns></returns>
        File GetFile(long id);

        /// <summary>
        /// Moves a file from its old path to a new path.
        /// </summary>
        /// <param name="file">The file to move (should include original path) </param>
        /// <param name="newPath">The new path of the file</param>
        void MoveFile(File file, string newPath);

        /// <summary>
        /// Events that the communicator should fire whenever the methods are called. 
        /// </summary>
        event FileEventHandler FileAdded, FileChanged, FileDeleted, FileMoved, FileRenamed, FilePulled;
    }

    /// <summary>
    /// Custom delegate to handle file events. 
    /// </summary>
    /// <param name="file"></param>
    public delegate void FileEventHandler(File file);
}
