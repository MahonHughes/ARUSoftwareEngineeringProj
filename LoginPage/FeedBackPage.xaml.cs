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
        public List<ComboBox> comboBoxes;
        private List<Button> previewButtons;
        public List<Button> customCommentButtons;

        private string currentJobPosition;
        private string currentTemplateSelected;

        private static int sectionIndex;
        private static int prevBtnIndex;
        private static int addBtnIndex;

        private static Button selectedApplcant;

        private CustomCommentWindow comWin = new CustomCommentWindow();

        public FeedBackPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// On page load sets up the pages display and variables.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            sectionIndex = 0;
            addBtnIndex = 0;
            prevBtnIndex = 0;

            //Store current template and job
            currentJobPosition = CurrentUser.selectedJobPosition;
            currentTemplateSelected = CurrentUser.currentlySelectedTemplate;

            txbJobPosition.Content = "Current Positon: " + CurrentUser.selectedJobPosition;
            txbSelectedTemplate.Content = "Selected Template: " + CurrentUser.currentlySelectedTemplate;

            applicantListBox.Items.Clear();

            InitialiseAndPopulateLists();

            //Sets the source for the list view.
            feedbackListView.ItemsSource = sections;

            PopulateApplicantsListBox();

            ProvidePreviousFeedbackListsForApplicants();
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

            ProvideSectionCountToApplicants();
        }

        /// <summary>
        /// Creates the applicant object (a button) with required settings and adds it to
        /// the applicant list box. Also selecting the first applicant as the currently selected.
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
                btn.Height = 45;
                btn.FontSize = 20;
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
                _btn.Background = Brushes.CornflowerBlue;
            }
            else
            {
                BrushConverter bc = new BrushConverter();
                selectedApplcant.Background = (Brush)bc.ConvertFrom("#FF7C96C3");

                selectedApplcant = _btn;
                _btn.Background = Brushes.CornflowerBlue;
            }
        }

        /// <summary>
        /// Allows each applicant to have a section count so each one knows how many 
        /// values it should have for previous feedback.
        /// </summary>
        private void ProvideSectionCountToApplicants()
        {
            for (int i = 0; i < applicants.Count; i++)
            {
                applicants[i].SetSectionsCount(sections.Count);
            }
        }

        /// <summary>
        /// Sets the comboboxes' selections to the previously selected options and adds the relevant visual cues for a custom comment.
        /// Called after page has loaded and when applicant transitions are made.
        /// </summary>
        private void SetPreviousFeedbackSelectionsInComboBoxes()
        {
            int currentIndex = FindSelectedApplicantIndex();

            //Sets the standard feedback selections
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

            //Sets the custom comment selections
            if (applicants[currentIndex].hasSavedCustomFeedback == true)
            {
                for (int i = 0; i < applicants[currentIndex].previousFeedbackCustomComments.Count; i++)
                {
                    for (int j = 0; j < comboBoxes.Count; j++)
                    {
                        if (sections[j].sectionID == applicants[currentIndex].previousFeedbackCustomComments[i][0])
                        {
                            comboBoxes[j].Items.Add("(User defined selection)");
                            comboBoxes[j].SelectedItem = "(User defined selection)";
                            comboBoxes[j].IsEnabled = false;
                            comboBoxes[j].Opacity = 150;

                            customCommentButtons[j].Background = Brushes.Black;
                            customCommentButtons[j].Foreground = Brushes.White;
                            customCommentButtons[j].Content = "Comment Added";
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Checks if each applicant has saved feedback before calling the database to get a list of the 
        /// section and comment ID's then calls to convert the comment ID's to the relevant combo box 
        /// indexes for that comment and sends the list to the applicant for storage until needed.
        /// </summary>
        private void ProvidePreviousFeedbackListsForApplicants()
        {
            for (int i = 0; i < applicants.Count; i++)
            {
                //for each applicant
                if (applicants[i].hasSavedFeedback)
                {
                    //Gets saved standard feedback if they have any
                    List<int[]> sections_comments = new List<int[]>();

                    sections_comments = DBConnection.SearchForPreviousFeedback(applicants[i].ID);

                    ChangeDBCommentIDsToComboBoxIndexes(sections_comments);

                    applicants[i].CreatePreviousFeedbackList(sections_comments);
                }

                //Gets saved custom feedback if they have any
                if (applicants[i].hasSavedCustomFeedback)
                {
                    List<int[]> sections_commentsCust = new List<int[]>();

                    sections_commentsCust = DBConnection.SearchForPreviousCustomCommentFeedback(applicants[i].ID);

                    for (int j = 0; j < sections_commentsCust.Count; j++)
                    {
                        applicants[i].previousFeedbackCustomComments.Add(sections_commentsCust[j]);
                    }
                }
            }
        }

        /// <summary>
        /// Takes a list of int arrays where each array holds the section ID and Comment ID for previously selected 
        /// feedback then alters the comment ID to the required combo box index.
        /// </summary>
        /// <param name="_list">List containing int arrays holding a section ID and a comment ID.</param>
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
        /// <returns>Int, currently selected applicants index in the applicant list.</returns>
        public int FindSelectedApplicantIndex()
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
                int appIndex = FindSelectedApplicantIndex();

                if (applicants[appIndex].hasSavedFeedback)
                {
                    ResetAllListViewElements();
                    SetPreviousFeedbackSelectionsInComboBoxes();
                }
                else
                {
                    ResetAllListViewElements();
                }
            }
        }

        /// <summary>
        /// Resets all the combo box and button elements in the list view to starting values.
        /// </summary>
        private void ResetAllListViewElements()
        {
            BrushConverter bc = new BrushConverter();

            for (int i = 0; i < sections.Count; i++)
            {
                comboBoxes[i].SelectedIndex = -1;
                comboBoxes[i].IsEnabled = true;

                customCommentButtons[i].Background = (Brush)bc.ConvertFrom("#FF7C96C3");
                customCommentButtons[i].Foreground = Brushes.Black;
                customCommentButtons[i].Content = "Add";
            }

            RemoveUserSelectionElementFromComboBoxes();
        }

        /// <summary>
        /// Removes the added element "(User defined selection)" from any combo boxes it has been added to.
        /// </summary>
        private void RemoveUserSelectionElementFromComboBoxes()
        {
            for (int i = 0; i < comboBoxes.Count; i++)
            {
                if (comboBoxes[i].Items.Contains("(User defined selection)"))
                {
                    comboBoxes[i].Items.Remove("(User defined selection)");
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
                if (comboBoxes[index].SelectedItem.ToString() == "(User defined selection)")
                {
                    MessageBox.Show("Error: To view a user defined comment open the custom comment window.");
                }
                else
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
        /// Called by the 'Add' button to save a custom comment for a section (showing the add custom comment window).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddCustomComment(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            int index = (int) btn.Tag;

            int sectionID = sections[index].sectionID;

            comWin.SetCustomCommentWindowDetails(this, index, sectionID);

            comWin.Show();
        }

        /// <summary>
        /// Allows the CustomCommentWindow to call back to the Feedback page to set what has been entered,
        /// if anything, in the DB and on the appliant. Also alter the UI.
        /// </summary>
        /// <param name="callingButtonIndex"></param>
        /// <param name="addedCustomComment"></param>
        public void CustomCommentWindowCallBack(int callingButtonIndex, string addedCustomComment, int sectionIdNumber)
        {
            if (comWin.validComment)
            {
                comboBoxes[callingButtonIndex].Items.Add("(User defined selection)");
                comboBoxes[callingButtonIndex].SelectedItem = "(User defined selection)";
                comboBoxes[callingButtonIndex].IsEnabled = false;
                comboBoxes[callingButtonIndex].Opacity = 150;

                int applicantID = GetApplicantID();
                int applicantIndex = FindSelectedApplicantIndex();

                int tempCommentID = DBConnection.InsertCustomCommentAndGetItsID(addedCustomComment, sectionIdNumber, applicantID);

                int[] tuple = new int[2];
                tuple[0] = sectionIdNumber;
                tuple[1] = tempCommentID;

                applicants[applicantIndex].previousFeedbackCustomComments.Add(tuple);
                applicants[applicantIndex].hasSavedCustomFeedback = true;

                customCommentButtons[callingButtonIndex].Background = Brushes.Black;
                customCommentButtons[callingButtonIndex].Foreground = Brushes.White;
                customCommentButtons[callingButtonIndex].Content = "Comment Added";
            }

            comWin = new CustomCommentWindow();
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

            //Allows the setting of previous feedback after the boxes have all loaded
            if(sectionIndex >= sections.Count -1)
            {
                SetPreviousFeedbackSelectionsInComboBoxes();
            }
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
                int applicantIndex = FindSelectedApplicantIndex();
                int applicantID = GetApplicantID();

                List<int> commentIDs = GetCommentIDs();

                if (!applicants[applicantIndex].hasSavedFeedback)
                {
                    for (int i = 0; i < commentIDs.Count; i++)
                    {
                        DBConnection.WriteFeedbackEntryToDatabase(applicantID, commentIDs[i]);
                    }

                    applicants[applicantIndex].hasSavedFeedback = true;
                    DBConnection.RunQuery(Constants.updateFeedbackStatus, applicantID);

                    ProvidePreviousFeedbackListsForApplicants();

                    MessageBox.Show("Feedback for " + name + ", has been saved.");
                }
                else
                {
                    DBConnection.RunQuery(Constants.removeFeedbackEntries, applicantID);

                    for (int i = 0; i < applicants[applicantIndex].previousFeedback.Count; i++)
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
        /// Gets the currently selected combo box items (comment codes) and converts them to their ID's 
        /// then adds them to the list.
        /// </summary>
        /// <returns>List of an applicants currently selected comment ID's.</returns>
        private List<int> GetCommentIDs()
        {
            List<int> list = new List<int>();

            for (int i = 0; i < comboBoxes.Count; i++)
            {
                if (comboBoxes[i].SelectedIndex == -1)
                {
                    MessageBox.Show("Error! You have not filled in all the sections.");
                    break;
                }
                else
                {
                    if (customCommentButtons[i].Content.ToString() == "Add")
                    {
                        //Gets the selected item from the relevant combo box
                        string code = comboBoxes[i].SelectedItem.ToString();

                        //Searches for the correct comment with the given code to display the full comment
                        for (int j = 0; j < sections[i].comments.Count; j++)
                        {
                            if (sections[i].comments[j].code_name == code)
                            {
                                list.Add(sections[i].comments[j].comment_id);
                                break;
                            }
                        }
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// Uses the currently selected applicants name to find the correct applicant ID,
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
        /// Takes user back a page when 'Back' button is pushed. If this will not cause data loss.
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
        /// <returns>Bool, true if no errors will be caused by a page or applicant transition.</returns>
        private bool CheckIfCanTransitionWithoutLossOfData()
        {
            bool noActionRequired = false;

            if (ComboBoxesAreAllEmpty())//If combo boxes are empty
            {
                int index = FindSelectedApplicantIndex();

                if (!applicants[index].hasSavedFeedback)//If applicant does not have feedback
                {
                    noActionRequired = true;//Combo boxes are empty, applicant has no saved feedback - no action
                }
                else
                {
                    ShowWarning();//Combo boxes are empty but applicant has feedback - error
                } 
            }
            else//Combo boxes are not empty
            {
                if (ComboBoxesAreAllFilled())//If combo boxes are all filled
                {
                    if (IsSelectedFeedbackEqualToSavedFeedback())//(Combo boxes are all filled) If the selections are equal to what the applicant has record of
                    {
                        noActionRequired = true;//Combo boxes are filled but selections saved - no atcion
                    }
                    else//Combo boxes are all filled but the selections are not saved - error
                    {
                        ShowWarning();
                    }
                }
                else//Combo boxes are not empty but not all filled - error
                {
                    ShowWarning();
                } 
            }

            return noActionRequired;
        }

        /// <summary>
        /// Displays a warning when trying to switch applicants or leave the page and there is unsaved work.
        /// </summary>
        private void ShowWarning()
        {
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

            int index = FindSelectedApplicantIndex();

            if (applicants[index].hasSavedFeedback)
            {
                List<int[]> listOne = new List<int[]>();
                List<int[]> listTwo = new List<int[]>();

                for (int i = 0; i < comboBoxes.Count; i++)
                {
                    int[] pair = new int[] { sections[i].sectionID, comboBoxes[i].SelectedIndex };
                    listOne.Add(pair);
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
                        }
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
        /// Saves the feedback for the current job position as a pdf, sends this feedback as an email 
        /// then takes user to dashboard when 'Finish' button is pushed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFinish_Click(object sender, RoutedEventArgs e)
        {
            if (DoAllApplicantsHaveFeedback())
            {
                for (int i = 0; i < applicants.Count; i++)
                {
                    CreatePDFAndSendFeedback(i);

                    //RemoveApplicantAndTheirRecords(i); //Disabled while testing so new data does not need to be added each time program is run
                }

                MessageBox.Show("Feedback saved as PDF's and sent to applicants.");

                MainWindow.mainPage.Content = MainWindow.mainPage.dashboard;
            }
            else
            {
                MessageBox.Show("Error! Feedback is not complete for all applicants. \nFinish the feedback for all applicants before finishing the group and sending the feedback.");
            }
        }

        /// <summary>
        /// Creates the necessary lists and variables and calls the Email class to create the PDF and send the email.
        /// </summary>
        /// <param name="_applicantIndex">The index of the current applicant to have their feedback sent.</param>
        private void CreatePDFAndSendFeedback(int _applicantIndex)
        {
            string filename = applicants[_applicantIndex].name + " - " + DateTime.Now.ToString("dd MMMM yyyy");
            string applicantName = applicants[_applicantIndex].name;
            string applicantEmail = applicants[_applicantIndex].emailAddress;
            string staffMember = CurrentUser.userName;
            string staffMemberEmail = CurrentUser.emailAddress;

            List<int[]> sections_comments = new List<int[]>();
            sections_comments = DBConnection.SearchForPreviousFeedback(applicants[_applicantIndex].ID);

            List<string> _comments = new List<string>();

            if (applicants[_applicantIndex].hasSavedCustomFeedback)
            {
                List<int[]> custom_sections_comments = new List<int[]>();
                custom_sections_comments = DBConnection.SearchForPreviousCustomCommentFeedback(applicants[_applicantIndex].ID);

                for (int i = 0; i < sections.Count; i++)
                {
                    for (int j = 0; j < sections_comments.Count; j++)
                    {
                        if (sections[i].sectionID == sections_comments[j][0])
                        {
                            string _comment = DBConnection.GetCommentText(Constants.getStandardCommentText, sections_comments[j][1]);
                            _comments.Add(_comment);
                            break;
                        }
                    }

                    for (int j = 0; j < custom_sections_comments.Count; j++)
                    {
                        if (sections[i].sectionID == custom_sections_comments[j][0])
                        {
                            string _comment = DBConnection.GetCommentText(Constants.getCustomCommentText, custom_sections_comments[j][1]);
                            _comments.Add(_comment);
                            break;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < sections.Count; i++)
                {
                    for (int j = 0; j < sections_comments.Count; j++)
                    {
                        if (sections[i].sectionID == sections_comments[j][0])
                        {
                            string _comment = DBConnection.GetCommentText(Constants.getStandardCommentText, sections_comments[j][1]);
                            _comments.Add(_comment);
                            break;
                        }
                    }
                }
            }

            List<string> _sections = new List<string>();
            for (int i = 0; i < sections.Count; i++)
            {
                _sections.Add(sections[i].sectionName);
            }
            
            Email.SendEmail(filename, applicantName, applicantEmail, staffMember, staffMemberEmail, _comments, _sections);
        }

        /// <summary>
        /// Removes an applicant from the database and all the entries for their saved feedback.
        /// </summary>
        /// <param name="_applicantIndex">The index of the current applicant to have their data removed.</param>
        private void RemoveApplicantAndTheirRecords(int _applicantIndex)
        {
            int _applicantID = applicants[_applicantIndex].ID;

            DBConnection.RunQuery(Constants.removeApplicant, _applicantID);
            DBConnection.RunQuery(Constants.removeApplicantFeedback, _applicantID);
            DBConnection.RunQuery(Constants.removeApplicantCustomFeedback, _applicantID);
        }

        /// <summary>
        /// Checks that all applicants have had their feedback completed so blank feedback 
        /// letters will not be sent and applicants will not be forgotten.
        /// </summary>
        /// <returns>Bool, true if feedback is complete for all applicants in current group.</returns>
        private bool DoAllApplicantsHaveFeedback()
        {
            bool allHaveFeedback = true;

            for (int i = 0; i < applicants.Count; i++)
            {
                if (applicants[i].hasSavedFeedback == false)
                {
                    allHaveFeedback = false;
                    break;
                }
            }

            return allHaveFeedback;
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
        /// Clears the current selections of the combo boxes 
        /// (not the custom comments which are done via the custom comment window).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearSelected_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < comboBoxes.Count; i++)
            {
                if (customCommentButtons[i].Content.ToString() == "Add")
                {
                    comboBoxes[i].SelectedIndex = -1;
                }
            }
        }

        /// <summary>
        /// Restores the combo box selections to the saved settings (If the clear button has been pressed
        /// or an unwanted change has been made).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Restore_Click(object sender, RoutedEventArgs e)
        {
            int index = FindSelectedApplicantIndex();

            if (applicants[index].hasSavedFeedback)
            {
                SetPreviousFeedbackSelectionsInComboBoxes();
            }
        }

        /// <summary>
        /// Called by the custom comment window to remove a custom comment (from the applicant and the DB).
        /// </summary>
        /// <param name="_customCommentID">Int, the ID of the comment to be removed.</param>
        /// <param name="_sectionID">Int, the ID of the section that comment was made for.</param>
        public void RemoveCustomCommentFromDatabaseAndApplicant(int _customCommentID, int _sectionID)
        {
            int _applicant = GetApplicantID();
            int index = FindSelectedApplicantIndex();

            DBConnection.RunQuery(Constants.removeCustomFeedbackEntry, _customCommentID);

            for (int i = 0; i < applicants[index].previousFeedbackCustomComments.Count; i++)
            {
                if (applicants[index].previousFeedbackCustomComments[i][0] == _sectionID)
                {
                    applicants[index].previousFeedbackCustomComments.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Maximises or normalises the window size when button pushed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Minimises the window when button pushed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Minimise_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainPage.WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// Closes the application when close button is pushed if this will not result in loss of data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            if (CheckIfCanTransitionWithoutLossOfData())
            {
                Application.Current.Shutdown();
            }
        }
    }
}