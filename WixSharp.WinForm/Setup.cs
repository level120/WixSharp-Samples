using System;
using WixSharp.WinForm.Dialogs;

namespace WixSharp.WinForm
{
    public sealed class Setup
    {
        public static void Main()
        {
            var project = new ManagedProject(
                "MyProduct",
                new Dir(@"%ProgramFiles%\My Company\My Product", new File("Setup.cs")))
            {
                GUID = new Guid("6fe30b47-2577-43ad-9095-1861ba25889b"),
                //custom set of standard UI dialogs
                ManagedUI = new ManagedUI()
            };

            project.ManagedUI.InstallDialogs.Add<WelcomeDialog>()
                                            .Add<LicenceDialog>()
                                            .Add<SetupTypeDialog>()
                                            .Add<FeaturesDialog>()
                                            .Add<InstallDirDialog>()
                                            .Add<ProgressDialog>()
                                            .Add<ExitDialog>();

            project.ManagedUI.ModifyDialogs.Add<MaintenanceTypeDialog>()
                                           .Add<FeaturesDialog>()
                                           .Add<ProgressDialog>()
                                           .Add<ExitDialog>();

            //project.SourceBaseDir = "<input dir path>";
            //project.OutDir = "<output dir path>";

            project.BuildMsi();
        }
    }
}