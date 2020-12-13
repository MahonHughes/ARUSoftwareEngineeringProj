using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginPage
{
    public  class Comment
    {
        // stores the name of the code the comments is attached to 
        public string code_name;
        // stores the id of the section the comment is attached to
        public int section_id;
        // stores the text of the comment 
        public string text;
        // stores next id, in order to give a propper id to the next comment and to use it for creating a new comment 
        private static int nextID = 0;
        // Stores the comment id, in case the comment need to be deleted from the database
        public int comment_id;
       
        /// <summary>
        /// Contrsuctor, used than a new comment is created in for the section 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="comm"></param>
        public Comment(string name, string comm)
        {
            this.code_name = name;
            this.text = comm;
            comment_id = nextID;
        }

        /// <summary>
        /// Constructor, used than comments are taken from the database 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="comm"></param>
        /// <param name="id"></param>
        public Comment(string name, string comm, int id)
        {
            this.code_name = name;
            this.text = comm;
            this.comment_id = id;
            nextID = comment_id + 1;
        }

        /// <summary>
        /// Contructor, used than comments are taken from the database to populate the Procced feedcback page with comments 
        /// </summary>
        /// <param name="_ID"></param>
        /// <param name="name"></param>
        /// <param name="comm"></param>
        public Comment(int _ID, string name, string comm)
        {
            comment_id = _ID;
            code_name = name;
            text = comm;
        }
    }
}
