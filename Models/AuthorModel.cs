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

        //first name
        public string? FirstName { get; set; }
        
        //last name
        public string? LastName { get; set; }

        public virtual ObservableCollection<ItemModel>? Items { get; set; }
    }
}
