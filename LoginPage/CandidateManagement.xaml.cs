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
            candidateListBox.Items.Add("Jane Simmonds: jane@gmail.com");
            string path = GetCSVPathFromUser();
            candidateListBox.Items.Add(path);
            Applicant[] applicants = readApplicantCSVFile(path);
            for (int i = 0; i < applicants.Length; i++)
            {
                candidateListBox.Items.Add(applicants[i].name + ": " + applicants[i].emailAddress);
            }
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
    }
}
