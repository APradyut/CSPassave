using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace Password_directory
{
    class DirectoryDBLayer
    {
        private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Adarsh\source\repos\Password_directory\Password_directory\Passave.mdf;Integrated Security=True";
        SqlConnection con;
        public bool state = false;
        public DirectoryDBLayer()
        {
            //Checking connection
            con = new SqlConnection(connectionString);
            try
            {
                con.Open();
                con.Close();
                this.state = true;
            }
            catch (Exception)
            {
                this.state = false;
            }
        }
        public string search(string location)   //returns a string containing <password>","<username>
        {
            string command_user = "select username from Directory where location = '"+ location +"'";
            string command_pass = "select password from Directory where location = '" + location + "'";
            try
            {
                SqlCommand pass_com = new SqlCommand(command_pass, con);
                SqlCommand user_com = new SqlCommand(command_user, con);
                con.Open();
                string pass_value = Convert.ToString(pass_com.ExecuteScalar());
                string user_value = Convert.ToString(user_com.ExecuteScalar());
                con.Close();
                return pass_value + "," + user_value;
            }
            catch(Exception e)
            {
                return "[ERROR]";
            }
        }
        public DataTable searchAll(string location)
        {
            string command = "select username, password from Directory where location = '"+location+"'";
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(command, con);
            sda.Fill(dt);
            return dt;
        }
        public bool add(string Location, string username, string password)
        {
            SqlCommand command = new SqlCommand();
            command.Connection = con;            // <== lacking

            command.CommandText = "INSERT into Directory (Username, Password, Location) VALUES (@Username, @Password, @Location)";
            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@Password", password);
            command.Parameters.AddWithValue("@Location", Location);

            try
            {
                con.Open();
                int recordsAffected = command.ExecuteNonQuery();
            }
            catch (SqlException)
            {
                return false;
            }
            finally
            {
                con.Close();
            }
            return true;
        }
        public DataTable get_all()
        {
            string command = "select * from directory";
            SqlDataAdapter sda = new SqlDataAdapter(command, con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }
    }
}
