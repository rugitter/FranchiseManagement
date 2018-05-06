using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment2.Models
{
    public class Store
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Store Name")]
        [StringLength(20)]
        public string Name { get; set; }

        public ICollection<StoreInventory> StoreInventories { get; set; }
    }
}
