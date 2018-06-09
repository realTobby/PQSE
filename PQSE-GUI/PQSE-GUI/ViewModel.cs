using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PQSE_GUI
{
    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string pathSelectedFile;
        public string PathSelectedFile
        {
            get { return pathSelectedFile; }
            set { pathSelectedFile = value; OnPropertyChanged(nameof(PathSelectedFile)); }
        }

        private string hexDump;
        public string HexDump
        {
            get { return hexDump; }
            set { hexDump = value; OnPropertyChanged(nameof(HexDump)); }
        }

        private byte[] currentByteArray;
        public byte[] CurrentByteArray
        {
            get { return currentByteArray; }
            set { currentByteArray = value; OnPropertyChanged(nameof(CurrentByteArray)); }
        }
        private SaveManager save;
        public SaveManager Save
        {
            get { return save; }
            set { save = value; OnPropertyChanged(nameof(Save)); }
        }

    }
}
