using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;

namespace WixSharp.Bootstrapper
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private bool _installEnabled;
        private bool _uninstallEnabled;
        private bool _isThinking;
        private CultureInfo _selectedLanguage;

        public bool InstallEnabled
        {
            get => _installEnabled;
            set
            {
                _installEnabled = value;
                OnPropertyChanged();
            }
        }

        public bool UninstallEnabled
        {
            get => _uninstallEnabled;
            set
            {
                _uninstallEnabled = value;
                OnPropertyChanged();
            }
        }

        public bool IsBusy
        {
            get => _isThinking;
            set
            {
                _isThinking = value;
                OnPropertyChanged();
            }
        }

        public CultureInfo SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                _selectedLanguage = value;

                if (Bootstrapper != null)
                    Bootstrapper.SelectedLanguage = value;

                OnPropertyChanged();
            }
        }

        public CultureInfo[] SupportedLanguages { get; }
        public EntryBootstrapperApplication Bootstrapper { get; set; }

        public MainWindowViewModel(EntryBootstrapperApplication bootstrapper)
        {
            SelectedLanguage = bootstrapper.SelectedLanguage;
            SupportedLanguages = bootstrapper.SupportedLanguages;

            IsBusy = false;

            Bootstrapper = bootstrapper;
            Bootstrapper.Error += OnError;
            Bootstrapper.ApplyComplete += OnApplyComplete;
            Bootstrapper.DetectPackageComplete += OnDetectPackageComplete;
            Bootstrapper.PlanComplete += OnPlanComplete;

            Bootstrapper.Engine.Detect();
        }

        public void InstallExecute()
        {
            IsBusy = true;

            Bootstrapper.Engine.Plan(LaunchAction.Install);
        }

        public void UninstallExecute()
        {
            IsBusy = true;
            Bootstrapper.Engine.Plan(LaunchAction.Uninstall);
        }

        public void ExitExecute()
        {
            //Dispatcher.BootstrapperDispatcher.InvokeShutdown();
        }

        /// <summary>
        /// Method that gets invoked when the Bootstrapper ApplyComplete event is fired.
        /// This is called after a bundle installation has completed. Make sure we updated the view.
        /// </summary>
        private void OnApplyComplete(object sender, ApplyCompleteEventArgs e)
        {
            IsBusy = false;
            InstallEnabled = false;
            UninstallEnabled = false;
        }

        /// <summary>
        /// Method that gets invoked when the Bootstrapper DetectPackageComplete event is fired.
        /// Checks the PackageId and sets the installation scenario. The PackageId is the ID
        /// specified in one of the package elements (msipackage, exepackage, msppackage,
        /// msupackage) in the WiX bundle.
        /// </summary>
        private void OnDetectPackageComplete(object sender, DetectPackageCompleteEventArgs e)
        {
            if (e.PackageId == "MyProductPackageId")
            {
                if (e.State == PackageState.Absent)
                    InstallEnabled = true;
                else if (e.State == PackageState.Present)
                    UninstallEnabled = true;
            }
        }

        /// <summary>
        /// Method that gets invoked when the Bootstrapper PlanComplete event is fired.
        /// If the planning was successful, it instructs the Bootstrapper Engine to
        /// install the packages.
        /// </summary>
        private void OnPlanComplete(object sender, PlanCompleteEventArgs e)
        {
            if (e.Status >= 0)
                Bootstrapper.Engine.Apply(System.IntPtr.Zero);
        }

        private void OnError(object sender, ErrorEventArgs e)
        {
            MessageBox.Show(e.ErrorMessage);
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}