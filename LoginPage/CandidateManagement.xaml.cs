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
using System.IO;

namespace LoginPage
{
    /// <summary>
    /// Interaction logic for CandidateManagement.xaml
    /// </summary>
    public partial class CandidateManagement : Page
    {
        Applicant[] applicants;
        public CandidateManagement()
        {
            InitializeComponent();
        }

        /// <summary>
        /// This method sends the user to the dashboard when they click the 'back' button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bt_goBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainPage.Content = MainWindow.mainPage.dashboard;
        }
        /// <summary>
        /// This method is executed when the user clicks the 'upload csv' button.
        /// The method then populates the listbox with all of the candidates from the CSV file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExecuteBtn_Click(object sender, RoutedEventArgs e)
        {
            //Get the path of the CSV file from the user
            string path = GetCSVPathFromUser();
            //Check that the user selected a file and didn't just cancel or close the openfiledialogue
            if (path != null && path != "")
            {
                //Read the CSV file and put the extracted applicants into the applicants array
                applicants = readApplicantCSVFile(path);
                //Clear the list box (in case there were already applicants in there from another csv file)
                candidateListBox.Items.Clear();
                //for each applicant
                for (int i = 0; i < applicants.Length; i++)
                {
                    //Add the applicant's details to the list box
                    candidateListBox.Items.Add(applicants[i].name + ": " + applicants[i].emailAddress);
                }

                //Fetch job positions and populate dropdown.
                string[] JobPositions = DBConnection.GetJobPositionsFromDatabase();
                JobPositionsDropdown.Items.Clear();
                for (int i = 0; i < JobPositions.Length; i++)
                {
                    JobPositionsDropdown.Items.Add(JobPositions[i]);
                }
            }

            /*DBConnection.InsertApplicants(applicants);
            Applicant[] applicants1 = DBConnection.GetApplicantsFromDatabase().ToArray();
            for (int i = 0; i < applicants1.Length; i++)
            {
                candidateListBox.Items.Add(applicants1[i].name + ": " + applicants1[i].emailAddress);
            }*/
        }
        /// <summary>
        /// This method is responsible for reading a CSV file containing the applicants and their email addresses
        /// The method takes a string (the file path) as input and returns an array containing each of the applicants from the CSV file.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private Applicant[] readApplicantCSVFile(string path)
        {
            //Get a string array, each string in the array containing one applicant.
            string[] CSVLines = File.ReadAllLines(path);
            //make new array of applicants
            Applicant[] applicants = new Applicant[CSVLines.Length - 1];
            //i starts at one to ignore the first line of the csv file where we expect heading names to be.
            for (int i = 1; i < CSVLines.Length; i++)
            {
                //Create a string array, each element co taining one 'cell' of the csv file
                string[] cells = CSVLines[i].Split(',');
                //Add the newly created applican object to the array
                applicants[i-1] = new Applicant(cells[0], cells[1], i, 0);
            }
            return applicants;
        }

        /// <summary>
        /// This method asks the user for a csv file and returns its path
        /// </summary>
        /// <returns></returns>
        private string GetCSVPathFromUser()
        {
            //Declare new openfiledialogue
            Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();

            //Filter for just CSV files:
            openFileDlg.DefaultExt = ".csv";
            openFileDlg.Filter = "CSV Files (.csv)|*.csv";

            //open a file dialogue, if the user chooses a file, the bool will be true, otherwise, it could be false or null
            bool? result = openFileDlg.ShowDialog();

            if (result == true)
            {
                //return file path
                return openFileDlg.FileName;
                //TextBlock1.Text = System.IO.File.ReadAllText(openFileDlg.FileName);
            }
            //if no file selected, return null
            return null;
        }
        /// <summary>
        /// This method is called when the confirmation button is clicked and adds the applicants from the csv file into the database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            //get the groupID from the selected item in the dropdown. 
            int groupID = JobPositionsDropdown.SelectedIndex + 1;
            //If no position is selected, the groupID will be 0, make sure a position is selected
            if (groupID > 0)
            {
                //for each applicant
                for (int i = 0; i < applicants.Length; i++)
                {
                    //set these default parameters.
                    applicants[i].groupID = groupID;
                    applicants[i].hasSavedFeedback = false;
                    applicants[i].hasSavedCustomFeedback = false;
                }
                //Insert applicants into the database
                DBConnection.InsertApplicants(applicants);
                //clear the applicants, ready to receive a new CSV
                candidateListBox.Items.Clear();
                //Clear the job position dropdown so no position is accidentally selected
                JobPositionsDropdown.Items.Clear();
                //Tell the user the upload was succesful via messagebox.
                MessageBox.Show("Applicants succesfully inserted!");
            }
            else
            {
                //If no job position was selected, ask the user to select one.
                MessageBox.Show("Please select a job position!");
            }
        }
        /// <summary>
        /// This method maximises the window when the maximise button is clicked
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
        /// This method minimises the window when the minimise button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Minimise_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainPage.WindowState = WindowState.Minimized;
        }
    }
}
