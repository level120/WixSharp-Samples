using System;
using System.Windows.Media.Imaging;
using WixSharp.UI.Forms;
using WixSharp.UI.WPF;

namespace WixSharp.WPF.Dialogs
{
    public class CustomWpfDialogViewModel : Caliburn.Micro.Screen
    {
        private bool _canProceed;

        public ManagedForm Host { get; set; }
        public BitmapImage Banner => Host?.Runtime.Session.GetResourceBitmap("WixUI_Bmp_Banner").ToImageSource();

        public string User { get; set; } = Environment.UserName;
        public bool CanGoNext => CanProceedIsChecked;
        public bool CanProceedIsChecked
        {
            get => _canProceed;
            set
            {
                _canProceed = value;
                NotifyOfPropertyChange(() => CanProceedIsChecked);
                NotifyOfPropertyChange(() => CanGoNext);
            }
        }

        public void GoPrev() => Host?.Shell.GoPrev();

        public void GoNext() => Host?.Shell.GoNext();

        public void Cancel() => Host?.Shell.Cancel();
    }
}