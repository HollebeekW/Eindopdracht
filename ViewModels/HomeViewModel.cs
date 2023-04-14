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
        //bind button to relaycommand
        public RelayCommand AddAuthorCommand { get; set; }
        public AuthorModel Author { get; set; }

        //bind textboxes to firstname and lastname
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
            //check if textboxes are filled in
            if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName))
            {
                //if any textbox is left empty, show message
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

                    //check to prevent duplicate authors from being added
                    using (var contextCheckAuthors = new MyDbContext(optionsBuilder.Options))
                    {
                        //count rows where author first and last name are the same as inputs from view
                        var count = contextCheckAuthors.Authors.Count(a => a.FirstName == FirstName && a.LastName == LastName);

                        //if no rows found, add author
                        if (count == 0)
                        {
                            using (var contextAddAuthor = new MyDbContext(optionsBuilder.Options))
                            {
                                contextAddAuthor.Authors.Add(newAuthor);
                                contextAddAuthor.SaveChanges();
                            }

                            //show message if successful
                            MessageBox.Show("Auteur toegevoegd");
                        }
                        else
                        {
                            //show message if author already exists
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
