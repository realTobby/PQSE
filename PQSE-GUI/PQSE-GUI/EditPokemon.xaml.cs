using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            for(int i = 1; i < 152; i++)
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
                comboAllPoke.SelectedIndex = pokeResult.monsterNo-1;
            }

            InitPStones();

            pokemonAttack.Text = pokeResult.attack.ToString();
            pokemonHealth.Text = pokeResult.hp.ToString();
            pokemonLevel.Text = pokeResult.level.ToString();
            pokemonName.Text = TransformPokeName(pokeResult.name);
            pokemonExp.Text = pokeResult.exp.ToString();
            //pokemonRR.Text = pokeResult.rareRandom.ToString();
            //pokemonSeikaku.Text = pokeResult.seikaku.ToString();


            // check if pokemon is shiny

            checkIfShiny.IsChecked = CheckShinyStatus(pokeResult.id, pokeResult.rareRandom);


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

            var r1 = p1.Xor(p2);
            var r2 = r1.Xor(p3);
            var r3 = r2.Xor(p4);

            for (var i = 0; i < r3.Length; i++)
            {
                if (r3[i] != targetVal[i])
                    return false;
            }
            return true;

        }

        /// <summary>
        /// THE UGLIEST CODE IVE EVER WRITTEN => SORY FOR EVERYBODY THAT WITTNESSED THIS, THERES NO EXCUSE TO FIND. I WILL CHANGE THIS SOON, BUT FOR NOW IT WORKS. SORRY LMAO
        /// </summary>
        private void InitPStones()
        {
            for (int i = 0; i < 3; i++)
            {
                Image slotImage = new Image();
                slotImage.Source = new BitmapImage(new Uri("icons/pStone/types/" + i + ".png", UriKind.Relative));

                ComboBoxItem newOne = new ComboBoxItem();
                newOne.Content = slotImage;

                pStone0.Items.Add(newOne);
            }
            for (int i = 0; i < 3; i++)
            {
                Image slotImage = new Image();
                slotImage.Source = new BitmapImage(new Uri("icons/pStone/types/" + i + ".png", UriKind.Relative));

                ComboBoxItem newOne = new ComboBoxItem();
                newOne.Content = slotImage;

                pStone1.Items.Add(newOne);
            }
            for (int i = 0; i < 3; i++)
            {
                Image slotImage = new Image();
                slotImage.Source = new BitmapImage(new Uri("icons/pStone/types/" + i + ".png", UriKind.Relative));

                ComboBoxItem newOne = new ComboBoxItem();
                newOne.Content = slotImage;

                pStone2.Items.Add(newOne);
            }
            for (int i = 0; i < 3; i++)
            {
                Image slotImage = new Image();
                slotImage.Source = new BitmapImage(new Uri("icons/pStone/types/" + i + ".png", UriKind.Relative));

                ComboBoxItem newOne = new ComboBoxItem();
                newOne.Content = slotImage;

                pStone3.Items.Add(newOne);
            }
            for (int i = 0; i < 3; i++)
            {
                Image slotImage = new Image();
                slotImage.Source = new BitmapImage(new Uri("icons/pStone/types/" + i + ".png", UriKind.Relative));

                ComboBoxItem newOne = new ComboBoxItem();
                newOne.Content = slotImage;

                pStone4.Items.Add(newOne);
            }
            for (int i = 0; i < 3; i++)
            {
                Image slotImage = new Image();
                slotImage.Source = new BitmapImage(new Uri("icons/pStone/types/" + i + ".png", UriKind.Relative));

                ComboBoxItem newOne = new ComboBoxItem();
                newOne.Content = slotImage;

                pStone5.Items.Add(newOne);
            }
            for (int i = 0; i < 3; i++)
            {
                Image slotImage = new Image();
                slotImage.Source = new BitmapImage(new Uri("icons/pStone/types/" + i + ".png", UriKind.Relative));

                ComboBoxItem newOne = new ComboBoxItem();
                newOne.Content = slotImage;

                pStone6.Items.Add(newOne);
            }
            for (int i = 0; i < 3; i++)
            {
                Image slotImage = new Image();
                slotImage.Source = new BitmapImage(new Uri("icons/pStone/types/" + i + ".png", UriKind.Relative));

                ComboBoxItem newOne = new ComboBoxItem();
                newOne.Content = slotImage;

                pStone7.Items.Add(newOne);
            }
            for (int i = 0; i < 3; i++)
            {
                Image slotImage = new Image();
                slotImage.Source = new BitmapImage(new Uri("icons/pStone/types/" + i + ".png", UriKind.Relative));

                ComboBoxItem newOne = new ComboBoxItem();
                newOne.Content = slotImage;

                pStone8.Items.Add(newOne);
            }

            pStone0.SelectedIndex = pokeResult.potential.slotPropertyTypes[0];
            pStone1.SelectedIndex = pokeResult.potential.slotPropertyTypes[1];
            pStone2.SelectedIndex = pokeResult.potential.slotPropertyTypes[2];
            pStone3.SelectedIndex = pokeResult.potential.slotPropertyTypes[3];
            pStone4.SelectedIndex = pokeResult.potential.slotPropertyTypes[4];
            pStone5.SelectedIndex = pokeResult.potential.slotPropertyTypes[5];
            pStone6.SelectedIndex = pokeResult.potential.slotPropertyTypes[6];
            pStone7.SelectedIndex = pokeResult.potential.slotPropertyTypes[7];
            pStone8.SelectedIndex = pokeResult.potential.slotPropertyTypes[8];
        }

        internal SaveCharacterData GetPokeResult()
        {
            return pokeResult;
        }

        private void SavePStoneTypes()
        {
            pokeResult.potential.slotPropertyTypes[0] = (sbyte)pStone0.SelectedIndex;
            pokeResult.potential.slotPropertyTypes[1] = (sbyte)pStone1.SelectedIndex;
            pokeResult.potential.slotPropertyTypes[2] = (sbyte)pStone2.SelectedIndex;
            pokeResult.potential.slotPropertyTypes[3] = (sbyte)pStone3.SelectedIndex;
            pokeResult.potential.slotPropertyTypes[4] = (sbyte)pStone4.SelectedIndex;
            pokeResult.potential.slotPropertyTypes[5] = (sbyte)pStone5.SelectedIndex;
            pokeResult.potential.slotPropertyTypes[6] = (sbyte)pStone6.SelectedIndex;
            pokeResult.potential.slotPropertyTypes[7] = (sbyte)pStone7.SelectedIndex;
            pokeResult.potential.slotPropertyTypes[8] = (sbyte)pStone8.SelectedIndex;
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
    }
}
