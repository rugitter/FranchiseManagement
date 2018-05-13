using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment2.Models
{
    public class Product
    {
        public int ProductID { get; set; }

        [Required, StringLength(50), Display(Name = "Product Name")]
        public string Name { get; set; }

        [Required, DataType(DataType.Currency), Display(Name = "Unit Price")]
        [Range(0.01, 1000.00, ErrorMessage = "Unit Price must be between 0.01 and 1000.00")]
        public decimal UnitPrice { get; set; }
    }
}
