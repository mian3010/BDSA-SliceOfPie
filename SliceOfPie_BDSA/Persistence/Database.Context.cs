﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SliceOfLifeEntities : DbContext
    {
        public SliceOfLifeEntities()
            : base("name=SliceOfLifeEntities")
        {
          base.Configuration.ProxyCreationEnabled = false;
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Change> Changes { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<FileInstance> FileInstances { get; set; }
        public DbSet<FileMetaData> FileMetaDatas { get; set; }
        public DbSet<MetaDataType> MetaDataTypes { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
