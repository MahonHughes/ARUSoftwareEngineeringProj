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
        public int iD;

        public Applicant(string _name, string _emailAddress, int _iD)
        {
            name = _name;
            emailAddress = _emailAddress;
            iD = _iD;
        }
    }
}
