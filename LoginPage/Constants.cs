﻿using System;
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

        //SQL Query - Gets the comment ID and the Section ID from the Applicant_Comment table in the database
        public static string getPreviousFeedbackQuery =
            "SELECT Applicant_Comment.comment_Id, Comments.section_id FROM Applicant_Comment INNER JOIN Comments On Applicant_Comment.comment_Id = Comments.comment_id WHERE Applicant_Comment.applicant_Id = ";

        //SQL Query - Get the all comment details from the Comment table
        public static string getAllCommentDetails = "SELECT * FROM Comments WHERE section_id = '";

        //SQL Query - Insert feedback entry into the database (Applicant_Comment table)
        public static string insertFeedbackEntry = "INSERT INTO Applicant_Comment (applicant_Id, comment_Id) VALUES (@applicant_Id, @comment_Id)";

        //SQL Query - Update the hasFeedback entry of an applicant
        public static string updateFeedbackStatus = "UPDATE [Applicants] SET hasFeedback = 1 WHERE applicant_Id = ";
    }
}
