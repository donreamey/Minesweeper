using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Windows.Forms;
using Minesweeper;

namespace Reversi
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
            buttonState[middlex, middley].IsEmpty = false;
            buttonState[middlex, middley].Enabled = false;

            buttonState[middlex, middley + 1].BackColor = Color.Black;
            buttonState[middlex, middley + 1].IsEmpty = false;
            buttonState[middlex, middley + 1].Enabled = false;

            buttonState[middlex + 1, middley + 1].BackColor = Color.White;
            buttonState[middlex + 1, middley + 1].IsEmpty = false;
            buttonState[middlex, middley + 1].Enabled = false;
            buttonState[middlex + 1, middley + 1].Enabled = false;

            buttonState[middlex + 1, middley].BackColor = Color.Black;
            buttonState[middlex + 1, middley].IsEmpty = false;
            buttonState[middlex + 1, middley].Enabled = false;

            this.utilties = new Utilities(buttonState);
            turnButton = new Button();
            turnButton.Text = "Black";
            turnButton.BackColor = Color.Black;
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
            var tempState = (ReversiButton[,])buttonState.Clone();

            Queue<Point> movePoints = new Queue<Point>();
            if (tempState[this.lastKnownStart.X, this.lastKnownStart.Y].IsEmpty)
            {
                return;
            }
            Utilities u = new Utilities(tempState);

            List<Point> availableMoves = new List<Point>();

            var tempPoint = this.lastKnownStart;

            foreach(Direction direction in Enum.GetValues(typeof(Direction)))
            {
                tempPoint = ModifyPointDirection(direction , this.lastKnownStart);
                if (u.IsValidAndEmptyReversi(tempPoint))
                { 
                    this.emptyVisited[tempPoint.X, tempPoint.Y] = true;
                    availableMoves.Add(tempPoint);
                    tempState[tempPoint.X, tempPoint.Y].Text = "VALID";
                }
            }
            
            var m = new List<Tuple<int, FlipCounts>>();
            var map = new Dictionary<int, FlipCounts>();
            if (availableMoves.Count > 0)
            {
                int flipCount;
                Trace.WriteLine("check available moves");
                foreach(Point start in availableMoves)
                {
                    Trace.WriteLine("check available moves x = " + start.X +", Y = "+ start.Y);
                    var startTempButton = tempState[start.X, start.Y].Clone();
                    var guessState = new State[12, 12];
                    var points = new List<Point>();
                    flipCount = CheckValidMove(startTempButton, tempState, out points, out guessState, true);

                    // reset tempState;
                    tempState = (ReversiButton[,])buttonState.Clone();

                    if (flipCount > 0)
                    {
                        // guess moves only play as white so change 
                        // the color of the start button only when
                        // the flip count is greater 0
                        startTempButton.BackColor = Color.White;
                        guessState[startTempButton.SquarePoint.X, startTempButton.SquarePoint.Y].IsEmpty = false;
                        guessState[startTempButton.SquarePoint.X, startTempButton.SquarePoint.Y].BackColor = Color.White;

                        points.Add(new Point(startTempButton.SquarePoint.X, startTempButton.SquarePoint.Y));
                        var flipCounts = new FlipCounts
                        {
                            states = guessState,
                            points = points
                        };

                        m.Add(new Tuple<int, FlipCounts>(points.Count, flipCounts));
                        //map.Add(points.Count, flipCounts);
                    }
                }
            }

            if (m.Count > 0)
            {
                ApplyBestMove(m);
            }

            lastKnownStart = tempPoint;
        }

        private void ApplyBestMove(List<Tuple<int, FlipCounts>> map)
        {
            int count = 0;
            int index = 0;

            foreach (var x in map)
            {
                if (x.Item1 > count)
                {
                    index++;
                    count = x.Item1;
                }
            }

            var flips = map[index-1].Item2;

            //var point = new Point(flips.startButton.SquarePoint.X, flips.startButton.SquarePoint.Y);
            //this.buttonState[point.X, point.Y] = flips.startButton.Clone();

            foreach (var p in flips.points)
            {
                ((ReversiButton)this.buttonState[p.X, p.Y]).BackColor = flips.states[p.X, p.Y].BackColor;
                ((ReversiButton)this.buttonState[p.X, p.Y]).SquarePoint = p;
                ((ReversiButton)this.buttonState[p.X, p.Y]).IsEmpty = flips.states[p.X, p.Y].IsEmpty;
                ((ReversiButton)this.buttonState[p.X, p.Y]).BackColor = flips.states[p.X, p.Y].BackColor;
            }

            FlipColorsOnCount(flips.points.Count);

            currentColor = Color.Black;
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

            var guessState = new State[0,0];
            var points = new List<Point>();
            int flipCount = CheckValidMove(start, this.buttonState,out points, out guessState);
            FlipColorsOnCount(flipCount);
            lastKnownStart = start.SquarePoint;

            if (currentColor == Color.White)
            {
               GuessBestMove();
            }
        }

        private int CheckValidMove(ReversiButton start, ReversiButton[,] tempState, out List<Point> points, out State[,] guessState, bool isGuess = false)
        {
            points = new List<Point>();

            if (isGuess)
            {
                guessState = new State[12,12];
            }
            else
            {
                guessState = new State[1,1];
                tempState[start.SquarePoint.X, start.SquarePoint.Y].IsEmpty = false;
            }

            if (start.BackColor != this.emptyColor)
            {
                return 0;
            }


            int flipCount = 0;
            List<Point> tempPoints = new List<Point>();

            flipCount += GenericValidate(start, Direction.LEFT, tempState, points, guessState, isGuess);
            tempPoints.AddRange(points);

            flipCount += GenericValidate(start, Direction.TOP, tempState, points, guessState, isGuess);
            tempPoints.AddRange(points);

            flipCount += GenericValidate(start, Direction.TOPLEFT, tempState, points, guessState, isGuess);
            tempPoints.AddRange(points);

            flipCount += GenericValidate(start, Direction.TOPRIGHT, tempState, points, guessState, isGuess);
            tempPoints.AddRange(points);

            flipCount += GenericValidate(start, Direction.RIGHT, tempState, points, guessState, isGuess);
            tempPoints.AddRange(points);

            flipCount += GenericValidate(start, Direction.BOTTOMRIGHT, tempState, points, guessState, isGuess);
            tempPoints.AddRange(points);

            flipCount += GenericValidate(start, Direction.BOTTOM, tempState, points, guessState, isGuess);
            tempPoints.AddRange(points);

            flipCount += GenericValidate(start, Direction.BOTTOMLEFT, tempState, points, guessState, isGuess);
            tempPoints.AddRange(points);

            points = tempPoints;

            return flipCount;
        }

        private void FlipColorsOnCount(int flipCount)
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
                currentColor = Color.White;
                turnButton.Text = "White";

                if (flipCount > 0)
                {
                    this.whiteCount -= flipCount;
                    this.blackCount += flipCount;
                }
            }
            else
            {
                currentColor = Color.Black;
                turnButton.Text = "Black";

                if (flipCount > 0)
                {
                    this.whiteCount += flipCount;
                    this.blackCount -= flipCount;
                }
            }

            this.blackCountBox.Text = blackCount.ToString();
            this.whiteCountBox.Text = whiteCount.ToString();
            turnButton.BackColor = currentColor;
        }

        private int GenericValidate(ReversiButton start, Direction direction, ReversiButton[,] tempState, List<Point> points, State[,] guessState, bool isGuess)
        {
            int directionCounter = 0;
            bool isValid = false;

            while (true)
            {
                directionCounter++;
                Point modifiedPoint = ModifyPointDirection(direction, directionCounter, start.SquarePoint);

                if (this.utilties.isValid(modifiedPoint.X, modifiedPoint.Y))
                {
                    if (tempState[modifiedPoint.X, modifiedPoint.Y].BackColor != Color.Green && tempState[modifiedPoint.X, modifiedPoint.Y].BackColor != this.currentColor)
                    {
                        points.Add(new Point(modifiedPoint.X, modifiedPoint.Y));
                    }
                    else
                    {
                        if (points.Count > 0 && tempState[modifiedPoint.X, modifiedPoint.Y].BackColor == this.currentColor)
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
            
            if (!isGuess)
            {
                if (points.Count > 0 && isValid)
                {
                    start.BackColor = this.currentColor;
                    foreach (Point pt in points)
                    {
                        tempState[pt.X, pt.Y].BackColor = this.currentColor;
                        tempState[pt.X, pt.Y].IsEmpty = false;
                    }

                    return points.Count;
                }
            }
            else
            {
                if (points.Count > 0 && isValid)
                {
                    var p = start.SquarePoint;
                    guessState[p.X, p.Y] = new State
                    {
                        BackColor = this.currentColor
                    };

                    foreach (Point pt in points)
                    {
                        guessState[pt.X, pt.Y] = new State
                        {
                            BackColor = this.currentColor,
                            IsEmpty = false
                        };
                    }

                    return points.Count;
                }
            }

            if (!isValid)
            {
                points.Clear();
            }

            return 0;
        }

        private int GenericValidateWithState(Point start, Direction direction, State[,] tempState)
        {
            int directionCounter = 0;
            bool isValid = false;
            List<Point> points = new List<Point>();

            while (true)
            {
                directionCounter++;
                Point modifiedPoint = ModifyPointDirection(direction, directionCounter, start);

                if (this.utilties.isValid(modifiedPoint.X, modifiedPoint.Y))
                {
                    if (tempState[modifiedPoint.X, modifiedPoint.Y].BackColor != Color.Green && tempState[modifiedPoint.X, modifiedPoint.Y].BackColor != this.currentColor)
                    {
                        points.Add(new Point(modifiedPoint.X, modifiedPoint.Y));
                    }
                    else
                    {
                        if (points.Count > 0 && tempState[modifiedPoint.X, modifiedPoint.Y].BackColor == this.currentColor)
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
                tempState[start.X, start.Y].BackColor = this.currentColor;
                foreach (Point pt in points)
                {
                    tempState[pt.X, pt.Y].BackColor = this.currentColor;
                    tempState[pt.X, pt.Y].IsEmpty = false;
                }

                return points.Count;
            }

            return 0;
        }

        private static Point ModifyPointDirection(Direction direction, int directionCounter, Point modifiedPoint)
        {
            switch (direction)
            {
                case Direction.TOP:
                    {
                        modifiedPoint.Y = modifiedPoint.Y - directionCounter;
                        break;
                    }
                case Direction.LEFT:
                    {
                        modifiedPoint.X = modifiedPoint.X - directionCounter;
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

            return modifiedPoint;
        }

        private static Point ModifyPointDirection(Direction direction, Point modifiedPoint)
        {
            switch (direction)
            {
                case Direction.TOP:
                    {
                        modifiedPoint.Y = modifiedPoint.Y - 1;
                        break;
                    }
                case Direction.LEFT:
                    {
                        modifiedPoint.X = modifiedPoint.X - 1;
                        break;
                    }
                case Direction.RIGHT:
                    {
                        modifiedPoint.X = modifiedPoint.X + 1;
                        break;
                    }
                case Direction.BOTTOM:
                    {
                        modifiedPoint.Y = modifiedPoint.Y + 1;
                        break;
                    }
                case Direction.TOPLEFT:
                    {
                        modifiedPoint.Y = modifiedPoint.Y - 1;
                        modifiedPoint.X = modifiedPoint.X - 1;
                        break;
                    }
                case Direction.TOPRIGHT:
                    {
                        modifiedPoint.Y = modifiedPoint.Y - 1;
                        modifiedPoint.X = modifiedPoint.X + 1;
                        break;
                    }
                case Direction.BOTTOMLEFT:
                    {
                        modifiedPoint.Y = modifiedPoint.Y + 1;
                        modifiedPoint.X = modifiedPoint.X - 1;
                        break;
                    }
                case Direction.BOTTOMRIGHT:
                    {
                        modifiedPoint.Y = modifiedPoint.Y + 1;
                        modifiedPoint.X = modifiedPoint.X + 1;
                        break;
                    }
            }

            return modifiedPoint;
        }
    }


    internal class State
    {
        public int gridX;
        public int gridY;
        public Color Color;
        public Color BackColor;
        public bool IsEmpty = true;
    }

    internal class FlipCounts
    {
        public ReversiButton startButton;
        public List<Point>points;
        public State[,] states;
    }
}
