using Microsoft.EntityFrameworkCore;
using PrintToCash.AppData;
using PrintToCash.AppData.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
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
using System.Xml.Linq;

namespace PrintToCash.Pages
{
    /// <summary>
    /// Interaction logic for EditMaterialPage.xaml
    /// </summary>
    public partial class EditMaterialPage : Page
    {
        private MaterialsPage materialsPage;
        public EditMaterialPage(Material material, MaterialsPage materialsPage)
        {
            InitializeComponent();
            DataContext = material;
            this.materialsPage = materialsPage;
        }

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

        private void AddToMaterialsBtn_Click(object sender, RoutedEventArgs e)
        {
            materialsPage.LoadMaterials();
            materialsPage.NavigateBackToMaterials();
        }

        private async void UpdateMaterialToDbBtn_Click(object sender, RoutedEventArgs e)
        {
            var newName = materialNameTextBox.Text.Trim();
            var newPrice = 0.0m;
            try
            {
                newPrice = decimal.Parse(materialPriceTextBox.Text.Trim());

                if (string.IsNullOrEmpty(newName) || string.IsNullOrEmpty(materialPriceTextBox.Text.Trim()) || newPrice <0.1m)
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
                    var mat = (Material)DataContext;
                    var material = await dbContext.Materials.FindAsync(mat.Id);

                    if (material == null)
                    {
                        throw new ArgumentException("Material is null.");
                    }

                    material.Name = newName;
                    material.Price = newPrice;
                    await dbContext.SaveChangesAsync();
                    MessageBox.Show("Successfully updated material info.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception)
                {
                    MessageBox.Show("Oops.. Something strange happened!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
        }
    }
}
