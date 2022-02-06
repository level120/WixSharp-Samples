using System;
using WixSharp.UI.Forms;
using WixSharp.WPF.Dialogs;

namespace WixSharp.WPF
{
    public sealed class Setup
    {
        public static void Main()
        {
            var project = new ManagedProject(
                "MyProduct",
                new Dir(@"%ProgramFiles%\My Company\My Product", new File("Setup.cs")))
            {
                GUID = new Guid("6f330b47-2577-43ad-9095-1861ba25889b"),
                ManagedUI = new ManagedUI()
            };

            // system.windows.forms
            project.ManagedUI.InstallDialogs
                .Add<WelcomeDialog>()       // stock WinForm dialog
                .Add<FeaturesDialog>()      // stock WinForm dialog
                .Add<CustomDialogWith<CustomDialogPanel>>() // custom WPF dialog (minimalistic)
                .Add<CustomWpfDialogRaw>() // custom WPF dialog
                .Add<CustomWpfDialog>()    // custom WPF dialog (with Claiburn.Micro as MVVM)
                .Add<ProgressDialog>()      // stock WinForm dialog
                .Add<ExitDialog>();         // stock WinForm dialog

            project.ManagedUI.ModifyDialogs
                .Add<ProgressDialog>()
                .Add<ExitDialog>();

            //project.PreserveTempFiles = true;
            //project.SourceBaseDir = @"..\..\";

            project.BuildMsi();
        }
    }
}