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

        private void bt_goBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainPage.Content = MainWindow.mainPage.dashboard;
        }

        private void ExecuteBtn_Click(object sender, RoutedEventArgs e)
        {
            //candidateListBox.Items.Add("Jane Simmonds: jane@gmail.com");
            string path = GetCSVPathFromUser();
            //candidateListBox.Items.Add(path);
            if (path != null && path != "")
            {
                applicants = readApplicantCSVFile(path);
                candidateListBox.Items.Clear();
                for (int i = 0; i < applicants.Length; i++)
                {
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

        private Applicant[] readApplicantCSVFile(string path)
        {
            string[] CSVLines = File.ReadAllLines(path);
            //i starts at one to ignore the first line of the csv file where we expect heading names to be
            Applicant[] applicants = new Applicant[CSVLines.Length - 1];
            for (int i = 1; i < CSVLines.Length; i++)
            {
                string[] cells = CSVLines[i].Split(',');
                applicants[i-1] = new Applicant(cells[0], cells[1], i, 0);
            }
            return applicants;
        }

        private string GetCSVPathFromUser()
        {
            //Declare dialogue
            Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();

            //Filter for just CSV:
            openFileDlg.DefaultExt = ".csv";
            openFileDlg.Filter = "CSV Files (.csv)|*.csv";


            bool? result = openFileDlg.ShowDialog();

            if (result == true)
            {
                return openFileDlg.FileName;
                //TextBlock1.Text = System.IO.File.ReadAllText(openFileDlg.FileName);
            }
            return null;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            int groupID = JobPositionsDropdown.SelectedIndex + 1;
            if (groupID > 0)
            {
                for (int i = 0; i < applicants.Length; i++)
                {
                    applicants[i].groupID = groupID;
                    applicants[i].hasSavedFeedback = false;
                    applicants[i].hasSavedCustomFeedback = false;
                }
                DBConnection.InsertApplicants(applicants);
                candidateListBox.Items.Clear();
                JobPositionsDropdown.Items.Clear();
                MessageBox.Show("Applicants succesfully inserted!");
            }
            else
            {
                MessageBox.Show("Please select a job position!");
            }
        }
    }
}
