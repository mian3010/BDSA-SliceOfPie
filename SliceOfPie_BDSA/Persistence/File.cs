//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SliceOfPie_Model.Persistence
{
    using System;
    using System.Collections.Generic;
    
    public partial class File
    {
        public File()
        {
            this.Changes = new HashSet<Change>();
            this.FileInstances = new HashSet<FileInstance>();
            this.FileMetaDatas = new HashSet<FileMetaData>();
        }
    
        public int id { get; set; }
        public string name { get; set; }
        public string serverpath { get; set; }
        public Nullable<sbyte> deleted { get; set; }
        public Nullable<int> Project_id { get; set; }
        public decimal Version { get; set; }
        public byte[] Content { get; set; }
    
        public virtual ICollection<Change> Changes { get; set; }
        public virtual Project Project { get; set; }
        public virtual ICollection<FileInstance> FileInstances { get; set; }
        public virtual ICollection<FileMetaData> FileMetaDatas { get; set; }
    }
}