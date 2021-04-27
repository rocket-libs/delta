using System.IO;
using Rocket.Libraries.Delta.Projects;

namespace Rocket.Libraries.Delta.Running
{
    public interface IOutputsCopier
    {
        void CopyOutputs(string projectPath, Project project);
    }

    public class OutputsCopier : IOutputsCopier
    {
        public void CopyOutputs (string projectPath, Project project)
        {
            var outputsDirectory = $"{Path.GetDirectoryName(projectPath)}/{project.BuildOutputDirectory}";
            var stagingDirectory = $"./staging-directory/{project.Label}/";
            CopyAll (new DirectoryInfo (outputsDirectory), new DirectoryInfo (stagingDirectory));
        }
        private void CopyAll (DirectoryInfo source, DirectoryInfo target)
        {
            if (source.FullName.ToLower () == target.FullName.ToLower ())
            {
                return;
            }

            if (Directory.Exists (target.FullName) == false)
            {
                Directory.CreateDirectory (target.FullName);
            }

            // Copy each file into it's new directory.
            foreach (FileInfo fi in source.GetFiles ())
            {
                fi.CopyTo (Path.Combine (target.ToString (), fi.Name), true);
            }

            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories ())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory (diSourceSubDir.Name);
                CopyAll (diSourceSubDir, nextTargetSubDir);
            }
        }
    }
}