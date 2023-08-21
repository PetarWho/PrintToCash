using Microsoft.EntityFrameworkCore;
using PrintToCash.AppData;
using PrintToCash.AppData.Entities;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PrintToCash.Pages
{
    /// <summary>
    /// Interaction logic for AddMaterialPage.xaml
    /// </summary>
    public partial class AddMaterialPage : Page
    {
        private MaterialsPage materialsPage;
        public AddMaterialPage(MaterialsPage materialsPage)
        {
            InitializeComponent();
            this.materialsPage = materialsPage;
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

        private async void AddMaterialToDbBtn_Click(object sender, RoutedEventArgs e)
        {
            string name = "";
            decimal price = 0.0m;
            try
            {
                name = materialNameTextBox.Text.Trim();
                var priceStr = materialPriceTextBox.Text.Trim();
                price = decimal.Parse(priceStr);
                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(priceStr) || price < 0.1m)
                {
                    throw new ArgumentException("Name/Price empty!");
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
                    if (await dbContext.Materials.AnyAsync(x => x.Name == name))
                    {
                        MessageBox.Show($"There is already {name} in the list.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    var material = new Material()
                    {
                        Name = name,
                        Price = price
                    };

                    await dbContext.AddAsync<Material>(material);
                    await dbContext.SaveChangesAsync();
                    MessageBox.Show($"Successfully added {name}.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    materialNameTextBox.Text = "";
                    materialPriceTextBox.Text = "0.00";
                }
                catch (Exception)
                {
                    MessageBox.Show("Oops.. Something strange happened!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void AddToMaterialsBtn_Click(object sender, RoutedEventArgs e)
        {
            materialsPage.LoadMaterials();
            materialsPage.NavigateBackToMaterials();
        }
    }
}
