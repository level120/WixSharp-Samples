using System.Windows.Controls;
using Caliburn.Micro;
using WixSharp.UI.WPF;

namespace WixSharp.WPF.Dialogs
{
    /// <summary>
    /// Interaction logic for CustomDialogPanel.xaml
    /// </summary>
    public partial class CustomDialogPanel : UserControl, IWpfDialogContent
    {
        public CustomDialogPanel()
        {
            InitializeComponent();
        }

        public void Init(CustomDialogBase parentDialog)
        {
            var model = new CustomDialogPanelViewModel { ParentDialog = parentDialog };
            ViewModelBinder.Bind(model, /*view*/this, null);

            // insert Validate button on the left from the "Back" button
            var validateButton = new Button
            {
                Content = "Validate",
                MinWidth = 73
            };
            validateButton.Click += (s, e) => model.Validate();

            parentDialog.ButtonsPanel.Children.Insert(0, new Separator { Opacity = 0, Width = 30 });
            parentDialog.ButtonsPanel.Children.Insert(0, validateButton);
        }
    }
}
