using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SliceOfPie_Model
{
    public enum MetaType
    {
        Type_Document, Type_Image, 
        Author, Length, Publisher

    };

    public enum FileModification
    {
        Modify, Delete, Add, MergeReady, Rename, Move

    };
}
