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
    /// Interaction logic for ManageTemplatesPage.xaml
    /// </summary>
    public partial class ManageTemplatesPage : Page
    {
        public ManageTemplatesPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //this.NavigationService.Navigate(new DashboardPage());
            MainWindow.mainPage.Content = MainWindow.mainPage.dashboard;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ListBox_Loaded(object sender, RoutedEventArgs e)
        {
            string[] templateNameArray = DBConnection.GetTemplateNamesFromDatabase();
            for (int i = 0; i < templateNameArray.Length; i++)
            {
                templatesListBox.Items.Add(templateNameArray[i]);
            }
        }
    }
}
