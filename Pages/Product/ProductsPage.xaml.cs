using PrintToCash.AppData;
using PrintToCash.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PrintToCash.Pages.Product
{
    /// <summary>
    /// Interaction logic for ProductsPage.xaml
    /// </summary>
    public partial class ProductsPage : Page
    {
        private MainMenuPage mainPage;
        private ObservableCollection<AppData.Entities.Product>? productsObsCollection;

        public ProductsPage(MainMenuPage mainPage)
        {
            InitializeComponent();
            this.mainPage = mainPage;
            LoadProducts();
        }
        public void LoadProducts()
        {
            using (var dbContext = new AppDbContext())
            {
                this.productsObsCollection = new ObservableCollection<AppData.Entities.Product>(dbContext.Products.ToList());
                productsListView.ItemsSource = this.productsObsCollection;
            }
        }

        private void ProductsToMainMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            mainPage.NavigateBackToMainMenu();
        }

        private void AddProductBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new AddProductPage(this);
        }
        public void NavigateBackToProducts()
        {
            Application.Current.MainWindow.Content = this;
        }

        private async void EditProductBtn_Click(object sender, RoutedEventArgs e)
        {
            if (productsListView.SelectedItem is AppData.Entities.Product selectedProduct)
            {
                string name = selectedProduct.Name;

                using (var dbContext = new AppDbContext())
                {
                    var prod = await dbContext.Products.FindAsync(selectedProduct.Id);

                    if (prod != null)
                    {
                        var model = new EditProductPageViewModel();
                        model.Product = new ProductViewModel()
                        {
                            Id = prod.Id,
                            Name = prod.Name,
                            Grams = prod.Grams,
                            Description = prod.Description,
                            MinutesToPrint = prod.SecondsNeededToPrint/60,
                            Price = prod.Price,
                        };
                        var editProductPage = new EditProductPage(model, this);

                        Application.Current.MainWindow.Content = editProductPage;
                    }
                    else
                    {
                        MessageBox.Show("No products in database.", "", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            else
            {
                MessageBox.Show("Select a material.", "", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private async void DeleteProductBtn_Click(object sender, RoutedEventArgs e)
        {
            if (productsListView.SelectedItem is AppData.Entities.Product selectedProduct)
            {
                string name = selectedProduct.Name;

                MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete {name}?", $"Delete {name}", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    using (var dbContext = new AppDbContext())
                    {
                        dbContext.Products.Remove(selectedProduct);
                        await dbContext.SaveChangesAsync();
                        LoadProducts();
                    }
                }
            }
            else
            {
                MessageBox.Show("Select a product.", "Selection error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
