using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
namespace Game
{
    
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void startGame(User user)
        {
        
            var game = new GameWindow(user);
            game.Show();
            this.Close();
        }
        private void login(object sender, RoutedEventArgs e)
        {
            bool i = true;

            String nick = username.Text.ToString();
            
            String pwd = password.Text.ToString();

            var db = new MySqlConnection("Server=127.0.0.1;Database=info;uid=root;pwd=G458d942;");
            
            db.Open();

            var query = new MySqlCommand("select name,password from user",db);

            var data = query.ExecuteReader();

            while (data.Read() && i)
            {
                if(nick == data.GetValue(0).ToString()   &&   pwd == data.GetValue(1).ToString())
                {
                    i = false;  
                }
            }
            if (!i)
            {
                User user = new User();
                user.username = nick;
                startGame(user);
            }
            else
            {
                MessageBox.Show("Username Or Password Is Wrong");
            }
            db.Close();

        }  
        private void register(object sender, RoutedEventArgs e)
        {
            bool i = true;
            String nick = username.Text.ToString();
            String pwd = password.Text.ToString();

            var db = new MySqlConnection("Server=127.0.0.1;Database=info;uid=root;pwd=G458d942;");
            db.Open();

            var query = new MySqlCommand("select name from user", db);

            var data = query.ExecuteReader();

            while (data.Read()&& i)
            {
                if (nick == data.GetValue(0).ToString())
                {
                    i = false;
                }
            }
            if (!i)
            {
                MessageBox.Show("Please try another username !");
            }
            if (i)
            {
                data.Close();
                query = new MySqlCommand(String.Format("insert into user(name,password) values ('{0}','{1}')",nick,pwd), db);

                query.ExecuteNonQuery();
                MessageBox.Show("Registered succesfully !");
                var user = new User();
                user.username = nick;
                startGame(user);
            }
        }

        }
}
