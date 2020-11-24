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
    /// Interaction logic for CustomCommentWindow.xaml
    /// </summary>
    public partial class CustomCommentWindow : Window
    {
        public CustomCommentWindow()
        {
            InitializeComponent();

            txbCustomComment.Focus();
        }

        private void windowClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //if (String.IsNullOrWhiteSpace(tbSectionName.Text))
            //{
            //    System.Windows.MessageBox.Show("Invalid data, no input.");

            //    notInvalid = false;
            //}
        }
    }
}
