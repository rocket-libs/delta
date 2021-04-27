using System;
using System.Collections.Generic;
using System.IO;
using Rocket.Libraries.Delta.ProjectDefinitions;

namespace Rocket.Libraries.Delta.Running
{
    public interface IProjectValidator
    {
        void FailIfProjectInvalid(ProjectDefinition projectDefinition, Guid projectId);
    }

    public class ProjectValidator : IProjectValidator
    {
        public void FailIfProjectInvalid(ProjectDefinition projectDefinition, Guid projectId)
        {
            var errorConditions = new HashSet<Action>
            {
                () =>
                {
                    if (projectDefinition == default)
                    {
                        throw new Exception ($"Could not find project with id '{projectId}'");
                    }
                },
                () =>
                {
                    if (!File.Exists (projectDefinition.ProjectPath))
                    {
                        throw new Exception ($"Could not find delta project file at path '{projectDefinition.ProjectPath}'");
                    }
                }
            };
        }
    }
}