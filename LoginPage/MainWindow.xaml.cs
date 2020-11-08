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

        public MainWindow()
        {
            InitializeComponent();
            //Puts cursur in first text box on load
            
        }

        private void MainWindow_OnMouseDown(object sender, MouseButtonEventArgs e)
        {

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        } 

        public static MainPage mainPage;

        private void Submit_Button_Click(object sender, RoutedEventArgs e)
        {
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //To be deleted, to avoid retyping user details when testing
            if (this.Name_Text_Box.Text == "" &&  this.Password_Text_Box.Text == "")
            {
                DBConnection.GetInstance();
                //Sets the username in the CurrentUser class
                CurrentUser.userName = "Test User";

                mainPage = new MainPage();
                mainPage.Show();
                this.Hide();
            }
            else
            {
                DBConnection.GetInstance();
                bool flag = UserAuthorization.Verification(this.Name_Text_Box.Text, this.Password_Text_Box.Text);
                if (flag)
                {
                    //Sets the username in the CurrentUser class
                    CurrentUser.userName = this.Name_Text_Box.Text;

                    mainPage = new MainPage();
                    mainPage.Show();
                    this.Hide();
                }
                else
                {
                    System.Windows.MessageBox.Show("Invalid data");
                }
            }




            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //DBConnection.GetInstance();
            //bool flag = UserAuthorization.Verification(this.Name_Text_Box.Text, this.Password_Text_Box.Text);
            //if (flag)
            //{
            //    //Sets the username in the CurrentUser class
            //    user = new CurrentUser(this.Name_Text_Box.Text);

            //    mainPage = new MainPage();
            //    mainPage.Show();
            //    this.Hide();
            //}
            //else
            //{
            //    System.Windows.MessageBox.Show("Invalid data");
            //}
        }
    }
}
