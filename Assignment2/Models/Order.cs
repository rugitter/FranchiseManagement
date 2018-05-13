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
        [Required]
        public int StoreID { get; set; }
        [Required]
        public int ProductID { get; set; }
        [Required, Range(0, 100)]
        public int Quantity { get; set; }

        //[Display(Name = "Total Price"), DataType(DataType.Currency)]
        //public decimal SubTotal {
        //    get {
        //        foreach(ShoppingCart i in ShoppingCarts)
        //        { sub += i.SubPrice; }

        //        return sub;
        //    }
        //}

        // The subtotal price = item's unit price x amout of unit
        public decimal SubPrice
        {
            get
            {
                return Product.UnitPrice * Quantity;
            }
        }

        public Store Store { get; set; }
        public Product Product { get; set; }
    }
}
