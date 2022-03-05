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

        public Form1()
        {
            grid = new MineSweeperGrid(12, Controls);
            grid.InitializeGrid();
            InitializeComponent();
        }
    }


}
