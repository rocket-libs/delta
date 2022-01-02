using System;
using System.Collections.Immutable;

namespace Rocket.Libraries.Delta.Projects
{
    /// <summary>
    /// Definition of build process.
    /// </summary>
    public class Project
    {
        /// <summary>
        /// Gets or sets a list of commands to be run to perform the build.
        /// These are run synchronously and processing will stop if any one of the commands returns a non zero result.
        /// Commands are ran in the order in which they are entered
        /// </summary>
        public ImmutableList<string> BuildCommands { get; set; }

        /// <summary>
        /// Gets or sets the directory where the results of a successful build are placed.
        /// </summary>
        /// <value></value>
        public string BuildOutputDirectory { get; set; }

        /// <summary>
        /// Gets or sets a unique value that allows delta to identify a build process.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets a user friendly name to allow identification of each build process.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the url to push the built outputs to.
        /// </summary>
        public string PublishUrl { get; set; }

        /// <summary>
        /// Gets or sets the stages of the build process that are disabled.
        /// </summary>
        public ImmutableHashSet<string> DisabledStages { get; set; }

        /// <summary>
        /// Gets or sets the currently active branch.
        /// </summary>
        public string Branch { get; set; }

        /// <summary>
        /// Gets or sets the optional set of commands to be run after a successful build.
        /// These commands are run after all other stages have been completed.
        /// They are run synchronously and processing will stop if any one of the commands returns a non zero result.
        /// </summary>
        public ImmutableList<BuildCommand> OnSuccessPostBuildCommands { get; set; }

        /// <summary>
        /// Gets or sets the optional set of commands to be run after a failing build.
        /// These commands are run after all other stages have been completed.
        /// They are run synchronously and processing will stop if any one of the commands returns a non zero result.
        /// </summary>
        public ImmutableList<BuildCommand> OnFailurePostBuildCommands { get; set; }
    }
}