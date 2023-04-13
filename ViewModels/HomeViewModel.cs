using CommunityToolkit.Mvvm.Input;
using Eindopdracht.Models;
using Eindopdracht.Views;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Eindopdracht.ViewModels
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        public RelayCommand AddAuthorCommand { get; set; }
        public AuthorModel Author { get; set; }

        private string _firstName;
        private string _lastName;

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

        public HomeViewModel()
        {
            AddAuthorCommand = new RelayCommand(AddAuthor);
            Author = new AuthorModel();
        }

        private void AddAuthor()
        {
            if (string.IsNullOrWhiteSpace(Author.FirstName) || string.IsNullOrWhiteSpace(Author.LastName))
            {
                MessageBox.Show("Vul alle velden in");
            }
            else
            {
                var Author = new AuthorModel()
                {
                    FirstName = FirstName,
                    LastName = LastName
                };

                try
                {
                    using (var connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MYDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"))
                    {
                        var query = "INSERT INTO Authors (FirstName,LastName) VALUES (@FirstName,@LastName)";

                        using (var command = new SqlCommand(query, connection))
                        {
                            command.Parameters.Add("@FirstName", System.Data.SqlDbType.NVarChar).Value = Author.FirstName;
                            command.Parameters.Add("@LastName", System.Data.SqlDbType.NVarChar).Value = Author.LastName;

                            connection.Open();

                            int RowsAdded = command.ExecuteNonQuery();

                            if (RowsAdded > 0)
                            {
                                MessageBox.Show("Auteur toegevoegd", "Klik op OK om terug te gaan", MessageBoxButton.OK);
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
