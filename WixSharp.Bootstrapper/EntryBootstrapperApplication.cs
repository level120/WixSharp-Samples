using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using WixSharp.Bootstrapper;

[assembly: BootstrapperApplication(typeof(EntryBootstrapperApplication))]

namespace WixSharp.Bootstrapper
{
    public class EntryBootstrapperApplication : BootstrapperApplication
    {
        protected override void Run()
        {
            new MainWindow(this).ShowDialog();
            Engine.Quit(0);
        }
    }
}