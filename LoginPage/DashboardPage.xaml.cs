﻿using System;
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
    /// Interaction logic for DashboardPage.xaml
    /// </summary>
    public partial class DashboardPage : Page
    {
        public DashboardPage()
        {
            InitializeComponent();
        }

        private void btnToFeedback_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainPage.Content = MainWindow.mainPage.proceedToFeedbackPage;
        }

        private void btnManageTemps_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainPage.Content = MainPage.manageTemplatesPage;
        }

        private void btnCreateSection_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainPage.Content = MainPage.createNewSectionPage;
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainPage.Close();
            MainWindow main = new MainWindow();
            main.Show();

        }

        private void btnManageCandidates_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainPage.Content = MainWindow.mainPage.candidateManagementPage;
        }

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

        private void Minimise_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainPage.WindowState = WindowState.Minimized;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
