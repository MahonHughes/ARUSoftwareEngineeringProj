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
        public static string deleteSection = "DELETE FROM Sections WHERE section_id = @section";


        //SQL queries for ProceedToFeedback
        public static string grabTemplates = "SELECT template_name FROM Templates";
        public static string grabJobPositions = "SELECT position_title FROM Job_position";
        public static int templatesNameColumnIndex = 0;
        public static int jobPositionsNameColumnIndex = 0;

        //SQL queries for Comment class
        public static string getComments = "SELECT code_name, comment_text, comment_id FROM [Comments] WHERE Comments.section_id = @section";
        public static string insertComment = "INSERT INTO Comments (code_name, comment_text, section_id) VALUES (@code_name, @comment_text, @section_id)";
        public static string deleteComment = "DELETE FROM Comments WHERE comment_id = @comment";
        public static string deleteComments = "DELETE FROM Comments WHERE section_id = @section";

        public static string insertApplicant = "INSERT INTO Applicants (applicant_name, applicant_email, job_Id, hasFeedback, hasCustomFeedback) VALUES (@applicant_name, @applicant_email, @job_Id, @hasFeedback, @hasCustomFeedback)";

        //SQL queries for FeedbackPage
        public static string getApplicants = "SELECT * FROM Applicants";// WHERE Applicants.job_id = @selected job;

        //SQL Query for getting Job IDs
        public static readonly string getJobTitles = "SELECT * FROM Job_position";

        //SQL query for getting the relevant sections for the selected template (using the templates name)
        public static string getTemplatesSections = "SELECT Sections.section_id, Sections.section_name FROM Sections INNER JOIN Template_has_Sections ON Sections.section_id = Template_has_Sections.section_id INNER JOIN Templates ON Template_has_Sections.template_id = Templates.template_id WHERE Templates.template_name = '";
        public static string getTemplatesID = "SELECT template_id FROM Templates";
        public static string insertTemplate = " INSERT INTO Templates (template_name)  VALUES (@template_name)";
        public static string insertTemplateSections = "INSERT INTO Template_has_Sections (template_id, section_id) VALUES (@template_id, @section_id)";
        public static string getTemplateSections = "SELECT section_name FROM Sections WHERE section_id IN (SELECT section_id FROM Template_has_sections WHERE template_id IN (SELECT template_id FROM Templates WHERE template_name = @name))";
        public static string deleteTemplate = "DELETE FROM Templates WHERE template_name = @name";
        public static string deleteTemplateSections = "DELETE FROM Template_has_Sections WHERE template_id = (SELECT template_id FROM Templates WHERE template_name = @name)";
        //SQL Query - Gets the comment ID and the Section ID from the Applicant_Comment table in the database
        public static string getPreviousFeedbackQuery ="SELECT Applicant_Comment.comment_Id, Comments.section_id FROM Applicant_Comment INNER JOIN Comments On Applicant_Comment.comment_Id = Comments.comment_id WHERE Applicant_Comment.applicant_Id = ";

        //SQL Query - Gets the comment ID and the Section ID from the Applicant_Comment table in the database
        public static string getPreviousCustomComments = "SELECT TemporaryComment.tempComment_Id, TemporaryComment.section_id FROM TemporaryComment WHERE TemporaryComment.applicant_Id = ";

        //SQL Query - Get the all comment details from the Comment table
        public static string getAllCommentDetails = "SELECT * FROM Comments WHERE section_id = '";

        //SQL Query - Insert feedback entry into the database (Applicant_Comment table)
        public static string insertFeedbackEntry = "INSERT INTO Applicant_Comment (applicant_Id, comment_Id) VALUES (@applicant_Id, @comment_Id)";

        //SQL Query - Update the hasFeedback entry of an applicant
        public static string updateFeedbackStatus = "UPDATE [Applicants] SET hasFeedback = 1 WHERE applicant_Id = ";

        //SQL Query - Remove feedback entries from the Applicant_Comment table of the database
        public static string removeFeedbackEntries = "DELETE FROM Applicant_Comment WHERE applicant_Id = ";

        //public static string insertCustomFeedbackEntry = "INSERT INTO TemporaryComment (comment_text, section_id, applicant_Id) VALUES ('@comment_text', @section_id, @applicant_Id)";
        public static string insertCustomFeedbackEntry(string text, string secID, string appID)
        {
            string query = "INSERT INTO TemporaryComment (comment_text, section_id, applicant_Id) VALUES ('" + text + "', " + secID + ", " + appID + ")";
            return query;
        }

        public static string updateCustomFeedbackStatus = "UPDATE [Applicants] SET hasCustomFeedback = 1 WHERE applicant_Id = ";

        public static string getCustomCommentID = "SELECT tempComment_Id FROM TemporaryComment WHERE comment_text = '";

        public static string getCustomCommentText = "SELECT comment_text FROM TemporaryComment WHERE tempComment_Id = ";

        public static string removeCustomFeedbackEntry = "DELETE FROM TemporaryComment WHERE tempComment_Id = ";
    }
}
