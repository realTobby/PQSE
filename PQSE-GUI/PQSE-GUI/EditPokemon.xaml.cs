using System;
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

        public EditPokemon(SaveCharacterData pokemon)
        {
            InitializeComponent();
            pokeResult = pokemon;
            LoadStuff();
        }

        private void LoadStuff()
        {
            // make pokemon available
            for(int i = 1; i < 152; i++)
            {
                Image pokeFace = new Image();
                pokeFace.Source = new BitmapImage(new Uri("icons/pokemon/" + i + ".png", UriKind.Relative));
                pokeFace.Width = 48;
                pokeFace.Height = 48;



                innerPokeData.Children.Add(pokeFace);
            }



        }

        internal SaveCharacterData GetPokeResult()
        {
            return pokeResult;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
