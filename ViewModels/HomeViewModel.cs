using CommunityToolkit.Mvvm.Input;
using Eindopdracht.Models;
using Eindopdracht.Views;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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

        //bind textboxes to firstName
        private string _firstName;
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        //bind textbox to lastName
        private string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }

        //bind list to authors
        private ObservableCollection<AuthorModel> _authors;
        public ObservableCollection<AuthorModel> Authors
        {
            get { return _authors; }
            set
            {
                _authors = value;
                OnPropertyChanged(nameof(Authors));
            }
        }

        public HomeViewModel()
        {
            Author = new AuthorModel();

            //add author
            AddAuthorCommand = new RelayCommand(AddAuthor);

            //show list of authors
            List<AuthorModel> authors = LoadAuthorsFromDatabase();
            Authors = new ObservableCollection<AuthorModel>(authors);

            //update list when new author is added
            Authors.CollectionChanged += Authors_CollectionChanged;
        }

        private void Authors_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                Authors = new ObservableCollection<AuthorModel>(LoadAuthorsFromDatabase());
            }
        }

        //Add Author
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

                            //add author to list
                            Authors.Add(newAuthor);

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

        //Show all authors
        private List<AuthorModel> LoadAuthorsFromDatabase()
        {

            var optionsBuilder = new DbContextOptionsBuilder<MyDbContext>();

            using (var contextAuthorList = new MyDbContext(optionsBuilder.Options))
            {
                return contextAuthorList.Authors.ToList();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
