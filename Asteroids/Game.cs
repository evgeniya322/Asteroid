using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Asteroids
{
    static class Game
    {
        private static Timer timer = new Timer();
        public static Random Rnd = new Random();
        static BufferedGraphicsContext context;
        static public BufferedGraphics buffer;
        // Свойства
        // Ширина и высота игрового поля
        static public int Width { get; set; }
        static public int Height { get; set; }
        static List<BaseObject> objs;
        static List<Asteroid> asteroid;
        static List<Life> lives;
        static public int score = 0;
        //static int count = 0;
        static List<Bullets> bullets=new List<Bullets>();
        static int CountAster = 30;
        static int CountLives = 6;
        static Image image = Image.FromFile("Pictures\\player.png");
        private static Ship ship = new Ship(new Point(10, 400), new Point(15, 15), new Size(50, 50),image);

        static public void Init(Form form)
        {
            // Графическое устройство для вывода графики            
            Graphics g;
            // предоставляет доступ к главному буферу графического контекста для текущего приложения
            context = BufferedGraphicsManager.Current;
            g = form.CreateGraphics();// Создаём объект - поверхность рисования и связываем его с формой
                                      // Запоминаем размеры формы
            Width = form.Width;
            Height = form.Height;
            
            // Связываем буфер в памяти с графическим объектом.
            // для того, чтобы рисовать в буфере

                buffer = context.Allocate(g, new Rectangle(0, 0, Width, Height));


            
            //Draw();
            Load();
            timer.Interval = 100;
            timer.Tick += Timer_Tick;
            timer.Start();
            form.KeyDown += Form_KeyDown;
            Ship.MessageDie += Finish;
            Ship.MJournal += ShowMassage;
            BaseObject.MJournal += ShowMassage;
        }

        private static void ShowMassage(string message)
        {
            Console.WriteLine(message+$" Energy: {ship.Energy} Score: {score} ");
        }

        private static void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey) bullets.Add(new Bullets(new Point(ship.Rect.X + 25, ship.Rect.Y + 25), new Point(10, 0), new Size(4, 1)));
            if (e.KeyCode == Keys.Up) ship.Up();
            if (e.KeyCode == Keys.Down) ship.Down();
        }
        public static void Finish()
        {
            timer.Stop();
            buffer.Graphics.DrawString("The End", new Font(FontFamily.GenericSansSerif, 60, FontStyle.Underline), Brushes.White, 200, 100);
            buffer.Render();
        }

        private static void Timer_Tick(object sender, EventArgs e)
        {
            Update();
            Draw();

        }

        static public void Load()
        {
            Random rand = new Random();
            objs = new List<BaseObject>();
            asteroid = new List<Asteroid>();
            lives = new List<Life>();
            Image image = Image.FromFile("Pictures\\Stars.png");

            for (int i = 0; i < 30; i++)
            {
                int size = (int)rand.Next(5, 20);
                objs.Add(new Star(new Point((int)rand.Next(Game.Width), (int)rand.Next(Game.Height)), new Point(-(int)rand.Next(15, 30), -(int)rand.Next(5, 15)), new Size(size,size), image));
            }

            image = Image.FromFile("Pictures\\Aster.png");
            for (int i = 0; i < CountAster ; i++)
            {
                int size = (int)rand.Next(30, 50);
                asteroid.Add(new Asteroid(new Point((int)rand.Next(Game.Width / 3, Game.Width), (int)rand.Next(Game.Height-25)), new Point(-5, 0), new Size(size, size), image));
            }

            image = Image.FromFile("Pictures\\lives.png");
            for (int i = 0; i < CountLives; i++)
            {
                int size = (int)rand.Next(30, 50);
                lives.Add( new Life(new Point((int)rand.Next(Game.Width / 3, Game.Width), (int)rand.Next(Game.Height-25)), new Point(-5, 0), new Size(size, size), image));
            }

            image = Image.FromFile("Pictures\\Planet.png");
            objs.Add( new Asteroid(new Point((int)rand.Next(Game.Width), (int)rand.Next(Game.Height)/2), new Point(-2, 0), new Size(100, 100), image));

            //bullet = new Bullets(new Point(0, (int)rand.Next(Game.Height)), new Point(10, 0), new Size(10, 10));
        }

        static public void Draw()
        {
            buffer.Graphics.Clear(Color.Black);
            foreach (BaseObject obj in objs)
                obj.Draw();
            foreach (Asteroid a in asteroid)
            {
                a.Draw();
            }
            foreach (Life l in lives)
            {
                l.Draw();
            }
            foreach (var b in bullets)
            {
                b.Draw();
            }
            
            ship?.Draw();
            if (ship != null)
            {
                buffer.Graphics.DrawString("Energy:" + ship.Energy, SystemFonts.DefaultFont, Brushes.White, 0, 0);
                buffer.Graphics.DrawString("Score:" + score, SystemFonts.DefaultFont, Brushes.White, 0, 30);
            }
                
            buffer.Render();
        }

        static public void Update()
        {
            foreach (BaseObject obj in objs) obj.Update();

            foreach (var b in bullets)
            {
                b.Update();
            }

            for (int i = 0; i < asteroid.Count; i++)
            {
                asteroid[i].Update();
                for (int j = 0; j < bullets.Count; j++)
                    if (asteroid.Count!=0 && bullets[j].Collision(asteroid[i]))
                    {
                        System.Media.SystemSounds.Hand.Play();
                        score++;
                        asteroid.Remove(asteroid[i]);
                        bullets.Remove(bullets[j]);
                        if (i != 0) i--;
                        if (j != 0) j--;
                        
                    }

                if (score<0) ship?.Die();
                if (asteroid.Count!=0 && ship.Collision(asteroid[i]))
                {
                    var rnd = new Random();
                    ship?.EnergyLow(rnd.Next(1, 10));
                    asteroid.Remove(asteroid[i]);
                    if (i != 0) i--;
                    System.Media.SystemSounds.Asterisk.Play();
                    if (ship.Energy <= 0) ship?.Die();
                    
                }
                if (asteroid.Count == 0)
                {
                    Random rand = new Random();
                    CountAster++;
                    image = Image.FromFile("Pictures\\Aster.png");
                    for (int k = 0; k < CountAster; k++)
                    {
                        int size = (int)rand.Next(30, 50);
                        asteroid.Add(new Asteroid(new Point((int)rand.Next(Game.Width, 2*Game.Width), (int)rand.Next(Game.Height-25)), new Point(-5, 0), new Size(size, size), image));
                        asteroid[k].Draw();
                    }
                }
            }

            
            for (int i = 0; i <lives.Count; i++)
            {
                lives[i].Update();
                for (int j = 0; j < bullets.Count; j++)
                    if (lives.Count!=0 && bullets[j].Collision(lives[i]))
                    {
                        System.Media.SystemSounds.Hand.Play();
                        score--;
                        lives.Remove(lives[i]);
                        bullets.Remove(bullets[j]);
                        if (i != 0) i--;
                        if (j != 0) j--;
                        
                    }
               
                
                if (score < 0) ship?.Die();
                if (lives.Count!=0 && ship.Collision(lives[i]))
                {
                    var rnd = new Random();
                    ship?.EnergyHigh(rnd.Next(1, 10));
                    lives.Remove(lives[i]);
                    if (i != 0) i--;
                    System.Media.SystemSounds.Asterisk.Play();
                    
                }
                if (lives.Count == 0)
                {
                    Random rand = new Random();
                    if(CountLives!=1) CountLives--;
                    image = Image.FromFile("Pictures\\lives.png");
                    for (int k = 0; k < CountLives; k++)
                    {
                        int size = (int)rand.Next(30, 50);
                        lives.Add(new Life(new Point((int)rand.Next(Game.Width, 2*Game.Width), (int)rand.Next(Game.Height-25)), new Point(-5, 0), new Size(size, size), image));
                        lives[k].Draw();
                    }
                    
                }

            }

        }
    }
}
