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
        int id;
        // The section's name as displayed to the user
        public  string name;
        //List of section objects
        List<TemplateSection> templateSections = new List<TemplateSection>();


        public Template(string template_name, List<TemplateSection> sections )
        {
            name = template_name;
            templateSections = sections;
        }
        
    }
}
