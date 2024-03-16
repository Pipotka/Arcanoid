using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Arkanoid
{
    struct Platform
    {
        public Point Position;
        public readonly int Width;
        public readonly int Height;

        public Platform(Point positionOfPlatform, int widthOfPlatform, int heightOfPlatform)
        {
            Position = positionOfPlatform;
            Width = widthOfPlatform;
            Height = heightOfPlatform;
            var widthOfPlatformPart = widthOfPlatform / 4;
        }

        public bool IsConsist(Point point)
        {
            if (((point.X >= Position.X) && (point.X <= Position.X + Width)) && ((point.Y >= Position.Y) && (point.Y <= Position.Y + Height)))
            {
                return true;
            }
            return false;
        }

        public ModsChangingPositionOfBall IdentifyPartOfPlatform(Point point)
        {
            if (((point.X >= Position.X) && (point.X <= Position.X + Width / 2)) && ((point.Y >= Position.Y) && (point.Y <= Position.Y + Height)))
            {
                return ModsChangingPositionOfBall.LeftPartOfPlatform;
            }
            else if (((point.X >= Position.X + Width / 2) && (point.X <= Position.X + Width)) && ((point.Y >= Position.Y) && (point.Y <= Position.Y + Height)))
            {
                return ModsChangingPositionOfBall.RightPartOfPlatform;
            }
            else
            {
                return ModsChangingPositionOfBall.None;
            }
        }

        public void Move(Point newPosition)
        {
            Position.X = newPosition.X - Width / 2;
        }
    }
}
