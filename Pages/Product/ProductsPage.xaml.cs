using PrintToCash.AppData;
using PrintToCash.ViewModels;
using System;
using System.Collections.Generic;
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
        public ProductsPage(MainMenuPage mainPage)
        {
            InitializeComponent();
            this.mainPage = mainPage;
        }

        private void ProductsToMainMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            mainPage.NavigateBackToMainMenu();
        }

        private void AddProductBtn_Click(object sender, RoutedEventArgs e)
        {

        }
        public void NavigateBackToProducts()
        {
            Application.Current.MainWindow.Content = this;
        }
    }
}
