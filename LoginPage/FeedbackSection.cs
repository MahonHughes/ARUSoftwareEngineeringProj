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
        /// Used by the FeedbackSection object to get the Section to populate it's list of comments.
        /// </summary>
        private void GetComments()
        {
            comments = DBConnection.GetCommentFromDatabaseWithID(sectionID);
        }

        /// <summary>
        /// Used by the FeedbackSection object to get the combo box index for the 
        /// selected section by comparing the database ID to the list of comments ID's.
        /// </summary>
        /// <param name="_commentID">The comment ID that the combo box index is required for.</param>
        /// <returns>The given comments combo box ID.</returns>
        public int GetCommentIndex(int _commentID)
        {
            int result = -1;

            for (int i = 0; i < comments.Count; i++)
            {
                if(comments[i].comment_id == _commentID)
                {
                    result = i;
                    break;
                }
            }

            return result;
        }
    }
}
