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
        //bind button to add author
        public RelayCommand AddAuthorCommand { get; set; }

        //bind button to add item
        public RelayCommand AddItemCommand { get; set; }

        //author model
        public AuthorModel Author { get; set; }

        //item model
        public ItemModel Item { get; set; }

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

        //bind textbox to itemName
        private string _itemName;
        public string ItemName
        {
            get { return _itemName; }
            set
            {
                _itemName = value;
                OnPropertyChanged(nameof(ItemName));
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

        //bind list to items
        private ObservableCollection<ItemModel> _items;
        public ObservableCollection<ItemModel> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        public HomeViewModel()
        {
            

            //add author
            AddAuthorCommand = new RelayCommand(AddAuthor);

            //add item
            AddItemCommand = new RelayCommand(AddItem);

            //show list of authors
            List<AuthorModel> authors = LoadAuthorsFromDatabase();
            Authors = new ObservableCollection<AuthorModel>(authors);

            //update list when new author is added
            Authors.CollectionChanged += Authors_CollectionChanged;

            //show list of items
            List<ItemModel> items = LoadItemsFromDatabase();
            Items = new ObservableCollection<ItemModel>(items);

            Items.CollectionChanged += Items_CollectionChanged;
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

        private void Items_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                Items = new ObservableCollection<ItemModel>(LoadItemsFromDatabase());
            }
        }

        //Add Item
        private void AddItem()
        {
            if (string.IsNullOrWhiteSpace(ItemName))
            {
                MessageBox.Show("Vul iets in");
            }
            else
            {
                var NewItem = new ItemModel()
                {
                    Title = ItemName,
                };

                try
                {
                    var optionsBuilder = new DbContextOptionsBuilder<MyDbContext>();

                    using (var contextAddItem = new MyDbContext(optionsBuilder.Options))
                    {
                        contextAddItem.Items.Add(NewItem);
                        contextAddItem.SaveChanges();
                        MessageBox.Show("Item toegevoegd");
                    }

                    Items.Add(NewItem);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
                
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        //Show all items
        private List<ItemModel> LoadItemsFromDatabase()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MyDbContext>();

            using (var contextItemList = new MyDbContext(optionsBuilder.Options))
            {
                return contextItemList.Items.ToList();
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
