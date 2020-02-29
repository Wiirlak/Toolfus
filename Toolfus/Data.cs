using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Toolfus
{
    public class Data
    {
        public static List<Process> dofus = new List<Process>();
        
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }
    }
}