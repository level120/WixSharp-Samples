using Caliburn.Micro;
using WixSharp.UI.WPF;

namespace WixSharp.WPF.Dialogs
{
    /// <summary>
    /// Interaction logic for CustomWpfDialog.xaml
    /// </summary>
    public partial class CustomWpfDialog : WpfDialog, IWpfDialog
    {
        public CustomWpfDialog()
        {
            InitializeComponent();
        }

        public void Init()
        {
            ViewModelBinder.Bind(new CustomWpfDialogViewModel { Host = ManagedFormHost }, this, null);
        }
    }
}
