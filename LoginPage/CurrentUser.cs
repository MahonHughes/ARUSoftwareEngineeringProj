using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginPage
{
    //Used for storing the current users name
    //For displaying the user deltails in screen or to aid retrieval of info form database
    public class CurrentUser
    {
        //Current user's name
        public string userName;

        /// <summary>
        /// Constuctor to set username
        /// </summary>
        /// <param name="_name">Name from log-in screen.</param>
        public CurrentUser(string _name)
        {
            userName = _name;
        }
    }
}
