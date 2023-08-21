using PrintToCash.AppData;
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
    /// Interaction logic for MainMenuPage.xaml
    /// </summary>
    public partial class MainMenuPage : Page
    {
        public MainMenuPage()
        {
            InitializeComponent();
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        public void NavigateBackToMainMenu()
        {
            Application.Current.MainWindow.Content = this;
        }

        private void ConfigBtn_Click(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new AppDbContext())
            {
                var configuration = dbContext.Configuration.FirstOrDefault();

                if (configuration != null)
                {
                    var configurationPage = new ConfigurationPage(configuration, this);

                    Application.Current.MainWindow.Content = configurationPage;
                }
                else
                {
                    MessageBox.Show("No configuration found in the database.");
                }
            }
        }
    }
}
