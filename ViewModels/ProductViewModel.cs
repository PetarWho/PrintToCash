namespace PrintToCash.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public float Grams { get; set; }
        public int MinutesToPrint { get; set; }
    }
}
