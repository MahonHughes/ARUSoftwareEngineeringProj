using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

                    dbConnetion.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        /// Called by CreateNewSectionPage to retrieve a list of TemplateSections. Searches database for all
        /// sections then creates an TemplateSection object for each entry and adds it to the list. Passes to
        /// the TemplateSection object the database entries name and ID.
        /// </summary>
        /// <returns>A list of TemplateSection objects.</returns>
        public static List<TemplateSection> GetSectionsFromDatabase()
        {
            using (dbConnetion = new SqlConnection(connString))
            {
                //New list for the TemplateSection objects
                List<TemplateSection> _sections = new List<TemplateSection>();

                dbConnetion.Open();

                SqlCommand cmd = new SqlCommand(Constants.sectionsFetchData, dbConnetion);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    //Variable to hold the ID from the database
                    int _id = 0;

                    //Only if the entry has an ID, though all should
                    if (Int32.TryParse(reader[0].ToString(), out _id))
                    {
                        //Creates object
                        TemplateSection section = new TemplateSection(reader[1].ToString(), _id);
                        //Adds it to the list
                        _sections.Add(section);
                    }
                }

                dbConnetion.Close();

                return _sections;
            }
        }

        /// <summary>
        /// Used by the FeedbackPage to get the relevant applicants for the selected job.
        /// </summary>
        /// <returns>List of applicants.</returns>
        public static List<Applicant> GetApplicantsFromDatabase()
        {
            using (dbConnetion = new SqlConnection(connString))
            {
                List<Applicant> applicants = new List<Applicant>();

                dbConnetion.Open();

                SqlCommand cmd = new SqlCommand(Constants.getApplicants, dbConnetion);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int _id = 0;

                    if (Int32.TryParse(reader[0].ToString(), out _id))
                    {
                        bool hasFeedback = false;

                        if (reader[4].ToString().ToLower() == "true")
                        {
                            hasFeedback = true;
                        }

                        Applicant _applicant = new Applicant(reader[1].ToString(), reader[2].ToString(), _id, hasFeedback);
                        applicants.Add(_applicant);
                    }
                }

                dbConnetion.Close();

                return applicants;
            }
        }

        /// <summary>
        /// Called by AddSectionWindow to insert a new section into the database.
        /// </summary>
        /// <param name="_section">Newly created template section.</param>
        public static void InsertAddedSection(TemplateSection _section)
        {
            using (dbConnetion = new SqlConnection(connString))
            {
                dbConnetion.Open();

                SqlCommand cmd = new SqlCommand(Constants.insertSection, dbConnetion);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.Add(new SqlParameter("section_name", _section.sectionName)); //May need more for relation to template
                cmd.ExecuteNonQuery();

                dbConnetion.Close();
            }
        }

        public static void InsertComment(Comment comment)
        {
            using (dbConnetion = new SqlConnection(connString))
            {
                dbConnetion.Open();

                SqlCommand cmd = new SqlCommand(Constants.insertComment, dbConnetion);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.Add(new SqlParameter("code_name", comment.code_name));
                cmd.Parameters.Add(new SqlParameter("comment_text", comment.text));
                cmd.Parameters.Add(new SqlParameter("section_id", comment.section_id));
                cmd.ExecuteNonQuery();
            }
        }

        public static void InsertApplicants(Applicant[] ApplicantArray)
        {
            using (dbConnetion = new SqlConnection(connString))
            {
                dbConnetion.Open();
                for (int i = 0; i < ApplicantArray.Length; i++)
                {
                    SqlCommand cmd = new SqlCommand(Constants.insertApplicant, dbConnetion);
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.Add(new SqlParameter("applicant_name", ApplicantArray[i].name));
                    cmd.Parameters.Add(new SqlParameter("applicant_email", ApplicantArray[i].emailAddress));
                    cmd.Parameters.Add(new SqlParameter("job_Id", ApplicantArray[i].groupID));
                    cmd.Parameters.Add(new SqlParameter("hasFeedback", ApplicantArray[i].hasSavedFeedback));
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static List<Comment> GetCommentFromDatabase(int section_id)
        {

            using (dbConnetion = new SqlConnection(connString))
            {
                List<Comment> comments = new List<Comment>();
                dbConnetion.Open();

                SqlCommand cmd = new SqlCommand(Constants.GetCommets(section_id), dbConnetion);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Int32.TryParse(reader[2].ToString(), out int temp);
                    Comment comment = new Comment(reader[0].ToString(), reader[1].ToString(), temp);
                    comments.Add(comment);
                }

                return comments;
            }

        }

        public static string[] GetJobPositionsFromDatabase()
        {

            using (dbConnetion = new SqlConnection(connString))
            {
                List<string> jobTitles = new List<string>();
                dbConnetion.Open();

                SqlCommand cmd = new SqlCommand(Constants.getJobTitles, dbConnetion);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string jobTitle = reader[1].ToString();
                    jobTitles.Add(jobTitle);
                }

                return jobTitles.ToArray();
            }

        }

        /// <summary>
        ///
        /// </summary>
        /// <returns>A list of Template objects.</returns>
        public static string[] GetTemplateNamesFromDatabase()
        {
            using (dbConnetion = new SqlConnection(connString))
            {
                //New list for the Template objects
                List<Template> _templates = new List<Template>();

                dbConnetion.Open();

                SqlCommand cmd = new SqlCommand(Constants.grabTemplates, dbConnetion);
                SqlDataReader reader = cmd.ExecuteReader();

                List<string> TemplateNameList = new List<string>();

                while (reader.Read())
                {
                    TemplateNameList.Add(reader[0].ToString());
                }

                dbConnetion.Close();

                return TemplateNameList.ToArray();
            }
        }

        public static int GetLastTemplateID()
        {
            int last_templateId = -1;

            using (dbConnetion = new SqlConnection(connString))
            {               
                dbConnetion.Open();
                SqlCommand cmd = new SqlCommand(Constants.getTemplatesID, dbConnetion);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Int32.TryParse(reader[0].ToString(), out  last_templateId);
                }
            }

            return last_templateId ;
        }

        /// <summary>
        /// Gets a list of the relevant sections of a template from the database.
        /// </summary>
        /// <param name="currentlySelectedTemplate">The template that the sections are needed for.</param>
        /// <returns>A list of the section objects.</returns>
        public static List<FeedbackSection> GetFeedbackSectionsFromDatabase(string currentlySelectedTemplate)
        {
            using (dbConnetion = new SqlConnection(connString))
            {
                //New list for the TemplateSection objects
                List<FeedbackSection> _sections = new List<FeedbackSection>();

                dbConnetion.Open();

                string _query = Constants.getTemplatesSections + currentlySelectedTemplate + "'";

                SqlCommand cmd = new SqlCommand(_query, dbConnetion);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    //Variable to hold the ID from the database
                    int _id;

                    //Only if the entry has an ID, though all should
                    if (Int32.TryParse(reader[0].ToString(), out _id))
                    {
                        //Creates object
                        FeedbackSection section = new FeedbackSection(reader[1].ToString(), _id);
                        //Adds it to the list
                        _sections.Add(section);
                    }
                }

                dbConnetion.Close();

                return _sections;
            }
        }

        public static void DeleteSectionFromDatabase(int section_id)
        {
            using (dbConnetion = new SqlConnection(connString))
            {
                dbConnetion.Open();
                SqlCommand cmd = new SqlCommand(Constants.DeleteComments(section_id), dbConnetion);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteNonQuery();

                SqlCommand cmd2 = new SqlCommand(Constants.DeleteSection(section_id), dbConnetion);
                cmd2.CommandType = System.Data.CommandType.Text;
                cmd2.ExecuteNonQuery();
            }
        }

        public static void DeleteComment(int comment_id)
        {
            using (dbConnetion = new SqlConnection(connString))
            {
                dbConnetion.Open();
                SqlCommand cmd = new SqlCommand(Constants.DeleteComment(comment_id), dbConnetion);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteReader();
            }
        }

        /// <summary>
        /// Searches the Applicant_Comment table for the saved feedback entries.
        /// </summary>
        /// <param name="_applicantID">The applicant to search for the feedback for.</param>
        /// <returns>A list of arrays, arrays contain a section ID and a Comment ID for each instance of a saved selection.</returns>
        public static List<int[]> SearchForPreviousFeedback(int _applicantID)
        {
            using (dbConnetion = new SqlConnection(connString))
            {
                List<int[]> sections_comments = new List<int[]>();
                string _query = Constants.getPreviousFeedbackQuery + _applicantID.ToString();

                dbConnetion.Open();
                SqlCommand cmd = new SqlCommand(_query, dbConnetion);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int[] tuple = new int[2];

                        if (Int32.TryParse(reader[1].ToString(), out tuple[0]) && Int32.TryParse(reader[0].ToString(), out tuple[1]))
                        {
                            sections_comments.Add(tuple);
                        }
                    }
                }

                dbConnetion.Close();

                return sections_comments;
            }
        }

        /// <summary>
        /// Allows for creation of comments with all their details.
        /// </summary>
        /// <param name="section_id">Relevant section to get comments for.</param>
        /// <returns>List of comments.</returns>
        public static List<Comment> GetCommentFromDatabaseWithID(int _section_id)
        {
            using (dbConnetion = new SqlConnection(connString))
            {
                List<Comment> comments = new List<Comment>();
                string _query = Constants.getAllCommentDetails + _section_id + "'";

                dbConnetion.Open();
                SqlCommand cmd = new SqlCommand(_query, dbConnetion);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int _ID;

                    if (Int32.TryParse(reader[0].ToString(), out _ID))
                    {
                        Comment comment = new Comment(_ID, reader[1].ToString(), reader[3].ToString());
                        comments.Add(comment);
                    }
                }

                dbConnetion.Close();

                return comments;
            }
        }

        /// <summary>
        /// Writes selected feedback to the Applicant_Comment table row by row.
        /// </summary>
        /// <param name="_applicantID">The applicant having feedback saved.</param>
        /// <param name="_commentID">The comment that has been selected for that applicant.</param>
        public static void WriteFeedbackEntryToDatabase(int _applicantID, int _commentID)
        {
            using (dbConnetion = new SqlConnection(connString))
            {
                dbConnetion.Open();

                SqlCommand cmd = new SqlCommand(Constants.insertFeedbackEntry, dbConnetion);
                string one = _applicantID.ToString();
                string two = _commentID.ToString();

                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.Add(new SqlParameter("applicant_Id", one));
                cmd.Parameters.Add(new SqlParameter("comment_Id", two));
                cmd.ExecuteNonQuery();
            }

            dbConnetion.Close();
        }

        /// <summary>
        /// Alters the hasFeedback entry in the database for an applicant.
        /// </summary>
        /// <param name="_applicantID">The applicant that has had feedback saved.</param>
        public static void UpdateApplicantsFeedbackStatus(int _applicantID)
        {
            using (dbConnetion = new SqlConnection(connString))
            {
                string _query = Constants.updateFeedbackStatus + _applicantID.ToString();

                dbConnetion.Open();

                SqlCommand cmd = new SqlCommand(_query, dbConnetion);

                cmd.ExecuteNonQuery();
            }
 
            dbConnetion.Close();
        }

        /// <summary>
        /// Removes feedback entries from the Applicant_Commment table using the applicant's ID.
        /// </summary>
        /// <param name="_applicantID">The applicant that needs to have their data removed.</param>
        public static void RemoveOldFeedBackEntires(int _applicantID)
        {
            using (dbConnetion = new SqlConnection(connString))
            {
                string _query = Constants.removeFeedbackEntries + _applicantID.ToString();

                dbConnetion.Open();

                SqlCommand cmd = new SqlCommand(_query, dbConnetion);

                cmd.ExecuteNonQuery();
            }

            dbConnetion.Close();
        }

        /// <summary>
        /// Used to save new template to the database
        /// </summary>
        /// <param name="tempateName"> Name of the template </param>
        public static void InsertTemplate(string tempateName)
        {
            using (dbConnetion = new SqlConnection(connString))
            {
                dbConnetion.Open();
                SqlCommand cmd = new SqlCommand(Constants.insertTemplate, dbConnetion);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.Add(new SqlParameter("template_name", tempateName));
                cmd.ExecuteNonQuery();
            }
        }

        public static void InsertTemplateSection(Template template)
        {
            using (dbConnetion = new SqlConnection(connString))
            {
                dbConnetion.Open();
                for (int i = 0; i < template.templateSections.Count; i++)
                {
                    SqlCommand cmd = new SqlCommand(Constants.insertTemplateSections, dbConnetion);
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.Add(new SqlParameter("template_id", template.id));
                    cmd.Parameters.Add(new SqlParameter("section_id", template.templateSections[i].sectionID));
                    cmd.ExecuteNonQuery();
                }
            }       
        }


        public static List<string> GetTemplateSection (string templateName)
        {
            List<string> selectedSections = new List<string>();
            using (dbConnetion = new SqlConnection(connString))
            {
                dbConnetion.Open();

                SqlCommand cmd = new SqlCommand(Constants.getTemplateSectoins,dbConnetion);
                cmd.Parameters.Add(new SqlParameter("name", templateName));
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    selectedSections.Add(reader[0].ToString());   
                }

                return selectedSections;
            }
        }


        public static void UpdateTemplateSection(Template template)
        {
            using (dbConnetion = new SqlConnection(connString))
            {
                dbConnetion.Open();
                for (int i = 0; i < template.templateSections.Count; i++)
                {
                    SqlCommand cmd = new SqlCommand(Constants.insertTemplateSections, dbConnetion);
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.Add(new SqlParameter("template_id", template.id));
                    cmd.Parameters.Add(new SqlParameter("section_id", template.templateSections[i].sectionID));
                    cmd.ExecuteNonQuery();

                }

            }
        }
        /// <summary>
        /// Method for testing the insert templatesections method 
        /// </summary>
        public static void GetTemplateSections()
        {
            string a = "SELECT * FROM TEMPLATE_has_Sections";
            List<int> template = new List<int>();
            List<int> section = new List<int>();
            using (dbConnetion = new SqlConnection(connString))
            {
                dbConnetion.Open();
                SqlCommand cmd = new SqlCommand(a, dbConnetion);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Int32.TryParse(reader[0].ToString(), out int t);
                    Int32.TryParse(reader[1].ToString(), out int s);
                    template.Add(t);
                    section.Add(s);
                }
            }
        }

        /// <summary>
        /// Gets the section ID and the tempComment ID from the database and adds them to a list as arrays.
        /// </summary>
        /// <param name="_id">The applicant that comments are required for.</param>
        /// <returns>List of int arrays that hold the section ID that comment is for and the comment ID.</returns>
        public static List<int[]> SearchForPreviousCustomCommentFeedback(int _id)
        {
            using (dbConnetion = new SqlConnection(connString))
            {
                List<int[]> sections_comments = new List<int[]>();
                string _query = Constants.getPreviousCustomComments + _id.ToString();

                dbConnetion.Open();
                SqlCommand cmd = new SqlCommand(_query, dbConnetion);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int[] tuple = new int[2];

                        if (Int32.TryParse(reader[0].ToString(), out tuple[0]) && Int32.TryParse(reader[1].ToString(), out tuple[1]))
                        {
                            sections_comments.Add(tuple);
                        }
                    }
                }

                dbConnetion.Close();

                return sections_comments;
            }
        }
    }
}
