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
        static int a = 0;

        AddSectionWindow sectionWindow = new AddSectionWindow();
        public CreateNewSectionPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            sectionWindow.Show();
           
        }

        private void sectionsListBox_Loaded(object sender, RoutedEventArgs e)
        {
          
            if (a==0)
            {
                Sections.FetchDataFromTheDatabase();
                for (int i = 0; i < Sections.sections.Count; i++)
                {
                    sectionsListBox.Items.Add(Sections.sections[i].sectionName);
                }
                a++;
            }
        
        }

        private void bt_goBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainPage.Content = MainWindow.mainPage.dashboard;
        }

        private void bt_comment_Click(object sender, RoutedEventArgs e)
        {
           
        }

    }
}
