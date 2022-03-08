using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Minesweeper
{
    internal class Utilities
    {
        private Button[,] buttons;

        internal Utilities(Button[,] buttons)
        {
            this.buttons = buttons;
        }

        internal bool isValid(Point p)
        {
            return isValid(p.X, p.Y);
        }

        internal  bool isValid(int x, int y)
        {
            int xupperBound = buttons.GetUpperBound(1);
            int yupperBound = buttons.GetUpperBound(0);

            if (x < 0 || x > xupperBound || y < 0 || y > yupperBound)
            {
                return false;
            }

            return true;
        }

        internal bool IsValidAndEmptyReversi(int x, int y)
        {
            if(!isValid(x, y) || !((ReversiButton)this.buttons[x, y]).IsEmpty)
            {
                return false;
            }

            return true;
        }
    }

    internal class Visited
    {
        public int x;
        public int y;
        public Color color;
    }
}
