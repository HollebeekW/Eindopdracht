using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eindopdracht.Models
{
    [Table("Users")]
    public partial class UserModel
    {
        [Key]
        public int Id { get; set; }

        private string? _firstName;
        [StringLength(50)]

        public string? FirstName { get; set; }

        private string? _lastName;
        [StringLength(50)]

        public string? LastName { get; set; }
    }
}
