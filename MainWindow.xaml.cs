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
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Submit_Button_Click(object sender, RoutedEventArgs e)
        {
            DBConnection.GetInstance();
            bool flag = UserAuthorization.Verification(this.Name_Text_Box.Text, this.Password_Text_Box.Text);
            if (flag)
            {
                MainPage mainPage = new MainPage();
                mainPage.Show();
                this.Hide();
            }
            else
            {
                System.Windows.MessageBox.Show("Invalid data");
            }
           

        }
    }
}
