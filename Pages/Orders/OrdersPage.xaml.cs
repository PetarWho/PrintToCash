using Microsoft.EntityFrameworkCore;
using PrintToCash.AppData;
using PrintToCash.AppData.Entities;
using PrintToCash.Pages.Product;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PrintToCash.Pages.Orders
{
    /// <summary>
    /// Interaction logic for OrdersPage.xaml
    /// </summary>
    public partial class OrdersPage : Page
    {
        private MainMenuPage mainPage;
        private ObservableCollection<Order>? ordersObsCollection;

        public OrdersPage(MainMenuPage mainPage)
        {
            InitializeComponent();
            this.mainPage = mainPage;
            LoadOrders();
        }
        public void LoadOrders()
        {
            using (var dbContext = new AppDbContext())
            {
                this.ordersObsCollection = new ObservableCollection<Order>(dbContext.Orders.Include(x=>x.ProductsOrders).ToList());
                ordersListView.ItemsSource = this.ordersObsCollection;
            }
        }

        private void OrdersToMainMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            mainPage.NavigateBackToMainMenu();
        }

        public void NavigateBackToOrders()
        {
            Application.Current.MainWindow.Content = this;
        }

        private void AddOrderBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new AddOrderPage(this);
        }

        private void EditOrderBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void DeleteOrderBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ordersListView.SelectedItems.Count > 0)
            {
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete the selected order(s)?", $"Delete Orders", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    var orders = ordersListView.SelectedItems.OfType<Order>();
                    using (var dbContext = new AppDbContext())
                    {
                        dbContext.Orders.RemoveRange(orders);
                        await dbContext.SaveChangesAsync();
                        LoadOrders();
                    }
                }
            }
            else
            {
                MessageBox.Show("Select an order.", "Selection error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
