using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Asteroids
{
    class Star:BaseObject
    {

        public Star(Point pos, Point dir, Size size, Image image):base(pos,dir,size,image)
        {
        }


        public override void Update()
        {
            pos.X = pos.X + dir.X;
            pos.Y += (int)(dir.Y * Math.Sin(pos.X));
            if (pos.X + size.Width < 0) pos.X = Game.Width;
            if (pos.Y < 0) dir.Y = -dir.Y;
            if (pos.Y > Game.Height) dir.Y = -dir.Y;
        }
    }
}
