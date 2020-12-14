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
            if (MainWindow.adminPage.WindowState == WindowState.Normal)
            {
                MainWindow.adminPage.WindowState = WindowState.Maximized;
            }
            else
            {
                MainWindow.adminPage.WindowState = WindowState.Normal;
            }
        }
        /// <summary>
        /// This method minimises the window when the minimise button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Minimise_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.adminPage.WindowState = WindowState.Minimized;
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

        /// <summary>
		/// This method checks if a given username already exists in the database
		/// </summary>
		/// <param name="_username"></param>
		/// <returns>Returns a boolean value, true=username exists already in database, false = else</returns>
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

        /// <summary>
		/// This method adds a new user to the database given that it passes all of the relevent checks
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            //cheaks whether a username has been entered into the text box
            if (usernameTextBox.Text == "")
            {
                MessageBox.Show("No username specified");
            }
            //Checks that the new account is not trying to override the administrator account
            else if (usernameTextBox.Text == "admin")
            {
                MessageBox.Show("Admin user already exists");
            }
            //cheaks whether a username has already been taken by another user account
            else if (UsernameTaken(usernameTextBox.Text))
            {
                MessageBox.Show("This user already exists!");
            }
            //checks whether a password has been entered into the text box
            else if (passwordTextBox.Text == "")
            {
                MessageBox.Show("Please specify a password!");
            }
            //If the username and password pass the checks
            else
            {
                //Add the user account to the database
                DBConnection.InsertUser(usernameTextBox.Text, passwordTextBox.Text, emailTextBox.Text);

                //Reload the list of users from the database to show the newly added account
                LoadUserList();

                //Clear the text boxes so a new user account can be added
                usernameTextBox.Text = "";
                passwordTextBox.Text = "";
                emailTextBox.Text = "";

                //Show a message telling the user the accounts have been added
                MessageBox.Show("Done.");
            }
        }

        /// <summary>
		/// This method deletes the user account that has been selected in the list.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            // Get the ID of the selected user account
            int selectedID = usersListBox.SelectedIndex;

            //if no user is selected in the list, a value of -1 will heve been returned
            if (selectedID == -1)
            {
                MessageBox.Show("Please select a user to delete");
            }
            //If the account selected is the admin account, do not allow the account to be deleted.
            else if (users[selectedID].username == "admin")
            {
                MessageBox.Show("You may NOT detete the admin account");
            }
            //In any other case, delete the account
            else
            {
                //Tell the database to delete the account
                DBConnection.DeleteUser(users[selectedID].username);
                //Display a message to the user confirming the deletion
                MessageBox.Show("Deleted user: " + users[selectedID].username);
                //Refresh the user list to reflect the deletion of the user
                LoadUserList();
            }
        }
    }
}
