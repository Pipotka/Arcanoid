using System;
using System.Data.Common;
using System.Drawing;
using System.Drawing.Text;

namespace Arkanoid
{
    public partial class Arkanoid : Form
    {
        private Bitmap backgroundBitmap = new Bitmap(Properties.Resources.Background);
        private Bitmap platformBitmap = new Bitmap(Properties.Resources.Platform);
        private Bitmap ballBitmap = new Bitmap(Properties.Resources.Ball);
        private Graphics canvas;
        private BufferedGraphicsContext currentBuffer;
        private BufferedGraphics buffer;
        private int widthOfGameRectangle = 70;
        private int heightOfGameRectangle = 20;
        private int widthOfPlatform = Properties.Resources.Platform.Width;
        private int heightOfPlatform = Properties.Resources.Platform.Height;
        private int columns;
        private int rows;
        private bool isStartGame = false;
        private GameRectangle[,] GameRectangles;
        private Platform platform;
        private Point platformPosition;
        private int widthOfBall = Properties.Resources.Ball.Width;
        private int heightOfBall = Properties.Resources.Ball.Height;
        private int heightOfScreen = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size.Height;
        private int widthOfScreen = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size.Width;
        private Ball ball;
        public Arkanoid()
        {
            InitializeComponent();
            canvas = CreateGraphics();
            currentBuffer = BufferedGraphicsManager.Current;
            buffer = currentBuffer.Allocate(this.CreateGraphics(), this.DisplayRectangle);
            var temporaryColumns = widthOfScreen / widthOfGameRectangle;
            var temporaryLines = (heightOfScreen - heightOfGameRectangle * 20) / heightOfGameRectangle;
            columns = temporaryColumns;
            rows = temporaryLines;
            GameRectangles = new GameRectangle[rows, columns];
            InitializationOfGameRectangles();
            var startPositionOfPlatform = new Point(heightOfScreen - (heightOfGameRectangle * 3), widthOfScreen / 2);
            platform = new Platform(startPositionOfPlatform, widthOfPlatform, heightOfPlatform);
            Point startPositionOfBall = new Point(startPositionOfPlatform.X + widthOfPlatform / 2 - widthOfBall / 2,
                startPositionOfPlatform.Y - heightOfBall);
            ball = new Ball(startPositionOfBall, heightOfBall, widthOfBall);
            platformBitmap.MakeTransparent(Color.White);
            ballBitmap.MakeTransparent(Color.White);
            platformPosition = new Point(startPositionOfPlatform.X, startPositionOfPlatform.Y);
            GameTimer.Start();
        }

        private void InitializationOfGameRectangles()
        {
            var offsetYCoordinateOfRectanglesLocation = heightOfScreen % heightOfGameRectangle;
            var offsetXCoordinateOfRectanglesLocation = widthOfScreen % widthOfGameRectangle;
            int xCoordinateOfUpperLeftCornerOfGameRectangle = offsetXCoordinateOfRectanglesLocation / 2;
            int yCoordinateOfUpperLeftCornerOfGameRectangle = offsetYCoordinateOfRectanglesLocation / 2;
            var randomColorOfGameRectangle = new Random();
            SolidBrush brush = new SolidBrush(Color.Black);
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    brush.Color = DetermineColor(randomColorOfGameRectangle.Next(0, 4));
                    GameRectangles[row, column] = new GameRectangle(
                        xCoordinateOfUpperLeftCornerOfGameRectangle,
                        yCoordinateOfUpperLeftCornerOfGameRectangle,
                        widthOfGameRectangle,
                        heightOfGameRectangle,
                        false,
                        brush
                        );
                    SetStartingAvailability(ref GameRectangles[row, column], column, row);
                    xCoordinateOfUpperLeftCornerOfGameRectangle += widthOfGameRectangle;
                }
                xCoordinateOfUpperLeftCornerOfGameRectangle = offsetXCoordinateOfRectanglesLocation / 2;
                yCoordinateOfUpperLeftCornerOfGameRectangle += heightOfGameRectangle;
            }
        }
        private Color DetermineColor(int numberOfColor)
        {
            var result = new Color();
            switch (numberOfColor)
            {
                case 0:
                    result = Color.Red;
                    break;
                case 1:
                    result = Color.Blue;
                    break;
                case 2:
                    result = Color.Green;
                    break;
                case 3:
                    result = Color.Yellow;
                    break;
                case 4:
                    result = Color.Orange;
                    break;
                default:
                    result = Color.Blue;
                    break;
            }
            return result;
        }
        private void SetStartingAvailability(ref GameRectangle gameRectangle, int column, int line)
        {
            if (column == 0)
            {
                gameRectangle.IsAvailable = true;
            }
            else if (column == columns - 1)
            {
                gameRectangle.IsAvailable = true;
            }
            else if (line == 0)
            {
                gameRectangle.IsAvailable = true;
            }
            else if (line == rows - 1)
            {
                gameRectangle.IsAvailable = true;
            }
        }

        private void DrawGameRectangles()
        {
            for (int line = 0; line < rows; line++)
            {
                for (int column = 0; column < columns; column++)
                {
                    if (GameRectangles[line, column].IsVisible)
                    {
                        buffer.Graphics.FillRectangle(GameRectangles[line, column].ColorOfGameRectangle, GameRectangles[line, column].Rect);
                        buffer.Graphics.DrawRectangle(Pens.Black, GameRectangles[line, column].Rect);
                    }
                }
            }
        }

        private void GameTimerTick(object sender, EventArgs e)
        {
            buffer.Graphics.DrawImage(backgroundBitmap, 0, 0);
            DrawGameRectangles();
            buffer.Graphics.DrawImage(platformBitmap, platform.Position);
            ChangingPositionOfBall(DetermineModeOfMotionOfBall());
            platform.Move(platformPosition.X);
            buffer.Graphics.DrawImage(ballBitmap, ball.Position);
            buffer.Render();
        }

        private void ArkanoidMouseMove(object sender, MouseEventArgs e)
        {
            platformPosition.X = e.Location.X;
        }

        private void ChangingPositionOfBall(ModsChangingPositionOfBall mod)
        {
            if (mod == ModsChangingPositionOfBall.NotStarted)
            {
                ball.Position.X = platform.Position.X + platform.Width / 2 - widthOfBall / 2;
                return;
            }
            if (mod == ModsChangingPositionOfBall.LeftÑollision)
            {
                ball.SetPositiveDirectionX();
            }
            else if (mod == ModsChangingPositionOfBall.BottomÑollision)
            {
                ball.SetNegativeDirectionY();
            }
            else if (mod == ModsChangingPositionOfBall.RightÑollision)
            {
                ball.SetNegativeDirectionX();
            }
            else if (mod == ModsChangingPositionOfBall.TopÑollision)
            {
                ball.SetPositiveDirectionY();
            }
            if ((mod == ModsChangingPositionOfBall.LeftPartOfPlatform) || (mod == ModsChangingPositionOfBall.RightPartOfPlatform))
            {
                ball.SetNegativeDirectionY();
                ball.XSpeedIncrease(0.5);
                ball.YSpeedIncrease(0.5);
            }
            ball.Move();
        }

        private ModsChangingPositionOfBall DetermineModeOfMotionOfBall()
        {
            var mod = new ModsChangingPositionOfBall();
            mod = DetermineIfBallHasCollidedWithPlatform();
            if (mod != ModsChangingPositionOfBall.None)
            {
                return mod;
            }
            if (ball.LeftSide.X < 0)
            {
                return ModsChangingPositionOfBall.LeftÑollision;
            }
            else if (ball.RightSide.X > widthOfScreen)
            {
                return ModsChangingPositionOfBall.RightÑollision;
            }
            else if (ball.TopSide.Y < 0)
            {
                return ModsChangingPositionOfBall.TopÑollision;
            }
            else if (ball.TopSide.Y > heightOfScreen)
            {
                GameTimer.Stop();
                MessageBox.Show("Âû ïðîèãðàëè",
                    "Arcanoid",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                Application.Exit();
            }
            if (!isStartGame)
            {
                return ModsChangingPositionOfBall.NotStarted;
            }
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    if ((GameRectangles[row, column].IsVisible) && (GameRectangles[row, column].IsAvailable))
                    {
                        if (GameRectangles[row, column].isContains(ball.LeftSide))
                        {
                            GameRectangles[row, column].IsVisible = false;
                            SetAvailabilityOfSurroundingRectangles(row, column);
                            return ModsChangingPositionOfBall.LeftÑollision;
                        }
                        else if (GameRectangles[row, column].isContains(ball.BottomSide))
                        {
                            GameRectangles[row, column].IsVisible = false;
                            SetAvailabilityOfSurroundingRectangles(row, column);
                            return ModsChangingPositionOfBall.BottomÑollision;
                        }
                        else if (GameRectangles[row, column].isContains(ball.RightSide))
                        {
                            GameRectangles[row, column].IsVisible = false;
                            SetAvailabilityOfSurroundingRectangles(row, column);
                            return ModsChangingPositionOfBall.RightÑollision;
                        }
                        else if (GameRectangles[row, column].isContains(ball.TopSide))
                        {
                            GameRectangles[row, column].IsVisible = false;
                            SetAvailabilityOfSurroundingRectangles(row, column);
                            return ModsChangingPositionOfBall.TopÑollision;
                        }
                    }
                }
            }
            return ModsChangingPositionOfBall.SimpleMove;
        }

        private void SetAvailabilityOfSurroundingRectangles(int row, int column)
        {
            if (column - 1 >= 0)
            {
                GameRectangles[row, column - 1].IsAvailable = true;
            }
            if (column + 1 < columns)
            {
                GameRectangles[row, column + 1].IsAvailable = true;
            }
            if (row - 1 >= 0)
            {
                GameRectangles[row - 1, column].IsAvailable = true;
            }
            if (row + 1 < rows)
            {
                GameRectangles[row + 1, column].IsAvailable = true;
            }
        }

        private ModsChangingPositionOfBall DetermineIfBallHasCollidedWithPlatform()
        {
            if (platform.IdentifyPartOfPlatform(ball.LeftSide) != ModsChangingPositionOfBall.None)
            {
                ball.XSpeedIncrease(10);
                return ModsChangingPositionOfBall.LeftÑollision;
            }
            if (platform.IdentifyPartOfPlatform(ball.RightSide) != ModsChangingPositionOfBall.None)
            {
                ball.XSpeedIncrease(10);
                return ModsChangingPositionOfBall.RightÑollision;
            }
            if (platform.IdentifyPartOfPlatform(ball.BottomSide) != ModsChangingPositionOfBall.None)
            {
                return ModsChangingPositionOfBall.LeftPartOfPlatform;
            }
            if (platform.IdentifyPartOfPlatform(ball.TopSide) != ModsChangingPositionOfBall.None)
            {
                return ModsChangingPositionOfBall.TopÑollision;
            }
            return ModsChangingPositionOfBall.None;
        }
        private void ArkanoidDoubleClick(object sender, EventArgs e)
        {
            if (!isStartGame)
            {
                isStartGame = true;
                ball.XSpeedIncrease(0.5);
                ball.YSpeedIncrease(5.0);
                ball.SetStartDirection();
            }
        }
    }
}

