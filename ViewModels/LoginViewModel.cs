using Azure.Core;
using CommunityToolkit.Mvvm.Input;
using Eindopdracht.Models;
using Eindopdracht.Views;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows;

namespace Eindopdracht.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        //binding button
        public RelayCommand LoginCommand { get; set; }
        public UserModel User { get; set; }

        //binding textboxes
        private string _email;
        private string _password;

        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }
        public string Password
        {
            get { return _password;  }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public LoginViewModel()
        {
            User = new UserModel();
            LoginCommand = new RelayCommand(Login);
        }

        private void Login()
        {
            //check if all fields are filled in
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("Vul alle velden in");
            }
            else
            { 
                User = new UserModel()
                {
                    Email = Email,
                    Password = Password
                };

                try
                {
                    var User = new UserModel
                    {
                        Email = Email,
                        Password = Password
                    };

                    var optionsBuilder = new DbContextOptionsBuilder<MyDbContext>();

                    using (var context = new MyDbContext(optionsBuilder.Options))
                    {
                        //count rows where input email and password match with data in database
                        var count = context.Users.Count(u => u.Email == Email && u.Password == Password);

                        //if row found open new window
                        if (count > 0)
                        {
                            Application.Current.Windows[0].Close();

                            var AdminPanel = new HomeView();
                            AdminPanel.Show();
                        }

                        //if no rows found, show message
                        else
                        {
                            MessageBox.Show("Geen gebruiker gevonden");
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
