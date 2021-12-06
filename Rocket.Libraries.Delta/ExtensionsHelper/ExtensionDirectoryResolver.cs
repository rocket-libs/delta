using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Rocket.Libraries.Delta.ExtensionsHelper
{
    public interface IExtensionHelper
    {
        string GetExtensionExecutablePath(string extensionName);
    }

    public class ExtensionHelper : IExtensionHelper
    {
        private ExtensionPlatformSpecificData PlatformData
        {
            get
            {
                var mappings = new Dictionary<Func<bool>, ExtensionPlatformSpecificData>
                {
                    { () => RuntimeInformation.IsOSPlatform(OSPlatform.Windows), new ExtensionPlatformSpecificData
                    {
                        Directory = "windows",
                        FileExtension = ".exe"
                    }},
                    { () => RuntimeInformation.IsOSPlatform(OSPlatform.Linux), new ExtensionPlatformSpecificData
                    {
                        Directory = "linux",
                        FileExtension = string.Empty
                    }},
                    { () => RuntimeInformation.IsOSPlatform(OSPlatform.OSX), new ExtensionPlatformSpecificData
                    {
                        Directory = "osx",
                        FileExtension = string.Empty
                    }}
                };
                return mappings.Single(mapping => mapping.Key()).Value;
            }
        }

        private string GetExtensionDirectory(string extensionName)
        {
            var extensionDirectory = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "extensions",
                extensionName);
            return extensionDirectory;
        }


        public string GetExtensionExecutablePath(string extensionName)
        {
            var platformData = PlatformData;
            var extensionExecutableDirectory = $"{GetExtensionDirectory(extensionName)}/{platformData.Directory}";
            return $"{extensionExecutableDirectory}/{extensionName}{platformData.FileExtension}";
        }
    }
}