using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Data;

namespace Password_directory
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Window
    {
        private DataTable incoming;
        private bool isAdmin;
        private DirectoryDBLayer dirdblayer;
        private UserDBLayer userdblayer;
        private AdminDBLayer admindblayer;
        private int i=0;
        public MainPage()
        {
            InitializeComponent();
        }
        public MainPage(bool isAdmin)
        {
            InitializeComponent();
            this.isAdmin = isAdmin;
            dirdblayer = new DirectoryDBLayer();
            userdblayer = new UserDBLayer();
            admindblayer = new AdminDBLayer();
            if (dirdblayer.state == false)
            {
                MessageBox.Show("There is a problem with DataBase");
            }
            user_tabs.Visibility = Visibility.Visible;
            admin_tabs.Visibility = Visibility.Hidden;
            AdminIcon.Visibility = Visibility.Visible;
            UserIcon.Visibility = Visibility.Hidden;
            NextBtn.Visibility = Visibility.Hidden;
            PrevBtn.Visibility = Visibility.Hidden;
        }
        private void CloseApp(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Search(object sender, MouseButtonEventArgs e)
        {
            incoming = dirdblayer.searchAll(Location.Text);
            if (incoming.Rows.Count > 0)
            {
                user_txt.Text = incoming.Rows[0][0].ToString();
                pass_txt.Text =  incoming.Rows[0][1].ToString();
                if (incoming.Rows.Count > 1)
                {
                    NextBtn.Visibility = Visibility.Visible;
                }
                else
                {
                    NextBtn.Visibility = Visibility.Hidden;
                    PrevBtn.Visibility = Visibility.Hidden;
                }
            }
            else
            {
                MessageBox.Show("Check location");
                NextBtn.Visibility = Visibility.Hidden;
                PrevBtn.Visibility = Visibility.Hidden;
            }
        }

        private void Add(object sender, MouseButtonEventArgs e)
        {
            if(Location_add.Text==" " || Username_add.Text==" "|| password_add.Text==" "|| Location_add.Text == null || Username_add.Text == null || password_add.Text == null)
            {
                MessageBox.Show("Check Fields");
                return;
            }
            if (dirdblayer.add(Location_add.Text, Username_add.Text, password_add.Text)==true)
            {
                MessageBox.Show("Successful");
            }
            else
            {
                MessageBox.Show("Failed");
            }
        }

        private void OpenSearch(object sender, MouseButtonEventArgs e)
        {
        }

        private void Signout(object sender, MouseButtonEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Visibility = Visibility.Visible;
            this.Visibility = Visibility.Hidden;
        }

        private void OpenAdmin(object sender, MouseButtonEventArgs e)
        {
            if(this.isAdmin == true)
            {
                adminSettings();
            }
            else
            {
                MessageBox.Show("Sorry you are not an Administrator!");
            }
        }
        private void adminSettings()
        {

                user_tabs.Visibility = Visibility.Hidden;
                admin_tabs.Visibility = Visibility.Visible;
                AdminIcon.Visibility = Visibility.Hidden;
                UserIcon.Visibility = Visibility.Visible;
                dg_Directory.ItemsSource = dirdblayer.get_all().DefaultView;
                dg_Users.ItemsSource = userdblayer.get_all().DefaultView;
        }

        private void OpenUser(object sender, MouseButtonEventArgs e)
        {
            user_tabs.Visibility = Visibility.Visible;
            admin_tabs.Visibility = Visibility.Hidden;
            AdminIcon.Visibility = Visibility.Visible;
            UserIcon.Visibility = Visibility.Hidden;
        }
        private string[] items = { "id", "UserName", "Password", "isAdmin" };
        private string selectedTable = "";
        private bool flag = false;
        private void STgoNext(object sender, SelectionChangedEventArgs e)
        {
            refreshFields();
        }
        private void refreshFields()
        {
            
            if (table.SelectedIndex == 0|| table_Copy.SelectedIndex == 0)
            {
                Known_attribute.ItemsSource = null;
                Change_attribute.ItemsSource = null;
                Known_attribute_Copy.ItemsSource = null;
                items[3] = "isAdmin";
                Known_attribute.ItemsSource = items;
                Change_attribute.ItemsSource = items;
                Known_attribute_Copy.ItemsSource = items;
                selectedTable = "Users";
                flag = true;
            }
            else if (table.SelectedIndex == 1 || table_Copy.SelectedIndex == 1)
            {
                Known_attribute.ItemsSource = null;
                Change_attribute.ItemsSource = null;
                Known_attribute_Copy.ItemsSource = null;
                items[3] = "Location";
                Known_attribute.ItemsSource = items;
                Change_attribute.ItemsSource = items;
                Known_attribute_Copy.ItemsSource = items;
                selectedTable = "Directory";
                flag = true;
            }

            
        }
        private void modify(object sender, MouseButtonEventArgs e)
        {
            if (flag == true)
            {
                try
                {
                    string command = "update " + selectedTable + " set " + items[Change_attribute.SelectedIndex] + " = '" + Change_value.Text + "' where " + items[Known_attribute.SelectedIndex] + " = '" + Known_value.Text+"' ";
                    int affectedRows = admindblayer.executeCommand(command);
                    if (affectedRows == -1)
                    {
                        MessageBox.Show("Modification Failed!");
                        return;
                    }
                    else
                    {
                        MessageBox.Show("No. of rows affected: " + affectedRows);
                    }
                }
                catch (Exception)
                { /*ignoring errors */}
            }
        }

        private void refresh(object sender, MouseButtonEventArgs e)
        {
            dg_Directory.ItemsSource = dirdblayer.get_all().DefaultView;
            dg_Users.ItemsSource = userdblayer.get_all().DefaultView;
        }

        private void STgoNext(object sender, RoutedEventArgs e)
        {
            refreshFields();
        }

        private void STgoNext(object sender, MouseEventArgs e)
        {
            refreshFields();
        }

        //private void reset(object sender, MouseButtonEventArgs e)
        //{
        //    Known_attribute.SelectedIndex = -1;
        //    table.SelectedIndex = -1;
        //    Known_value.Text = "";
        //    Change_attribute.SelectedIndex = -1;
        //    Change_value.Text = "";
        //}

        private void refreshAll(object sender, MouseButtonEventArgs e)
        {
            refreshFields();
        }

        private void next(object sender, MouseButtonEventArgs e)
        {
            //for(int i=0;i<incoming.Rows.Count;i++)
            //{
            //    for(int j =0; j<2;j++)
            //    {
            //        Console.WriteLine(i+", "+j+":"+incoming.Rows[i][j].ToString());
            //    }
            //}
            i++;
            if (i > -1 && i < incoming.Rows.Count)
            {
                PrevBtn.Visibility = Visibility.Visible;
                user_txt.Text = incoming.Rows[i][0].ToString();
                pass_txt.Text = incoming.Rows[i][1].ToString();
            }
        }

        private void prev(object sender, MouseButtonEventArgs e)
        {
            i--;
            if (i > -1 && i< incoming.Rows.Count)
            {
                user_txt.Text = incoming.Rows[i][0].ToString();
                pass_txt.Text = incoming.Rows[i][1].ToString();
            }
        }

        private void deleteRows(object sender, MouseButtonEventArgs e)
        {
            try
            {
                string command = "Delete from "+ selectedTable + " where "+ items[Known_attribute_Copy.SelectedIndex] + " = '" + Known_value_Copy.Text+"' ";
                int affectedRows = admindblayer.executeCommand(command);
                if (affectedRows == -1)
                {
                    MessageBox.Show("Deleting Failed!");
                    return;
                }
                else
                {
                    MessageBox.Show("No. of rows deleted: " + affectedRows);
                }
            }
            catch(Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        private void Add_Users(object sender, MouseButtonEventArgs e)
        {
            if (userdblayer.add(UserName.Text, Password.Text,isAdminCB.IsChecked == true) == true)
            {
                MessageBox.Show("Successful!");
            }
            else
            {
                MessageBox.Show("Failed");
            }
        }
    }
}
