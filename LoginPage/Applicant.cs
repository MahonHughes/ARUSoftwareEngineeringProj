﻿using System;
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
        public int iD;
        public bool hasSavedFeedback = false;
        public List<int[]> previousFeedback;

        public Applicant(string _name, string _emailAddress, int _iD)
        {
            name = _name;
            emailAddress = _emailAddress;
            iD = _iD;
        }

        public Applicant(string _name, string _emailAddress, int _iD, bool _hasFeedback)
        {
            name = _name;
            emailAddress = _emailAddress;
            iD = _iD;
            hasSavedFeedback = _hasFeedback;
        }

        public void CreatePreviousFeedbackList(List<int[]> _previousFeedback)
        {
            previousFeedback = _previousFeedback;
        }
    }
}
