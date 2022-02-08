using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
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

            var mappingItems = specification.MappingItems
                .Select(GenerateMappingItem)
                .OfType<WixObject>()
                .ToArray();

            var project = new ManagedProject(name: specification.InstallerName, items: mappingItems)
            {
                GUID = specification.GUID,
                UpgradeCode = specification.GUID,
                ManagedUI = new ManagedUI()
            };

            project.DefaultRefAssemblies.Add(@"bin\Debug\WixSharp.Common.dll");

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

            // todo: Exception 발생 이유 확인
            /*
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
            */

            project
                .Mapping(p => p.LicenceFile, specification.LicenseFilePath)
                .Mapping(p => p.ManagedUI.Icon, specification.IconPath)
                .Mapping(p => p.BannerImage, specification.BannerImagePath)
                .Mapping(p => p.BackgroundImage, specification.BackgroundImagePath)
                .SetLocalize(specification.SupportLanguages)
                .BuildMsi();
        }

        private static Dir GenerateMappingItem(Item item)
        {
            var paths = item.Source.Split('/', '\\');
            var filePath = paths.Last();
            var containsMasking = filePath.Contains("*");

            var childItems = new List<WixEntity>();

            if (containsMasking)
            {
                childItems.Add(new Files(
                    item.Source,
                    filename => Filter(filename, item.ExcludeType)));
            }
            else
            {
                childItems.Add(new File(item.Source));
            }

            childItems.AddRange(item.MappingItems.Select(GenerateMappingItem));

            return new Dir(item.Destination, childItems.ToArray());
        }

        private static bool Filter(string filename, string excludeType)
        {
            if (string.IsNullOrEmpty(excludeType))
                return false;

            return !excludeType.Split('|').All(filename.EndsWith);
        }

        private static ManagedProject Mapping(
            this ManagedProject project, Expression<Func<ManagedProject, string>> selector, string value)
        {
            if (!string.IsNullOrEmpty(value) && System.IO.File.Exists(value))
            {
                if (selector.Body is MemberExpression expression)
                {
                    // When property
                    if (expression.Member is PropertyInfo propertyInfo)
                    {
                        propertyInfo.SetValue(project, value);
                    }
                    // When field
                    else if (expression.Member is FieldInfo fieldInfo)
                    {
                        fieldInfo.SetValue(project, value);
                    }
                }
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