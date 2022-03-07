using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Minesweeper
{
    internal class MineSweeperGrid
    {
        private MineSweeperButton[,] buttons;
        private int upperXBound;
        private int upperYBound;
        private int startX = 128;
        private int startY = 80;
        private bool[,] visited;
        private int visitCount = 0;
        private Utilities utilties;

        internal MineSweeperGrid(int size, Control.ControlCollection controls)
        {
            buttons = new MineSweeperButton[size, size];
            upperXBound = buttons.GetLength(0);
            upperYBound = buttons.GetLength(1);

            int numberOfBombs = (int)Math.Round(upperYBound * upperYBound * .10);
            visited = new bool[buttons.GetUpperBound(1) + 1, buttons.GetUpperBound(0) + 1];
            int[] bombArray = new int[numberOfBombs];

            for (int i = 0; i < numberOfBombs; i++)
            {
                System.Threading.Thread.Sleep(5);
                Random random = new Random(DateTime.Now.Millisecond);
                bombArray[i] = random.Next(0, upperXBound * upperYBound);
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

            this.utilties = new Utilities(buttons);
        }

        internal void SetMineSweeperButton(int i, int j, bool hasBomb)
        {

        }

        private void CheckGridLocation(object sender, EventArgs e)
        {
            if (visitCount == upperYBound * upperYBound)
            {
                MessageBox.Show("Game Over");
                return;
            }
            
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

                        if (this.buttons[i, j].HasBomb)
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

                MessageBox.Show("Game Over - You lose!");
            }
            else
            {
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
                    visitCount++;

                    if (buttons[p.X, p.Y].IsEmpty)
                    {
                        buttons[p.X, p.Y].BackColor = Color.White;

                        if (utilties.isValid(p.X - 1, p.Y - 1))
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

                        if (utilties.isValid(p.X - 1, p.Y))
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

                        if (utilties.isValid(p.X - 1, p.Y + 1))
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

                        if (utilties.isValid(p.X, p.Y + 1))
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

                        if (utilties.isValid(p.X, p.Y - 1))
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

                        if (utilties.isValid(p.X + 1, p.Y + 1))
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

                        if (utilties.isValid(p.X + 1, p.Y))
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

                        if (utilties.isValid(p.X + 1, p.Y - 1))
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

        //private bool isValid(Point p)
        //{
        //    return isValid(p.X, p.Y);
        //}

        //private bool isValid(int x, int y)
        //{
        //    int xupperBound = buttons.GetUpperBound(1);
        //    int yupperBound = buttons.GetUpperBound(0);

        //    if (x < 0 || x > xupperBound || y < 0 || y > yupperBound)
        //    {
        //        return false;
        //    }

        //    return true;
        //}

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

                if (this.utilties.isValid(SetDirection(p, Direction.TOPLEFT)))
                {
                    bombCount = Enqueue(q, bombCount, p, Direction.TOPLEFT);
                }

                if (this.utilties.isValid(SetDirection(p, Direction.LEFT)))
                {
                    bombCount = Enqueue(q, bombCount, p, Direction.LEFT);
                }

                if (this.utilties.isValid(SetDirection(p, Direction.BOTTOMLEFT)))
                {
                    bombCount = Enqueue(q, bombCount, p, Direction.BOTTOMLEFT);
                }

                if (this.utilties.isValid(SetDirection(p, Direction.BOTTOM)))
                {
                    bombCount = Enqueue(q, bombCount, p, Direction.BOTTOM);
                }

                if (this.utilties.isValid(SetDirection(p, Direction.TOP)))
                {
                    bombCount = Enqueue(q, bombCount, p, Direction.TOP);
                }

                if (this.utilties.isValid(SetDirection(p, Direction.TOPRIGHT)))
                {
                    bombCount = Enqueue(q, bombCount, p, Direction.TOPRIGHT);
                }

                if (this.utilties.isValid(SetDirection(p, Direction.RIGHT)))
                {
                    bombCount = Enqueue(q, bombCount, p, Direction.RIGHT );
                }

                if (this.utilties.isValid(SetDirection(p, Direction.BOTTOMRIGHT)))
                {
                    bombCount = Enqueue(q, bombCount, p, Direction.BOTTOMRIGHT);
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

        private int Enqueue(Queue<Point> q, int bombCount, Point p, Direction d)
        {
            Point pdir = SetDirection(p, d);
            if (buttons[pdir.X, pdir.Y].HasBomb)
            {
                bombCount++;
            }

            q.Enqueue(pdir);
            return bombCount;
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
}
