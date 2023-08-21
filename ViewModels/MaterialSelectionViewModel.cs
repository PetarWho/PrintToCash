using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using PrintToCash.AppData.Entities;

namespace PrintToCash.ViewModels
{
    public class MaterialSelectionViewModel : INotifyPropertyChanged
    {
        private Material _selectedMaterial;

        public ObservableCollection<Material> Materials { get; } = new ObservableCollection<Material>();

        public Material SelectedMaterial
        {
            get => _selectedMaterial;
            set
            {
                _selectedMaterial = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
