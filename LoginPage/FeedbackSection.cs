using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LoginPage
{
    public class FeedbackSection
    {
        public string sectionName { get; set; }
        public int sectionID { get; set; }

        //List for the sections comments
        public List<Comment> comments = new List<Comment>();

        /// <summary>
        /// Constructor, Sets the name and ID number and calls to get the sections comments.
        /// </summary>
        /// <param name="_name">The sections name.</param>
        /// <param name="_iD">The sections ID.</param>
        public FeedbackSection(string _name, int _iD)
        {
            sectionName = _name;
            sectionID = _iD;

            GetComments();
        }

        /// <summary>
        /// Used by the FeedbackSection object to get the comments for the selected section from the database.
        /// </summary>
        private void GetComments()
        {
            comments = DBConnection.GetCommentFromDatabase(sectionID);
        }
    }
}
