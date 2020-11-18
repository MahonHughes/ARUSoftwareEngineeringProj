using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginPage
{
    public class Applicant
    {
        public string name;
        public string emailAddress;
        public int ID;
        public int? groupID;
        public bool hasFeedback;

        public Applicant(string _name, string _emailAddress, int _iD)
        {
            name = _name;
            emailAddress = _emailAddress;
            ID = _ID;
            groupID = _groupID;
            hasFeedback = false;
        }
        }
    }
}
