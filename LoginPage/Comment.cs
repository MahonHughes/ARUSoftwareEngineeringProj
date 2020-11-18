using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LoginPage
{
    public  class Comment
    {
        public string code_name;
        public int section_id;
        public string text;
        public int commentID;

        public Comment(string name, string comm)
        {
            this.code_name = name;
            this.text = comm;
        }

        public Comment(int _ID, string name, string comm)
        {
            commentID = _ID;
            code_name = name;
            text = comm;
        }

        public Comment(string name, string comm, int section)
        {
            this.section_id = section;
            this.code_name = name;
            this.text = comm;
        }
    }
}
