﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginPage
{
    public class Applicant
    {
        public string name;
        public int iD;

        public Applicant(string _name, int _iD)
        {
            name = _name;
            iD = _iD;
        }
    }
}