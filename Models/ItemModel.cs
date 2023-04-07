using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Eindopdracht.Models
{
    [Table("Items")]

    public partial class ItemModel
    {
        //primary key id 
        [Key]
        public int Id { get; set; }

        //name
        public string? Title { get; set; }

        //short description
        public string? ShortDesc { get; set; }

        //long description
         public string? LongDesc { get; set; }

        //author
        public virtual AuthorModel? Authors { get; set; }

        //category
        public virtual CategoryModel? Categories { get; set; }
    }
}
