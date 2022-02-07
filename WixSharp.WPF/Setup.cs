using System;
using System.Collections.Generic;
using WixSharp.Common;
using WixSharp.Common.Serialization;
using WixSharp.CommonTasks;
using WixSharp.UI.Forms;

namespace WixSharp.WPF
{
    public static class Setup
    {
        public static void Main()
        {
            var specification = JsonSerialization.DeserializeFrom<Specification>("specification.json");

            if (specification == null)
                throw new NullReferenceException("Specification data is null");

            var project = new ManagedProject(name: specification.InstallerName)
            {
                GUID = specification.GUID,
                UpgradeCode = specification.GUID,
                ManagedUI = new ManagedUI()
            };

            project.ManagedUI.InstallDialogs
                .Add<WelcomeDialog>()
                .Add<LicenceDialog>()
                .Add<SetupTypeDialog>()
                .Add<FeaturesDialog>()
                .Add<InstallDirDialog>()
                .Add<ProgressDialog>()
                .Add<ExitDialog>();

            project.ManagedUI.ModifyDialogs
                .Add<MaintenanceTypeDialog>()
                .Add<FeaturesDialog>()
                .Add<ProgressDialog>()
                .Add<ExitDialog>();

            //project.PreserveTempFiles = true;
            //project.SourceBaseDir = @"..\..\";

            project.BeforeInstall += args =>
            {
                if (specification.UseService)
                    Tasks.StopService(specification.ServiceName, throwOnError: false);
            };

            project.AfterInstall += args =>
            {
                if (specification.UseService)
                    Tasks.StartService(specification.ServiceName, throwOnError: false);
            };

            project
                .SetLicense(specification.LicenseFilePath)
                .SetIcon(specification.IconPath)
                .SetBannerImage(specification.BannerImagePath)
                .SetBackgroundImage(specification.BackgroundImagePath)
                .SetLocalize(specification.SupportLanguages)
                .BuildMsi();
        }

        private static ManagedProject SetLicense(this ManagedProject project, string licensePath)
        {
            if (!string.IsNullOrEmpty(licensePath) && System.IO.File.Exists(licensePath))
            {
                project.LicenceFile = licensePath;
            }

            return project;
        }

        private static ManagedProject SetIcon(this ManagedProject project, string iconPath)
        {
            if (!string.IsNullOrEmpty(iconPath) && System.IO.File.Exists(iconPath))
            {
                project.ManagedUI.Icon = iconPath;
            }

            return project;
        }

        private static ManagedProject SetBannerImage(this ManagedProject project, string bannerImagePath)
        {
            if (!string.IsNullOrEmpty(bannerImagePath) && System.IO.File.Exists(bannerImagePath))
            {
                project.BannerImage = bannerImagePath;
            }

            return project;
        }

        private static ManagedProject SetBackgroundImage(this ManagedProject project, string backgroundImagePath)
        {
            if (!string.IsNullOrEmpty(backgroundImagePath) && System.IO.File.Exists(backgroundImagePath))
            {
                project.BackgroundImage = backgroundImagePath;
            }

            return project;
        }

        private static ManagedProject SetLocalize(this ManagedProject project, IEnumerable<Language> supportedLanguages)
        {
            foreach (var language in supportedLanguages)
            {
                project.AddBinary(new Binary(new Id(language.WixId), language.WxlFilePath));
            }

            // todo: test 코드 제거
            // project.UIInitialized += args =>
            // {
            //     var runtime = args.ManagedUI.Shell.MsiRuntime();
            //     runtime.UIText.InitFromWxl(args.Session.ReadBinary("ko_kr"));
            // };

            return project;
        }
    }
}