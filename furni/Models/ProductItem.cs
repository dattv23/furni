using System;
namespace furni.Models
{
    public class ProductItem
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string Name { get; set; }
        public string TotalPrice { get; set; }
        public string ImageUrl { get; set; } // Url Image product 
    }

}

