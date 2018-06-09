
using Microsoft.Win32;
using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
            var decSave = Encryption.DecryptSave(encSave);


            currentView.Save = new SaveManager(encSave);

            foreach(var pokeData in currentView.Save.SerializeData.characterStorage.characterDataDictionary)
            {
                var pokemon = pokeData.Value.data;

                string name = "";
                for(int i = 0; i < pokemon.name.Count; i++)
                {
                    name = name + pokemon.name[i];
                }
                pokemons.Items.Add(name);
            }

            File.WriteAllBytes("latestSave.sav", currentView.Save.Export());

            //currentView.CurrentByteArray = decSave;
            //File.WriteAllBytes("latestDecryptedSave", currentView.CurrentByteArray);

            //List<string> hexVals = Encryption.ByteArrayToHex(decSave);
            //string convertedHex = "";
            //for (int i = 0; i < hexVals.Count; i++)
            //{
            //    convertedHex = convertedHex + hexVals[i];
            //}
            //currentView.HexDump = convertedHex;
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            var decSave = currentView.CurrentByteArray;
            File.WriteAllBytes("latestEncryptedSave", Encryption.EncryptSave(decSave));
        }

        private void pokemons_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            int combIndx = pokemons.SelectedIndex;

        }

    }
}
