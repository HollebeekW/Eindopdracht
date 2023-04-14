using CommunityToolkit.Mvvm.Input;
using Eindopdracht.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel;
using System.Windows;


using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Eindopdracht.ViewModels
{
    public class RegistrationViewModel : INotifyPropertyChanged
    { 
        public UserModel User { get; set; }
        public RelayCommand AddUserCommand { get; set; }

        //Bind Textboxes from view
        private string _email;
        private string _firstName;
        private string _lastName;
        private string _password;
        private string _confirmPassword;

        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }
        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set
            {
                _confirmPassword = value;
                OnPropertyChanged(nameof(ConfirmPassword));
            }
        }

        public RegistrationViewModel()
        {
            User = new UserModel();

            //Create instance of relaycommand, so button from view executes function "AddUser"
            AddUserCommand = new RelayCommand(AddUser);
        }
        private void AddUser()
        {
            //Check if all textboxes are filled in
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrEmpty(LastName) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrEmpty(ConfirmPassword))
            {
                MessageBox.Show("Vul alle velden in");
            }
            
            //Check if Password and Password Confirmation match
            if (Password != ConfirmPassword)
            {
                MessageBox.Show("Wachtwoorden zijn niet hetzelfde");
            }

            //check if email is valid (contains @ and .)
            if(!Email.Contains('@') && !Email.Contains('.'))
            {
                MessageBox.Show("Vul een geldig email-adres in");
            }

            //If all checks pass, execute query code
            else
            {
                try
                {
                    //bind textboxes input to new user 
                    var newUser = new UserModel()
                    {
                        Email = Email,
                        FirstName = FirstName,
                        LastName = LastName,
                        Password = Password
                    };

                    var optionsBuilder = new DbContextOptionsBuilder<MyDbContext>();

                    using (var context = new MyDbContext(optionsBuilder.Options))
                    {
                        //insert new user into database
                        context.Users.Add(newUser);
                        context.SaveChanges();
                    }

                    //show messagebox if sucessfully registered
                    MessageBox.Show("Account geregistreerd");
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
