using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SliceOfPie_Model.Exceptions
{
    /// <summary>
    /// Throw this exception if no node is selected in the File Tree directory of the GUI.
    /// </summary>
    public class NoNodeSelectedException : System.ApplicationException
    {
    public NoNodeSelectedException() { }
    public NoNodeSelectedException(string message) { }
    public NoNodeSelectedException(string message, System.Exception inner) { }
    }
}
