using CommunityToolkit.Mvvm.Input;
using Eindopdracht.Models;
using Eindopdracht.Views;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
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
                OnPropertyChanged(nameof(_firstName));
            }
        }

        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(_lastName));
            }
        }

        public HomeViewModel()
        {
            Author = new AuthorModel();
            AddAuthorCommand = new RelayCommand(AddAuthor);
        }

        private void AddAuthor()
        {
            if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName))
            {
                MessageBox.Show("Vul alle velden in");
            }

            else
            {
                try
                {
                    var newAuthor = new AuthorModel()
                    {
                        FirstName = FirstName,
                        LastName = LastName
                    };

                    var optionsBuilder = new DbContextOptionsBuilder<MyDbContext>();

                    using (var contextCheckAuthors = new MyDbContext(optionsBuilder.Options))
                    {
                        var count = contextCheckAuthors.Authors.Count(a => a.FirstName == FirstName && a.LastName == LastName);

                        if (count == 0)
                        {
                            using (var contextAddAuthor = new MyDbContext(optionsBuilder.Options))
                            {
                                contextAddAuthor.Authors.Add(newAuthor);
                                contextAddAuthor.SaveChanges();
                            }

                            MessageBox.Show("Auteur toegevoegd");
                        }
                        else
                        {
                            MessageBox.Show("Deze auteur is al toegevoegd");
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
