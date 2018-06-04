
using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace PQSE_GUI
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ViewModel currentView;
        public MainWindow()
        {
            InitializeComponent();
            currentView = new ViewModel();
            this.DataContext = currentView;
        }

        private void btn_selectLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog chooseSaveFileDialog = new OpenFileDialog();
            chooseSaveFileDialog.Filter = "All Files (*.*)|*.*";
            chooseSaveFileDialog.Multiselect = false;
            string selectedFile = "";
            if (chooseSaveFileDialog.ShowDialog() == true)
            { 
                selectedFile = chooseSaveFileDialog.FileName;
            }
            currentView.PathSelectedFile = selectedFile;
        }

        private void Save()
        {
            // just here to keep it
            //var decSave = File.ReadAllBytes(txtBox_selectedSave.Text);
            //File.WriteAllBytes(txtBox_saveloc.Text + "\\encryptedSave", EncryptSave(decSave));
        }

        private void btn_read_Click(object sender, RoutedEventArgs e)
        {
            var encSave = File.ReadAllBytes(currentView.PathSelectedFile);
            var read = Crypto.DecryptSave(encSave);

            currentView.CurrentByteArray = read;
            File.WriteAllBytes("latestDecryptedSave", currentView.CurrentByteArray);

            List<string> hexVals = Crypto.ByteArrayToHex(read);
            string convertedHex = "";
            for (int i = 0; i < hexVals.Count; i++)
            {
                convertedHex = convertedHex + hexVals[i];
            }
            currentView.HexDump = convertedHex;
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            var decSave = currentView.CurrentByteArray;
            File.WriteAllBytes("latestEncryptedSave", Crypto.EncryptSave(decSave));
        }
    }
}
