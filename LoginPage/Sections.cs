using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media;

namespace LoginPage
{
    class Sections : DBConnection
    {
        public string sectionName;
        int sectionId;
        static int nexSectionId = 1;
        static int flag;
        public static  List<Sections> sections = new List<Sections>();
        //public static List<Sections> tempAddedSection = new List<Sections>();
        
        public static void FetchDataFromTheDatabase()
        {

            using (dbConnetion = new SqlConnection(connString))
            {
                dbConnetion.Open();
                SqlCommand cmd = new SqlCommand(Constants.sectionsFetchData,dbConnetion);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Sections section = new Sections(reader[0].ToString());               
                }
               
            }
        }
        public Sections(string name)
        {
            sectionId = nexSectionId;
            sectionName = name;
            sections.Add(this);
            nexSectionId++;
           
        }
        public static void SaveAddedSections(Sections section)
        {
            using (dbConnetion = new SqlConnection(connString))
            {
                dbConnetion.Open();
                SqlCommand cmd = new SqlCommand(Constants.insertSections, dbConnetion);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new SqlParameter("section_name", section.sectionName));
                cmd.ExecuteNonQuery();

            }

        }
    }
}
