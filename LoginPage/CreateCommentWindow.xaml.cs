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
    /// Interaction logic for CreateCommentWindow.xaml
    /// </summary>
    public partial class CreateCommentWindow : Window
    {
        public CreateCommentWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Method is used to submit the creatinon of new comment, 
        /// if comment name is typed along with the text of the comment, new comment will be created and added to the database  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Submit_click(object sender, RoutedEventArgs e)
        {
         
            if (string.IsNullOrEmpty(tb_codeName.Text)  || string.IsNullOrEmpty(tb_comment.Text))
            {
                MessageBox.Show("Invalid data");
            }
            else
            {
                int section_id = CreateNewSectionPage.currentSectionID;
                Comment comment = TemplateSection.sections[section_id].AddComment(tb_codeName.Text, tb_comment.Text);
                comment.section_id = TemplateSection.sections[section_id].sectionID;
                DBConnection.InsertComment(comment);
                CreateNewSectionPage.ShowCommentsButtons();
                MainPage.createNewSectionPage.commentWindow.Close();
            }
        }

  

      
    }
}
