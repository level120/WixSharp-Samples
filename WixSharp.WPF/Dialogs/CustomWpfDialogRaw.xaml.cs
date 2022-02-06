using System;
using System.Windows;
using WixSharp.UI.WPF;

namespace WixSharp.WPF.Dialogs
{
    /// <summary>
    /// Interaction logic for CustomWpfDialogRaw.xaml
    /// </summary>
    public partial class CustomWpfDialogRaw : WpfDialog, IWpfDialog
    {
        public string User { get; set; } = Environment.UserName;

        public CustomWpfDialogRaw()
        {
            InitializeComponent();

            DataContext = this;
            CanProceedIsChecked_Click(null, null);
        }
        public void Init()
        {
            Banner.Source = ManagedFormHost?.Runtime.Session.GetResourceBitmap("WixUI_Bmp_Banner").ToImageSource();
        }

        private void GoPrev_Click(object sender, RoutedEventArgs e)
        {
            Host?.Shell.GoPrev();
        }

        private void GoNext_Click(object sender, RoutedEventArgs e)
        {
            Host?.Shell.GoNext();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Host?.Shell.Cancel();
        }

        private void CanProceedIsChecked_Click(object sender, RoutedEventArgs e)
        {
            GoNext.IsEnabled = CanProceedIsChecked.IsChecked == true;
        }
    }
}
