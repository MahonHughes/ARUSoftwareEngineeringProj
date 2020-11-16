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

            //Set text labels to show current info

            applicantListBox.Items.Clear();

            InitialiseAndPopulateLists();

            //Sets the source for the list view.
            feedbackListView.ItemsSource = sections;

            PopulateApplicantsListBox();
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
        /// applicant is selected.
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
        /// Called when an applicant is selected to change the UI for giving feedback to
        /// a different applicant.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ApplicantSelected(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            SetSelectedApplicantButton(btn);

            //Currently just resets the selected will ned to change to load the previously selected
            for (int i = 0; i < feedbackListView.Items.Count; i++)
            {
                comboBoxes[i].SelectedIndex = -1;
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
            MessageBox.Show("Feedback saved. (If it was implemented)");

            //int applicantID = GetApplicantID();

            //List<int> sectionIDs = GetSectionIDs();

            //List<int> commentIDs = GetCommentIDs();

            //for (int i = 0; i < sectionIDs.Count; i++)
            //{
            //    DBConnection.WriteFeedbackToDatabase(applicantID, sectionIDs[i], commentIDs[i]);
            //}
        }

        /// <summary>
        /// Gets the ID's for the currently available sections.
        /// </summary>
        /// <returns>Int list of section ID's.</returns>
        private List<int> GetSectionIDs()
        {
            List<int> list = new List<int>();

            for (int i = 0; i < sections.Count; i++)
            {
                list.Add(sections[i].sectionID);
            }

            return list;
        }

        /// <summary>
        /// Gets the currently selected comments codes adn adds them to a list.
        /// </summary>
        /// <returns>List of an applicants currently selected comment ID's.</returns>
        private List<int> GetCommentIDs()
        {
            List<int> list = new List<int>();

            for (int i = 0; i < comboBoxes.Count; i++)
            {
                if (comboBoxes[i].SelectedIndex == -1)
                {
                    MessageBox.Show("Error! You have not filled in all the sections.");//Need to be altered for custom comments
                    break;
                }
                else
                {
                    //Gets the selected item from the relevant combo box
                    string code = comboBoxes[i].SelectedItem.ToString();

                    //Searches for the correct comment with the given code to display the full comment
                    for (int j = 0; j < sections[j].comments.Count; j++)
                    {
                        if (sections[i].comments[j].code_name == code)
                        {
                            list.Add(sections[i].comments[j].section_id);
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
                    ID = applicants[i].iD;
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
            MainWindow.mainPage.Content = MainWindow.mainPage.proceedToFeedbackPage;
        }

        /// <summary>
        /// Takes user to dashboard when 'Finish' button is pushed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFinish_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainPage.Content = MainWindow.mainPage.dashboard;
        }
    }
}
//To do
//Load previously selected options (if any)
//Sort custom comments
//New template_has_sections table for many to many? (avoid recreating identical sections)