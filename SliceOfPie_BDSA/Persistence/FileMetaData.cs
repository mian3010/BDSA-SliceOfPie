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
    
    public partial class FileMetaData
    {
        public int id { get; set; }
        public string value { get; set; }
        public string MetaDataType_Type { get; set; }
        public int File_id { get; set; }
    
        public virtual File File { get; set; }
        public virtual MetaDataType MetaDataType { get; set; }
    }
}