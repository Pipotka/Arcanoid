using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arkanoid
{
    struct GameRectangle
    {
        public Rectangle Rect;
        public SolidBrush ColorOfGameRectangle { get; }
        public bool IsAvailable { get; set; }
        public bool IsVisible { get; set; }

        public GameRectangle(int x, int y, int widthOfGameRectangle, int heightOfGameRectangle, bool isAvailable, SolidBrush colorOfGameRectangle)
        {
            ColorOfGameRectangle = new SolidBrush(colorOfGameRectangle.Color);
            Rect = new Rectangle(x, y, widthOfGameRectangle, heightOfGameRectangle);
            IsVisible = true;
            IsAvailable = isAvailable;
        }
        public bool isContains(Point point)
        {
            return Rect.Contains(point);
        }
    }
}
