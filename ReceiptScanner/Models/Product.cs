using System;
namespace ReceiptScanner.Models
{
    public class Product
    {
        public string Name { get; set; }
        public bool Domestic { get; set; }
        public double Price { get; set; }
        public double Weight { get; set; }
        public string Description { get; set; }
    }
}

