using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LoginPage
{
    /// <summary>
    /// Interaction logic for CreateNewTemplate.xaml
    /// </summary>
    public partial class CreateNewTemplate : Page
    {
        /// <summary>
        /// List of templates to populate the list box
        /// </summary>
        List<TemplateSection> templateSections = new List<TemplateSection>();

        /// <summary>
        /// lisst of buttons 
        /// </summary>
        static List<Button> sectionButtons = new List<Button>();
        
        // Type of the page which will be loaded (there are there types: Create new template, Create new template from Selected, Edit selected template)
        int pageType;

        public CreateNewTemplate(int page_type)
        {
            InitializeComponent();
            pageType = page_type;
        }

        /// <summary>
        /// Go back button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void back_btn_click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainPage.Content = MainPage.manageTemplatesPage;
        }


        /// <summary>
        /// Loadsd the interfaces for different types of pages (depends on what the user chose in the ManageTemplate page)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sections_list_box_Loaded(object sender, RoutedEventArgs e)
        {

            if (pageType == 1)
            {
                CreateNewTemplateInterface();
            }

            else if (pageType == 2)
            {
                CreateNewTemplateFromSelectedInterface();
            }

            else if (pageType == 3)
            {
                EditTemplateInterface();
            }
           
        }

        /// <summary>
        /// Interface for creating new Template page 
        /// </summary>
        private void CreateNewTemplateInterface()
        {
            sections_list_box.Items.Clear();
            sectionButtons.Clear();
            // Takes sections from the database 
            templateSections = DBConnection.GetSectionsFromDatabase();
            //Poplates the list box of sections 
            for (int i = 0; i < templateSections.Count; i++)
            {
                Button btn = new Button();
                btn.Content = templateSections[i].sectionName;
                btn.Height = 30;
                btn.Width = 420;
                btn.Tag = i;
                btn.Click += ButtonSelected;
                btn.Name = "Unselected";
                sectionButtons.Add(btn);
                sections_list_box.Items.Add(btn);
            }
        }

        /// <summary>
        /// Interface for creating the new Template from selected template 
        /// (Sections which are considered to be the part of the base template will be the part of the new template this cannot be changed) 
        /// </summary>
        private void CreateNewTemplateFromSelectedInterface()
        {
            sections_list_box.Items.Clear();
            sectionButtons.Clear();
            // Takes sections from the database 
            templateSections = DBConnection.GetSectionsFromDatabase();
            List<string> selectedSections = DBConnection.GetTemplateSection(ManageTemplatesPage.currentTemplate);
            //Poplates the list box of sections
            for (int i = 0; i < templateSections.Count; i++)
            {
                Button btn = new Button();
                btn.Content = templateSections[i].sectionName;
                btn.Height = 30;
                btn.Width = 420;
                btn.Tag = i;
                btn.Name = "Unselected";
                btn.Click += ButtonSelected;
                for (int j = 0; j < selectedSections.Count; j++)
                {
                    if (selectedSections[j] == templateSections[i].sectionName)
                    {
                        btn.Background = Brushes.White;
                        btn.Foreground = Brushes.Black;
                        btn.Name = "Selected";
                        btn.IsEnabled = false;
                    }               
                }
                sectionButtons.Add(btn);
                sections_list_box.Items.Add(btn);
            }


        }
        
        /// <summary>
        /// Interface for editing the template (Cannot change the name of the template)
        /// </summary>
        private void EditTemplateInterface()
        {
            sections_list_box.Items.Clear();
            sectionButtons.Clear();
            // get 
            templateSections = DBConnection.GetSectionsFromDatabase();
            // name of the selected template
            templateName.Text = ManageTemplatesPage.currentTemplate;
            // user cannot chage the name of the tempate
            templateName.IsEnabled = false;
            // Gets the name of the sections of the selected template from the database 
            List<string> selectedSections = DBConnection.GetTemplateSection(ManageTemplatesPage.currentTemplate);
            //Poplates the list box of sections
            for (int i = 0; i < templateSections.Count; i++)
            {
                Button btn = new Button();
                btn.Content = templateSections[i].sectionName;
                btn.Height = 30;
                btn.Width = 420;
                btn.Tag = i;
                btn.Name = "Unselected";
                btn.Click += ButtonSelected;
                for (int j = 0; j < selectedSections.Count; j++)
                {
                    if (selectedSections[j] == templateSections[i].sectionName)
                    {
                        btn.Background = Brushes.White;
                        btn.Foreground = Brushes.Black;
                        btn.Name = "Selected";
                    }
                }
                sectionButtons.Add(btn);
                sections_list_box.Items.Add(btn);
            }


        }

        /// <summary>
        /// If the section button is selected or  unselected, its color will be changed 
        /// The color is used in the program to remember which sections are selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ButtonSelected(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.Name == "Unselected" || btn.Name == null) 
            {
                btn.Background = Brushes.White;
                btn.Foreground = Brushes.Black;
                btn.Name = "Selected";
            }
             else
            {
                btn.Background = Brushes.Transparent;
                btn.Foreground = Brushes.White;
                btn.Name = "Unselected";
            }
        }

       /// <summary>
       /// Submit button to save new template or edited template 
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void Submit(object sender, RoutedEventArgs e)
        {
            // Checks if the field for the tamplate name is emmpty, if yes an error window will pop up ob the screen and template won't be created  
            if (string.IsNullOrEmpty(templateName.Text))
            {
                MessageBox.Show("Invalid data");
            }
            // checks if the name of template exists in the database,if yes the template will not be created and a relative message will  pop up on the screen
            else if (pageType !=3 && TemplateSection.TemplateExists(templateName.Text))
            {
                MessageBox.Show("Template alreade exists, chage the name");
            }
            else
            {
               
                List<TemplateSection> sections = TemplateSection.SectionsForTemplate(sectionButtons);
                int template_id = DBConnection.GetLastTemplateID();
                template_id++;
                Template template = new Template(templateName.Text, sections, template_id);
            
              
                // Used for edit template type of page, the old version of the template will be deleted and a new obe will be created on its place 
                if (pageType == 3)
                {
                    DBConnection.DeleteTemplate(template.name);
                }
                // Saves the template
                DBConnection.InsertTemplate(template.name);
                DBConnection.InsertTemplateSection(template);
                
                templateName.Text = "";
                
                // if the type of the page is Create new section,  after the template was created it will be reloaded to let the user create another new template,
                // otherwise the manage template page will be opened 
                if (pageType == 1)
                {
                    ManageTemplatesPage.templateNameArray = DBConnection.GetTemplateNamesFromDatabase();
                    CreateNewTemplateInterface();
                }
                else
                {
                    MainWindow.mainPage.Content = MainPage.manageTemplatesPage;
                }
          

            }          
        }

        private void Maximise_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.mainPage.WindowState == WindowState.Normal)
            {
                MainWindow.mainPage.WindowState = WindowState.Maximized;
            }
            else
            {
                MainWindow.mainPage.WindowState = WindowState.Normal;
            }
        }

        private void Minimise_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainPage.WindowState = WindowState.Minimized;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }


    }
}
