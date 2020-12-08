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
    public partial class UserManagementPage : Page
    {
        User[] users;
        public UserManagementPage()
        {
            InitializeComponent();
            LoadUserList();
        }

        /// <summary>
        /// This method sends the user to the login page after signing them out
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bt_goBack_Click(object sender, RoutedEventArgs e)
        {
            //AdminPage.
            MainWindow.adminPage.Close();
            MainWindow main = new MainWindow();
            main.Show();
        }

        private void LoadUserList()
        {
            usersListBox.Items.Clear();
            users = DBConnection.GetUsersFromDatabase();
            foreach (User user in users)
            {
                string line = user.username + ": " + user.emailAddress;
                if (user.emailAddress == "")
                {
                    line += "<NO EMAIL>";
                }
                usersListBox.Items.Add(line);
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

        /// <summary>
        /// Closes the application.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private bool UsernameTaken(string _username)
        {
            for (int i = 0; i < users.Length; i++)
            {
                if (users[i].username == _username)
                {
                    return true;
                }
            }
            return false;
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            if (usernameTextBox.Text == "")
            {
                MessageBox.Show("No username specified");
            }
            else if (usernameTextBox.Text == "admin")
            {
                MessageBox.Show("Admin user already exists");
            }
            else if (UsernameTaken(usernameTextBox.Text))
            {
                MessageBox.Show("This user already exists!");
            }
            else if (passwordTextBox.Text == "")
            {
                MessageBox.Show("Please specify a password!");
            }
            else
            {
                DBConnection.InsertUser(usernameTextBox.Text, passwordTextBox.Text, emailTextBox.Text);
                LoadUserList();
                usernameTextBox.Text = "";
                passwordTextBox.Text = "";
                emailTextBox.Text = "";
                MessageBox.Show("Done.");
            }
        }

        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            int selectedID = usersListBox.SelectedIndex;
            if (selectedID == -1)
            {
                MessageBox.Show("Please select a user to delete");
            }
            else if (users[selectedID].username == "admin")
            {
                MessageBox.Show("You may NOT detete the admin account");
            }
            else
            {
                DBConnection.DeleteUser(users[selectedID].username);
                MessageBox.Show("Deleted user: " + users[selectedID].username);
                LoadUserList();
            }
        }
    }
}
