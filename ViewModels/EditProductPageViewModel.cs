using PrintToCash.AppData;
using PrintToCash.AppData.Entities;

namespace PrintToCash.ViewModels
{
    public class EditProductPageViewModel
    {
        public ProductViewModel Product { get; set; }
        public MaterialSelectionViewModel MaterialViewModel { get; set; }

        public EditProductPageViewModel()
        {
            Product = new ProductViewModel();
            MaterialViewModel = new MaterialSelectionViewModel();

            using (var dbContext = new AppDbContext())
            {
                foreach (var material in dbContext.Materials)
                {
                    MaterialViewModel.Materials.Add(material);
                }
            }
        }
    }
}
