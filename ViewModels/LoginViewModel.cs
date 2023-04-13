using CommunityToolkit.Mvvm.Input;
using Eindopdracht.Models;
using Eindopdracht.Views;
using Microsoft.Data.SqlClient;
using System;
using System.ComponentModel;
using System.Data;
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
                    using (var connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MYDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"))
                    {
                        var query = "SELECT * FROM Users WHERE Email=@Email AND Password=@Password";

                        using (var command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Email", System.Data.SqlDbType.NVarChar).Value = User.Email;
                            command.Parameters.AddWithValue("@Password", System.Data.SqlDbType.NVarChar).Value = User.Password;

                            //create datatable with selected rows
                            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);
                            DataTable dataTable = new DataTable();
                            sqlDataAdapter.Fill(dataTable);

                            //open connection, execute query and then close connection
                            connection.Open();
                            command.ExecuteNonQuery();
                            connection.Close();

                            //if row exists for matching email and password, close this window and open new one
                            if (dataTable.Rows.Count > 0)
                            {
                                //close login screen
                                Application.Current.Windows[1].Close();

                                //open admin panel
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
