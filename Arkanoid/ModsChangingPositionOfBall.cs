using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arkanoid
{
    enum ModsChangingPositionOfBall
    {
        None,
        NotStarted,
        SimpleMove,
        LeftСollision,
        RightСollision,
        TopСollision,
        BottomСollision,
        LeftPartOfPlatform, 
        RightPartOfPlatform
    }
}
