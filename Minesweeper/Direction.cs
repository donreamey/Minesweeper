using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
    internal enum Direction
    {
        TOP = 0x1,
        RIGHT = 0x2,
        BOTTOM = 0x4,
        LEFT = 0x8,
        TOPRIGHT = TOP | RIGHT,
        TOPLEFT = TOP | LEFT,
        BOTTOMLEFT = BOTTOM | LEFT,
        BOTTOMRIGHT = BOTTOM | RIGHT,
    }
}
