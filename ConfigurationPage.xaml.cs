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
using System.Windows.Shapes;
using System.IO;
using System.Data.SqlClient;

namespace Password_directory
{
    /// <summary>
    /// Interaction logic for ConfigurationPage.xaml
    /// </summary>
    public partial class ConfigurationPage : Window
    {
        bool flag = false;
        public ConfigurationPage()
        {
            InitializeComponent();
            Details.IsEnabled = false;
        }

        private void refresh()
        {
            try
            {
                StreamReader file = new StreamReader("DBConnectionString.bin");
                string data = file.ReadLine();
                data = formatDBCS(data);
                DataStringCurrent.Text = data;
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        private void CloseApp(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void checkPassword(object sender, MouseButtonEventArgs e)
        {
            if (passwordBox.Password.ToString() == "adminhere")
            {
                flag = true;
                Details.IsEnabled = true;
                refresh();
            }
            else
            {
                flag = false;
                MessageBox.Show("Check the password (You entered: " + passwordBox.Password.ToString() + ")");
            }
        }
        private string formatDBCS(string oldDBCS)
        {
            string newDBCS = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename="+oldDBCS+";Integrated Security=True";
            return newDBCS;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(DataStringNew.Text != null || DataStringNew.Text != " ")
            {
                string newDBCS = formatDBCS(DataStringNew.Text);
                SqlConnection con = new SqlConnection(newDBCS);
                try
                {
                    con.Open();
                    StreamWriter file = new StreamWriter("DBConnectionString.bin");
                    file.WriteLine(newDBCS);
                    refresh();
                    con.Close();
                }
                catch(Exception)
                {
                    MessageBox.Show("Check location: "+newDBCS);
                    refresh();
                }
            }
        }

        private void goBack(object sender, MouseButtonEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Visibility = Visibility.Visible;
            this.Visibility = Visibility.Hidden;
        }
    }
}
