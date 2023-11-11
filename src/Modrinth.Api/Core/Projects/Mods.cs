using System.Threading;
using System.Threading.Tasks;
using Modrinth.Api.Core.Repository;
using Modrinth.Api.Core.System;
using Modrinth.Api.Models.Projects;

namespace Modrinth.Api.Core.Projects
{
    public class Mods : Projects, IProjectRepository
    {
        private readonly ModrinthApi _api;

        public Mods(ModrinthApi api, HttpClientFactory httpClientFactory) : base(api, httpClientFactory)
        {
            _api = api;
        }

        public new async Task<TProject> FindAsync<TProject>(string identifier, CancellationToken token)
        {
            var project = await base.FindAsync<TProject>(identifier, token);

            if (project is Project modProject)
                modProject.Api = _api;

            return project;
        }
    }
}
