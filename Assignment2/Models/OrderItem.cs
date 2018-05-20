using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment2.Models
{
    public class OrderItem
    {
        [Required]
        public int OrderID { get; set; }
        [Required]
        public int StoreID { get; set; }
        [Required]
        public int ProductID { get; set; }
        [Required, Range(0, 100)]
        public int Quantity { get; set; }

        public Order Order { get; set; }
        public Store Store { get; set; }
        public Product Product { get; set; }
    }
}
