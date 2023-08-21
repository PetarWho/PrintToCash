using System;
using System.Collections.Generic;

namespace PrintToCash.AppData.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public string? Note { get; set; }
        public DateTime Date { get; set;} = DateTime.Now;
        public decimal PricePaid { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
