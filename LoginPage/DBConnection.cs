using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

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

        /// <summary>
        /// Called by ProceedToFeedbackPage on initialisation to populate the dropdown boxes
        /// for job and template selection.
        /// </summary>
        /// <param name="_dropdown">The Dropdown box to be populated.</param>
        /// <param name="_query">The query to be run.</param>
        /// <param name="_index">The column index for the required results.</param>
        public static void PopulateDropDowns(ComboBox _dropdown, string _query, int _index)
        {
            try
            {
                using (dbConnetion = new SqlConnection(connString))
                {
                    dbConnetion.Open();

                    SqlCommand cmd = new SqlCommand(_query, dbConnetion);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        string item = reader.GetString(_index);
                        _dropdown.Items.Add(item);
                    }

                }
            }
            catch (Exception e) 
            {
                //Nothing for now
            }
        }
    }
}
