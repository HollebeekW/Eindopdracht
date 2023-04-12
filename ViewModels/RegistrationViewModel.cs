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

            //If all checks pass, execute query code
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
                    //Database connection
                    using (var connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MYDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"))
                    {
                        //Query to insert users' data into Users table
                        var query = "INSERT INTO Users (Email,FirstName,Lastname,Password) VALUES (@Email,@FirstName,@LastName,@Password)";

                        using (var command = new SqlCommand(query, connection))
                        {
                            //Change all data types to NVarChar, to match database table
                            command.Parameters.Add("@Email", System.Data.SqlDbType.NVarChar).Value = User.Email;
                            command.Parameters.Add("@FirstName", System.Data.SqlDbType.NVarChar).Value = User.FirstName;
                            command.Parameters.Add("@LastName", System.Data.SqlDbType.NVarChar).Value = User.LastName;
                            command.Parameters.Add("@Password", System.Data.SqlDbType.NVarChar).Value = User.Password;

                            connection.Open();

                            //store amount of added rows in integer "RowsAdded" while executing query
                            int RowsAdded = command.ExecuteNonQuery();

                            //if RowsAdded > 0 (meaning row was succesfully added) show message
                            if (RowsAdded > 0)
                            {
                                MessageBox.Show("Account geregistreerd");
                            }
                            //Show error if row was not added
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
