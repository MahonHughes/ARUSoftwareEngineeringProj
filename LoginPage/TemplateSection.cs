using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace LoginPage
{
    public class TemplateSection
    {
        //Variables for the section name and ID number
        public string sectionName;
        public int sectionID;
        private static int nextSectionID;
        
        // List of commments per section 
        public List<Comment> comments;

        //Declares list for sections
        public static List<TemplateSection> sections = new List<TemplateSection>();


        /// <summary>
        /// Constuctor assigns name and ID to object.
        /// </summary>
        /// <param name="_name"></param>
        /// <param name="_id"></param>
        public TemplateSection(string _name, int _id)
        {
            sectionName = _name;
            sectionID = _id;
            nextSectionID++;
            comments = new List<Comment>();
        }

        /// <summary>
        /// Alternate constructor, used by add section window to allow for the creation
        /// of a id-less TemplateSection (Created in the database).
        /// </summary>
        /// <param name="_name"></param>
        public TemplateSection(string _name)
        {
            sectionName = _name;
            comments = new List<Comment>();
            sectionID = nextSectionID;
            nextSectionID++;
        }

        /// <summary>
        /// Method to create a new comment for the section 
        /// </summary>
        /// <param name="code_name"></param>
        /// <param name="text"></param>
        public Comment AddComment(string code_name, string text)
        {
            Comment comment = new Comment(code_name, text);
            comments.Add(comment);
            return comment;
        }

        /// <summary>
        /// Takes the section bttons from Manage Template class and checks whether the foregroud of the butonn is black, if yes, this means 
        /// thhe 
        /// </summary>
        /// <param name="buttons"></param>
        /// <returns></returns>
        public static List<TemplateSection> SectionsForTemplate(List<Button> buttons)
        {
            sections = DBConnection.GetSectionsFromDatabase();
            List<TemplateSection> return_sections = new List<TemplateSection>();

            for (int i =0;i< buttons.Count;i++)
            {
                if (buttons[i].Foreground == Brushes.Black)
                {
                    return_sections.Add(sections[i]);
                }
            } 

            return return_sections;
        }

        public static bool TemplateExists(string text)
        {
            bool flag = false;
            foreach (string a in ManageTemplatesPage.templateNameArray)
            {
                if (a == text)
                {
                    flag = true;
                    break;
                }
            }
            return flag;
        }
    }
}
