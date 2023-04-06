using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eindopdracht.Models
{
    [Table("Authors")]
    public partial class AuthorModel
    {
        [Key]
        public int Id { get; set; }

        private string? _firstName;
        public string? FirstName { get; set; }
        [StringLength(50)]

        private string? _lastName;
        [StringLength(50)]
        public string? LastName { get; set; }

        public virtual ObservableCollection<ItemModel> Items { get; set; }
    }
}
