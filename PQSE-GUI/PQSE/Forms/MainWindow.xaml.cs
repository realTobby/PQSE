using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;

namespace PQSE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnLoadFile_Click(object sender, RoutedEventArgs e)
        {
            //ResetAllFields();
            OpenFileDialog chooseSaveFileDialog = new OpenFileDialog();
            chooseSaveFileDialog.Filter = "All Files (*.*)|*.*";
            chooseSaveFileDialog.Multiselect = false;
            string selectedFile = "";
            if (chooseSaveFileDialog.ShowDialog() == true)
            {
                selectedFile = chooseSaveFileDialog.FileName;
            }
            SaveManager loadedSave = new SaveManager(File.ReadAllBytes(selectedFile));
        }
    }
}
