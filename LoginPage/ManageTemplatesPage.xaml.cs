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
        public static string currentTemplate;
        public ManageTemplatesPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
          
            MainWindow.mainPage.Content = MainPage.createNewTemplate;
        }

        private void ListBox_Loaded(object sender, RoutedEventArgs e)
        {
            string[] templateNameArray = DBConnection.GetTemplateNamesFromDatabase();
            templatesListBox.Items.Clear();
            for (int i = 0; i < templateNameArray.Length; i++)
            {
                Button btn = new Button();
                btn.Content = templateNameArray[i];
                btn.Height = 30;
                btn.Width = 245;
                btn.Click += TemplateSelected;
                templatesListBox.Items.Add(btn);
            }
        }
        private static void TemplateSelected(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            currentTemplate = btn.Content.ToString();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow.mainPage.Content = MainWindow.mainPage.dashboard;
        }

        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            if (currentTemplate != null)
            {
                MainWindow.mainPage.Content = MainPage.createNewTemplateFromSelected;
            }
          
        }

        private void Button_Click3(object sender, RoutedEventArgs e)
        {
            if (currentTemplate != null)
            {
                MainWindow.mainPage.Content = MainPage.editTemplate;
            }
        }
    }
}
