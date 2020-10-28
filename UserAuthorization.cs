using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginPage
{
    /// <summary>
    /// Child class of the DBConnection
    /// </summary>
    public class UserAuthorization: DBConnection
    {
      
        /// <summary>
        /// Takes the data, from the user input and checks it in the database 
        /// </summary>
        /// <param name="data"> can be either name of the user or the password </param>
        /// <param name="sqlCmd">can be either command for password or for the </param>
        /// <returns></returns>
        private static bool SearchData(string data, string sqlCmd)
        {
            bool result = false;
            using (dbConnetion = new SqlConnection (connString))
            {
                dbConnetion.Open();
                SqlCommand cmd = new SqlCommand(sqlCmd,dbConnetion);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (reader[0].ToString() == data)
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }
      
        public static bool Verification(string name, string password)
        {
            bool result = false;
            bool flag1 = SearchData(name, "select user_name from [Users]");
            bool flag2 = SearchData(password, "select user_password from [Users]");
            if (flag1 && flag2)
                result = true;
            return result;
        }
       
    }
}
