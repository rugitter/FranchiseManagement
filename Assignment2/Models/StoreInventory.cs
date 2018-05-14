using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment2.Models
{
    public class StoreInventory
    {
        [Required]
        public int StoreID { get; set; }
        [Required]
        public int ProductID { get; set; }
        [Required, Range(0, 10000), Display(Name = "Stock Level")]
        public int StockLevel { get; set; }

        public Store Store { get; set; }
        public Product Product { get; set; }

    }
}
