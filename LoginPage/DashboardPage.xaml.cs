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
    /// Interaction logic for DashboardPage.xaml
    /// </summary>
    public partial class DashboardPage : Page
    {
        public DashboardPage()
        {
            InitializeComponent();
        }

        private void btnToFeedback_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new ProceedFeedbackPage());
        }

        private void btnManageTemps_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new ManageTemplatesPage());
        }

        private void btnCreateSection_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new CreateNewSectionPage());
        }
    }
}
