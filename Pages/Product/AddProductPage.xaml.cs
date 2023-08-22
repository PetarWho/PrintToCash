using Microsoft.EntityFrameworkCore;
using PrintToCash.AppData;
using PrintToCash.AppData.Entities;
using PrintToCash.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace PrintToCash.Pages.Product
{
    /// <summary>
    /// Interaction logic for AddProductPage.xaml
    /// </summary>
    public partial class AddProductPage : Page
    {
        private ProductsPage productsPage;
        public AddProductPage(ProductsPage productsPage)
        {
            InitializeComponent();
            this.productsPage = productsPage;

            DataContext = new MaterialSelectionViewModel();
            using (var dbContext = new AppDbContext())
            {
                foreach (var material in dbContext.Materials)
                {
                    ((MaterialSelectionViewModel)DataContext).Materials.Add(material);
                }
            }
        }

        private void AddToProductsBtn_Click(object sender, RoutedEventArgs e)
        {
            productsPage.LoadProducts();
            productsPage.NavigateBackToProducts();
        }

        private async void AddProductToDbBtn_Click(object sender, RoutedEventArgs e)
        {
            var name = "";
            var grams = 0.0f;
            var seconds = 0;
            var description = string.Empty;
            var finalTouchMinutes = 0;

            try
            {
                name = productNameTextBox.Text.Trim();
                grams = float.Parse(productWeightTextBox.Text.Trim());
                seconds = (int)Math.Ceiling(decimal.Parse(productTimeTextBox.Text.Trim()) * 60);
                finalTouchMinutes = (int)Math.Ceiling(decimal.Parse(productFinalTouchTimeTextBox.Text.Trim()));
                description = new TextRange(productDescriptionRichBox.Document.ContentStart, productDescriptionRichBox.Document.ContentEnd).Text.Trim();
                var material = (Material)materialsComboBox.SelectedItem;

                if (string.IsNullOrEmpty(name) || grams < 0.1f || seconds < 1 || finalTouchMinutes < 0)
                {
                    throw new ArgumentException("Name, grams or time empty!");
                }

                using (var dbContext = new AppDbContext())
                {
                    try
                    {
                        var config = await dbContext.Configuration.FirstOrDefaultAsync();
                        if (config == null) throw new ArgumentException("No config!");

                        var electricityCost = (((decimal)seconds) / 3600) * (config.CurrentCostElectricity * config.PrinterElectricityConsumptionKW); ;
                        var finalTouchCost = finalTouchMinutes * (config.FinalTouchHourlyFee / 60);
                        var materialCost = (material.Price/1000) * (decimal)grams;
                        var total = electricityCost + finalTouchCost + materialCost;

                        var product = new AppData.Entities.Product()
                        {
                            Name = name,
                            Grams = grams,
                            SecondsNeededToPrint = seconds,
                            Description = description,
                            Price = total + (total * (config.TaxPercentage * 0.01m))
                        };

                        await dbContext.AddAsync<AppData.Entities.Product>(product);
                        await dbContext.SaveChangesAsync();
                        MessageBox.Show($"Successfully added {name}.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        productNameTextBox.Text = "";
                        productTimeTextBox.Text = "";
                        productWeightTextBox.Text = "";
                        productDescriptionRichBox.Document = new FlowDocument(new Paragraph(new Run("")));
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Oops.. Something strange happened!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Enter correct values!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
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


    }
}
