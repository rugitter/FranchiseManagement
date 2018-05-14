using System.ComponentModel.DataAnnotations;

namespace Assignment2.Models
{
    // Represent a single item in Shopping Cart
    public class ShoppingCart
    {
        [Required]
        public int StoreID { get; set; }
        [Required]
        public int ProductID { get; set; }
        [Required, Range(1, 100)]
        public int Quantity { get; set; }

        // The subtotal price = item's unit price x amout of unit
        //public decimal SubPrice {
        //    get {
        //        return Product.UnitPrice * Quantity;
        //    }
        //}

        public Store Store { get; set; }
        public Product Product { get; set; }
    }
}
