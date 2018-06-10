
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
            ResetAllFields();
            OpenFileDialog chooseSaveFileDialog = new OpenFileDialog();
            chooseSaveFileDialog.Filter = "All Files (*.*)|*.*";
            chooseSaveFileDialog.Multiselect = false;
            string selectedFile = "";
            currentView.ShowingPath = selectedFile;
            if (chooseSaveFileDialog.ShowDialog() == true)
            {
                selectedFile = chooseSaveFileDialog.FileName;
            }
            
            if (selectedFile != string.Empty)
            {
                currentView.PathSelectedFile = selectedFile;
                var encSave = File.ReadAllBytes(currentView.PathSelectedFile);
                currentView.Save = new SaveManager(encSave);

                LoadEditable();
            }
        }

        private void ResetAllFields()
        {
            currentView.Save = null;
            txtBlueCommon.Clear();
            txtBlueUncommon.Clear();
            txtGreyCommon.Clear();
            txtGreyUncommon.Clear();
            txtLegend.Clear();
            txtPlayerName.Clear();
            txtPokeAttack.Clear();
            txtPokeExp.Clear();
            txtPokeHP.Clear();
            txtPokeLevel.Clear();
            txtPokeName.Clear();
            txtPokeSpecies.Clear();
            txtRare.Clear();
            txtRedCommon.Clear();
            txtRedUncommon.Clear();
            txtTickets.Clear();
            txtYellowCommon.Clear();
            txtYellowUncommon.Clear();
            pokemonList.Items.Clear();
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

            // items
            foreach(var item in currentView.Save.SerializeData.itemStorage.datas)
            {
                string tmpVal = item.num.ToString();

                switch(item.id)
                {
                    case Item.BlueCommon:
                        txtBlueCommon.Text = tmpVal;
                        break;
                    case Item.BlueUnCommon:
                        txtBlueUncommon.Text = tmpVal;
                        break;
                    case Item.GreyCommon:
                        txtGreyCommon.Text = tmpVal;
                        break;
                    case Item.GreyUnCommon:
                        txtGreyUncommon.Text = tmpVal;
                        break;
                    case Item.Legend:
                        txtLegend.Text = tmpVal;
                        break;
                    case Item.Rare:
                        txtRare.Text = tmpVal;
                        break;
                    case Item.RedCommon:
                        txtRedCommon.Text = tmpVal;
                        break;
                    case Item.RedUnCommon:
                        txtRedUncommon.Text = tmpVal;
                        break;
                    case Item.YellowCommon:
                        txtYellowCommon.Text = tmpVal;
                        break;
                    case Item.YellowUnCommon:
                        txtYellowUncommon.Text = tmpVal;
                        break;
                }
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
            checkUnlockPSlots.IsChecked = false;
            int selectedPokeIndx = pokemonList.SelectedIndex;
            var allPoke = currentView.Save.SerializeData.characterStorage.characterDataDictionary;

            int counter = 0;
            foreach(var poke in allPoke)
            {
                if(counter >= selectedPokeIndx)
                {

                    // general
                    txtPokeSpecies.Text = poke.Value.data.monsterNo.ToString();
                    txtPokeName.Text = TransformPokeName(poke.Value.data.name);
                    txtPokeLevel.Text = poke.Value.data.level.ToString();
                    txtPokeExp.Text = poke.Value.data.exp.ToString();
                    txtPokeHP.Text = poke.Value.data.hp.ToString();
                    txtPokeAttack.Text = poke.Value.data.attack.ToString();


                    // pstones

                    pStone0.SelectedIndex = poke.Value.data.potential.slotPropertyTypes[0];
                    pStone1.SelectedIndex = poke.Value.data.potential.slotPropertyTypes[1];
                    pStone2.SelectedIndex = poke.Value.data.potential.slotPropertyTypes[2];
                    pStone3.SelectedIndex = poke.Value.data.potential.slotPropertyTypes[3];
                    pStone4.SelectedIndex = poke.Value.data.potential.slotPropertyTypes[4];
                    pStone5.SelectedIndex = poke.Value.data.potential.slotPropertyTypes[5];
                    pStone6.SelectedIndex = poke.Value.data.potential.slotPropertyTypes[6];
                    pStone7.SelectedIndex = poke.Value.data.potential.slotPropertyTypes[7];
                    pStone8.SelectedIndex = poke.Value.data.potential.slotPropertyTypes[8];





                    string iconpath = "icons/pokemon/" + poke.Value.data.monsterNo.ToString() + ".png" ;
                    pokeIcon.Source = new BitmapImage(new Uri(iconpath, UriKind.Relative));

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

            // items
            var itemStorageTMP = currentView.Save.SerializeData.itemStorage.datas;
            foreach (var item in itemStorageTMP)
            {
                string tmpVal = "";

                switch (item.id)
                {
                    case Item.BlueCommon:
                        tmpVal = txtBlueCommon.Text;
                        break;
                    case Item.BlueUnCommon:
                        tmpVal = txtBlueUncommon.Text;
                        break;
                    case Item.GreyCommon:
                        tmpVal = txtGreyCommon.Text;
                        break;
                    case Item.GreyUnCommon:
                        tmpVal = txtGreyUncommon.Text;
                        break;
                    case Item.Legend:
                        tmpVal = txtLegend.Text;
                        break;
                    case Item.Rare:
                        tmpVal = txtRare.Text;
                        break;
                    case Item.RedCommon:
                        tmpVal = txtRedCommon.Text;
                        break;
                    case Item.RedUnCommon:
                        tmpVal = txtRedUncommon.Text;
                        break;
                    case Item.YellowCommon:
                        tmpVal = txtYellowCommon.Text;
                        break;
                    case Item.YellowUnCommon:
                        tmpVal = txtYellowUncommon.Text;
                        break;
                }
                currentView.Save.SerializeData.itemStorage.datas.Where(x => x.id == item.id).FirstOrDefault().num = (short)Convert.ToInt32(tmpVal);
            }                   
        }

        private void btnExportSav_Click(object sender, RoutedEventArgs e)
        {
            if(currentView.Save != null)
            {
                SaveEverything();
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                if (saveFileDialog.ShowDialog() == true)
                    File.WriteAllBytes(saveFileDialog.FileName, currentView.Save.Export());

                MessageBox.Show("Successfully exported .sav!");
            }
            
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

                    currentView.Save.SerializeData.characterStorage.characterDataDictionary[counter].data.potential.slotPropertyTypes[0] = (sbyte)pStone0.SelectedIndex;
                    currentView.Save.SerializeData.characterStorage.characterDataDictionary[counter].data.potential.slotPropertyTypes[1] = (sbyte)pStone1.SelectedIndex;
                    currentView.Save.SerializeData.characterStorage.characterDataDictionary[counter].data.potential.slotPropertyTypes[2] = (sbyte)pStone2.SelectedIndex;
                    currentView.Save.SerializeData.characterStorage.characterDataDictionary[counter].data.potential.slotPropertyTypes[3] = (sbyte)pStone3.SelectedIndex;
                    currentView.Save.SerializeData.characterStorage.characterDataDictionary[counter].data.potential.slotPropertyTypes[4] = (sbyte)pStone4.SelectedIndex;
                    currentView.Save.SerializeData.characterStorage.characterDataDictionary[counter].data.potential.slotPropertyTypes[5] = (sbyte)pStone5.SelectedIndex;
                    currentView.Save.SerializeData.characterStorage.characterDataDictionary[counter].data.potential.slotPropertyTypes[6] = (sbyte)pStone6.SelectedIndex;
                    currentView.Save.SerializeData.characterStorage.characterDataDictionary[counter].data.potential.slotPropertyTypes[7] = (sbyte)pStone7.SelectedIndex;
                    currentView.Save.SerializeData.characterStorage.characterDataDictionary[counter].data.potential.slotPropertyTypes[8] = (sbyte)pStone8.SelectedIndex;

                    MessageBox.Show("Successfully wrote data!");
                    break;
                }
                counter++;
            }
        }

        private void checkUnlockPSlots_Checked(object sender, RoutedEventArgs e)
        {
            if(checkUnlockPSlots.IsChecked == true)
            {
                int selectedPokeIndx = pokemonList.SelectedIndex;

                var allPoke = currentView.Save.SerializeData.characterStorage.characterDataDictionary;

                int counter = 0;
                foreach (var poke in allPoke)
                {
                    if (counter >= selectedPokeIndx)
                    {
                        for (int i = 0; i < 9; i++)
                        {
                            currentView.Save.SerializeData.characterStorage.characterDataDictionary[counter].data.potential.slotPropertyTypes[i] = 2;
                        }
                        //currentView.Save.SerializeData.characterStorage.characterDataDictionary[counter].data.potential.nextSlotProgress = 5;
                        currentView.Save.SerializeData.characterStorage.characterDataDictionary[counter].data.potential.nextActivateSlotIndex = 8;
                        currentView.Save.SerializeData.characterStorage.characterDataDictionary[counter].data.potential.activeSlots = 255;
                        break;
                    }
                    counter++;
                }
            }
        }
    }
}
