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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainPage mainPage;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Submit_Button_Click(object sender, RoutedEventArgs e)
        {
            DBConnection.GetInstance();
            bool flag = UserAuthorization.Verification(this.Name_Text_Box.Text, this.Password_Text_Box.Password);
            if (flag)
            {
                //Sets the username in the CurrentUser class
                CurrentUser.userName = Name_Text_Box.Text;
                CurrentUser.emailAddress = DBConnection.GetUserEmailAddress(CurrentUser.userName);

                mainPage = new MainPage();
                mainPage.Show();
                Hide();
            }
            else
            {
                System.Windows.MessageBox.Show("Invalid data");
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            for (int intCounter = App.Current.Windows.Count - 1; intCounter >= 0; intCounter--)  
                App.Current.Windows[intCounter].Close();
            //shutdowns WPF app
        }

        private void textBox_1_Enter(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Name_Text_Box.Text.Trim()))
            {
                Name_Text_Box.Text = "";
            }
        }

        private void textBox__Enter(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Name_Text_Box.Text))
            {
                Name_Text_Box.Text = "Username";
            }
        }

        private void PWBox_1_Enter(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Password_Text_Box.Password.Trim()))
            {
                Password_Text_Box.Password = "";
            }
        }

        private void PWWBox__Enter(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Password_Text_Box.Password))
            {
                Password_Text_Box.Password = "Password";
            }
        }

        private void MainWindow_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }   
}
