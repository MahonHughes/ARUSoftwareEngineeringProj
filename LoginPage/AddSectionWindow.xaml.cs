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

        /// <summary>
        /// Executed by button click to add new section to the database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool notInvalid = true;

            //Prevents duplicate entries into database
            foreach(var sec in CreateNewSectionPage.sections)
            {
                if(sec.sectionName == tbSectionName.Text)
                {
                    System.Windows.MessageBox.Show("Invalid data, duplicate.");

                    notInvalid = false;
                    break;
                }
            }

            //Prevents null entries into database
            if (String.IsNullOrWhiteSpace(tbSectionName.Text))
            {
                System.Windows.MessageBox.Show("Invalid data, no input.");

                notInvalid = false;
            }
            
            //Add new section to the database
            if (notInvalid)
            {
                //Creates new section with given name
                TemplateSection _section = new TemplateSection(tbSectionName.Text);

                //Inserts entry to database
                DBConnection.InsertAddedSection(_section);

                //Reloads CreateNewSectionPage (so the list is created each time directly from the database so the program has an accurate ID for relations to comments)
                MainWindow.mainPage.createNewSectionPage.InitializeComponent();

                this.Hide();
            }
        }
    }
}
