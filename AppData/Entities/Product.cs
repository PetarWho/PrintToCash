using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrintToCash.AppData.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public float Grams { get; set; }
        public int SecondsNeededToPrint { get; set; }
        public int FinalTouchMinutes { get; set; } = 0;
        public Material Material { get; set; } = null!;

        [ForeignKey(nameof(Material))]
        public int MaterialId { get; set; }
        public ICollection<ProductOrder> ProductsOrders { get; set; } = new List<ProductOrder>();
    }
}
