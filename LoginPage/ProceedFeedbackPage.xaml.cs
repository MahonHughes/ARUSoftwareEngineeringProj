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
    /// Interaction logic for ProceedFeedbackPage.xaml
    /// </summary>
    public partial class ProceedFeedbackPage : Page
    {
        public ProceedFeedbackPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Takes user back to Dashboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Back(object sender, RoutedEventArgs e)
        {
            MainWindow.mainPage.Content = MainWindow.mainPage.dashboard;
        }

        /// <summary>
        /// Populates dropdown boxes on load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            selectJobDropdown.Items.Clear();
            selectTemplateDropdown.Items.Clear();

            //Gets the values for each dropdown box
            DBConnection.PopulateDropDowns(selectJobDropdown, Constants.grabJobPositions, Constants.jobPositionsNameColumnIndex);
            DBConnection.PopulateDropDowns(selectTemplateDropdown, Constants.grabTemplates, Constants.templatesNameColumnIndex);
        }

        /// <summary>
        /// Moves user to feedback page when job position and template have been selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnProceed_Click(object sender, RoutedEventArgs e)
        {
            if (selectJobDropdown.SelectedItem != null && selectTemplateDropdown.SelectedItem != null)
            {
                //Saves selction for generating correnct feedback window
                CurrentUser.selectedJobPosition = selectJobDropdown.SelectedItem.ToString();
                CurrentUser.currentlySelectedTemplate = selectTemplateDropdown.SelectedItem.ToString();

                //Renews Page to avoid old data
                MainWindow.mainPage.feedbackPage = new FeedBackPage();

                //Switches to FeedbackPage
                MainWindow.mainPage.Content = MainWindow.mainPage.feedbackPage;
            }
        }

        /// <summary>
        /// Maximises the window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Minimises the window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Minimise_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainPage.WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// Closes the application.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
