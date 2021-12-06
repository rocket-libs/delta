using System;
using System.IO;
using System.Linq;
using Rocket.Libraries.Delta.ExtensionsHelper;

namespace Rocket.Libraries.Delta.ProcessRunning
{
    public interface IProcessFilenameResolver
    {
        string ResolveFilename (string filename);
    }

    public class ProcessFilenameResolver : IProcessFilenameResolver
    {
        private readonly IExtensionHelper extensionHelper;

        public ProcessFilenameResolver (
            IExtensionHelper extensionHelper
        )
        {
            this.extensionHelper = extensionHelper;
        }

        public string ResolveFilename (string filename)
        {

            var extensionFilename = extensionHelper.GetExtensionExecutablePath (filename);
            if (File.Exists (extensionFilename))
            {
                return extensionFilename;
            }
            else
            {
                var filenameFromEnvironment = GetFromEnvironment (filename);
                if (!string.IsNullOrEmpty (filenameFromEnvironment))
                {
                    return filenameFromEnvironment;
                }
                else
                {
                    return filename;
                }
            }
        }

        private string GetFromEnvironment (string filename)
        {
            var pathEnvValue = Environment.GetEnvironmentVariable ("path".ToUpper ());
            var paths = pathEnvValue.Split (new char[] { ';', ':' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var candidatePath in paths)
            {
                var cleanedCandidatePath = candidatePath.Trim ();
                if(cleanedCandidatePath.StartsWith("~/"))
                {
                    cleanedCandidatePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + cleanedCandidatePath.Substring(1);
                }
                var candidateFilename = Path.Combine (cleanedCandidatePath, filename);
                if (File.Exists (candidateFilename))
                {
                    return candidateFilename;
                }
            }
            return string.Empty;
        }
    }
}