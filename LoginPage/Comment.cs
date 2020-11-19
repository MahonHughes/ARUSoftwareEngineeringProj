using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginPage
{
    public  class Comment
    {
        public string code_name;
        public int section_id;
        public string text;
        private static int nextID = 0;
        public int comment_id;

        public Comment(string name, string comm)
        {
            this.code_name = name;
            this.text = comm;
            comment_id = nextID;
        }
        public Comment(string name, string comm, int id)
        {
            this.code_name = name;
            this.text = comm;
            this.comment_id = id;
            nextID = comment_id + 1;
        }

        public Comment(int _ID, string name, string comm)
        {
            comment_id = _ID;
            code_name = name;
            text = comm;
        }
    }
}
