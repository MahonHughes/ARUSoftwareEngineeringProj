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

        public Comment(string name, string comm)
        {
            this.code_name = name;
            this.text = comm;
        }

        public Comment(string name, string comm, int section)
        {
            this.section_id = section;
            this.code_name = name;
            this.text = comm;
        }
    }
}
