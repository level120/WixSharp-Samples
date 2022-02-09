using System;
using WixSharp.CommonTasks;

namespace WixSharp.Bootstrapper
{
    public sealed class Setup
    {
        public static void Main()
        {
            var productProj =
                new ManagedProject("My Product",
                    new Dir(@"%ProgramFiles%\My Company\My Product",
                        new File("Setup.cs")));

            productProj.InstallScope = InstallScope.perMachine;
            productProj.GUID = new Guid("6f330b47-2577-43ad-9095-1861bb258777");
            productProj.Language = EntryBootstrapperApplication.Languages;

            productProj.OutFileName = $"{productProj.Name}.ml.v{productProj.Version}";

            var msiFile = productProj.BuildMultilanguageMsi();

            //------------------------------------

            var bootstrapper =
                new Bundle("My Product",
                           new PackageGroupRef("NetFx40Web"),
                           new MsiPackage(msiFile)
                           {
                               Id = EntryBootstrapperApplication.MainPackageId,
                               DisplayInternalUI = true,
                               Visible = true,
                               MsiProperties = "TRANSFORMS=[TRANSFORMS]"
                           });

            bootstrapper.SetVersionFromFile(msiFile);
            bootstrapper.UpgradeCode = new Guid("6f330b47-2577-43ad-9095-1861bb25889a");
            bootstrapper.Application = new ManagedBootstrapperApplication("%this%", "BootstrapperCore.config");

            //bootstrapper.PreserveTempFiles = true;
            bootstrapper.SuppressWixMbaPrereqVars = true;

            bootstrapper.Build(msiFile.PathChangeExtension(".exe"));
        }
    }
}