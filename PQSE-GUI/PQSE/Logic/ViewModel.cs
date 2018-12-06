using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroFormatter;

namespace PQSE
{
    public class ViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private SaveManager loadedSave = null;
        public SaveManager LoadedSave
        {
            get
            {
                return loadedSave;
            }
            set
            {
                loadedSave = value;
                OnPropertyChanged(nameof(ViewModel.LoadedSave));
            }
        }

        private ILazyDictionary<int, CharacterStorage.ManageData> pokemonStorage = null;
        public ILazyDictionary<int, CharacterStorage.ManageData> PokemonStorage
        {
            get
            {
                return pokemonStorage;
            }
            set
            {
                pokemonStorage = value;
                OnPropertyChanged(nameof(ViewModel.PokemonStorage));
            }
        }

    }
}
