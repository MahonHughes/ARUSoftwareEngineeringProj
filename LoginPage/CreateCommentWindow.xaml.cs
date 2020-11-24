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

        private void Button_Click(object sender, RoutedEventArgs e)
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

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
