using CommunityToolkit.Mvvm.Input;
using Eindopdracht.Views;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Eindopdracht.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        //bind button to view registration page
        public RelayCommand NavigateToRegistration { get; set; }
        
        //bind button to view login page
        public RelayCommand NavigateToLogin { get; set; }

        public MainWindowViewModel()
        { 
            //create instances of RelayCommand to open registration page
            NavigateToRegistration = new RelayCommand(OpenRegistrationPage);

            //do the same for login page
            NavigateToLogin = new RelayCommand(OpenLoginPage);
        }

        private void OpenRegistrationPage()
        {
            //show registration page in new window
            var RegistrationPage = new RegistrationView();
            RegistrationPage.Show();
        }

        private void OpenLoginPage()
        {
            //show login page in new window
            var LoginPage = new LoginView();
            LoginPage.Show();
        }

        public event PropertyChangedEventHandler? PropertyChanged; 
    }
}
