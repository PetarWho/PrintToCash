using PrintToCash.AppData;
using PrintToCash.AppData.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for MaterialsPage.xaml
    /// </summary>
    public partial class MaterialsPage : Page
    {
        private MainMenuPage mainPage;
        public MaterialsPage(MainMenuPage mainPage)
        {
            InitializeComponent();
            this.mainPage = mainPage;

            using (var dbContext = new AppDbContext())
            {
                ObservableCollection<Material> materials = new ObservableCollection<Material>(dbContext.Materials.ToList());
                MaterialsListView.ItemsSource = materials;
            }
        }

        private void MaterialsToMainMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            mainPage.NavigateBackToMainMenu();
        }

        private void AddMaterialBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
