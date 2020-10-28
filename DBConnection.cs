using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LoginPage
{
    public class DBConnection
    {
        // variable to connect program to the database
        protected static string connString;

        // variable to get interact with the DB
        protected static SqlConnection dbConnetion;

        // object of this class 
        private static DBConnection instance;

        /// <summary>
        /// Constructor 
        /// </summary>
        protected DBConnection()
        {
            connString = Properties.Settings.Default.userDBconnStr;
        }
 
        public static DBConnection GetInstance()
        {
            if (instance == null)
                instance = new DBConnection();
            return instance;
        }
        

    }
}
