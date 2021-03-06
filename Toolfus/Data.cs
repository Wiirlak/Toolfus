﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Toolfus
{
    public class Data
    {
        // public static List<Process> dofus = new List<Process>();
        public static ObservableCollection<Process> DofusList = new ObservableCollection<Process>();
        public static Process CurrentDofus;
        
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }
        
        public static Keybind KeyMap = new Keybind();
    }
}