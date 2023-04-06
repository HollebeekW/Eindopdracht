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
    [Table("Roles")]

    public partial class RoleModel
    {
        //primary key id
        [Key]
        public int Id { get; set; }

        //role name
        private string? _roleName;
        [StringLength(50)]

        public string? RoleName { get; set; }

        //user_id
        public virtual ObservableCollection<UserModel> Users { get; set; }
    }
}
