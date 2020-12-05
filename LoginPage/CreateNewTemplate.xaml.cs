﻿using System;
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
        int pageType;

        public CreateNewTemplate(int page_type)
        {
            InitializeComponent();
            pageType = page_type;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainPage.Content = MainPage.manageTemplatesPage;
        }

        private void list_box_Loaded(object sender, RoutedEventArgs e)
        {

            if (pageType == 1)
            {
                CreateNewTemplateInterface();
            }

            else if (pageType == 2)
            {
                CreateNewTemplateFromSelectedInterface();
            }

            else if (pageType == 3)
            {
                EditTemplateInterface();
            }
           
        }

        private void CreateNewTemplateInterface()
        {
            list_box.Items.Clear();
            buttons.Clear();
            templateSections = DBConnection.GetSectionsFromDatabase();
            for (int i = 0; i < templateSections.Count; i++)
            {
                Button btn = new Button();
                btn.Content = templateSections[i].sectionName;
                btn.Height = 30;
                btn.Width = 420;
                btn.Tag = i;
                btn.Click += ButtonSelected;
                btn.Name = "Unselected";
                buttons.Add(btn);
                list_box.Items.Add(btn);
            }
        }

        private void CreateNewTemplateFromSelectedInterface()
        {
            list_box.Items.Clear();
            buttons.Clear();
            templateSections = DBConnection.GetSectionsFromDatabase();
            List<string> selectedSections = DBConnection.GetTemplateSection(ManageTemplatesPage.currentTemplate);
            for (int i = 0; i < templateSections.Count; i++)
            {
                Button btn = new Button();
                btn.Content = templateSections[i].sectionName;
                btn.Height = 30;
                btn.Width = 420;
                btn.Tag = i;
                btn.Name = "Unselected";
                btn.Click += ButtonSelected;
                for (int j = 0; j < selectedSections.Count; j++)
                {
                    if (selectedSections[j] == templateSections[i].sectionName)
                    {
                        btn.Background = Brushes.White;
                        btn.Foreground = Brushes.Black;
                        btn.Name = "Selected";
                        btn.IsEnabled = false;
                    }               
                }
                buttons.Add(btn);
                list_box.Items.Add(btn);
            }


        }

        private void EditTemplateInterface()
        {
            list_box.Items.Clear();
            buttons.Clear();
            templateSections = DBConnection.GetSectionsFromDatabase();
            textBox.Text = ManageTemplatesPage.currentTemplate;
            textBox.IsEnabled = false;
            List<string> selectedSections = DBConnection.GetTemplateSection(ManageTemplatesPage.currentTemplate);
            for (int i = 0; i < templateSections.Count; i++)
            {
                Button btn = new Button();
                btn.Content = templateSections[i].sectionName;
                btn.Height = 30;
                btn.Width = 420;
                btn.Tag = i;
                btn.Name = "Unselected";
                btn.Click += ButtonSelected;
                for (int j = 0; j < selectedSections.Count; j++)
                {
                    if (selectedSections[j] == templateSections[i].sectionName)
                    {
                        btn.Background = Brushes.White;
                        btn.Foreground = Brushes.Black;
                        btn.Name = "Selected";
                    }
                }
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
            else if (pageType !=3 && TemplateSection.TemplateExists(textBox.Text))
            {
                MessageBox.Show("Template alreade exists, chage the name");
            }
            else
            {
               
                List<TemplateSection> sections = TemplateSection.SectionsForTemplate(buttons);
                int template_id = DBConnection.GetLastTemplateID();
                template_id++;//You should have no ID and enter it in the table as NULL as it's auto increment but this increment will give you the right one if you still want to use ID
                Template template = new Template(textBox.Text, sections, template_id);
            
              

                if (pageType == 3)
                {
                    DBConnection.DeleteTemplate(template.name);
                }

                DBConnection.InsertTemplate(template.name);
                DBConnection.InsertTemplateSection(template);
                
                textBox.Text = "";

                if (pageType == 1)
                {
                    ManageTemplatesPage.templateNameArray = DBConnection.GetTemplateNamesFromDatabase();
                    CreateNewTemplateInterface();
                }
                else
                {
                    MainWindow.mainPage.Content = MainPage.manageTemplatesPage;
                }
          

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
