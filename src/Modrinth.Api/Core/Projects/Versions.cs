using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Modrinth.Api.Core.Endpoints;
using Modrinth.Api.Core.System;
using Modrinth.Api.Models.Dto;

namespace Modrinth.Api.Core.Projects
{
    public class Versions
    {
        private readonly ModrinthApi _api;
        private readonly HttpClientFactory _httpClientFactory;

        public Versions(ModrinthApi modrinthApi, HttpClientFactory httpClientFactory)
        {
            _api = modrinthApi;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IEnumerable<Version>> GetDependenciesAsync(Version version, string loaderName, CancellationToken token)
        {
            var dependencyVersions = new List<Version>();

            foreach (var dependency in version.Dependencies)
            {
                Version? dependencyVersion = null;
                if (!string.IsNullOrEmpty(dependency.VersionId))
                {
                    dependencyVersion = await GetVersionById(dependency.VersionId, token);
                }

                if (!string.IsNullOrEmpty(dependency.ProjectId))
                {
                    dependencyVersion = await _api.Mods.GetLastVersionAsync(dependency.ProjectId, loaderName, token);
                }

                if (dependencyVersion != null)
                    dependencyVersions.Add(dependencyVersion);
            }

            return dependencyVersions;
        }

        private async Task<Version?> GetVersionById(string identifier, CancellationToken token)
        {
            var endPoint = ModrinthEndpoints.Version.Replace("{id}", identifier);

            var response = await _httpClientFactory.HttpClient.GetAsync(endPoint, token);

            RequestHelper.UpdateApiRequestInfo(_api, response);

            if (!response.IsSuccessStatusCode)
            {
                return default;
            }

            var content = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<Version>(content) ?? null;
        }
    }
}
