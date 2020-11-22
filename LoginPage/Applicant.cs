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
        public List<int[]> previousFeedback;

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

        public void CreatePreviousFeedbackList(List<int[]> _previousFeedback)
        {
            previousFeedback = _previousFeedback;
        }
    }
}
