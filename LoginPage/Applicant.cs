using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LoginPage
{
    public class Applicant
    {
        public string name;
        public string emailAddress;
        public int ID;
        public int? groupID;
        public bool hasSavedFeedback = false;
        public int sectionsCount;

        public List<int[]> previousFeedback;
        public List<int[]> previousFeedbackCustomComments;

        public Applicant(string _name, string _emailAddress, int _ID, int? _groupID)
        {
            name = _name;
            emailAddress = _emailAddress;
            ID = _ID;
            groupID = _groupID;
            hasSavedFeedback = false;
        }

        public Applicant(string _name, string _emailAddress, int _ID, int? _groupID, bool _hasFeedback)
        {
            name = _name;
            emailAddress = _emailAddress;
            ID = _ID;
            groupID = _groupID;
            hasSavedFeedback = _hasFeedback;
        }

        public Applicant(string _name, string _emailAddress, int _ID, bool _hasFeedback)
        {
            name = _name;
            emailAddress = _emailAddress;
            ID = _ID;
            hasSavedFeedback = _hasFeedback;
        }

        /// <summary>
        /// Allows the applicant to have the previously set feedback stored for use with multiple sessions.
        /// </summary>
        /// <param name="_previousFeedback">List of int[]'s containing the section ID and the Comment ID of a previous selection.</param>
        public void CreatePreviousFeedbackList(List<int[]> _previousFeedback)
        {
            previousFeedback = _previousFeedback;

            if (previousFeedback.Count != sectionsCount)
            {
                previousFeedbackCustomComments = DBConnection.SearchForPreviousCustomCommentFeedback(ID);
            }
        }

        /// <summary>
        /// Allows the Feedback Page to provide each applicant a count of the number of 
        /// sections they should have filled with feedback.
        /// </summary>
        /// <param name="_sectionsCount">Int, the count of sections that will be filled for the applicant.</param>
        public void SetSectionsCount(int _sectionsCount)
        {
            sectionsCount = _sectionsCount;
        }
    }
}
