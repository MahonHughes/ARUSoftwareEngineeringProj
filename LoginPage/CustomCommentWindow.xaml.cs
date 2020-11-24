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
        private FeedBackPage parent;
        private int buttonIndex;
        private string customComment;
        public bool validComment = false;

        public CustomCommentWindow(FeedBackPage _parent)
        {
            InitializeComponent();
            parent = _parent;

            txbCustomComment.Focus();
        }

        public void SetTalkingButtonIndex(int _index)
        {
            buttonIndex = _index;
        }

        private void windowClose_Click(object sender, RoutedEventArgs e)
        {
            Close();

            parent.CustomCommentWindowCallBack(buttonIndex, customComment);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txbCustomComment.Text))
            {
                validComment = false;
            }
            else
            {
                validComment = true;
                customComment = txbCustomComment.Text;
            }

            Close();

            parent.CustomCommentWindowCallBack(buttonIndex, customComment);
        }
    }
}
