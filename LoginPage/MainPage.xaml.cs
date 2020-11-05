using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LoginPage
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Window
    {
        public  DashboardPage dashboard = new DashboardPage();
        public  ManageTemplatesPage manageTemplatesPage = new ManageTemplatesPage();
        public  ProceedFeedbackPage proceedToFeedbackPage = new ProceedFeedbackPage();
        public  CreateNewSectionPage createNewSectionPage = new CreateNewSectionPage();
        public FeedBackPage feedbackPage = new FeedBackPage();

        public MainPage()
        {
            InitializeComponent();
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Content = dashboard;
            ///mainFrame.NavigationService.Navigate(new DashboardPage());
        }
    }
}
