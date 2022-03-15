using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Reversi
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
            ForeColor = Color.Red;
            InitializeComponent();
        }

        public Point SquarePoint
        {
            get { return new Point(gridX, gridY); }
            set { gridX = value.X; gridY = value.Y; }
        }

        public bool IsEmpty
        {
            get
            {
                return isEmpty;
            }
            set
            {
                isEmpty = value;
            }
        }

        public ReversiButton Clone()
        {
            ReversiButton b = new ReversiButton(this.gridX, this.gridY, this.color);
            b.BackColor = this.BackColor;
            b.Text = this.Text;
            return b;
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
            if(string.IsNullOrEmpty(this.Text))
            {
                this.Text = "X=" + this.gridX + ",Y="+ this.gridY;
                return;
            }
            
            if(this.Text.Contains("VALID"))
            {
                this.Text = "VALID\r\n" + "X=" + this.gridX + ",Y=" + this.gridY;
            }
        }
    }
}
