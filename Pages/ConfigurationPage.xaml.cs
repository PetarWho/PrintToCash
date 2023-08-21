using PrintToCash.AppData;
using PrintToCash.AppData.Entities;
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

namespace PrintToCash
{
    /// <summary>
    /// Interaction logic for ConfigurationPage.xaml
    /// </summary>
    public partial class ConfigurationPage : Page
    {
        private MainMenuPage mainPage;
        public ConfigurationPage(Config configuration, MainMenuPage mainPage)
        {
            InitializeComponent();
            DataContext = configuration;
            this.mainPage = mainPage;
        }

        private void ConfigToMainMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            mainPage.NavigateBackToMainMenu();
        }

        private async void UpdateConfigBtn_Click(object sender, RoutedEventArgs e)
        {
            var elPrice = ElectricityTextBox.Text;
            var finalTouchFee = FinalTouchFeeTextBox.Text;

            using (var dbContext = new AppDbContext())
            {
                var configuration = dbContext.Configuration.FirstOrDefault();

                if (configuration != null)
                {
                    try
                    {
                        configuration.CurrentCostElectricity = decimal.Parse(elPrice);
                        configuration.FinalTouchHourlyFee = decimal.Parse(finalTouchFee);
                        await dbContext.SaveChangesAsync();
                        MessageBox.Show("Successfully updated prices.", "Success",MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Oops.. Something strange happened!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("No configuration found in the database.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
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
    }
}
