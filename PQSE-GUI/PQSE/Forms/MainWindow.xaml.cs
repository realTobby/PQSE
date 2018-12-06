using System.Windows;

namespace PQSE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public PQSE_LOGIC PQSE;
        public MainWindow()
        {
            PQSE = new PQSE_LOGIC();
            DataContext = PQSE.CurrentView;
            InitializeComponent();
        }

        private void btnLoadFile_Click(object sender, RoutedEventArgs e)
        {
            PQSE.LoadSave();
        }
    }
}
