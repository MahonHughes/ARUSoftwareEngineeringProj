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
            GetCSVPathFromUser();
        }

        private void GetCSVPathFromUser()
        {
            //Declare dialogue
            Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();

            //Filter for just CSV:
            openFileDlg.DefaultExt = ".csv";
            openFileDlg.Filter = "CSV Files (.csv)|*.csv";


            bool? result = openFileDlg.ShowDialog();

            if (result == true)
            {
                Console.WriteLine(openFileDlg.FileName);
                //TextBlock1.Text = System.IO.File.ReadAllText(openFileDlg.FileName);
            }
        }
    }
}
