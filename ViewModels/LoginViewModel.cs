using CommunityToolkit.Mvvm.Input;
using Eindopdracht.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Eindopdracht.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public RelayCommand LoginCommand { get; set; }
        public UserModel User { get; set; }

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

                            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);
                            DataTable dataTable = new DataTable();
                            sqlDataAdapter.Fill(dataTable);

                            connection.Open();
                            command.ExecuteNonQuery();
                            connection.Close();

                            if (dataTable.Rows.Count > 0)
                            {
                                MessageBox.Show("Ingelogd!");
                            }
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
