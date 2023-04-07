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
    [Table("Users")]
    public partial class UserModel
    {
        //primary key ID
        [Key]
        public int Id { get; set; }

        //first
        public string? FirstName { get; set; }

        //last name
        public string? LastName { get; set; }

        //email
        public string? Email { get; set; }

        //password
        public string? Password { get; set;}

        //role_id
        public virtual RoleModel? Role { get; set; }
    }
}
