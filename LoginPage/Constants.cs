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
        public static string DeleteSection(int section_id)
        {
            return "DELETE FROM Sections WHERE section_id = " + section_id.ToString();
        }


        //SQL queries for ProceedToFeedback
        public static string grabTemplates = "SELECT template_name FROM Templates";
        public static string grabJobPositions = "SELECT position_title FROM Job_position";
        public static int templatesNameColumnIndex = 0;
        public static int jobPositionsNameColumnIndex = 0;

        //SQL queris for Comment class
        public static string GetCommets(int section_id)
        {
            return  "SELECT code_name, comment_text, comment_id FROM [Comments] WHERE Comments.section_id = " + section_id.ToString();
        }
        public static string insertComment = "INSERT INTO Comments (code_name, comment_text, section_id) VALUES (@code_name, @comment_text, @section_id)";
        public static string DeleteComments(int section_id)
        {
            return "DELETE FROM Comments WHERE section_id = " + section_id.ToString();
        }
        public static string DeleteComment(int comment_id)
        {
            return "DELETE FROM Comments WHERE comment_id = " + comment_id.ToString();
        }

        //SQL queries for FeedbackPage
        public static string getApplicants = "SELECT * FROM Applicants";// WHERE Applicants.job_id = @selected job;

        //SQL query for getting the relevant sections for the selected template (using the templates name)
        public static string getTemplatesSections = "SELECT * FROM Sections INNER JOIN Templates ON Sections.template_id = Templates.template_id WHERE Templates.template_name = '";
    }
}
