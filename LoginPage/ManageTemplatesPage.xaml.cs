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
        public static string[] templateNameArray;

        public ManageTemplatesPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Opens the page dor creating new template
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Create_new_template_Click(object sender, RoutedEventArgs e)
        {
          
            MainWindow.mainPage.Content = MainPage.createNewTemplate;
        }


        /// <summary>
        /// Opens the page for populating the list box wiith the existing templates 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TemplateListBox_Loaded(object sender, RoutedEventArgs e)
        {
            templateNameArray = DBConnection.GetTemplateNamesFromDatabase();
            templatesListBox.Items.Clear();
            for (int i = 0; i < templateNameArray.Length; i++)
            {
                Button btn = new Button();
                btn.Content = templateNameArray[i];
                btn.Height = 50;
                btn.Width = 465;
                btn.FontSize = 23;
                btn.Click += TemplateSelected;
                templatesListBox.Items.Add(btn);
            }
        }
        private static void TemplateSelected(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            currentTemplate = btn.Content.ToString();
        }

        private void Back__click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainPage.Content = MainWindow.mainPage.dashboard;
        }

        private void New_from_selected_click(object sender, RoutedEventArgs e)
        {
            if (currentTemplate != null)
            {
                MainWindow.mainPage.Content = MainPage.createNewTemplateFromSelected;
            }
          
        }

        private void Edit_click(object sender, RoutedEventArgs e)
        {
            if (currentTemplate != null)
            {
                MainWindow.mainPage.Content = MainPage.editTemplate;
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
