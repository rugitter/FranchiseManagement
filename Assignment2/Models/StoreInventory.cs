﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment2.Models
{
    public class StoreInventory
    {
        public int StoreID { get; set; }
        public int ProductID { get; set; }
        public int StockLevel { get; set; }

        public Store Store { get; set; }
        public Product Product { get; set; }
    }
}
