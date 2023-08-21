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
    }
}
