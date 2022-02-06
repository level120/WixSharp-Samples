using System;
using System.Diagnostics;
using System.Windows;
using WixSharp.UI.Forms;
using WixSharp.UI.WPF;

namespace WixSharp.WPF.Dialogs
{
    public class CustomDialogPanelViewModel : Caliburn.Micro.Screen
    {
        private bool _canProceed;

        public ManagedForm Host => ParentDialog?.ManagedFormHost;
        public CustomDialogBase ParentDialog { get; set; }

        public string User => Environment.UserName;

        public bool CanProceedIsChecked
        {
            get => _canProceed;
            set
            {
                _canProceed = value;
                NotifyOfPropertyChange(() => CanProceedIsChecked);

                if (ParentDialog != null)
                    ParentDialog.GoNextButton.IsEnabled = value;
            }
        }

        public void Validate()
        {
            MessageBox.Show("Performing validation...");
        }

        public void ShowReadme()
        {
            Process.Start("https://github.com/oleg-shilo/wixsharp");
        }
    }
}