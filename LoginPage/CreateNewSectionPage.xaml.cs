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
    /// Interaction logic for CreateNewSectionPage.xaml
    /// </summary>
    public partial class CreateNewSectionPage : Page
    {
        //Declares sectionWindow
        AddSectionWindow sectionWindow;

        //Declares list for sections
        public static List<TemplateSection> sections;

        public CreateNewSectionPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Opens window to add new section.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Creates new section window each time button is pushed
            sectionWindow = new AddSectionWindow();

            sectionWindow.Show();           
        }

        /// <summary>
        /// When page loads the sections list box is populated with buttons (one for each section in the database)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sectionsListBox_Loaded(object sender, RoutedEventArgs e)
        {
            sectionsListBox.Items.Clear();

            sections = new List<TemplateSection>();

            sections = DBConnection.GetSectionsFromDatabase();

            for (int i = 0; i < sections.Count; i++)
            {
                Button btn = new Button();
                //Changes buttons text
                btn.Content = sections[i].sectionName;
                //Adds click event to the button
                btn.Click += SectionSelected;
                //Sets the buttons width and height
                btn.Width = 215;
                btn.Height = 30;
                //Adds the button to the list box
                sectionsListBox.Items.Add(btn);
            }       
        }

        /// <summary>
        /// Method called by section buttons when clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void SectionSelected(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("List button was clicked");

            //Load comments in similar fashion to sections
            //store ID for comment entry to database (section ID needed for section/comment relation)
        }

        private void bt_goBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainPage.Content = MainWindow.mainPage.dashboard;
        }

        private void bt_comment_Click(object sender, RoutedEventArgs e)
        {
           
        }

        /// <summary>
        /// Called by AddSectionWindow to reload list box values.
        /// </summary>
        public void ResetPage()
        {
            sectionsListBox_Loaded(this, null);
        }
    }
}
