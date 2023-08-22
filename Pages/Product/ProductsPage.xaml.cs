using PrintToCash.AppData;
using PrintToCash.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

        private void EditProductBtn_Click(object sender, RoutedEventArgs e)
        {

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
                MessageBox.Show("Select a material.", "", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
