using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginPage
{
    class Constants
    {
        // SQL commands for the UserAuthorization class
        public static string userAuthorizaztionName = "select user_name from [Users]";
        public static string userAuthorizaztionPassword = "select user_password from [Users]";


        // SQl commands for Sections class
        public static string sectionsFetchData = "select * from [Sections]";
        public static string insertSection = "INSERT INTO Sections (section_name) VALUES (@section_name)";

        //SQL queries for ProceedToFeedback
        public static string grabTemplates = "SELECT template_name FROM [templates]";                       // May change names when table created
        public static string grabJobPositions = "SELECT position_name FROM [Job_positions]";                // May change names when table created
        public static int templatesNameColumnIndex = 1;                                                     // May change names when table created
        public static int jobPositionsNameColumnIndex = 1;                                                  // May change names when table created
    }
}
