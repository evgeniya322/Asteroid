using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Asteroids
{
    class Life:BaseObject
    {
        
        public Life(Point pos, Point dir, Size size, Image image) : base(pos, dir, size, image) //base(pos,dir,size)
        {

        }



        public override void Update()
        {
            pos.X += dir.X;
            pos.Y += dir.Y;
            if (pos.X + size.Width < 0) pos.X = Game.Width;
        }
    }
}
