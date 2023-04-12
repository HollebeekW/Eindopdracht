using CommunityToolkit.Mvvm.Input;
using Eindopdracht.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Eindopdracht.ViewModels
{
    public class RegistrationViewModel : INotifyPropertyChanged
    { 
        public UserModel User { get; set; }
        public RelayCommand AddUserCommand { get; set; }

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
            AddUserCommand = new RelayCommand(AddUser);
        }
        private void AddUser()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrEmpty(LastName) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrEmpty(ConfirmPassword))
            {
                MessageBox.Show("Vul alle velden in");
            }
            
            if (Password != ConfirmPassword)
            {
                MessageBox.Show("Wachtwoorden zijn niet hetzelfde");
            }

            else
            {
                var User = new UserModel
                {
                    Email = Email,
                    FirstName = FirstName,
                    LastName = LastName,
                    Password = Password
                };

                try
                {
                    using (var connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MYDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"))
                    {
                        var query = "INSERT INTO Users (Email,FirstName,Lastname,Password) VALUES (@Email,@FirstName,@LastName,@Password)";

                        using (var command = new SqlCommand(query, connection))
                        {
                            command.Parameters.Add("@Email", System.Data.SqlDbType.NVarChar).Value = User.Email;
                            command.Parameters.Add("@FirstName", System.Data.SqlDbType.NVarChar).Value = User.FirstName;
                            command.Parameters.Add("@LastName", System.Data.SqlDbType.NVarChar).Value = User.LastName;
                            command.Parameters.Add("@Password", System.Data.SqlDbType.NVarChar).Value = User.Password;

                            connection.Open();
                            int RowsAdded = command.ExecuteNonQuery();
                            if (RowsAdded > 0)
                            {
                                MessageBox.Show("Account geregistreerd");
                            }
                            else
                            {
                                MessageBox.Show("Error");
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
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
