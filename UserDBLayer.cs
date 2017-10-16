using System;
using System.Data.SqlClient;
using System.Data;

namespace Password_directory
{
    class UserDBLayer
    {
        private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Adarsh\source\repos\Password_directory\Password_directory\Passave.mdf;Integrated Security=True";
        SqlConnection con;
        public bool state = false;
        public UserDBLayer()
        {
            //Checking connection
            con = new SqlConnection(connectionString);
            try
            {
                con.Open();
                con.Close();
                this.state = true;
            }
            catch(Exception e)
            {
                this.state = false;
            }
        }
        public int search(string username, string password)
        {
            if (this.state == false)
            {
                return 0;
            }
            con.Open();
            SqlCommand com = new SqlCommand("Select count(*) from Users where UserName='" + username + "' and password = '" + password + "'",con);
            int value = Convert.ToInt32(com.ExecuteScalar());
            SqlCommand isAdminCom = new SqlCommand("Select isAdmin from Users where UserName ='" + username + "'",con);
            bool isAdmin = Convert.ToBoolean(isAdminCom.ExecuteScalar());
            con.Close();
            //int value = 0;
            //string com = "select * from users";
            //SqlDataAdapter sda = new SqlDataAdapter(com,con);
            //DataTable dt = new DataTable();
            //sda.Fill(dt);
            //int i;
            //Console.WriteLine("username: "+username+"  password: "+password);
            //try
            //{
            //    for (i = 0; i < dt.Rows.Count; i++)
            //    {
            //        Console.WriteLine("1\t" + HideIt.HideIt.show("IlkAklm14", dt.Rows[i][1].ToString()));
            //        if (HideIt.HideIt.show("IlkAklm14", dt.Rows[i][1].ToString()) == username)
            //        {
            //            Console.WriteLine("2\t\t" + dt.Rows[i][1].ToString());
            //            if (HideIt.HideIt.show("IlkAklm14", dt.Rows[i][2].ToString()) == password)
            //            {
            //                Console.WriteLine("3\t\t\t" + dt.Rows[i][2].ToString());
            //                value = 1;
            //            }
            //        }
            //    }
            //}
            //catch(Exception e)
            //{
            //    Console.WriteLine("HERE: "+e.Message);
            //}
            if (value == 1 && isAdmin == false)
            {
                return 1;
            }
            else if(value == 1 && isAdmin == true)
            {
                return 2;
            }
            return 0;
        }
        public DataTable get_all()
        {
            string command = "select * from users";
            SqlDataAdapter sda = new SqlDataAdapter(command, con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }
        public bool add(string username, string password,bool isAdmin)
        {
            SqlCommand command = new SqlCommand();
            command.Connection = con;
            command.CommandText = "INSERT into Users (Username, Password, isAdmin) VALUES (@Username, @Password, @isAdmin)";
            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@Password", password);
            command.Parameters.AddWithValue("@isAdmin", isAdmin);

            try
            {
                con.Open();
                int recordsAffected = command.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            finally
            {
                con.Close();
            }
            return true;
        }
    }
}
