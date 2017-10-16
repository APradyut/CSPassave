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

namespace Password_directory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        UserDBLayer userlayer;
        public MainWindow()
        {
            InitializeComponent();
            userlayer = new UserDBLayer();
            if(userlayer.state == false)
            {
                MessageBox.Show("There is a problem with DataBase");
            }
        }

        private void CloseApp(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (userlayer.search(username.Text, password.Password) == 1)
            {
                MainPage mp = new MainPage(false);
                mp.Visibility = Visibility.Visible;
                this.Visibility = Visibility.Hidden;
            }
            else if(userlayer.search(username.Text, password.Password) == 2)
            {
                MainPage mp = new MainPage(true);
                mp.Visibility = Visibility.Visible;
                this.Visibility = Visibility.Hidden;
            }
            else
            {
                ErrorMsg.Visibility = Visibility.Visible;
                ErrorMsg.Content = "Please check the Username or Password";
            }
        }

        private void GotoConfig(object sender, MouseButtonEventArgs e)
        {
            ConfigurationPage cp = new ConfigurationPage();
            cp.Visibility = Visibility.Visible;
        }
    }
}
