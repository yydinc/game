using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace Game
{
    public class Character
    {
        public int sizeX;
        public int sizeY; 
        public int speed;
        public int positionX;
        public int positionY;
        public String color;
        public Rectangle display ;
        public Character(int x, int y , int s,int px,int py, Brush c) {
            this.sizeX = x;
            this.sizeY = y;
            this.speed = s;
            this.positionX = px;
            this.positionY = py;
            this.display = new Rectangle()
            {
                Width = this.sizeX,
                Height = this.sizeY,
                Fill = c
            };
            

        

        }
    
    }


    public class User
    {
        public String username;
        public int point;
        public User(){
            this.point = 0;
        }
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class GameWindow: Window { 
        static int rand = new Random().Next(700);
        static int rand2 = new Random().Next(700);
        
        public static Character hero = new Character(30, 30, 10,rand,rand2,Brushes.Black);

        static Character[] characters = new Character[] {hero };
        //  public Character water = new Character(10, 10, 0,500,500,Brushes.Blue);//  public Character water = new Character(10, 10, 0,500,500,Brushes.Blue);
        public User user = new User();
        public static Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        static IPAddress ip = IPAddress.Parse("192.168.1.109");
        EndPoint ep = new IPEndPoint(ip, 5050);
        
        
            
        
/*
       
        private void ifWaterDrinken()
        {

           
            if ((water.positionX < hero.positionX + hero.sizeX && water.positionX > hero.positionX && water.positionY>hero.positionY&& water.positionY<hero.positionY+hero.sizeY) ||
                (water.positionX < hero.positionX + hero.sizeX && water.positionX > hero.positionX && water.positionY+water.sizeY > hero.positionY && water.positionY+water.sizeY < hero.positionY + hero.sizeY) ||
                (water.positionX + water.sizeX < hero.positionX + hero.sizeX && water.positionX + water.sizeX > hero.positionX && water.positionY > hero.positionY && water.positionY < hero.positionY + hero.sizeY) ||
                (water.positionX + water.sizeX < hero.positionX + hero.sizeX && water.positionX + water.sizeX > hero.positionX && water.positionY+ water.sizeY > hero.positionY && water.positionY+water.sizeY < hero.positionY + hero.sizeY))
            {
                Random rnd = new Random();
                
                GameArea.Children.Remove(water.display);

                water.positionX = rnd.Next(1920 - water.sizeX);
                water.positionY = rnd.Next(1080 - water.sizeY);

                Canvas.SetLeft(water.display,water.positionX);
                Canvas.SetTop(water.display, water.positionY);
                GameArea.Children.Add(water.display);

            }
        }
        private void drawWater()
        {
            Canvas.SetLeft(water.display, water.positionX);
            Canvas.SetTop(water.display, water.positionY);
            GameArea.Children.Add(water.display);
        }
            
*/
       
       
        private void setUser(User user)
        {
            this.Title = user.username+"  ------- "+ user.point.ToString();
        }


        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            switch(e.Key)
            {
                case Key.Up:
                    hero.positionY -= hero.speed; ;
                    break;
                case Key.Down:
                    hero.positionY += hero.speed; ;
                    break;
                case Key.Left:
                    hero.positionX -= hero.speed; ;
                    break;
                case Key.Right:
                    hero.positionX += hero.speed; ;
                    break;
            }
            user.point += 1;
            setUser(user);
            GameArea.Children.Remove(hero.display);
            moveCharacter();
       //     ifWaterDrinken();
            
        }

        public void moveCharacter()
        {
            
            
            Canvas.SetLeft(hero.display, hero.positionX);
            Canvas.SetTop(hero.display, hero.positionY);
            GameArea.Children.Add(hero.display);
            String data = hero.positionX.ToString() + "," + hero.positionY + "," +ip;
            byte[] byData = System.Text.Encoding.ASCII.GetBytes(data);
            client.Send(byData);
        }

        public static string GetIP()
        {
            string strHostName = "";
            strHostName = System.Net.Dns.GetHostName();

            IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);

            IPAddress[] addr = ipEntry.AddressList;

            return addr[addr.Length - 1].ToString();

        }

        public void drawCharacter(Character character)
        {
            
            
            Canvas.SetLeft(character.display, character.positionX);
            Canvas.SetTop(character.display, character.positionY);
            GameArea.Children.Add(character.display);


        }


        private void contentRendered(object sender, EventArgs e)
        {
            drawCharacter(hero);
          //  drawWater();
        }
        private void listen()
        {
            while (true)
            {
                byte[] buffer = new byte[1024];
                int iRx = client.Receive(buffer);
                char[] chars = new char[iRx];

                System.Text.Decoder d = System.Text.Encoding.UTF8.GetDecoder();
                int charLen = d.GetChars(buffer, 0, iRx, chars, 0);
                System.String recv = new System.String(chars);
                String[] data = recv.Split(";");
                foreach(var dat in data)
                {
                    var x = dat.Split(",");
                    var enemy = new Character(Convert.ToInt32(x[0]), Convert.ToInt32(x[1]), 10, 0, 0, Brushes.Black);
                    drawCharacter(enemy);
    }

            }
        }
        public GameWindow(User suser)
        {
            user.username = suser.username;
            client.Connect(ep);
            string ip = GetIP();
            InitializeComponent();
            setUser(user);
            String data = hero.positionX.ToString()+","+hero.positionY + "," +ip;
            byte[] byData = System.Text.Encoding.ASCII.GetBytes(data);
            client.Send(byData);
        }


        }
    }
