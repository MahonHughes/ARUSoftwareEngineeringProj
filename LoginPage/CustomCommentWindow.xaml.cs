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
        private string customCommentText;
        private string startText;
        public bool validComment = false;
        private int sectionIdNumber;
        private int customCommentId = -1;
        private int applicantIndex;

        public CustomCommentWindow()
        {
            InitializeComponent();

            txbCustomComment.Focus();
        }

        public void SetCustomCommentWindowDetails(FeedBackPage _parent, int _index, int _sectionID)
        {
            parent = _parent;
            buttonIndex = _index;
            sectionIdNumber = _sectionID;
        }

        private void CustomComment_OnLoad(object sender, RoutedEventArgs e)
        {
            applicantIndex = parent.FindSelectedApplicantIndex();

            if (parent.applicants[applicantIndex].hasSavedCustomFeedback)
            {
                int _customCommentId = SearchForCustomCommentID(applicantIndex);

                customCommentText = DBConnection.GetCustomCommentText(_customCommentId);

                customCommentId = _customCommentId;

                txbCustomComment.Text = customCommentText;
                startText = customCommentText;
            }
            else
            {
                startText = null;
            }
        }

        private int SearchForCustomCommentID(int _index)
        {
            int comID = -1;

            for (int i = 0; i < parent.applicants[_index].previousFeedbackCustomComments.Count; i++)
            {
                if (parent.applicants[_index].previousFeedbackCustomComments[i][0] == sectionIdNumber)
                {
                    comID = parent.applicants[_index].previousFeedbackCustomComments[i][1];
                }
            }

            return comID;
        }

        private bool DecideCloseButtonLeavingCircumstances()
        {
            bool canClose = false;

            if (String.IsNullOrWhiteSpace(txbCustomComment.Text))
            {
                if (startText != customCommentText)
                {
                    MessageBox.Show("Error: To remove this comment press the 'Remove Comment' button.");
                }
                else
                {
                    canClose = true;
                }
            }
            else
            {
                canClose = true;
            }

            return canClose;
        }

        private void windowClose_Click(object sender, RoutedEventArgs e)
        {
            if (DecideCloseButtonLeavingCircumstances())
            {
                Close();

                parent.CustomCommentWindowCallBack(buttonIndex, customCommentText, sectionIdNumber);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txbCustomComment.Text) || txbCustomComment.Text == "")
            {
                MessageBox.Show("Error: You have not added a valid comment.");
            }
            else
            {
                if (txbCustomComment.Text == startText)
                {
                    MessageBox.Show("Error: The added comment is stored.");
                }
                else
                {
                    validComment = true;
                    customCommentText = txbCustomComment.Text;

                    MessageBox.Show("Comment added successfully.");
                }
            }
        }

        private void btnRemoveComment_Click(object sender, RoutedEventArgs e)
        {
            if (customCommentId != -1)
            {
                validComment = false;

                parent.RemoveCustomCommentFromDatabaseAndApplicant(customCommentId, sectionIdNumber);

                txbCustomComment.Text = null;

                BrushConverter bc = new BrushConverter();
                parent.customCommentButtons[buttonIndex].Background = (Brush)bc.ConvertFrom("#FF3A7E85");
                parent.customCommentButtons[buttonIndex].Foreground = Brushes.Black;
                parent.customCommentButtons[buttonIndex].Content = "Add";

                parent.comboBoxes[buttonIndex].Items.Remove("(User defined selection)");
                parent.comboBoxes[buttonIndex].SelectedIndex = -1;
                parent.comboBoxes[buttonIndex].IsEnabled = true;
                parent.comboBoxes[buttonIndex].Opacity = 255;
            }
        }
    }
}
