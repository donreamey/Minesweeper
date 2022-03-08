using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Windows.Forms;

namespace Minesweeper
{
    internal class ReversiGrid
    {
        private ReversiButton[,] buttonState;
        private bool[,] emptyVisited;
        private Color[,] c;
        private int upperXBound;
        private int upperYBound;
        private int startX = 128;
        private int startY = 80;
        private Color currentColor = Color.Black;
        private Color emptyColor = Color.Green;
        private Utilities utilties;
        private Button turnButton;
        private int whiteCount = 2;
        private int blackCount = 2;
        private TextBox whiteCountBox;
        private TextBox blackCountBox;
        private Point lastKnownStart;

        internal ReversiGrid(int size, Control.ControlCollection controls)
        {
            buttonState = new ReversiButton[size, size];
            emptyVisited = new bool[size, size];
            upperXBound = buttonState.GetLength(0);
            upperYBound = buttonState.GetLength(1);

            int numberOfBombs = (int)Math.Round(upperYBound * upperYBound * .10);

            for (int Y = 0; Y < upperYBound; Y++)
            {
                for (int X = 0; X < upperXBound; X++)
                {
                    buttonState[X, Y] = new ReversiButton(X, Y, Color.Empty);
                    buttonState[X, Y].BackColor = Color.Green;
                    buttonState[X, Y].Location = new Point(startX, startY);
                    buttonState[X, Y].Size = new Size(70, 70);
                    buttonState[X, Y].MouseDown += CheckGridLocation;
                    startX += 70 + 3;
                    controls.Add(buttonState[X, Y]);
                }

                startX = 128 + 3;
                startY += 70 + 3;
            }

            int middlex = buttonState.GetUpperBound(0) / 2;
            int middley = buttonState.GetUpperBound(1) / 2;
            buttonState[middlex, middley].BackColor = Color.White;
            buttonState[middlex, middley + 1].BackColor = Color.Black;
            buttonState[middlex + 1, middley + 1].BackColor = Color.White;
            buttonState[middlex + 1, middley].BackColor = Color.Black;
            buttonState[middlex, middley].Enabled = false;
            buttonState[middlex, middley + 1].Enabled = false;
            buttonState[middlex + 1, middley + 1].Enabled = false;
            buttonState[middlex + 1, middley].Enabled = false;

            this.utilties = new Utilities(buttonState);
            turnButton = new Button();
            turnButton.Text = "Black";
            turnButton.BackColor = Color.Black;
            turnButton.ForeColor = Color.White;
            turnButton.Location = new Point(0, 30);
            turnButton.Size = new Size(100,50);
            turnButton.Enabled = false;
            controls.Add(turnButton);

            var whiteLabel = new Label();
            whiteLabel.Text = "White";
            whiteLabel.Size = new Size(40,20);
            whiteLabel.Location = new Point(100, 30);
            whiteLabel.BorderStyle = BorderStyle.FixedSingle;
            controls.Add(whiteLabel);

            this.whiteCountBox = new TextBox();
            this.whiteCountBox.Size = new Size(140, 50);
            this.whiteCountBox.Enabled = false;
            this.whiteCountBox.Text = whiteCount.ToString();
            this.whiteCountBox.Location = new Point(160, 30);
            this.whiteCountBox.BorderStyle = BorderStyle.FixedSingle;
            controls.Add(whiteCountBox);

            var blackLabel = new Label();
            blackLabel.Text = "Black";
            blackLabel.Size = new Size(40, 20);
            blackLabel.Location = new Point(100, 60);
            blackLabel.BorderStyle = BorderStyle.FixedSingle;
            controls.Add(blackLabel);

            this.blackCountBox = new TextBox();
            this.blackCountBox.Size = new Size(140, 50);
            this.blackCountBox.Enabled = false;
            this.blackCountBox.Text = blackCount.ToString();
            this.blackCountBox.Location = new Point(160, 60);
            this.blackCountBox.BorderStyle = BorderStyle.FixedSingle;
            controls.Add(blackCountBox);

        }

        private void GuessBestMove()
        {
            var tempState = buttonState;
            Queue<Point> movePoints = new Queue<Point>();
            if (tempState[this.lastKnownStart.X, this.lastKnownStart.Y].IsEmpty)
            {
                return;
            }
            Utilities u = new Utilities(tempState);
            
            movePoints.Enqueue(this.lastKnownStart);

            while(movePoints.Count > 0)
            {
                var p = movePoints.Dequeue();
                if (u.IsValidAndEmptyReversi(p.X, p.Y - 1))
                {
                    this.emptyVisited[p.X, p.Y - 1] = true;
                    //movePoints.Enqueue(new Point(p.X + 1, p.Y));
                    //CheckValidMove(buttonState[p.X + 1, p.Y]);
                    int count = GenericValidate(tempState[p.X, p.Y - 1], Direction.LEFT);
                    break;
                }

                /*
                if (u.isValid(p.X - 1, p.Y) && tempState[p.X - 1, p.Y].BackColor != currentColor)
                {
                    movePoints.Enqueue(p);
                }

                if (u.isValid(p.X , p.Y + 1) && tempState[p.X, p.Y + 1].BackColor != currentColor)
                {
                    movePoints.Enqueue(p);
                }

                if (u.isValid(p.X, p.Y - 1) && tempState[p.X, p.Y - 1].BackColor != currentColor)
                {
                    movePoints.Enqueue(p);
                }

                if (u.isValid(p.X - 1, p.Y - 1) && tempState[p.X - 1, p.Y - 1].BackColor != currentColor)
                {
                    movePoints.Enqueue(p);
                }

                if (u.isValid(p.X + 1, p.Y + 1) && tempState[p.X + 1, p.Y + 1].BackColor != currentColor)
                {
                    movePoints.Enqueue(p);
                }

                if (u.isValid(p.X - 1, p.Y + 1) && tempState[p.X - 1, p.Y + 1].BackColor != currentColor)
                {
                    movePoints.Enqueue(p);
                }

                if (u.isValid(p.X + 1, p.Y - 1) && tempState[p.X + 1, p.Y - 1].BackColor != currentColor)
                {
                    movePoints.Enqueue(p);
                }
                */
            }
        }

        private void CheckNeighbors(Point p)
        {

        }

        private void CheckGridLocation(object sender, EventArgs e)
        {
            if (whiteCount + blackCount == 144)
            {
                MessageBox.Show("Game over no more squares");
            }

            ReversiButton start = (ReversiButton)sender;
            MouseEventArgs me = (MouseEventArgs)e;
            

            if (me.Button == MouseButtons.Right)
            {
                buttonState[start.SquarePoint.X, start.SquarePoint.Y].BackColor = Color.Yellow;
                buttonState[start.SquarePoint.X, start.SquarePoint.Y].Text = "?";
                return;
            }

            CheckValidMove(start);
        }

        private void CheckValidMove(ReversiButton start)
        {
            this.lastKnownStart = start.SquarePoint;
            if (start.BackColor != this.emptyColor)
            {
                return;
            }

            buttonState[start.SquarePoint.X, start.SquarePoint.Y].IsEmpty = false;

            int flipCount = 0;

            flipCount += GenericValidate(start, Direction.LEFT);
            flipCount += GenericValidate(start, Direction.TOP);
            flipCount += GenericValidate(start, Direction.TOPLEFT);
            flipCount += GenericValidate(start, Direction.TOPRIGHT);
            flipCount += GenericValidate(start, Direction.RIGHT);
            flipCount += GenericValidate(start, Direction.BOTTOMRIGHT);
            flipCount += GenericValidate(start, Direction.BOTTOM);
            flipCount += GenericValidate(start, Direction.BOTTOMLEFT);

            if (flipCount > 0)
            { 
                if (this.currentColor == Color.White)
                {
                    this.whiteCount++;
                }
                else if (this.currentColor == Color.Black)
                {
                    this.blackCount++;
                }

                if (currentColor == Color.Black)
                {
                    turnButton.ForeColor = currentColor;
                    currentColor = Color.White;
                    turnButton.Text = "White";
                    if (flipCount > 0)
                    { 
                        this.whiteCount -= flipCount;
                        this.blackCount += flipCount;
                    }
                    this.blackCountBox.Text = blackCount.ToString();
                    this.whiteCountBox.Text = whiteCount.ToString();
                }
                else
                {
                    turnButton.ForeColor = currentColor;
                    currentColor = Color.Black;
                    turnButton.Text = "Black";
                    if (flipCount > 0)
                    {
                        this.whiteCount += flipCount;
                        this.blackCount -= flipCount;
                    }
                    this.blackCountBox.Text = blackCount.ToString();
                    this.whiteCountBox.Text = whiteCount.ToString();
                }

                turnButton.BackColor = currentColor;
            }

            if (currentColor == Color.White)
            {
                GuessBestMove();
            }
        }


        private int GenericValidate(ReversiButton start, Direction direction)
        {
            var p = start.SquarePoint;
            int directionCounter = 0;
            bool isValid = false;
            List<Point> points = new List<Point>();

            while (true)
            {
                directionCounter++;
                Point modifiedPoint = start.SquarePoint;
                switch(direction)
                {
                    case Direction.TOP:
                        {
                            modifiedPoint.Y = modifiedPoint.Y - directionCounter;
                            break;
                        }
                    case Direction.LEFT:
                        {
                            modifiedPoint.X = modifiedPoint.X -directionCounter;
                            break;
                        }
                    case Direction.RIGHT:
                        {
                            modifiedPoint.X = modifiedPoint.X + directionCounter;
                            break;
                        }
                    case Direction.BOTTOM:
                        { 
                            modifiedPoint.Y = modifiedPoint.Y + directionCounter;
                            break;
                        }
                    case Direction.TOPLEFT:
                        {
                            modifiedPoint.Y = modifiedPoint.Y - directionCounter;
                            modifiedPoint.X = modifiedPoint.X - directionCounter;
                            break;
                        }
                    case Direction.TOPRIGHT:
                        {
                            modifiedPoint.Y = modifiedPoint.Y - directionCounter;
                            modifiedPoint.X = modifiedPoint.X + directionCounter;
                            break;
                        }
                    case Direction.BOTTOMLEFT:
                        {
                            modifiedPoint.Y = modifiedPoint.Y + directionCounter;
                            modifiedPoint.X = modifiedPoint.X - directionCounter;
                            break;
                        }
                    case Direction.BOTTOMRIGHT:
                        {
                            modifiedPoint.Y = modifiedPoint.Y + directionCounter;
                            modifiedPoint.X = modifiedPoint.X + directionCounter;
                            break;
                        }
                }

                
                if (this.utilties.isValid(modifiedPoint.X , modifiedPoint.Y))
                {
                    if (buttonState[modifiedPoint.X, modifiedPoint.Y].BackColor != Color.Green && buttonState[modifiedPoint.X, modifiedPoint.Y].BackColor != this.currentColor)
                    {
                        points.Add(new Point(modifiedPoint.X, modifiedPoint.Y));
                    }
                    else
                    {
                        if (points.Count > 0 && buttonState[modifiedPoint.X, modifiedPoint.Y].BackColor == this.currentColor)
                        {
                            isValid = true;
                        }
                        break;
                    }
                }
                else
                {
                    break;
                }
            }

            if (points.Count > 0 && isValid)
            {
                start.BackColor = this.currentColor;
                foreach (Point pt in points)
                {
                    buttonState[pt.X, pt.Y].BackColor = this.currentColor;
                    buttonState[pt.X, pt.Y].IsEmpty = false;
                }

                return points.Count;
            }

            return 0;
        }
    }
}
