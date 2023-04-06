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

        //first name
        private string? _firstName;
        [StringLength(50)]

        public string? FirstName { get; set; }

        //last name
        private string? _lastName;
        [StringLength(50)]

        public string? LastName { get; set; }

        //email
        private string? _email;
        [StringLength(50)]

        public string? Email { get; set; }

        //password
        private string? _password;
        [StringLength(50)]

        public string? Password { get; set;}

        //role_id
        public virtual RoleModel Role { get; set; }
    }
}
