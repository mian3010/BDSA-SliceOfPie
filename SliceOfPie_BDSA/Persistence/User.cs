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
    
    public partial class User
    {
        public User()
        {
            this.Changes = new HashSet<Change>();
            this.FileInstances = new HashSet<FileInstance>();
            this.Projects = new HashSet<Project>();
        }
    
        public string email { get; set; }
    
        public virtual ICollection<Change> Changes { get; set; }
        public virtual ICollection<FileInstance> FileInstances { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
    }
}