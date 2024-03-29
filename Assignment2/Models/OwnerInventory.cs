﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment2.Models
{
    public class OwnerInventory
    {
        [Key, ForeignKey("Product"), Display(Name = "Product ID")]
        public int ProductID { get; set; }

        [Required, Range(0, 10000), Display(Name = "Stock Level")]
        public int StockLevel { get; set; }

        public Product Product { get; set; }
    }
}
