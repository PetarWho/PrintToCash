using Microsoft.EntityFrameworkCore;
using PrintToCash.AppData;
using PrintToCash.AppData.Entities;
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
    /// Interaction logic for EditProductPage.xaml
    /// </summary>
    public partial class EditProductPage : Page
    {
        private ProductsPage productsPage;
        public EditProductPage(EditProductPageViewModel model, ProductsPage productsPage)
        {
            InitializeComponent();
            this.productsPage = productsPage;
            DataContext = model;
        }

        private void EditToProductsBtn_Click(object sender, RoutedEventArgs e)
        {
            productsPage.LoadProducts();
            productsPage.NavigateBackToProducts();
        }

        #region Validations

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

        private async void UpdateProductToDbBtn_Click(object sender, RoutedEventArgs e)
        {
            var newName = productNameTextBox.Text.Trim();
            var newWeight = 0.0f;
            var newTimeSeconds = 0;
            var newFinalTouchMinutes = 0;
            var newDescription = string.Empty;
            Material material;

            try
            {
                newDescription = new TextRange(productDescriptionRichBox.Document.ContentStart, productDescriptionRichBox.Document.ContentEnd).Text.Trim();
                newWeight = float.Parse(productWeightTextBox.Text.Trim());
                newTimeSeconds = (int)Math.Ceiling(decimal.Parse(productTimeTextBox.Text.Trim()) * 60);
                material = (Material)materialsComboBox.SelectedItem;
                if (decimal.TryParse(productFinalTouchTimeTextBox.Text.Trim(), out decimal a))
                {
                    newFinalTouchMinutes = (int)Math.Ceiling(a);
                }

                if (string.IsNullOrEmpty(newName) || newTimeSeconds < 1 || newWeight < 0.1f || newFinalTouchMinutes < 0)
                {
                    throw new ArgumentException("Empty fields!");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Enter correct values!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (var dbContext = new AppDbContext())
            {
                try
                {
                    var prod = await dbContext.Products.FindAsync(((EditProductPageViewModel)DataContext).Product.Id);

                    if (prod == null) throw new ArgumentException("Product not found.");

                    var config = await dbContext.Configuration.FirstOrDefaultAsync();
                    if (config == null) throw new ArgumentException("No config!");

                    var electricityCost = (((decimal)newTimeSeconds) / 3600) * (config.CurrentCostElectricity * config.PrinterElectricityConsumptionKW);
                    var finalTouchCost = newFinalTouchMinutes * (config.FinalTouchHourlyFee / 60);
                    var materialCost = (material.Price / 1000) * (decimal)newWeight;
                    var total = electricityCost + finalTouchCost + materialCost;

                    prod.Name = newName;
                    prod.Description = newDescription;
                    prod.Grams = newWeight;
                    prod.SecondsNeededToPrint = newTimeSeconds;
                    prod.Price = total + (total * (config.TaxPercentage * 0.01m));

                    await dbContext.SaveChangesAsync();

                    MessageBox.Show("Successfully updated product info.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception)
                {
                    MessageBox.Show("Oops.. Something strange happened!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
