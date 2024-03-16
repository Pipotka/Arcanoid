using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arkanoid
{
    struct Section
    {
        public int Start;
        public int End;

        public Section(int start, int end)
        {
            Start = start;
            End = end;
        }
    }
}
