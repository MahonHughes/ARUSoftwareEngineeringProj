using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginPage
{
    public class TemplateSection
    {
        //Variables for the section name and ID number
        public string sectionName;
        public int sectionID;

        /// <summary>
        /// Constuctor assigns name and ID to object.  j
        /// </summary>
        /// <param name="_name"></param>
        /// <param name="_id"></param>
        public TemplateSection(string _name, int _id)
        {
            sectionName = _name;
            sectionID = _id;
        }

        /// <summary>
        /// Alternate constructor, used by add section window to allow for the creation
        /// of a id-less TemplateSection (Created in the database).
        /// </summary>
        /// <param name="_name"></param>
        public TemplateSection(string _name)
        {
            sectionName = _name;
        }
    }
}
