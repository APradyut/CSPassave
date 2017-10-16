using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Password_directory
{
    class AdminDBLayer
    {
        private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Adarsh\source\repos\Password_directory\Password_directory\Passave.mdf;Integrated Security=True";
        SqlConnection con;
        public bool state = false;
        public string errorString = "";
        public AdminDBLayer()
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
        public int executeCommand(string commandString)
        {
            int affectedRows;
            try
            {
                SqlCommand command = new SqlCommand(commandString, con);
                con.Open();
                affectedRows = command.ExecuteNonQuery();
                con.Close();
                return affectedRows;
            }
            catch(Exception e)
            {
                errorString = e.Message; 
                return -1;
            }
        }
    }
}
