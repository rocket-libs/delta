using System;
using System.Collections.Immutable;
using System.Linq;

namespace Rocket.Libraries.Delta.ProjectRegistrar
{
    public interface IProjectDefinitionsReader
    {
        ProjectDefinition GetById(Guid projectId);
        ImmutableList<ProjectDefinition> GetProjectDefinitions();
    }

    public class ProjectDefinitionsReader : IProjectDefinitionsReader
    {
        public ImmutableList<ProjectDefinition> GetProjectDefinitions()
        {
            return ImmutableList<ProjectDefinition>.Empty.Add(new ProjectDefinition
            {
                ProjectId = default,
                ProjectPath = "/home/nyingi/work/code/rocket/dotnet/delta/.delta-file"
            });
        }

        public ProjectDefinition GetById(Guid projectId)
        {
            return GetProjectDefinitions().SingleOrDefault(project => project.ProjectId == projectId);
        }
    }
}