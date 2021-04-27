using System;
using System.IO;
using System.Text.Json;

namespace Rocket.Libraries.Delta.Projects
{
    public interface IProjectReader
    {
        Project GetByPath(string projectPath);
    }

    public class ProjectReader : IProjectReader
    {
        public Project GetByPath(string projectPath)
        {
            if (!File.Exists(projectPath))
            {
                throw new Exception($"Could not find a project at path '{projectPath}'");
            }
            using (var fileStream = new FileStream(projectPath, FileMode.Open))
            {
                using (var streamReader = new StreamReader(fileStream))
                {
                    return JsonSerializer.Deserialize<Project>(streamReader.ReadToEnd());
                }
            }
        }
    }
}