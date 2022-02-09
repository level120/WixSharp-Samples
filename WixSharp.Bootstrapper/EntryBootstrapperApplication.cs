using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Threading;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using WixSharp.Bootstrapper;

[assembly: BootstrapperApplication(typeof(EntryBootstrapperApplication))]

namespace WixSharp.Bootstrapper
{
    public class EntryBootstrapperApplication : BootstrapperApplication
    {
        public static string MainPackageId = "MyProductPackageId";
        public static string Languages = "en-US,ko-KR,ja-JP";
        public CultureInfo SelectedLanguage { get; set; }
        public CultureInfo[] SupportedLanguages => Languages.Split(',')
            .Select(x => new CultureInfo(x))
            .ToArray();

        public EntryBootstrapperApplication()
        {
            SelectedLanguage = SupportedLanguages.FirstOrDefault();
            ApplyComplete += (s, e) =>
            {
                Engine.Quit(0);
            };
        }

        protected override void Run()
        {
            var launchAction = Detect();

            if (launchAction == LaunchAction.Install)
            {
                var view = new MainWindow(this);
                var result = view.ShowDialog();

                if (result == true)
                {
                    var defaultLanguage = SelectedLanguage.LCID == SupportedLanguages.FirstOrDefault()?.LCID;

                    if (!defaultLanguage)
                        Engine.StringVariables["TRANSFORMS"] = $":{SelectedLanguage.LCID}";

                    Engine.Plan(launchAction);
                    Engine.Apply(new WindowInteropHelper(view).Handle);

                    Dispatcher.CurrentDispatcher.VerifyAccess();
                    Dispatcher.Run();
                }
            }
            else
            {
                // You can also show a small form with selection of the next action "Modify/Repair" vs "Uninstall"
                if (MessageBox.Show("Do you want to uninstall?", "My Product", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    Engine.Plan(launchAction);
                    Engine.Apply(IntPtr.Zero);

                    Dispatcher.CurrentDispatcher.VerifyAccess();
                    Dispatcher.Run();
                }
            }

            Engine.Quit(0);
        }

        private LaunchAction Detect()
        {
            var done = new AutoResetEvent(false);

            var launchAction = LaunchAction.Unknown;

            DetectPackageComplete += (sender, e) =>
            {
                if (e.PackageId == MainPackageId)
                {
                    if (e.State == PackageState.Absent)
                        launchAction = LaunchAction.Install;
                    else if (e.State == PackageState.Present)
                        launchAction = LaunchAction.Uninstall;

                    done.Set();
                }
            };

            Engine.Detect();

            done.WaitOne();

            return launchAction;
        }
    }
}