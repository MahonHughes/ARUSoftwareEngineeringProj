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
    /// Interaction logic for CreateNewTemplate.xaml
    /// </summary>
    public partial class CreateNewTemplate : Page
    {

        List<TemplateSection> templateSections = new List<TemplateSection>();
        static List<Button> buttons = new List<Button>();

        public CreateNewTemplate()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainPage.Content = MainPage.manageTemplatesPage;
        }

        private void list_box_Loaded(object sender, RoutedEventArgs e)
        {
            list_box.Items.Clear();
            buttons.Clear(); 
            templateSections = DBConnection.GetSectionsFromDatabase();

            for (int i = 0; i < templateSections.Count; i++)
            {
                Button btn = new Button();
                btn.Content = templateSections[i].sectionName;
                btn.Height = 30;
                btn.Width = 247;
                btn.Tag = i;
                btn.Click += ButtonSelected;
                btn.Name = "Unselected";
                buttons.Add(btn);
                list_box.Items.Add(btn);
            }
        }

        private static void ButtonSelected(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.Name == "Unselected" || btn.Name == null) 
            {
                btn.Background = Brushes.White;
                btn.Foreground = Brushes.Black;
                btn.Name = "Selected";
            }
             else
            {
                btn.Background = Brushes.Transparent;
                btn.Foreground = Brushes.White;
                btn.Name = "Unselected";
            }
        }

        private void Submit(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(textBox.Text))
            {
                MessageBox.Show("Invalid data");
            }
            else
            {
                List<TemplateSection> sections = TemplateSection.SectionsForTemplate(buttons);
                int template_id = DBConnection.GetLastTemplateID();
                template_id++;//---------------------------------------------------------
                Template template = new Template(textBox.Text, sections, template_id);

                DBConnection.InsertTemplate(template.name);
                DBConnection.InsertTemplateSection(template);

                for (int i =0 ; i < buttons.Count; i++)
                {
                    buttons[i].Foreground = Brushes.White;
                    buttons[i].Background = null;
                }

                textBox.Text = " ";
            }

            DBConnection.GetTemplateSections();
        }
    }
}
