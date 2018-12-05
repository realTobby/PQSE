using PQSE;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace PQSE_GUI
{
    /// <summary>
    /// Interaktionslogik für EditPokemon.xaml
    /// </summary>
    public partial class EditPokemon : Window
    {
        private SaveCharacterData pokeResult;
        private const uint shinyId = 3872211232;
        private const uint shinyRareRandom = 1602156701;
        private ComboBox[] pStones;
        /// <summary>
        /// Use global variable to prevent imgages from gc
        /// </summary>
        private Image[] slotImages=new Image[3];

        public EditPokemon(SaveCharacterData pokemon)
        {
            InitializeComponent();
            pokeResult = pokemon;
            LoadStuff();
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
        private void LoadStuff()
        {

            // make pokemon available
            for (int i = 1; i < 152; i++)
            {
                StackPanel comboItem = new StackPanel();
                Image pokeFace = new Image();
                pokeFace.Source = new BitmapImage(new Uri("icons/pokemon/" + i + ".png", UriKind.Relative));
                pokeFace.Width = 32;
                pokeFace.Height = 32;
                Label pokeNumber = new Label();
                pokeNumber.Content = "#" + i;
                comboItem.Orientation = Orientation.Horizontal;
                comboItem.Children.Add(pokeNumber);
                comboItem.Children.Add(pokeFace);
                comboAllPoke.Items.Add(comboItem);
                comboAllPoke.SelectedIndex = pokeResult.monsterNo - 1;
            }

            // bingo stuff here

            //BingoTest();

            InitPStones();

            pokemonAttack.Text = pokeResult.attack.ToString();
            pokemonHealth.Text = pokeResult.hp.ToString();
            pokemonLevel.Text = pokeResult.level.ToString();
            pokemonName.Text = TransformPokeName(pokeResult.name);
            pokemonExp.Text = pokeResult.exp.ToString();
            //pokemonRR.Text = pokeResult.rareRandom.ToString();
            //pokemonSeikaku.Text = pokeResult.seikaku.ToString();


            //InitAttackList();

            // check if pokemon is shiny
            checkIfShiny.IsChecked = CheckShinyStatus(pokeResult.id, pokeResult.rareRandom);


        }

        private void InitAttackList()
        {
            foreach (var item in pokeResult.potential.potentialSkill)
            {
                attacksOnThisPoke.Items.Add(item.skillID);
            }

        }

        private void BingoTest()
        {
            // Bingo bonus is pretty simple...just need to trial and error all the different statBonuses.
            // after gathering all ids make a enum inside the SaveTypes.cs and maybe even PullRequest it to TheAlexBarneys Repository (maybe helpful?)
            MessageBox.Show("This pokemon got the following bingo property: " + pokeResult.potential.bingoPropertyIndices);
        }

        /// <summary>
        /// HNUGE THANKS TO shearx! Provided this function, Maths, Explaination! Complete god.
        /// </summary>
        /// <param name="inputId"></param>
        /// <param name="inputrr"></param>
        /// <returns></returns>
        public bool CheckShinyStatus(uint inputId, uint inputrr)
        {
            string id = Convert.ToString(inputId, 2).PadLeft(32, '0');
            string rr = Convert.ToString(inputrr, 2).PadLeft(32, '0');

            string target = Convert.ToString(14, 2).PadLeft(16, '0');
            BitArray targetVal = new BitArray(target.Select(s => s == '1').ToArray());

            BitArray p1 = new BitArray(id.Substring(0, 16).Select(s => s == '1').ToArray());
            BitArray p2 = new BitArray(id.Substring(16, 16).Select(s => s == '1').ToArray());
            BitArray p3 = new BitArray(rr.Substring(0, 16).Select(s => s == '1').ToArray());
            BitArray p4 = new BitArray(rr.Substring(16, 16).Select(s => s == '1').ToArray());

            BitArray r1 = p1.Xor(p2);
            BitArray r2 = r1.Xor(p3);
            BitArray r3 = r2.Xor(p4);

            var result = getIntFromBitArray(r3);

            if (result < 16)
                return true;
            else
                return false;
        }

        private static long getIntFromBitArray(BitArray bitArray)
        {
            var str = "";

            for (int i = 0; i < bitArray.Count; i++)
            {
                if (bitArray[i])
                    str += 1;
                else
                    str += 0;
            }

            char[] MyChar = { '0' };
            string newStr = str.TrimStart(MyChar);

            return Convert.ToInt64(newStr, 2);
        }

        /// <summary>
        /// THE UGLIEST CODE IVE EVER WRITTEN => SORY FOR EVERYBODY THAT WITTNESSED THIS, THERES NO EXCUSE TO FIND. I WILL CHANGE THIS SOON, BUT FOR NOW IT WORKS. SORRY LMAO
        /// </summary>
        private void InitPStones()
        {
            pStones = new ComboBox[9] { pStone0, pStone1, pStone2, pStone3, pStone4, pStone5, pStone6, pStone7, pStone8 };

            for (int j = 0; j < 3; j++)
            {
                slotImages[j] = new Image();
                Image slotImage = slotImages[j];
                slotImage.Source = new BitmapImage(new Uri("icons/pStone/slot/" + j + ".png", UriKind.Relative));

                for (int i = 0; i < pStones.Length; i++)
                {
                    var pStone = pStones[i];
                    
                    ComboBoxItem newOne = new ComboBoxItem();
                    newOne.Content = slotImage;
                    //newOne.Content = "string";
                    pStone.Items.Add(newOne);
                }
            }

            for (int i = 0; i < pStones.Length; i++)
            {
                var pStone = pStones[i];
                pStone.SelectedIndex = pokeResult.potential.slotPropertyTypes[i];
            }
        }

        internal SaveCharacterData GetPokeResult()
        {
            return pokeResult;
        }

        private void SavePStoneTypes()
        {
            for (int i = 0; i < pStones.Length; i++)
            {
                pokeResult.potential.slotPropertyTypes[i] = (sbyte)pStones[i].SelectedIndex;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            pokeResult.attack = Convert.ToInt32(pokemonAttack.Text);
            pokeResult.exp = (uint)Convert.ToInt32(pokemonExp.Text);
            //pokeResult.formNo
            pokeResult.hp = Convert.ToInt32(pokemonHealth.Text);
            //pokeResult.id
            //pokeResult.isEvolve
            pokeResult.level = (ushort)Convert.ToInt32(pokemonLevel.Text);
            pokeResult.monsterNo = (ushort)Convert.ToInt32(comboAllPoke.SelectedIndex + 1);
            pokeResult.name = pokemonName.Text.ToList();
            SavePStoneTypes();
            //pokeResult.rareRandom = (uint)Convert.ToInt32(pokemonRR.Text);
            //pokeResult.seikaku = (byte)Convert.ToInt32(pokemonSeikaku.Text);
            //pokeResult.trainingSkillCount
            this.DialogResult = true;
        }

        private void setToShiny_Click(object sender, RoutedEventArgs e)
        {
            Random rnd = new Random();
            bool foundShinyVals = false;
            uint shinyValRR = 0;

            while (foundShinyVals == false)
            {
                uint randRR = (uint)rnd.Next(int.MaxValue);
                if (CheckShinyStatus(pokeResult.id, randRR) == true)
                {
                    shinyValRR = randRR;
                    foundShinyVals = true;
                    //MessageBox.Show("Found shiny value: " + System.Environment.NewLine + "id: " + randId + System.Environment.NewLine + "rareRandom: " + randRR);
                }
            }
            pokeResult.rareRandom = shinyValRR;
            LoadStuff();
        }

        private void pStone0_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
