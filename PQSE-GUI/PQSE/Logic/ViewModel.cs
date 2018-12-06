using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }
}
