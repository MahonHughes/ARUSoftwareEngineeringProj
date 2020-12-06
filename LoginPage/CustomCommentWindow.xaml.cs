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

        /// <summary>
        /// Allows the custom comment window to be aware of its parent (the feedback page) and the 
        /// index and section ID of the section that a comment will be added to. 
        /// </summary>
        /// <param name="_parent">Feedback page, The creator of this window.</param>
        /// <param name="_index">Int, the index on the page of the selected section.</param>
        /// <param name="_sectionID">Int, the database ID of the selected section.</param>
        public void SetCustomCommentWindowDetails(FeedBackPage _parent, int _index, int _sectionID)
        {
            parent = _parent;
            buttonIndex = _index;
            sectionIdNumber = _sectionID;
        }

        /// <summary>
        /// On load this finds the currently selected applicants index and if there is a previously 
        /// added custom comment for this section it sets it in the text box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomComment_OnLoad(object sender, RoutedEventArgs e)
        {
            applicantIndex = parent.FindSelectedApplicantIndex();

            if (parent.applicants[applicantIndex].hasSavedCustomFeedback)
            {
                int _customCommentId = SearchForCustomCommentID(applicantIndex);

                customCommentText = DBConnection.GetCommentText(Constants.getCustomCommentText, _customCommentId);

                customCommentId = _customCommentId;

                txbCustomComment.Text = customCommentText;
                startText = customCommentText;
            }
            else
            {
                startText = null;
            }
        }

        /// <summary>
        /// Retrieves a comment's ID from the currently selected applicant's custom comment list.
        /// </summary>
        /// <param name="_index">Int, the currently selected apllicants index in the applicants list.</param>
        /// <returns>Int, the custom comment needed's ID.</returns>
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

        /// <summary>
        /// Decides if the window can be closed without loss of data.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Closes the window if this does not result in loss of data then calls the windows parent to 
        /// have the UI reflect the fact that a custom comment has been added.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void windowClose_Click(object sender, RoutedEventArgs e)
        {
            if (DecideCloseButtonLeavingCircumstances())
            {
                Close();

                parent.CustomCommentWindowCallBack(buttonIndex, customCommentText, sectionIdNumber);
            }
        }

        /// <summary>
        /// When the add comment button is clicked this sets the validComment bool to true,
        /// if a valid comment has been added, so when this window calls the feedback page 
        /// after closing the comment will be added.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddComment_Click(object sender, RoutedEventArgs e)
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

        /// <summary>
        /// When the remove comment button is pushed this resets the UI and deletes the previous 
        /// comment from the database,
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
