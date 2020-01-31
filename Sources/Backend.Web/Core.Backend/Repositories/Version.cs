using Backend.Web.Core.Backend.Interfaces;

namespace Backend.Web.Core.Backend.Repositories
{
    public class VersionRepository: IVersionRepository
    {
        public string Version { get; } = "1.0.0.0";
    }
}
