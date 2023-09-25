using System;

namespace PrintToCash.AppData.Entities
{
    public class ProductOrder
    {
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
