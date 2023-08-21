using Microsoft.EntityFrameworkCore;
using PrintToCash.AppData;
using PrintToCash.AppData.Entities;
using PrintToCash.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace PrintToCash
{
    /// <summary>
    /// Interaction logic for MaterialsPage.xaml
    /// </summary>
    public partial class MaterialsPage : Page
    {
        private MainMenuPage mainPage;
        private ObservableCollection<Material>? materialsObsCollection;

        public MaterialsPage(MainMenuPage mainPage)
        {
            InitializeComponent();
            this.mainPage = mainPage;
            LoadMaterials();
        }

        public void LoadMaterials()
        {
            using (var dbContext = new AppDbContext())
            {
                this.materialsObsCollection = new ObservableCollection<Material>(dbContext.Materials.ToList());
                MaterialsListView.ItemsSource = this.materialsObsCollection;
            }
        }

        private void MaterialsToMainMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            mainPage.NavigateBackToMainMenu();
        }

        private void AddMaterialBtn_Click(object sender, RoutedEventArgs e)
        {
            var addMaterialsPage = new AddMaterialPage(this);

            Application.Current.MainWindow.Content = addMaterialsPage;
        }

        public void NavigateBackToMaterials()
        {
            Application.Current.MainWindow.Content = this;
        }

        private async void EditMaterialBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MaterialsListView.SelectedItem is Material selectedMaterial)
            {
                string name = selectedMaterial.Name;

                using (var dbContext = new AppDbContext())
                {
                    var mat = await dbContext.Materials.FirstOrDefaultAsync(x => x.Name == name);

                    if (mat != null)
                    {
                        var editMaterialPage = new EditMaterialPage(mat, this);

                        Application.Current.MainWindow.Content = editMaterialPage;
                    }
                    else
                    {
                        MessageBox.Show("No materials in database.", "", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            else
            {
                MessageBox.Show("Select a material.", "", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private async void DeleteMaterialBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MaterialsListView.SelectedItem is Material selectedMaterial)
            {
                string name = selectedMaterial.Name;

                MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete {selectedMaterial.Name}?", $"Delete {selectedMaterial.Name}", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    using (var dbContext = new AppDbContext())
                    {
                        dbContext.Materials.Remove(selectedMaterial);
                        await dbContext.SaveChangesAsync();
                        LoadMaterials();
                    }
                }
            }
            else
            {
                MessageBox.Show("Select a material.", "", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }
    }
}
