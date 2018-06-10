
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
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
        private void btnLoadSav_Click(object sender, RoutedEventArgs e)
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

            var encSave = File.ReadAllBytes(currentView.PathSelectedFile);
            currentView.Save = new SaveManager(encSave);

            LoadEditable();
        }

        private void LoadEditable()
        {
            // misc

            txtPlayerName.Text = currentView.Save.SerializeData.playerData.name;
            txtTickets.Text = currentView.Save.SerializeData.misc.fsGiftTicketNum.ToString();

            // pokemon

            foreach(var item in currentView.Save.SerializeData.characterStorage.characterDataDictionary)
            {
                var pokeData = item.Value.data;

                string pokeName = TransformPokeName(pokeData.name);
                pokeName = pokeName.Replace("\0", string.Empty);
                pokemonList.Items.Add(pokeName);
            }


        }

        public string TransformPokeName(IList<char> name)
        {
            string pokeName = "";
            for (int i = 0; i < name.Count; i++)
            {
                pokeName = pokeName + name[i];
            }
            return pokeName;
        }

        private void pokemonList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            int selectedPokeIndx = pokemonList.SelectedIndex;

            var allPoke = currentView.Save.SerializeData.characterStorage.characterDataDictionary;

            int counter = 0;
            foreach(var poke in allPoke)
            {
                if(counter >= selectedPokeIndx)
                {
                    txtPokeSpecies.Text = poke.Value.data.monsterNo.ToString();
                    txtPokeName.Text = TransformPokeName(poke.Value.data.name);
                    txtPokeLevel.Text = poke.Value.data.level.ToString();
                    txtPokeExp.Text = poke.Value.data.exp.ToString();
                    txtPokeHP.Text = poke.Value.data.hp.ToString();
                    txtPokeAttack.Text = poke.Value.data.attack.ToString();
                    break;
                }
                counter++;
            }


            
        }


        private void SaveEverything()
        {
            // misc

            currentView.Save.SerializeData.playerData.name = txtPlayerName.Text;
            currentView.Save.SerializeData.misc.fsGiftTicketNum = Convert.ToInt32(txtTickets.Text);


        }

        private void btnExportSav_Click(object sender, RoutedEventArgs e)
        {
            SaveEverything();
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
                File.WriteAllBytes(saveFileDialog.FileName, currentView.Save.Export());

            MessageBox.Show("Successfully exported .sav!");
        }

        private void savePokemonData(object sender, RoutedEventArgs e)
        {
            int selectedPokeIndx = pokemonList.SelectedIndex;

            var allPoke = currentView.Save.SerializeData.characterStorage.characterDataDictionary;

            int counter = 0;
            foreach (var poke in allPoke)
            {
                if (counter >= selectedPokeIndx)
                {
                    currentView.Save.SerializeData.characterStorage.characterDataDictionary[counter].data.attack = Convert.ToInt32(txtPokeAttack.Text);
                    currentView.Save.SerializeData.characterStorage.characterDataDictionary[counter].data.monsterNo = (ushort)Convert.ToInt32(txtPokeSpecies.Text);
                    currentView.Save.SerializeData.characterStorage.characterDataDictionary[counter].data.name = txtPokeName.Text.ToArray();
                    currentView.Save.SerializeData.characterStorage.characterDataDictionary[counter].data.level = (ushort)Convert.ToInt32(txtPokeLevel.Text);
                    currentView.Save.SerializeData.characterStorage.characterDataDictionary[counter].data.exp = (uint)Convert.ToInt32(txtPokeExp.Text);
                    currentView.Save.SerializeData.characterStorage.characterDataDictionary[counter].data.hp = Convert.ToInt32(txtPokeHP.Text);
                    MessageBox.Show("Successfully wrote data!");
                    break;
                }
                counter++;
            }
        }

        
    }
}
