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
using System.Xml.Schema;

namespace LoginPage
{
    /// <summary>
    /// Interaction logic for CreateNewSectionPage.xaml
    /// </summary>
    public partial class CreateNewSectionPage : Page
    {
        //Declares sectionWindow
        AddSectionWindow sectionWindow;

        public CreateCommentWindow commentWindow;

        public static int currentSectionID = 0;
        public static int currentCommentID = 0;



        static int sectionButtonTag;
 

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
            ShowSectionButtons();
        }

        /// <summary>
        /// Method called by section buttons when clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void SectionSelected(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            Int32.TryParse(btn.Tag.ToString(), out currentSectionID);
            MainPage.createNewSectionPage.text_box.Text = "";
            MainPage.createNewSectionPage.commentsListBox.Items.Clear();
       
            MainPage.createNewSectionPage.bt_comment.IsEnabled = true;
            MainPage.createNewSectionPage.commentsListBox.Items.Clear();
            ShowCommentsButtons();
            

        }
        public static void ShowSectionButtons()
        {
            MainPage.createNewSectionPage.sectionsListBox.Items.Clear();

            sectionButtonTag = 0;
            currentSectionID = 0;
            TemplateSection.sections = DBConnection.GetSectionsFromDatabase();
            for (int i = 0; i < TemplateSection.sections.Count; i++)
            {
                List<Comment> comments = DBConnection.GetCommentFromDatabase(TemplateSection.sections[i].sectionID);
                for (int j = 0; j < comments.Count; j++)
                {
                    TemplateSection.sections[i].comments.Add(comments[j]);
                }
            }

            for (int i = 0; i < TemplateSection.sections.Count; i++)
            {
                Button btn = new Button();
                //Changes buttons text
                btn.Content = TemplateSection.sections[i].sectionName;
                //Adds click event to the button
                btn.Click += SectionSelected;
                //Sets the buttons width and height
                btn.Width = 232;
                btn.Height = 30;
                // Set button style
                // btn.Background = Brushes.;
                btn.Tag = sectionButtonTag;
                sectionButtonTag++;
                btn.Opacity = 255;
                //Adds the button to the list box
                MainPage.createNewSectionPage.sectionsListBox.Items.Add(btn);
            }
            MainPage.createNewSectionPage.text_box.Text = "";
        }
        public static void ShowCommentsButtons()
        {
            MainPage.createNewSectionPage.commentsListBox.Items.Clear();
            int CommentButtontTag = 0;
            for (int i = 0; i < TemplateSection.sections[currentSectionID].comments.Count; i++)
            {
                Button button = new Button();
                button.Content = TemplateSection.sections[currentSectionID].comments[i];
                button.Click += CommentSelected;
                button.Content = TemplateSection.sections[currentSectionID].comments[i].code_name;
                button.Width = 232;
                button.Height = 30;
                button.Opacity = 255;
                button.Tag = CommentButtontTag;
                CommentButtontTag++;
                MainPage.createNewSectionPage.commentsListBox.Items.Add(button);
            }
            MainPage.createNewSectionPage.text_box.Text = "";
        }

        private static void CommentSelected(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            Int32.TryParse(btn.Tag.ToString(), out currentCommentID);
            MainPage.createNewSectionPage.text_box.Text = TemplateSection.sections[currentSectionID].comments[currentCommentID].text;
            MainPage.createNewSectionPage.bt_delete_Comment_.IsEnabled = true;
        }

        private void bt_goBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainPage.Content = MainWindow.mainPage.dashboard;
        }

        private void bt_comment_Click(object sender, RoutedEventArgs e)
        {
                    
            commentWindow = new CreateCommentWindow();

            commentWindow.Show();
        }

        /// <summary>
        /// Called by AddSectionWindow to reload list box values.
        /// </summary>
        public void ResetPage()
        {
            sectionsListBox_Loaded(this, null);
          
        }
        private void bt_Delete_Section(object sender, RoutedEventArgs e)
        {
            DBConnection.DeleteSectionFromDatabase(TemplateSection.sections[currentSectionID].sectionID);
            ShowSectionButtons();
            ShowCommentsButtons();

        }


        private void bt_Delete_Comment(object sender, RoutedEventArgs e)
        {
            DBConnection.DeleteComment(TemplateSection.sections[currentSectionID].comments[currentCommentID].comment_id);
            TemplateSection.sections[currentSectionID].comments.RemoveAt(currentCommentID);
            ShowCommentsButtons();
        }
    }
}
