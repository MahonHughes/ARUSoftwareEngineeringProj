using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginPage
{
    public class Template
    {
        // The ID number
        public int id;
        // The section's name as displayed to the user
        public  string name;
        //List of section objects
        public List<TemplateSection> templateSections = new List<TemplateSection>();

         /// <summary>
         /// Constructor for Table class
         /// </summary>
         /// <param name="template_name"></param>
         /// <param name="sections"></param>
         /// <param name="temp_id"></param>
        public Template(string template_name, List<TemplateSection> sections, int temp_id)
        {
            name = template_name;
            templateSections = sections;
            id = temp_id;    
        }        
       
    }
}
