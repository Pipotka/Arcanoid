﻿namespace Arkanoid
{
    struct Ball
    {
        public Point Position;
        public Point LeftSide;
        public Point BottomSide;
        public Point RightSide;
        public Point TopSide;
        private double xSpeed;
        private double ySpeed;
        private Direction direction;
        private int height;
        private int width;

        public Ball(Point position, int height, int width)
        {
            Position = new Point(position.X, position.Y);
            this.height = height;
            this.width = width;
            ySpeed = 0;
            xSpeed = 0;
            direction = new Direction(0, 0);
            LeftSide = new Point(Position.X, Position.Y + height / 2);
            RightSide = new Point(Position.X + width, Position.Y + height / 2);
            TopSide = new Point(Position.X + width / 2, Position.Y);
            BottomSide = new Point(Position.X + width / 2, Position.Y + height);
        }

        public void Move()
        {
            Position.X += direction.X * (int)xSpeed;
            Position.Y += direction.Y * (int)ySpeed;
            LeftSide.X = Position.X + 1;
            LeftSide.Y = Position.Y + height / 2;
            RightSide.X = Position.X + width - 1;
            RightSide.Y = Position.Y + height / 2;
            TopSide.X = Position.X + width / 2 + 1;
            TopSide.Y = Position.Y;
            BottomSide.X = Position.X + width / 2;
            BottomSide.Y = Position.Y + height - 1;
        }

        public void SetStartDirection()
        {
            var randomXdirection = new Random();
            direction.X = randomXdirection.Next(0, 2) >= 1 ? 1 : -1;
            direction.Y = -1;
        }

        public void SetOppositeDirectionX()
        {
            direction.X *= -1;
        }

        public void SetOppositeDirectionY()
        {
            direction.Y *= -1;
        }

        public void XSpeedIncrease(double changeSpeedByX)
        {
            if (xSpeed + changeSpeedByX >= 0)
            {
                xSpeed += changeSpeedByX;

            }
            else
            {
                xSpeed = 0;
            }
        }

        public void YSpeedIncrease(double changeSpeedByY)
        {
            if (ySpeed + changeSpeedByY >= 0)
            {
                ySpeed += changeSpeedByY;
            }
            else
            {
                ySpeed = 0;
            }
        }
    }
}