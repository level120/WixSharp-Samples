using System.Windows;

namespace WixSharp.Bootstrapper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _viewModel;

        public MainWindow(EntryBootstrapperApplication bootstrapper)
        {
            InitializeComponent();

            DataContext = _viewModel = new MainWindowViewModel(bootstrapper);
        }


        private void Install_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.InstallExecute();
        }

        private void Uninstall_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.UninstallExecute();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
