﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Eindopdracht.Models
{
    [Table("Categories")]
    public partial class CategoryModel
    {
        [Key]
        public int Id { get; set; }

        private string? _categoryName;
        [StringLength(50)]

        public string? CategoryName { get; set; }

        public virtual ObservableCollection<ItemModel> Items { get; set; }
    }
}
