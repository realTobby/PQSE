using Microsoft.Win32;

namespace PQSE
{

    public class PQSE_LOGIC
    {
        public ViewModel CurrentView;
        public PQSE_LOGIC()
        {
            CurrentView = new ViewModel();
        }

        public void LoadSave()
        {
            OpenFileDialog chooseSaveFileDialog = new OpenFileDialog();
            chooseSaveFileDialog.Filter = "All Files (*.*)|*.*";
            chooseSaveFileDialog.Multiselect = false;
            string selectedFile = "";
            if (chooseSaveFileDialog.ShowDialog() == true)
            {
                selectedFile = chooseSaveFileDialog.FileName;
            }
            CurrentView.LoadedSave = new SaveManager(System.IO.File.ReadAllBytes(selectedFile));

            CurrentView.PokemonStorage = CurrentView.LoadedSave.SerializeData.characterStorage.characterDataDictionary;

        }

        public void ExportSave()
        {
            if (CurrentView.LoadedSave != null)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                if (saveFileDialog.ShowDialog() == true)
                {
                    if (saveFileDialog.FileName != string.Empty)
                    {
                        System.IO.File.WriteAllBytes(saveFileDialog.FileName, CurrentView.LoadedSave.Export());
                    }

                }
            }
        }




    }
}
