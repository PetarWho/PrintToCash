using System.ComponentModel.DataAnnotations;

namespace PrintToCash.AppData.Entities
{
    public class Material
    {
        public int Id { get; set; }

        [MinLength(1)]
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
    }
}
