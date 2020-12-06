using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginPage
{
    //Used for storing the current users name...
    //For displaying the user deltails in screen or to aid retrieval of info from database
    public static class CurrentUser
    {
        //Current user's name
        public static string userName;

        //Current user's email address
        public static string emailAddress; 

        //Current user's template and job position selection
        public static string selectedJobPosition;
        public static string currentlySelectedTemplate;
    }
}
