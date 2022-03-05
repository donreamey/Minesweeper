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
        internal MineSweeperButton[,] buttons = new MineSweeperButton[12,12];
        internal MineSweeperGrid grid;
        internal int upperXBound = 0;
        internal int upperYBound = 0;

        internal enum Direction
        {
            TOP = 0x1,
            RIGHT = 0x2,
            BOTTOM = 0x4,
            LEFT = 0x8,
        }
        public Form1()
        {
            int startX = 128;
            int startY = 80;
            upperXBound = buttons.GetLength(0);
            upperYBound = buttons.GetLength(1);
            int numberOfBombs = (int)Math.Round(upperYBound * upperYBound * .10);
            int[] bombArray = new int[numberOfBombs];

            for(int i = 0; i< numberOfBombs; i++)
            {
                System.Threading.Thread.Sleep(5);
                Random random = new Random(DateTime.Now.Millisecond);
                bombArray[i] = random.Next(i,upperXBound * upperYBound);
            }

            grid = new MineSweeperGrid(12, Controls);
            grid.InitializeGrid();

            //for(int i = 0; i < upperXBound; i++)
            //{
            //    for(int j = 0; j < upperYBound; j++)
            //    {
            //        for(int b=0; b < numberOfBombs;b++)
            //        {
            //            if (bombCounter == bombArray[b])
            //            {
            //                buttons[i, j] = new MineSweeperButton(i, j, true);
            //                break;
            //            }
            //            else
            //            {
            //                buttons[i, j] = new MineSweeperButton(i, j, false);
            //            }
            //        }

            //        bombCounter++;
            //        buttons[i, j].BackColor = Color.Green;
            //        buttons[i, j].Location = new Point(startX, startY);
            //        buttons[i, j].Size = new Size(70, 70);
            //        buttons[i, j].MouseDown += CheckBomb;
            //        startX += 70 + 3;
            //        Controls.Add(buttons[i, j]);
            //    }

            //    startX = 128 + 3;
            //    startY += 70 + 3;
          
            //}

            //InitBombCount();
            InitializeComponent();
        }


        //bool isValid(Point p)
        //{
        //    return isValid(p.X, p.Y);
        //}

        //bool isValid(int x, int y)
        //{
        //    int xupperBound = buttons.GetUpperBound(1);
        //    int yupperBound = buttons.GetUpperBound(0);

        //    if (x < 0 || x > xupperBound || y < 0 || y > yupperBound)
        //    {
        //        return false;
        //    }

        //    return true;
        //}

        //private Point SetDirection(Point p, Direction direction)
        //{
        //    if (direction == Direction.TOP)
        //    {
        //        return new Point(p.X, p.Y - 1);
        //    }

        //    if (direction == (Direction.TOP | Direction.RIGHT))
        //    {
        //        return new Point(p.X + 1, p.Y - 1);
        //    }

        //    if (direction == Direction.RIGHT)
        //    {
        //        return new Point(p.X + 1, p.Y);
        //    }

        //    if (direction == (Direction.BOTTOM | Direction.RIGHT))
        //    {
        //        return new Point(p.X + 1, p.Y + 1);
        //    }

        //    if (direction == Direction.BOTTOM)
        //    {
        //        return new Point(p.X, p.Y + 1);
        //    }

        //    if(direction == (Direction.BOTTOM | Direction.LEFT))
        //    {
        //        return new Point(p.X - 1, p.Y + 1);
        //    }

        //    if(direction==Direction.LEFT)
        //    {
        //        return new Point(p.X - 1, p.Y);
        //    }

        //    if (direction == (Direction.TOP | Direction.LEFT))
        //    {
        //        return new Point(p.X -1, p.Y - 1);
        //    }

        //    throw new Exception("invalid direction");
        //}

        //private void InitBombCount()
        //{
        //    bool[,] visited = new bool[buttons.GetUpperBound(1) + 1, buttons.GetUpperBound(0) + 1];
        //    Point p1 = new Point(0, 0);
        //    Queue<Point> q = new Queue<Point>();
        //    int bombCount = 0;

        //    q.Enqueue(p1);
        //    while(q.Count > 0)
        //    {
        //        var p = q.Dequeue();
        //        if (visited[p.X, p.Y])
        //        {
        //            continue;
        //        }

        //        visited[p.X, p.Y] = true;

        //        if (isValid(SetDirection(p, Direction.TOP | Direction.LEFT)))
        //        {
        //            if (buttons[p.X - 1, p.Y - 1].HasBomb)
        //            {
        //                bombCount++;
        //            }

        //            q.Enqueue(new Point(p.X - 1, p.Y - 1));

        //        }

        //        if (isValid(SetDirection(p,Direction.LEFT)))
        //        {
        //            if (buttons[p.X - 1, p.Y].HasBomb)
        //            {
        //                bombCount++;
        //            }

        //            q.Enqueue(new Point(p.X - 1, p.Y));
        //        }

        //        if (isValid(SetDirection(p, Direction.BOTTOM | Direction.LEFT)))
        //        {
        //            if (buttons[p.X - 1, p.Y + 1].HasBomb)
        //            {
        //                bombCount++;
        //            }

        //            q.Enqueue(new Point(p.X - 1, p.Y + 1));
        //        }

        //        if (isValid(SetDirection(p, Direction.BOTTOM)))
        //        {
        //            if (buttons[p.X, p.Y + 1].HasBomb)
        //            {
        //                bombCount++;
        //            }
        //            q.Enqueue(new Point(p.X, p.Y + 1));
        //        }

        //        if (isValid(SetDirection(p, Direction.TOP)))
        //        {
        //            if (buttons[p.X, p.Y - 1].HasBomb)
        //            {
        //                bombCount++;
        //            }
        //            q.Enqueue(new Point(p.X, p.Y - 1));
        //        }

        //        if (isValid(p.X + 1, p.Y + 1))
        //        {
        //            if (buttons[p.X + 1, p.Y + 1].HasBomb)
        //            {
        //                bombCount++;
        //            }
        //            q.Enqueue(new Point(p.X + 1, p.Y + 1));
        //        }

        //        if (isValid(SetDirection(p, Direction.RIGHT)))
        //        {
        //            if (buttons[p.X + 1, p.Y].HasBomb)
        //            {
        //                bombCount++;
        //            }
        //            q.Enqueue(new Point(p.X + 1, p.Y));
        //        }

        //        if (isValid(SetDirection(p, Direction.TOP | Direction.RIGHT)))
        //        {
        //            if (buttons[p.X + 1, p.Y - 1].HasBomb)
        //            {
        //                bombCount++;
        //            }
        //            q.Enqueue(new Point(p.X + 1, p.Y - 1));
        //        }

        //        if (!buttons[p.X, p.Y].HasBomb)
        //        {
        //            if (bombCount == 0)
        //            {
        //                buttons[p.X, p.Y].Text = "";
        //                buttons[p.X, p.Y].Data = "";
        //            }
        //            else 
        //            { 
        //                buttons[p.X, p.Y].Data = "" + bombCount;
        //            }
        //        }

        //        bombCount = 0;
        //    }
        //}

        //private void CheckBomb(object sender, EventArgs e)
        //{
        //    MineSweeperButton bomb = (MineSweeperButton)sender;
        //    MouseEventArgs me = (MouseEventArgs)e;
        //    if (me.Button == MouseButtons.Right)
        //    {
        //        buttons[bomb.SquarePoint.X, bomb.SquarePoint.Y].BackColor = Color.Yellow;
        //        buttons[bomb.SquarePoint.X, bomb.SquarePoint.Y].Text = "?";
        //        return;
        //    }

        //    if (bomb.HasBomb)
        //    {
        //        buttons[bomb.SquarePoint.X, bomb.SquarePoint.Y].BackColor = Color.Red;
        //        buttons[bomb.SquarePoint.X, bomb.SquarePoint.Y].Text = "B";
        //        for (int i = 0; i < upperXBound; i++)
        //        {
        //            for (int j = 0; j < upperYBound; j++)
        //            {
        //                buttons[(int)i, j].Text = this.buttons[i, j].Data;
        //                this.buttons[i, j].Enabled = false;

        //                if (this.buttons[i, j].Data.Equals("B"))
        //                {
        //                    buttons[i, j].BackColor = Color.Red;
        //                    continue;
        //                }

        //                if(this.buttons[i,j].HasData)
        //                {
        //                    buttons[(int)i,j].BackColor = Color.Wheat;
        //                    continue;
        //                }

        //                if(this.buttons[(int)i,j].IsEmpty)
        //                {
        //                    buttons[(int)i, j].BackColor = Color.White;
        //                    this.buttons[i, j].Enabled = false;
        //                    continue;
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        bool[,] visited = new bool[buttons.GetUpperBound(1) + 1, buttons.GetUpperBound(0) + 1];
        //        Queue<Point> q = new Queue<Point>();

        //        q.Enqueue(bomb.SquarePoint);
        //        while (q.Count > 0)
        //        {
        //            var p = q.Dequeue();
        //            if (visited[p.X, p.Y])
        //            {
        //                continue;
        //            }

        //            visited[p.X, p.Y] = true;

        //            if(buttons[p.X, p.Y].IsEmpty)
        //            {
        //                buttons[p.X, p.Y].BackColor = Color.White;

        //                if (isValid(p.X - 1, p.Y - 1))
        //                {
        //                    if(buttons[p.X - 1, p.Y - 1].IsEmpty)
        //                    {
        //                        q.Enqueue(buttons[p.X - 1, p.Y - 1].SquarePoint);
        //                    }
        //                    else if (buttons[p.X - 1, p.Y - 1].HasData)
        //                    {
        //                        buttons[p.X - 1, p.Y - 1].Text = buttons[p.X - 1, p.Y - 1].Data;
        //                        buttons[p.X - 1, p.Y - 1].BackColor = Color.Wheat;
        //                    }
        //                }

        //                if (isValid(p.X - 1, p.Y))
        //                {
        //                    if (buttons[p.X - 1, p.Y].IsEmpty)
        //                    {
        //                        q.Enqueue(buttons[p.X - 1, p.Y].SquarePoint);
        //                    }
        //                    else if (buttons[p.X - 1, p.Y].HasData)
        //                    {
        //                        buttons[p.X - 1, p.Y].Text = buttons[p.X - 1, p.Y].Data;
        //                        buttons[p.X - 1, p.Y].BackColor = Color.Wheat;
        //                    }
        //                }

        //                if (isValid(p.X - 1, p.Y + 1))
        //                {
        //                    if (buttons[p.X - 1, p.Y + 1].IsEmpty)
        //                    {
        //                        q.Enqueue(buttons[p.X - 1, p.Y + 1].SquarePoint);
        //                    }
        //                    else if (buttons[p.X - 1, p.Y + 1].HasData)
        //                    {
        //                        buttons[p.X - 1, p.Y + 1].Text = buttons[p.X - 1, p.Y + 1].Data;
        //                        buttons[p.X - 1, p.Y + 1].BackColor = Color.Wheat;
        //                    }
        //                }

        //                if (isValid(p.X, p.Y + 1))
        //                {
        //                    if (buttons[p.X, p.Y + 1].IsEmpty)
        //                    {
        //                        q.Enqueue(buttons[p.X, p.Y + 1].SquarePoint);
        //                    }
        //                    else if (buttons[p.X, p.Y + 1].HasData)
        //                    {
        //                        buttons[p.X, p.Y + 1].Text = buttons[p.X, p.Y + 1].Data;
        //                        buttons[p.X, p.Y + 1].BackColor = Color.Wheat;
        //                    }
        //                }

        //                if (isValid(p.X, p.Y - 1))
        //                {
        //                    if (buttons[p.X, p.Y - 1].IsEmpty)
        //                    {
        //                        q.Enqueue(buttons[p.X, p.Y - 1].SquarePoint);
        //                    }
        //                    else if (buttons[p.X, p.Y - 1].HasData)
        //                    {
        //                        buttons[p.X, p.Y - 1].Text = buttons[p.X, p.Y - 1].Data;
        //                        buttons[p.X, p.Y - 1].BackColor = Color.Wheat;
        //                    }
        //                }

        //                if (isValid(p.X + 1, p.Y + 1))
        //                {
        //                    if (buttons[p.X + 1, p.Y + 1].IsEmpty)
        //                    {
        //                        q.Enqueue(buttons[p.X + 1, p.Y + 1].SquarePoint);
        //                    }
        //                    else if (buttons[p.X + 1, p.Y + 1].HasData)
        //                    {
        //                        buttons[p.X + 1, p.Y + 1].Text = buttons[p.X + 1, p.Y + 1].Data;
        //                        buttons[p.X + 1, p.Y + 1].BackColor = Color.Wheat;
        //                    }
        //                }

        //                if (isValid(p.X + 1, p.Y))
        //                {
        //                    if (buttons[p.X + 1, p.Y].IsEmpty)
        //                    {
        //                        q.Enqueue(buttons[p.X + 1, p.Y].SquarePoint);
        //                    }
        //                    else if (buttons[p.X + 1, p.Y].HasData)
        //                    {
        //                        buttons[p.X + 1, p.Y].Text = buttons[p.X + 1, p.Y].Data;
        //                        buttons[p.X + 1, p.Y].BackColor = Color.Wheat;
        //                    }
        //                }

        //                if (isValid(p.X + 1, p.Y - 1))
        //                {
        //                    if (buttons[p.X + 1, p.Y - 1].IsEmpty)
        //                    {
        //                        q.Enqueue(buttons[p.X + 1, p.Y - 1].SquarePoint);
        //                    }
        //                    else if (buttons[p.X + 1, p.Y - 1].HasData)
        //                    {
        //                        buttons[p.X + 1, p.Y - 1].Text = buttons[p.X + 1, p.Y - 1].Data;
        //                        buttons[p.X + 1, p.Y - 1].BackColor = Color.Wheat;
        //                    }
        //                }
        //            }

        //            if( !buttons[p.X, p.Y].IsEmpty)
        //            {
        //                if (buttons[p.X, p.Y].HasBomb)
        //                {
        //                    buttons[p.X, p.Y].BackColor = Color.Red;
        //                }
        //                if (buttons[p.X, p.Y ].HasData)
        //                {
        //                    buttons[p.X, p.Y].Text = buttons[p.X, p.Y].Data;
        //                    buttons[p.X, p.Y].BackColor = Color.Wheat;
        //                }
        //            }
        //        }
        //    }
           
        //}
    }

    internal class MineSweeperGrid
    {
        internal enum Direction
        {
            TOP = 0x1,
            RIGHT = 0x2,
            BOTTOM = 0x4,
            LEFT = 0x8,
        }

        private MineSweeperButton[,] buttons;
        private int upperXBound;
        private int upperYBound;
        private int startX = 128;
        private int startY = 80;
        internal MineSweeperGrid(int size, Control.ControlCollection controls)
        {
            buttons = new MineSweeperButton[size,size];
            upperXBound = buttons.GetLength(0);
            upperYBound = buttons.GetLength(1);

            int numberOfBombs = (int)Math.Round(upperYBound * upperYBound * .10);
            int[] bombArray = new int[numberOfBombs];

            for (int i = 0; i < numberOfBombs; i++)
            {
                System.Threading.Thread.Sleep(5);
                Random random = new Random(DateTime.Now.Millisecond);
                bombArray[i] = random.Next(i, upperXBound * upperYBound);
            }

            int bombCounter = 0;

            for (int i = 0; i < upperXBound; i++)
            {
                for (int j = 0; j < upperYBound; j++)
                {
                    for (int b = 0; b < numberOfBombs; b++)
                    {
                        if (bombCounter == bombArray[b])
                        {
                            buttons[i, j] = new MineSweeperButton(i, j, true);
                            break;
                        }
                        else
                        {
                            buttons[i, j] = new MineSweeperButton(i, j, false);
                        }
                    }

                    bombCounter++;
                    buttons[i, j].BackColor = Color.Green;
                    buttons[i, j].Location = new Point(startX, startY);
                    buttons[i, j].Size = new Size(70, 70);
                    buttons[i, j].MouseDown += CheckGridLocation;
                    startX += 70 + 3;
                    controls.Add(buttons[i, j]);
                }

                startX = 128 + 3;
                startY += 70 + 3;
            }
        }

        internal void SetMineSweeperButton(int i, int j, bool hasBomb)
        {

        }

        internal int X
        {
            get { return buttons.GetLength(0);}
        }

        internal int Y
        {
            get { return buttons.GetLength(1); }
        }

        private void CheckGridLocation(object sender, EventArgs e)
        {
            MineSweeperButton bomb = (MineSweeperButton)sender;
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Right)
            {
                buttons[bomb.SquarePoint.X, bomb.SquarePoint.Y].BackColor = Color.Yellow;
                buttons[bomb.SquarePoint.X, bomb.SquarePoint.Y].Text = "?";
                return;
            }

            if (bomb.HasBomb)
            {
                buttons[bomb.SquarePoint.X, bomb.SquarePoint.Y].BackColor = Color.Red;
                buttons[bomb.SquarePoint.X, bomb.SquarePoint.Y].Text = "B";
                for (int i = 0; i < this.upperXBound; i++)
                {
                    for (int j = 0; j < this.upperYBound; j++)
                    {
                        buttons[(int)i, j].Text = this.buttons[i, j].Data;
                        this.buttons[i, j].Enabled = false;

                        if (this.buttons[i, j].Data.Equals("B"))
                        {
                            buttons[i, j].BackColor = Color.Red;
                            continue;
                        }

                        if (this.buttons[i, j].HasData)
                        {
                            buttons[(int)i, j].BackColor = Color.Wheat;
                            continue;
                        }

                        if (this.buttons[(int)i, j].IsEmpty)
                        {
                            buttons[(int)i, j].BackColor = Color.White;
                            this.buttons[i, j].Enabled = false;
                            continue;
                        }
                    }
                }
            }
            else
            {
                bool[,] visited = new bool[buttons.GetUpperBound(1) + 1, buttons.GetUpperBound(0) + 1];
                Queue<Point> q = new Queue<Point>();

                q.Enqueue(bomb.SquarePoint);
                while (q.Count > 0)
                {
                    var p = q.Dequeue();
                    if (visited[p.X, p.Y])
                    {
                        continue;
                    }

                    visited[p.X, p.Y] = true;

                    if (buttons[p.X, p.Y].IsEmpty)
                    {
                        buttons[p.X, p.Y].BackColor = Color.White;

                        if (isValid(p.X - 1, p.Y - 1))
                        {
                            if (buttons[p.X - 1, p.Y - 1].IsEmpty)
                            {
                                q.Enqueue(buttons[p.X - 1, p.Y - 1].SquarePoint);
                            }
                            else if (buttons[p.X - 1, p.Y - 1].HasData)
                            {
                                buttons[p.X - 1, p.Y - 1].Text = buttons[p.X - 1, p.Y - 1].Data;
                                buttons[p.X - 1, p.Y - 1].BackColor = Color.Wheat;
                            }
                        }

                        if (isValid(p.X - 1, p.Y))
                        {
                            if (buttons[p.X - 1, p.Y].IsEmpty)
                            {
                                q.Enqueue(buttons[p.X - 1, p.Y].SquarePoint);
                            }
                            else if (buttons[p.X - 1, p.Y].HasData)
                            {
                                buttons[p.X - 1, p.Y].Text = buttons[p.X - 1, p.Y].Data;
                                buttons[p.X - 1, p.Y].BackColor = Color.Wheat;
                            }
                        }

                        if (isValid(p.X - 1, p.Y + 1))
                        {
                            if (buttons[p.X - 1, p.Y + 1].IsEmpty)
                            {
                                q.Enqueue(buttons[p.X - 1, p.Y + 1].SquarePoint);
                            }
                            else if (buttons[p.X - 1, p.Y + 1].HasData)
                            {
                                buttons[p.X - 1, p.Y + 1].Text = buttons[p.X - 1, p.Y + 1].Data;
                                buttons[p.X - 1, p.Y + 1].BackColor = Color.Wheat;
                            }
                        }

                        if (isValid(p.X, p.Y + 1))
                        {
                            if (buttons[p.X, p.Y + 1].IsEmpty)
                            {
                                q.Enqueue(buttons[p.X, p.Y + 1].SquarePoint);
                            }
                            else if (buttons[p.X, p.Y + 1].HasData)
                            {
                                buttons[p.X, p.Y + 1].Text = buttons[p.X, p.Y + 1].Data;
                                buttons[p.X, p.Y + 1].BackColor = Color.Wheat;
                            }
                        }

                        if (isValid(p.X, p.Y - 1))
                        {
                            if (buttons[p.X, p.Y - 1].IsEmpty)
                            {
                                q.Enqueue(buttons[p.X, p.Y - 1].SquarePoint);
                            }
                            else if (buttons[p.X, p.Y - 1].HasData)
                            {
                                buttons[p.X, p.Y - 1].Text = buttons[p.X, p.Y - 1].Data;
                                buttons[p.X, p.Y - 1].BackColor = Color.Wheat;
                            }
                        }

                        if (isValid(p.X + 1, p.Y + 1))
                        {
                            if (buttons[p.X + 1, p.Y + 1].IsEmpty)
                            {
                                q.Enqueue(buttons[p.X + 1, p.Y + 1].SquarePoint);
                            }
                            else if (buttons[p.X + 1, p.Y + 1].HasData)
                            {
                                buttons[p.X + 1, p.Y + 1].Text = buttons[p.X + 1, p.Y + 1].Data;
                                buttons[p.X + 1, p.Y + 1].BackColor = Color.Wheat;
                            }
                        }

                        if (isValid(p.X + 1, p.Y))
                        {
                            if (buttons[p.X + 1, p.Y].IsEmpty)
                            {
                                q.Enqueue(buttons[p.X + 1, p.Y].SquarePoint);
                            }
                            else if (buttons[p.X + 1, p.Y].HasData)
                            {
                                buttons[p.X + 1, p.Y].Text = buttons[p.X + 1, p.Y].Data;
                                buttons[p.X + 1, p.Y].BackColor = Color.Wheat;
                            }
                        }

                        if (isValid(p.X + 1, p.Y - 1))
                        {
                            if (buttons[p.X + 1, p.Y - 1].IsEmpty)
                            {
                                q.Enqueue(buttons[p.X + 1, p.Y - 1].SquarePoint);
                            }
                            else if (buttons[p.X + 1, p.Y - 1].HasData)
                            {
                                buttons[p.X + 1, p.Y - 1].Text = buttons[p.X + 1, p.Y - 1].Data;
                                buttons[p.X + 1, p.Y - 1].BackColor = Color.Wheat;
                            }
                        }
                    }

                    if (!buttons[p.X, p.Y].IsEmpty)
                    {
                        if (buttons[p.X, p.Y].HasBomb)
                        {
                            buttons[p.X, p.Y].BackColor = Color.Red;
                        }
                        if (buttons[p.X, p.Y].HasData)
                        {
                            buttons[p.X, p.Y].Text = buttons[p.X, p.Y].Data;
                            buttons[p.X, p.Y].BackColor = Color.Wheat;
                        }
                    }
                }
            }

        }

        bool isValid(Point p)
        {
            return isValid(p.X, p.Y);
        }

        bool isValid(int x, int y)
        {
            int xupperBound = buttons.GetUpperBound(1);
            int yupperBound = buttons.GetUpperBound(0);

            if (x < 0 || x > xupperBound || y < 0 || y > yupperBound)
            {
                return false;
            }

            return true;
        }

        internal void InitializeGrid()
        {
            bool[,] visited = new bool[buttons.GetUpperBound(1) + 1, buttons.GetUpperBound(0) + 1];
            Point p1 = new Point(0, 0);
            Queue<Point> q = new Queue<Point>();
            int bombCount = 0;

            q.Enqueue(p1);
            while (q.Count > 0)
            {
                var p = q.Dequeue();
                if (visited[p.X, p.Y])
                {
                    continue;
                }

                visited[p.X, p.Y] = true;

                if (isValid(SetDirection(p, Direction.TOP | Direction.LEFT)))
                {
                    if (buttons[p.X - 1, p.Y - 1].HasBomb)
                    {
                        bombCount++;
                    }

                    q.Enqueue(new Point(p.X - 1, p.Y - 1));

                }

                if (isValid(SetDirection(p, Direction.LEFT)))
                {
                    if (buttons[p.X - 1, p.Y].HasBomb)
                    {
                        bombCount++;
                    }

                    q.Enqueue(new Point(p.X - 1, p.Y));
                }

                if (isValid(SetDirection(p, Direction.BOTTOM | Direction.LEFT)))
                {
                    if (buttons[p.X - 1, p.Y + 1].HasBomb)
                    {
                        bombCount++;
                    }

                    q.Enqueue(new Point(p.X - 1, p.Y + 1));
                }

                if (isValid(SetDirection(p, Direction.BOTTOM)))
                {
                    if (buttons[p.X, p.Y + 1].HasBomb)
                    {
                        bombCount++;
                    }
                    q.Enqueue(new Point(p.X, p.Y + 1));
                }

                if (isValid(SetDirection(p, Direction.TOP)))
                {
                    if (buttons[p.X, p.Y - 1].HasBomb)
                    {
                        bombCount++;
                    }
                    q.Enqueue(new Point(p.X, p.Y - 1));
                }

                if (isValid(p.X + 1, p.Y + 1))
                {
                    if (buttons[p.X + 1, p.Y + 1].HasBomb)
                    {
                        bombCount++;
                    }
                    q.Enqueue(new Point(p.X + 1, p.Y + 1));
                }

                if (isValid(SetDirection(p, Direction.RIGHT)))
                {
                    if (buttons[p.X + 1, p.Y].HasBomb)
                    {
                        bombCount++;
                    }
                    q.Enqueue(new Point(p.X + 1, p.Y));
                }

                if (isValid(SetDirection(p, Direction.TOP | Direction.RIGHT)))
                {
                    if (buttons[p.X + 1, p.Y - 1].HasBomb)
                    {
                        bombCount++;
                    }
                    q.Enqueue(new Point(p.X + 1, p.Y - 1));
                }

                if (!buttons[p.X, p.Y].HasBomb)
                {
                    if (bombCount == 0)
                    {
                        buttons[p.X, p.Y].Text = "";
                        buttons[p.X, p.Y].Data = "";
                    }
                    else
                    {
                        buttons[p.X, p.Y].Data = "" + bombCount;
                    }
                }

                bombCount = 0;
            }
        }

        private Point SetDirection(Point p, Direction direction)
        {
            if (direction == Direction.TOP)
            {
                return new Point(p.X, p.Y - 1);
            }

            if (direction == (Direction.TOP | Direction.RIGHT))
            {
                return new Point(p.X + 1, p.Y - 1);
            }

            if (direction == Direction.RIGHT)
            {
                return new Point(p.X + 1, p.Y);
            }

            if (direction == (Direction.BOTTOM | Direction.RIGHT))
            {
                return new Point(p.X + 1, p.Y + 1);
            }

            if (direction == Direction.BOTTOM)
            {
                return new Point(p.X, p.Y + 1);
            }

            if (direction == (Direction.BOTTOM | Direction.LEFT))
            {
                return new Point(p.X - 1, p.Y + 1);
            }

            if (direction == Direction.LEFT)
            {
                return new Point(p.X - 1, p.Y);
            }

            if (direction == (Direction.TOP | Direction.LEFT))
            {
                return new Point(p.X - 1, p.Y - 1);
            }

            throw new Exception("invalid direction");
        }
    }

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
            get{return new Point(x, y);}
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
