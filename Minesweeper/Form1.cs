using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
