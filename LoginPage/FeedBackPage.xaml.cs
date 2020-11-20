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
    /// Interaction logic for FeedBackPage.xaml
    /// </summary>
    public partial class FeedBackPage : Page
    {
        public List<Applicant> applicants;
        public List<FeedbackSection> sections;
        private List<ComboBox> comboBoxes;
        private List<Button> previewButtons;
        private List<Button> customCommentButtons;

        private string currentJobPosition;
        private string currentTemplateSelected;

        private static int sectionIndex;
        private static int prevBtnIndex;
        private static int addBtnIndex;

        private static Button selectedApplcant;

        public FeedBackPage()
        {
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            sectionIndex = 0;
            addBtnIndex = 0;
            prevBtnIndex = 0;

            //Store current template and job
            currentJobPosition = CurrentUser.selectedJobPosition;
            currentTemplateSelected = CurrentUser.currentlySelectedTemplate;

            //Set text labels to show current info --------------------------------------------------------------------------------------------------

            applicantListBox.Items.Clear();

            InitialiseAndPopulateLists();

            //Sets the source for the list view.
            feedbackListView.ItemsSource = sections;

            PopulateApplicantsListBox();

            ProvidePreviousFeedbackListsForApplicants();

            SetPreviousFeedbackSelectionsInComboBoxes();
        }

        /// <summary>
        /// Initilaises the required lists and populates the applicants and sections lists
        /// from the database.
        /// </summary>
        private void InitialiseAndPopulateLists()
        {
            applicants = new List<Applicant>();
            comboBoxes = new List<ComboBox>();
            customCommentButtons = new List<Button>();
            previewButtons = new List<Button>();
            sections = new List<FeedbackSection>();

            applicants = DBConnection.GetApplicantsFromDatabase();
            sections = DBConnection.GetFeedbackSectionsFromDatabase(currentTemplateSelected);
        }

        /// <summary>
        /// Creates the applicant object (a button) with required settings and adds it to
        /// the applicant list box.
        /// </summary>
        private void PopulateApplicantsListBox()
        {
            for (int i = 0; i < applicants.Count; i++)
            {
                Button btn = new Button();
                //Changes buttons text
                btn.Content = applicants[i].name;
                //Sets buttons index
                btn.Tag = i;
                //Adds click event to the button
                btn.Click += ApplicantSelected;
                //Sets the buttons width and height
                btn.Width = applicantListBox.Width - 15;
                btn.Height = 25;
                //Set button style
                btn.Style = Application.Current.TryFindResource("ListBoxButton") as Style;
                //Adds the button to the list box
                applicantListBox.Items.Add(btn);

                if (i == 0)
                {
                    SetSelectedApplicantButton(btn);
                }
            }
        }

        /// <summary>
        /// Sets the background colour of the buttons so the user knows which
        /// applicant is selected and alters reference of the selected applicant.
        /// </summary>
        /// <param name="_btn">Button to be altered.</param>
        private void SetSelectedApplicantButton(Button _btn)
        {
            if(selectedApplcant == null)
            {
                selectedApplcant = _btn;
                _btn.Background = Brushes.Blue;
            }
            else
            {
                BrushConverter bc = new BrushConverter();
                selectedApplcant.Background = (Brush)bc.ConvertFrom("#FF3A7E85");//Colour from 'ListBoxButton' style in resource dictionary

                selectedApplcant = _btn;
                _btn.Background = Brushes.Blue;
            }
        }

        /// <summary>
        /// Sets the comboboxes' selections to the previously selected options.
        /// </summary>
        private void SetPreviousFeedbackSelectionsInComboBoxes()
        {
            int currentIndex = FindSelectedApplicant();

            if (applicants[currentIndex].hasSavedFeedback == true)
            {
                for (int i = 0; i < applicants[currentIndex].previousFeedback.Count; i++)
                {
                    for (int j = 0; j < comboBoxes.Count; j++)
                    {
                        if (sections[j].sectionID == applicants[currentIndex].previousFeedback[i][0])
                        {
                            comboBoxes[j].SelectedIndex = applicants[currentIndex].previousFeedback[i][1];
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Checks if each applicant has saved feedback before calling the database to get a list of the 
        /// section and comment ID's then calls to convert the comment ID's to the relevant combo box 
        /// indexes for the comment. Then sends the list to the applicant for storage until needed.
        /// </summary>
        private void ProvidePreviousFeedbackListsForApplicants()
        {
            for (int i = 0; i < applicants.Count; i++)
            {
                if (applicants[i].hasSavedFeedback)
                {
                    List<int[]> sections_comments = new List<int[]>();

                    sections_comments = DBConnection.SearchForPreviousFeedback(applicants[i].ID);

                    ChangeDBCommentIDsToComboBoxIndexes(sections_comments);

                    applicants[i].CreatePreviousFeedbackList(sections_comments);
                }
            }
        }

        /// <summary>
        /// Takes a list of int arrays where each array holds the section ID and Comment ID for previously selected 
        /// feedback then alters the comment ID to the required combo box index.
        /// </summary>
        /// <param name="_list">List to have the second int in each array changed from comment ID to combo box index.</param>
        /// <returns>List of int arrays, the second int in each array altered to be the combo box index for that comment.</returns>
        private List<int[]> ChangeDBCommentIDsToComboBoxIndexes(List<int[]> _list)
        {
            List<int[]> list = _list;

            for (int j = 0; j < list.Count; j++)
            {
                for (int k = 0; k < sections.Count; k++)
                {
                    if (sections[k].sectionID == list[j][0])
                    {
                        int comboBoxIndex = sections[k].GetCommentIndex(list[j][1]);

                        list[j][1] = comboBoxIndex;
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// Finds the index in the applicant list of the currently selected applicant (button).
        /// </summary>
        /// <returns>Int, the index in the applicant list.</returns>
        private int FindSelectedApplicant()
        {
            int applicantIndex = -1;

            for (int i = 0; i < applicants.Count; i++)
            {
                if (selectedApplcant.Content.ToString() == applicants[i].name)
                {
                    applicantIndex = i;
                    break;
                }
            }

            return applicantIndex;
        }

        /// <summary>
        /// Called when an applicant is selected to change the UI for giving feedback to
        /// a different applicant.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ApplicantSelected(object sender, RoutedEventArgs e)
        {
            if (CheckIfCanTransitionWithoutLossOfData())
            {
                Button btn = (Button)sender;

                SetSelectedApplicantButton(btn);
                int appIndex = FindSelectedApplicant();

                if (applicants[appIndex].hasSavedFeedback)
                {
                    SetPreviousFeedbackSelectionsInComboBoxes();
                }
                else
                {
                    for (int i = 0; i < feedbackListView.Items.Count; i++)
                    {
                        comboBoxes[i].SelectedIndex = -1;
                    }
                }
            }
        }

        /// <summary>
        /// Click event, called by the 'Preview' button to show a pop-up with the full comment of 
        /// the currently selected combo box option.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PreviewComment(object sender, RoutedEventArgs e)
        {
            //Gets clicked object
            Button btn = (Button) sender;
            //Sets the index (the buttons order in the list view)
            int index = (int)btn.Tag;

            //Only if an option is selected
            if (comboBoxes[index].SelectedIndex != -1)
            {
                //Gets the selected item from the relevant combo box
                string code = comboBoxes[index].SelectedItem.ToString();

                //Searches for the correct comment with the given code to display the full comment
                for (int i = 0; i < sections[index].comments.Count; i++)
                {
                    if (sections[index].comments[i].code_name == code)
                    {
                        MessageBox.Show("The full comment is: \n" + sections[index].comments[i].text);
                    }
                }
            }
        }

        /// <summary>
        /// Called by the 'Preview' buttons on load to assign them the correct tag
        /// and add them to the preview button list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PreviewButton_Loaded(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            if (sections.Count != 0)
            {
                previewButtons.Add(btn);
            }

            btn.Tag = prevBtnIndex;
            prevBtnIndex++;
        }

        /// <summary>
        /// Called by the 'Add' buttons on load to assign them the correct tag
        /// and add them to the custom comment button list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddButton_Loaded(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            if (sections.Count != 0)
            {
                customCommentButtons.Add(btn);
            }

            btn.Tag = addBtnIndex;
            addBtnIndex++;
        }

        /// <summary>
        /// Called by the 'Add' button to save a custom comment for a section.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddCustomComment(object sender, RoutedEventArgs e)
        {
            //Needs to store reference of custom added so can be reverted---------------------------------------------------------------------------------------

            Button btn = (Button)sender;
            int index = (int) btn.Tag;

            comboBoxes[index].IsEnabled = false;
            comboBoxes[index].SelectedIndex = -1;

            MessageBox.Show("Add button clicked.");
        }

        /// <summary>
        /// Called by combo boxes when they load to assign them the correct tags and add them 
        /// to the combo box list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox cB = (ComboBox)sender;
            cB.Tag = sectionIndex;

            if(sections.Count != 0)
            {
                comboBoxes.Add(cB);

                for (int i = 0; i < sections[sectionIndex].comments.Count; i++)
                {
                    cB.Items.Add(sections[sectionIndex].comments[i].code_name);
                }
            }

            sectionIndex++;
        }

        /// <summary>
        /// Saves the selected applicants feedback entries into the database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveFeedback_Click(object sender, RoutedEventArgs e)
        {
            string name = selectedApplcant.Content.ToString();

            if (ComboBoxesAreAllFilled())
            {
                int applicantIndex = FindSelectedApplicant();
                int applicantID = GetApplicantID();

                List<int> commentIDs = GetCommentIDs();

                if (!applicants[applicantIndex].hasSavedFeedback)
                {
                    for (int i = 0; i < sections.Count; i++)
                    {
                        DBConnection.WriteFeedbackEntryToDatabase(applicantID, commentIDs[i]);
                    }

                    applicants[applicantIndex].hasSavedFeedback = true;
                    DBConnection.UpdateApplicantsFeedbackStatus(applicantID);

                    ProvidePreviousFeedbackListsForApplicants();

                    MessageBox.Show("Feedback for " + name + ", has been saved.");
                }
                else
                {
                    DBConnection.RemoveOldFeedBackEntires(applicantID);

                    for (int i = 0; i < sections.Count; i++)
                    {
                        DBConnection.WriteFeedbackEntryToDatabase(applicantID, commentIDs[i]);
                    }

                    ProvidePreviousFeedbackListsForApplicants();

                    MessageBox.Show("Feedback for " + name + ", has been saved.");
                }
            }
            else
            {
                MessageBox.Show("Error, feedback is incomplete. \nSelect an option for all sections.");
            }
        }

        /// <summary>
        /// Gets the currently selected comments codes converts them to their ID's 
        /// and adds them to the list.
        /// </summary>
        /// <returns>List of an applicants currently selected comment ID's.</returns>
        private List<int> GetCommentIDs()
        {
            List<int> list = new List<int>();

            for (int i = 0; i < comboBoxes.Count; i++)
            {
                if (comboBoxes[i].SelectedIndex == -1)
                {
                    MessageBox.Show("Error! You have not filled in all the sections.");//Need to be altered for custom comments-----------------------------------------
                    break;
                }
                else
                {
                    //Gets the selected item from the relevant combo box
                    string code = comboBoxes[i].SelectedItem.ToString();

                    //Searches for the correct comment with the given code to display the full comment
                    for (int j = 0; j < sections[i].comments.Count; j++)
                    {
                        if (sections[i].comments[j].code_name == code)
                        {
                            list.Add(sections[i].comments[j].comment_id);
                        }
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// Uses the currently selected applicants name to find the correct ID
        /// for writing feedback to the database.
        /// </summary>
        /// <returns>Int the currently selected applicant's ID.</returns>
        private int GetApplicantID()
        {
            int ID = -1;

            for (int i = 0; i < applicants.Count; i++)
            {
                if (applicants[i].name == selectedApplcant.Content.ToString())
                {
                    ID = applicants[i].ID;
                    return ID;
                }
            }

            return ID;//Should never be reached
        }

        /// <summary>
        /// Takes user back a page when 'Back' button is pushed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (CheckIfCanTransitionWithoutLossOfData())
            {
                MainWindow.mainPage.Content = MainWindow.mainPage.proceedToFeedbackPage;
            }
        }

        /// <summary>
        /// Checks if it is ok to leave feedback page or transition to a different applicant.
        /// Prevents loss of unsaved progress.
        /// </summary>
        /// <returns>Bool, true if the selected feedback is either saved or there is none.</returns>
        private bool CheckIfCanTransitionWithoutLossOfData()
        {
            bool noActionRequired = false;

            if (ComboBoxesAreAllEmpty())
            {
                int index = FindSelectedApplicant();

                if (!applicants[index].hasSavedFeedback)
                {
                    //Combo boxes are empty, applicant has no saved feedback - no action
                    noActionRequired = true;
                }
                else
                {
                    ShowWarning();
                } 
            }
            else if (IsSelectedFeedbackEqualToSavedFeedback())
            {
                //Combo boxes not empty but feedback saved - no atcion
                noActionRequired = true;
            }
            else
            {
                ShowWarning();
            }

            return noActionRequired;
        }

        /// <summary>
        /// Displays a warning when trying to switch applicants or leave the page and there is unsaved work.
        /// </summary>
        private void ShowWarning()
        {
            //Combo boxes not empty and feedback not saved - warning
            MessageBox.Show("Warning! There are incomplete or unsaved selections. \nComplete and/or save your feedback before exiting.");
        }

        /// <summary>
        /// Compares the selected options in the combo boxes to the saved feedback for the current 
        /// applicant to determin if leaving the page will loose unsaved work or not.
        /// </summary>
        /// <returns>Bool, true if the currently selected feedback is equal to the saved feedback for the current user.</returns>
        private bool IsSelectedFeedbackEqualToSavedFeedback()
        {
            bool feedbackAndSelectedEqual = true;

            int index = FindSelectedApplicant();

            if (applicants[index].hasSavedFeedback)
            {
                List<int[]> listOne = new List<int[]>();
                List<int[]> listTwo = new List<int[]>();

                for (int i = 0; i < comboBoxes.Count; i++)
                {
                    int[] tuple = new int[] { sections[i].sectionID, comboBoxes[i].SelectedIndex };
                    listOne.Add(tuple);
                }

                listTwo = applicants[index].previousFeedback;

                for (int i = 0; i < listOne.Count; i++)
                {
                    for (int j = 0; j < listTwo.Count; j++)
                    {
                        if (listOne[i][0] == listTwo[j][0])
                        {
                            if (listOne[i][1] != listTwo[j][1])
                            {
                                feedbackAndSelectedEqual = false;
                                break;
                            }
                        }//Possible else when adding customs
                    }
                }
            }
            else
            {
                feedbackAndSelectedEqual = false;
            }

            return feedbackAndSelectedEqual;
        }

        /// <summary>
        /// Takes user to dashboard when 'Finish' button is pushed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFinish_Click(object sender, RoutedEventArgs e)
        {
            //Send feedback to pdf compile---------------------------------------------------------------------------------------------------------------

            MainWindow.mainPage.Content = MainWindow.mainPage.dashboard;
        }

        /// <summary>
        /// For chencking if any sections have got an option selected.
        /// </summary>
        /// <returns>Bool, true if applicant has had no feedback selected for any of the sections.</returns>
        private bool ComboBoxesAreAllEmpty()
        {
            bool empty = true;

            for (int i = 0; i < comboBoxes.Count; i++)
            {
                if (comboBoxes[i].SelectedIndex != -1)
                {
                    empty = false;
                    break;
                }
            }

            return empty;
        }

        /// <summary>
        /// For chencking if any sections haven't got an option selected. 
        /// </summary>
        /// <returns>Bool, true if all combo boxes have an option selected.</returns>
        private bool ComboBoxesAreAllFilled()
        {
            bool empty = true;

            for (int i = 0; i < comboBoxes.Count; i++)
            {
                if (comboBoxes[i].SelectedIndex == -1)
                {
                    empty = false;
                    break;
                }
            }

            return empty;
        }

        /// <summary>
        /// Clears the current selections of the combo boxes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearSelected_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < comboBoxes.Count; i++)
            {
                comboBoxes[i].SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Restores the combo box selections to the saved settings.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Restore_Click(object sender, RoutedEventArgs e)
        {
            int index = FindSelectedApplicant();

            for (int i = 0; i < comboBoxes.Count; i++)
            {
                comboBoxes[i].SelectedIndex = applicants[index].previousFeedback[i][1];//Also need changes for custom
            }
        }
    }
}
//To do
//Not populating the first applicants feedback on load
//Sort custom comments