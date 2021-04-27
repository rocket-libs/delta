using System;
using System.Collections.Immutable;
using System.IO;
using System.Text.Json;
using System.Threading;

namespace Rocket.Libraries.Delta.ProjectDefinitions
{
    public abstract class ProjectDefinitionStoreAccessor
    {
        protected static object Locker => new object ();
        protected static SemaphoreSlim Semaphore = new SemaphoreSlim (1, 2);
        public string ProjectsDefinitionStoreFile
        {
            get
            {
                lock (Locker)
                {
                    var filename = $"./project-definitions/projects-definitions.json";
                    var directory = Path.GetDirectoryName (filename);
                    if (!Directory.Exists (directory))
                    {
                        Directory.CreateDirectory (directory);
                    }
                    if (!File.Exists (filename))
                    {
                        File.WriteAllText (filename, JsonSerializer.Serialize (ImmutableList<ProjectDefinition>.Empty));
                    }
                    return filename;
                }
            }
        }

        
    }
}