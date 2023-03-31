using Rocket.Libraries.FormValidationHelper.Attributes.InBuilt.Strings;

namespace Rocket.Libraries.Delta.RemoteRepository
{
    public class RepositoryDetail
    {
        [StringIsNonNullable("Source Repository Url")]
        public string Url { get; set; }

        [StringIsNonNullable("Source Repository Branch")]
        public string Branch { get; set; }
    }
}