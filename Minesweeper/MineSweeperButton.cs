using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Minesweeper
{
    public class MineSweeperButton : Button
    {
        private int x;
        private int y;
        private bool hasBomb = false;
        private string data;
        public MineSweeperButton(int x, int y, bool hasBomb)
            : base()
        {
            this.x = x;
            this.y = y;
            this.hasBomb = hasBomb;
            if (hasBomb)
            {
                this.Data = "B";
            }
        }
        public Point SquarePoint
        {
            get { return new Point(x, y); }
        }
        public bool HasBomb
        {
            get { return hasBomb; }
        }
        public bool IsEmpty
        {
            get
            {
                return !HasBomb && !HasData;
            }
        }
        public bool HasData
        {
            get
            {
                return !string.IsNullOrEmpty(data);
            }
        }
        public string Data
        {
            get { return data; }
            set { data = value; }
        }
    }
}
