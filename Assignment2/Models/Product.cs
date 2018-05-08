using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment2.Models
{
    public class Product
    {
        public int ProductID { get; set; }

        [Required]
        [Display(Name = "Product Name")]
        [StringLength(50)]
        public string Name { get; set; }

    }
}
