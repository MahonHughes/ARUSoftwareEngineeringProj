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
    /// Interaction logic for FeedBackPage.xaml
    /// </summary>
    public partial class FeedBackPage : Page
    {
        List<string> applicants;
        private string currentJobPosition;
        private string currentTemplateSelected;

        public FeedBackPage()
        {
            InitializeComponent();

            currentJobPosition = CurrentUser.selectedJobPosition;
            currentTemplateSelected = CurrentUser.currentlySelectedTemplate;

            //Set text labels to show current info
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            applicants = new List<string>();

            applicants = DBConnection.GetApplicantsFromDatabase();

            for (int i = 0; i < applicants.Count; i++)
            {
                Button btn = new Button();
                //Changes buttons text
                btn.Content = applicants[i];
                //Adds click event to the button
                btn.Click += ApplicantSelected;
                //Sets the buttons width and height
                btn.Width = 148;
                btn.Height = 25;
                //Set button style
                btn.Style = Application.Current.TryFindResource("ListBoxButton") as Style;
                //Adds the button to the list box
                applicantListBox.Items.Add(btn);
            }
        }

        private void ApplicantSelected(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Applicant selected.");
            //Change colour
            //Generate the section, combo boxes for the feedback
            //Save button
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainPage.Content = MainWindow.mainPage.proceedToFeedbackPage;
        }
    }
}
