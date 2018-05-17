using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment2.Models
{
    public class Order
    {
        [Required]
        public int OrderID { get; set; }

        [Display(Name = "Order Date"), DataType(DataType.Date)]
        // [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime OrderDate { get; set; }

        //[Required]
        //public String OrderUserID { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }

    }
}
