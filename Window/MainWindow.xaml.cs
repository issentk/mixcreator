using System.Linq;
using System.Windows;
using MixCreator.Model;
using MixCreator.ViewModel;

namespace MixCreator.Window
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {

        private MainViewModel ViewModel { get; set; }

        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            this.DataContext = ViewModel;
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
