using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Asteroids
{
    public delegate void Message();
    public delegate void MJournal(string message);
    abstract class BaseObject:ICollision
    {
        
        public Point pos;
        protected Point dir;
        protected Size size;
        protected Image image;
        public static event MJournal MJournal;


        public BaseObject(Point pos, Point dir, Size size)
        {
            this.pos = pos;
            this.dir = dir;
            this.size = size;
        }

        public BaseObject(Point pos, Point dir,Size size,Image image)
        {
            this.pos = pos;
            this.dir = dir;
            this.size = size;
            this.image = image;
        }

        public Rectangle Rect => new Rectangle(pos, size);

        public bool Collision(ICollision obj)
        {
            string name1,name2;
            if (obj.Rect.IntersectsWith(Rect))
            {
                if (this is Bullets) name1 = "Пуля";
                else if (this is Ship) name1 = "Корабль";
                else name1 = "Неизвестный объект";

                if (obj is Asteroid) name2 = "Астероидом";
                else if (obj is Life) name2 = "Аптечкой";
                else name2 = "Неизвестный объект";

                MJournal.Invoke($"{name1} столкнулся с {name2}");
            };
            return obj.Rect.IntersectsWith(Rect);
        }

        public virtual void Draw()
        {
            Rectangle rec = new Rectangle(pos, size);
            Game.buffer.Graphics.DrawImage(image, rec);
        }


        public abstract void Update();


    }
}
