using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;

namespace pdachess
{
    static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [MTAThread]
        static void Main()
        {
            Application.Run(new Form1());
        }
    }
}