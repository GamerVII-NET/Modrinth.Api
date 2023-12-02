using System.Collections;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Modrinth.Api.Core.Repository;
using Modrinth.Api.Core.System;
using Modrinth.Api.Models.Dto;
using Modrinth.Api.Models.Projects;

namespace Modrinth.Api.Core.Projects
{
    public class Mods : Projects, IProjectRepository
    {
        private readonly ModrinthApi _api;
        private readonly FileLoader _fileLoader;

        public Mods(ModrinthApi api, HttpClientFactory httpClientFactory) : base(api, httpClientFactory)
        {
            _api = api;
            _fileLoader = new FileLoader();
        }

        public new async Task<TProject> FindAsync<TProject>(string identifier, CancellationToken token)
        {
            var project = await base.FindAsync<TProject>(identifier, token);

            if (project is Project modProject)
                modProject.Api = _api;

            return project;
        }

        public Task DownloadAsync(string path, Version version, string loaderName, bool loadDependencies, CancellationToken token)
        {
            return DownloadAsync(new DirectoryInfo(path), version, loaderName, loadDependencies, token);
        }

        public async Task DownloadAsync(DirectoryInfo directory, Version version, string loaderName, bool loadDependencies, CancellationToken token)
        {
            var filesUrls = version.Files.Select(c => c.Url).ToList();

            if (loadDependencies)
            {
                var dependenciesVersions = await version.GetRecursiveDependenciesUrlsAsync(loaderName, token);

                var dependencyFiles = dependenciesVersions.SelectMany(c => c.Files).Select(c => c.Url);

                filesUrls.AddRange(dependencyFiles);
            }

            await _fileLoader.DownloadFilesAsync(filesUrls.ToArray(), directory.FullName, token);

        }

        public async Task<Version?> GetLastVersionAsync(string identifier, string loaderName, CancellationToken token)
        {
            var versions = await GetProjectVersions(identifier, token);

            return versions.OrderByDescending(c => c.DatePublished).FirstOrDefault(c => c.Loaders.Contains(loaderName));
        }
    }
}
