using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace WindowsFormsApplication1.Helper
{
    class GetProcessUsedMemory
    {
        public double GetMemory()
        {
            double usedMemory = 0;
            usedMemory = Process.GetCurrentProcess().WorkingSet64 / 1024.0 / 1024.0;
            return usedMemory;
        }
    }
}
