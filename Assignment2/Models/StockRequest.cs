using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Assignment2.Models
{
    public class StockRequest
    {
        public int StockRequestID { get; set; }

        [Required]
        public int StoreID { get; set; }
        [Required]
        public int ProductID { get; set; }
        [Required]
        [Range(1,1000)]
        public int Quantity { get; set; }

        public Store Store { get; set; }
        public Product Product { get; set; }
    }
}
