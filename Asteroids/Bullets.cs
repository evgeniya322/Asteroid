using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Asteroids
{
    class Bullets : BaseObject
    {
        
        public Bullets(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
        }
        public override void Draw()
        {
            Game.buffer.Graphics.DrawEllipse(Pens.White, pos.X, pos.Y, size.Width, size.Height);
        }

        public override void Update()
        {
            Random rand = new Random();
            pos.X += dir.X;
            //if (pos.X > Game.Width)
            //{
            //    pos.X = 0;
            //    pos.Y = (int)rand.Next(Game.Height);
            //}
        }
    }
}
