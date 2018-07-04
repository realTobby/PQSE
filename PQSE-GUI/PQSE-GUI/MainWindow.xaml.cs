
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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
                currentView.Save = new SaveManager(File.ReadAllBytes(currentView.PathSelectedFile));
                LoadEditable();
            }
        }

        public void LoadPokemon()
        {
            foreach (var poke in currentView.Save.SerializeData.characterStorage.characterDataDictionary)
            {
                Button pokeButton = new Button();
                pokeButton.Tag = poke.Value.data;
                pokeButton.Click += new RoutedEventHandler(EditPoke);

                Image pokeFace = new Image();
                pokeFace.Source = new BitmapImage(new Uri("icons/pokemon/" + poke.Value.data.monsterNo + ".png", UriKind.Relative));
                pokeFace.Width = 48;
                pokeFace.Height = 48;

                pokeButton.Content = pokeFace;
                pokeFacesPanel.Children.Add(pokeButton);
            }

            Button newPokemon = new Button();
            CharacterStorage.ManageData freshpoke = new CharacterStorage.ManageData();
            freshpoke.data = new SaveCharacterData();
            freshpoke.data.attack = 1;
            freshpoke.data.exp = 0;
            freshpoke.data.name = "FreshlyAddedPokemon".ToList();
            freshpoke.data.formNo = 0;
            freshpoke.data.hp = 1;
            freshpoke.data.id = 1;
            freshpoke.data.isEvolve = false;
            freshpoke.data.level = 1;
            freshpoke.data.monsterNo = 1;
            freshpoke.data.rareRandom = 6988666;

            freshpoke.data.potential = new SaveCharacterPoteintialData();
            freshpoke.data.potential.slotPropertyTypes = new List<sbyte>();

            for(int i = 0; i < 9; i++)
            {
                freshpoke.data.potential.slotPropertyTypes.Add(2);
            }




            newPokemon.Tag = freshpoke.data;
            newPokemon.Content = "+";
            newPokemon.FontSize = 36;
            newPokemon.Click += new RoutedEventHandler(addPoke);
            pokeFacesPanel.Children.Add(newPokemon);
        }

        private void addPoke(object sender, RoutedEventArgs e)
        {
            Button pokeFace = sender as Button;
            SaveCharacterData clickedPokemon = (SaveCharacterData)pokeFace.Tag;

            

            int keyPair = currentView.Save.SerializeData.characterStorage.characterDataDictionary.Count() + 1;


            CharacterStorage.ManageData tmpNew = new CharacterStorage.ManageData();
            tmpNew.data = clickedPokemon;

            currentView.Save.SerializeData.characterStorage.characterDataDictionary.Add(keyPair, tmpNew);
            EditPoke(sender, e);
        }

        private void EditPoke(object sender, RoutedEventArgs e)
        {
            Button pokeFace = sender as Button;
            SaveCharacterData clickedPokemon = (SaveCharacterData)pokeFace.Tag;
            var poke = currentView.Save.SerializeData.characterStorage.characterDataDictionary.Where(x => x.Value.data == clickedPokemon).FirstOrDefault();
            //MessageBox.Show("Clicked on: " + TransformPokeName(poke.Value.data.name));
            SaveCharacterData result = null;
            EditPokemon editing = new EditPokemon(poke.Value.data);

            editing.ShowDialog();
            
            if (editing.DialogResult.HasValue && editing.DialogResult.Value)
            {
                result = editing.GetPokeResult();
                currentView.Save.SerializeData.characterStorage.characterDataDictionary.Where(x => x.Value.data == clickedPokemon).FirstOrDefault().Value.data = result;
                pokeFacesPanel.Children.Clear();
                LoadPokemon();
                MessageBox.Show("Successfully wrote data to: " + TransformPokeName(result.name));
            }
        }


        private void ResetAllFields()
        {
            currentView.Save = null;

            pokeFacesPanel.Children.Clear();
            stonePanel.Children.Clear();
        }

        private void LoadStones()
        {
            foreach (var item in currentView.Save.SerializeData.potentialStorage.potentialDatas)
            {
                StoneData stone = item.Value as StoneData;

                string baseLink = "icons/pStone/";

                // get 45th byte and determine if attack or hp
                switch(stone.stoneData[72])
                {
                    default:
                        // if program lands here, the stone is a skill-stone not a passive stone
                        baseLink = "icons/pStone/skill/cyan.png";






                        break;
                    case 2:
                        baseLink = baseLink + "health/bronze.png";
                        break;
                    case 0:
                        baseLink = baseLink + "attack/bronze.png";
                        break;
                }

                Image stoneImg = new Image();
                stoneImg.Source = new BitmapImage(new Uri(baseLink, UriKind.Relative));



                Button stoneTmp = new Button();
                stoneTmp.Content = stoneImg;
                stoneTmp.Tag = stone;
                stoneTmp.Width = 48;
                stoneTmp.Height = 48;
                stoneTmp.Click += new RoutedEventHandler(EditStone);

                if(stone.stoneData.Contains(14) || stone.stoneData.Contains(28) || stone.stoneData.Contains(29) || stone.stoneData.Contains(40) || stone.stoneData.Contains(41) || stone.stoneData.Contains(48) || stone.stoneData.Contains(49))
                    stonePanel.Children.Add(stoneTmp);

            }
        }

        private void EditStone(object sender, RoutedEventArgs e)
        {
            Button stoneButton = sender as Button;
            StoneData stone = (StoneData)stoneButton.Tag;


            // DEBUG
            string info = "";

            foreach (var bite in stone.stoneData)
            {
                info = info + ";" + bite.ToString();
            }

            MessageBox.Show(info);

        }


        private void LoadEditable()
        {
            // stones

            LoadStones();

            // misc

            txtPlayerName.Text = currentView.Save.SerializeData.playerData.name;
            txtTickets.Text = currentView.Save.SerializeData.misc.fsGiftTicketNum.ToString();
            txtBattery.Text = currentView.Save.SerializeData.misc.battery.ToString();
            txtPokeStorage.Text = currentView.Save.SerializeData.characterStorage.dataCapacity.ToString();
            txtStoneStorage.Text = currentView.Save.SerializeData.potentialStorage.dataCapacity.ToString();

            // pokemon

            LoadPokemon();

            // items
            foreach (var item in currentView.Save.SerializeData.itemStorage.datas)
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

        private void SaveEverything()
        {
            // misc

            currentView.Save.SerializeData.playerData.name = txtPlayerName.Text;
            currentView.Save.SerializeData.misc.fsGiftTicketNum = Convert.ToInt32(txtTickets.Text);
            currentView.Save.SerializeData.misc.battery = Convert.ToInt32(txtBattery.Text);

            currentView.Save.SerializeData.characterStorage.dataCapacity = Convert.ToInt32(txtPokeStorage.Text);
            currentView.Save.SerializeData.potentialStorage.dataCapacity = Convert.ToInt32(txtStoneStorage.Text);

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

                if (currentView.Save.SerializeData.itemStorage.datas.Where(x => x.id == item.id).FirstOrDefault() != null)
                    currentView.Save.SerializeData.itemStorage.datas.Where(x => x.id == item.id).FirstOrDefault().num = (short)Convert.ToInt32(tmpVal);
                else
                {
                    ItemStorage.Core addItem = new ItemStorage.Core();
                    addItem.id = item.id;
                    addItem.isNew = true;
                    addItem.num = (short)Convert.ToInt32(tmpVal);
                    currentView.Save.SerializeData.itemStorage.datas.Add(addItem);
                }
            }                   
        }

            private void btnExportSav_Click(object sender, RoutedEventArgs e)
        {
            if(currentView.Save != null)
            {
                SaveEverything();
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                if (saveFileDialog.ShowDialog() == true)
                {
                    if(saveFileDialog.FileName != string.Empty)
                    {
                        File.WriteAllBytes(saveFileDialog.FileName, currentView.Save.Export());
                        MessageBox.Show("Successfully exported .sav!");
                    }
                    
                }
            }
            
        }

        private void savePokemonData(object sender, RoutedEventArgs e)
        {
            //int selectedPokeIndx = pokemonList.SelectedIndex;

            //var allPoke = currentView.Save.SerializeData.characterStorage.characterDataDictionary;

            //int counter = 0;
            //foreach (var poke in allPoke)
            //{
            //    if (counter >= selectedPokeIndx)
            //    {
            //        currentView.Save.SerializeData.characterStorage.characterDataDictionary[counter].data.attack = Convert.ToInt32(txtPokeAttack.Text);
            //        currentView.Save.SerializeData.characterStorage.characterDataDictionary[counter].data.monsterNo = (ushort)Convert.ToInt32(txtPokeSpecies.Text);
            //        currentView.Save.SerializeData.characterStorage.characterDataDictionary[counter].data.name = txtPokeName.Text.ToArray();
            //        currentView.Save.SerializeData.characterStorage.characterDataDictionary[counter].data.level = (ushort)Convert.ToInt32(txtPokeLevel.Text);
            //        currentView.Save.SerializeData.characterStorage.characterDataDictionary[counter].data.exp = (uint)Convert.ToInt32(txtPokeExp.Text);
            //        currentView.Save.SerializeData.characterStorage.characterDataDictionary[counter].data.hp = Convert.ToInt32(txtPokeHP.Text);

            //        currentView.Save.SerializeData.characterStorage.characterDataDictionary[counter].data.potential.slotPropertyTypes[0] = (sbyte)pStone0.SelectedIndex;
            //        currentView.Save.SerializeData.characterStorage.characterDataDictionary[counter].data.potential.slotPropertyTypes[1] = (sbyte)pStone1.SelectedIndex;
            //        currentView.Save.SerializeData.characterStorage.characterDataDictionary[counter].data.potential.slotPropertyTypes[2] = (sbyte)pStone2.SelectedIndex;
            //        currentView.Save.SerializeData.characterStorage.characterDataDictionary[counter].data.potential.slotPropertyTypes[3] = (sbyte)pStone3.SelectedIndex;
            //        currentView.Save.SerializeData.characterStorage.characterDataDictionary[counter].data.potential.slotPropertyTypes[4] = (sbyte)pStone4.SelectedIndex;
            //        currentView.Save.SerializeData.characterStorage.characterDataDictionary[counter].data.potential.slotPropertyTypes[5] = (sbyte)pStone5.SelectedIndex;
            //        currentView.Save.SerializeData.characterStorage.characterDataDictionary[counter].data.potential.slotPropertyTypes[6] = (sbyte)pStone6.SelectedIndex;
            //        currentView.Save.SerializeData.characterStorage.characterDataDictionary[counter].data.potential.slotPropertyTypes[7] = (sbyte)pStone7.SelectedIndex;
            //        currentView.Save.SerializeData.characterStorage.characterDataDictionary[counter].data.potential.slotPropertyTypes[8] = (sbyte)pStone8.SelectedIndex;

            //        MessageBox.Show("Successfully wrote data!");
            //        break;
            //    }
            //    counter++;
            //}
        }

        private void checkUnlockPSlots_Checked(object sender, RoutedEventArgs e)
        {
            //if(checkUnlockPSlots.IsChecked == true)
            //{
            //    int selectedPokeIndx = pokemonList.SelectedIndex;

            //    var allPoke = currentView.Save.SerializeData.characterStorage.characterDataDictionary;

            //    int counter = 0;
            //    foreach (var poke in allPoke)
            //    {
            //        if (counter >= selectedPokeIndx)
            //        {
            //            for (int i = 0; i < 9; i++)
            //            {
            //                currentView.Save.SerializeData.characterStorage.characterDataDictionary[counter].data.potential.slotPropertyTypes[i] = 2;
            //            }
            //            //currentView.Save.SerializeData.characterStorage.characterDataDictionary[counter].data.potential.nextSlotProgress = 5;
            //            currentView.Save.SerializeData.characterStorage.characterDataDictionary[counter].data.potential.nextActivateSlotIndex = 8;
            //            currentView.Save.SerializeData.characterStorage.characterDataDictionary[counter].data.potential.activeSlots = 255;
            //            break;
            //        }
            //        counter++;
            //    }
            //}
        }
        private void makeAllPotsIdle_click(object sender, RoutedEventArgs e)
        {
            IList<SaveCookingPot> potList = currentView.Save.SerializeData.visitCharacter.cookingPotList;
            int tmpindx = 0;
            foreach(SaveCookingPot item in currentView.Save.SerializeData.visitCharacter.cookingPotList)
            {
                SaveCookingPot editItem = item;

                editItem.state = CookingPotState.Finish;
                editItem.recipeID = 1;
                editItem.cookingProgress = 12;
                editItem.cookTime = 11;

                potList[tmpindx] = editItem;
                tmpindx++;

            }
        }
    }
}
