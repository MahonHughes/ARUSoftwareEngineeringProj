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
using System.Windows.Shapes;

namespace LoginPage
{
    /// <summary>
    /// Interaction logic for AddSectionWindow.xaml
    /// </summary>
    public partial class AddSectionWindow : Window
    {
        public AddSectionWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool flag = true;
            foreach(var section in Sections.sections)
            {
                if(section.sectionName == tbSectionName.Text)
                {
                    System.Windows.MessageBox.Show("Invalid data");
                    flag = false;
                    break;
                }
            }
            if (String.IsNullOrWhiteSpace(tbSectionName.Text))
            {
                System.Windows.MessageBox.Show("Invalid data");
                flag = false;

            }
         
            if (flag)
            {
                Sections section = new Sections(tbSectionName.Text);
                Sections.SaveAddedSections(section);
                MainWindow.mainPage.createNewSectionPage.sectionsListBox.Items.Add(section.sectionName);
                this.Hide();
                tbSectionName.Text = "";
            }
                

        }
    }
}
