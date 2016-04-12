using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scr
{
    class iLine
    {
        private int[] start, end;

        public iLine()
        {
            start = new int[0];
            end = new int[0];
        }

        public iLine (int[] _start, int[] _end)
        {
            start = _start;
            end = _end;
        }
    }
}
