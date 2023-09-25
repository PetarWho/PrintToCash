using System;
using System.Collections.Generic;

namespace PrintToCash.AppData.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public string? Note { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public decimal PricePaid { get; set; }
        public int ProductsCount { get; set; }
        public ICollection<ProductOrder> ProductsOrders { get; set; } = new List<ProductOrder>();
    }
}
