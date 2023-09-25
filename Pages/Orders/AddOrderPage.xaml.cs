using Microsoft.EntityFrameworkCore;
using PrintToCash.AppData;
using PrintToCash.AppData.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace PrintToCash.Pages.Orders
{
    /// <summary>
    /// Interaction logic for AddOrderPage.xaml
    /// </summary>
    public partial class AddOrderPage : Page
    {
        private OrdersPage ordersPage;
        private ObservableCollection<AppData.Entities.Product>? productsObsCollection;
        public AddOrderPage(OrdersPage ordersPage)
        {
            InitializeComponent();
            this.ordersPage = ordersPage;
            LoadProducts();
        }

        public void LoadProducts()
        {
            using (var dbContext = new AppDbContext())
            {
                this.productsObsCollection = new ObservableCollection<AppData.Entities.Product>(dbContext.Products.OrderBy(x=>x.Name).ToList());
                orderProductsListView.ItemsSource = this.productsObsCollection;
            }
        }

        #region Validation
        private void DecimalInputValidation(object sender, TextCompositionEventArgs e)
        {
            // Allow only digits and a single dot (.) character
            if (!IsValidDecimalInput(((TextBox)sender).Text + e.Text))
            {
                e.Handled = true;
            }
        }

        private bool IsValidDecimalInput(string input)
        {
            return decimal.TryParse(input, System.Globalization.NumberStyles.AllowDecimalPoint,
                                    System.Globalization.CultureInfo.InvariantCulture, out _);
        }

        #endregion

        private void AddToOrdersBtn_Click(object sender, RoutedEventArgs e)
        {
            ordersPage.LoadOrders();
            ordersPage.NavigateBackToOrders();
        }

        private void orderProductsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (orderProductsListView.SelectedItems.Count <= 0)
            {
                orderPriceTextBox.Text = "0.00";
                return;
            }

            decimal sum = 0.0m;

            foreach (AppData.Entities.Product product in orderProductsListView.SelectedItems)
            {
                sum += product.Price;
            }

            orderPriceTextBox.Text = sum.ToString();
        }

        private async void AddOrderToDbBtn_Click(object sender, RoutedEventArgs e)
        {
            var note = string.Empty;
            var date = DateTime.Now;
            var price = 0.0m;
            var products = new List<AppData.Entities.Product>();

            try
            {
                if (addOrderCalendar.SelectedDate != null)
                {
                    date = addOrderCalendar.SelectedDate.Value;
                }

                price = decimal.Parse(orderPriceTextBox.Text.Trim());
                note = new TextRange(orderNoteRichBox.Document.ContentStart, orderNoteRichBox.Document.ContentEnd).Text.Trim();

                foreach (var item in orderProductsListView.SelectedItems.OfType<AppData.Entities.Product>())
                {
                    products.Add(item);
                }

                if (price < 0.01m || products.Count == 0)
                {
                    MessageBox.Show("Check selected items and price!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

                using (var dbContext = new AppDbContext())
                {
                    try
                    {
                        var order = new AppData.Entities.Order()
                        {
                            Note = note,
                            Date = date,
                            PricePaid = price,
                            ProductsCount = products.Count,
                        };

                        // Attach the order to the context
                        dbContext.Attach(order);

                        // Load the products from the database
                        foreach (var product in products)
                        {
                            dbContext.Entry(product).State = EntityState.Unchanged;
                        }

                        var prodOrders = products.Select(x => new ProductOrder()
                        {
                            Order = order,
                            Product = x
                        });

                        dbContext.AddRange(prodOrders);

                        // Associate products with the order
                        

                        dbContext.SaveChanges();

                        MessageBox.Show($"Successfully created an order.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        orderPriceTextBox.Text = "";
                        addOrderCalendar.SelectedDate = DateTime.Today;
                        orderProductsListView.SelectedItems.Clear();
                        orderNoteRichBox.Document = new FlowDocument(new Paragraph(new Run("")));
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Oops.. Something strange happened!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }

            catch (Exception)
            {
                MessageBox.Show("Check selected items and price!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
        }
    }
}
