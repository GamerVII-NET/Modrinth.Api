using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Modrinth.Api.Models.Dto.Entities;

namespace Modrinth.Api.Models.Dto
{
    public class Version
    {
        [JsonPropertyName("name")] public string Name { get; set; }

        [JsonPropertyName("version_number")] public string VersionNumber { get; set; }

        [JsonPropertyName("changelog")] public string Changelog { get; set; }

        [JsonPropertyName("dependencies")] public List<Dependency> Dependencies { get; set; }

        [JsonPropertyName("game_versions")] public List<string> GameVersions { get; set; }

        [JsonPropertyName("version_type")] public string VersionType { get; set; }

        [JsonPropertyName("loaders")] public List<string> Loaders { get; set; }

        [JsonPropertyName("featured")] public bool Featured { get; set; }

        [JsonPropertyName("status")] public string Status { get; set; }

        [JsonPropertyName("requested_status")] public string RequestedStatus { get; set; }

        [JsonPropertyName("id")] public string Id { get; set; }

        [JsonPropertyName("project_id")] public string ProjectId { get; set; }

        [JsonPropertyName("author_id")] public string AuthorId { get; set; }

        [JsonPropertyName("date_published")] public DateTimeOffset DatePublished { get; set; }

        [JsonPropertyName("downloads")] public int Downloads { get; set; }

        [JsonPropertyName("changelog_url")] public object ChangelogUrl { get; set; }

        [JsonPropertyName("files")] public List<File> Files { get; set; }
        public ModrinthApi Api { get; set; }

        public Task<IEnumerable<Version>> GetDependenciesUrlsAsync(string loaderName, CancellationToken token)
        {
            return Api.Versions.GetDependenciesAsync(this, loaderName, token);
        }

        public Task<IEnumerable<Version>> GetRecursiveDependenciesUrlsAsync(string loaderName, CancellationToken token)
        {
            return GetRecursiveDependenciesUrlsAsync(this, loaderName, token);
        }

        public async Task<IEnumerable<Version>> GetRecursiveDependenciesUrlsAsync(Version version, string loaderName, CancellationToken token)
        {
            var versions = (await Api.Versions.GetDependenciesAsync(version, loaderName, token)).ToList();

            var childVersions = versions.ToList();

            foreach (var dependencyVersion in childVersions)
            {
                var dependencies = await GetRecursiveDependenciesUrlsAsync(dependencyVersion, loaderName, token);


                versions.AddRange(dependencies);
            }

            return versions;
        }
    }
}
