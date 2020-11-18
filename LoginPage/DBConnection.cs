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
                        Applicant _applicant = new Applicant(reader[1].ToString(), reader[2].ToString(), _id, int.Parse(reader[3].ToString()), (bool) reader[4]);
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
                    cmd.Parameters.Add(new SqlParameter("hasFeedback", ApplicantArray[i].hasFeedback));
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
    }
}
