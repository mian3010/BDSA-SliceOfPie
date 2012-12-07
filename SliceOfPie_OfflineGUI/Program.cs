using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SliceOfPie_OfflineGUI
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main(String[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form3());
           
             
        }
    }
}
