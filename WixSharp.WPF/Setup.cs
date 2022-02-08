using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using WixSharp.Common;
using WixSharp.Common.Serialization;
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
                .Select(GetMappingItem)
                .OfType<WixObject>()
                .ToArray();

            var project = new ManagedProject(name: specification.InstallerName, items: mappingItems)
            {
                GUID = specification.GUID,
                UpgradeCode = specification.GUID,
                ManagedUI = new ManagedUI()
            };

            // Use external library
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

            // if need wxs file
            //project.PreserveTempFiles = true;

            /*
            project.BeforeInstall += args =>
            {
                if (args.IsInstalling)
                {
                }
            };

            project.AfterInstall += args =>
            {
                if (specification.WindowsService != null)
                {
                    var serviceInfo = specification.WindowsService;
                    var serviceFilePath = System.IO.Path.Combine(args.InstallDir, serviceInfo.ServiceFile);

                    Tasks.InstallService(serviceFilePath, serviceInfo.IsInstalling, serviceInfo.ServiceArguments);
                    Tasks.StartService(serviceInfo.ServiceName, throwOnError: false);
                }
            };
            */

            project
                .Mapping(p => p.LicenceFile, specification.LicenseFilePath)
                .Mapping(p => p.ManagedUI.Icon, specification.IconPath)
                .Mapping(p => p.BannerImage, specification.BannerImagePath)
                .Mapping(p => p.BackgroundImage, specification.BackgroundImagePath)
                .MappingShortcuts(specification.Shortcuts)
                .BuildMsi();
        }

        private static Dir GetMappingItem(Item item)
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

            childItems.AddRange(item.MappingItems.Select(GetMappingItem));

            return new Dir(item.Destination, childItems.ToArray());
        }

        private static bool Filter(string filename, string excludeType)
        {
            if (string.IsNullOrEmpty(excludeType))
                return true;

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

        private static ManagedProject MappingShortcuts(
            this ManagedProject project, IEnumerable<Common.Shortcut> shortcuts)
        {
            foreach (var shortcut in shortcuts)
            {
                var file = project.FindFirstFile(shortcut.FileName);

                file.Shortcuts = shortcut.Items
                    .Select(item => new FileShortcut(item.ShortcutName, item.ShortcutPath))
                    .ToArray();
            }

            return project;
        }
    }
}