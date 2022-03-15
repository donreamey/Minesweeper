using System;
using System.Windows.Forms;
using Reversi;

namespace Minesweeper
{
    public partial class Form1 : Form
    {
        internal MineSweeperGrid grid;
        internal ReversiGrid reversi;

        public Form1()
        {

            InitializeComponent();
        }

        private void minesweeperToolStripMenuItem_Click(object sender, EventArgs e)
        {
            grid = new MineSweeperGrid(12, Controls);
            grid.InitializeGrid();
        }

        private void reversiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            reversi = new ReversiGrid(12, Controls);
        }
    }


}
