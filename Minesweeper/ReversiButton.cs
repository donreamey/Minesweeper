using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Minesweeper
{
    internal class ReversiButton : Button
    {
        private int gridX;
        private int gridY;
        private System.Drawing.Color color;
        bool isEmpty = true;
        public ReversiButton(int x, int y, System.Drawing.Color color)
            : base()
        {
            this.gridX = x;
            this.gridY = y;
            this.color = color;
            isEmpty = color == Color.Empty;
            InitializeComponent();
        }


        public Point SquarePoint
        {
            get { return new Point(gridX, gridY); }
        }

        public bool IsEmpty
        {
            get
            {
                return isEmpty;
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ReversiButton
            // 
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ReversiButton_MouseMove);
            this.ResumeLayout(false);

        }

        private void ReversiButton_MouseMove(object sender, MouseEventArgs e)
        {
            this.Text = "X=" + this.gridX + ",Y="+ this.gridY;
        }
    }
}
