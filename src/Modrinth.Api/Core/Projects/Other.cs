using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Modrinth.Api.Core.System;
using Modrinth.Api.Models.Dto;

namespace Modrinth.Api.Core.Projects
{
    public class Other
    {
        private readonly ModrinthApi _api;
        private readonly HttpClientFactory _httpClientFactory;

        private IEnumerable<MinecraftVersion> _minecraftVersionsStorage = new List<MinecraftVersion>();
        private IEnumerable<Category> _categoriesStorage = new List<Category>();

        public Other(ModrinthApi api, HttpClientFactory httpClientFactory)
        {
            _api = api;
            _httpClientFactory = httpClientFactory;
        }


        public async Task<IEnumerable<MinecraftVersion>> GetMinecraftVersionsAsync(CancellationToken token)
        {
            if (_minecraftVersionsStorage.Any())
                return _minecraftVersionsStorage;

            var request = await _httpClientFactory.HttpClient.GetAsync("/v2/tag/game_version", token);

            if (!request.IsSuccessStatusCode)
                return Enumerable.Empty<MinecraftVersion>();

            var data = await request.Content.ReadAsStringAsync();

            _minecraftVersionsStorage = JsonSerializer.Deserialize<IEnumerable<MinecraftVersion>>(data)
                                        ?? Enumerable.Empty<MinecraftVersion>();

            return _minecraftVersionsStorage;
        }
        public async Task<IEnumerable<Category>> GetCategoriesAsync(CancellationToken token)
        {
            if (_categoriesStorage.Any())
                return _categoriesStorage;

            var request = await _httpClientFactory.HttpClient.GetAsync("/v2/tag/category", token);

            if (!request.IsSuccessStatusCode)
                return Enumerable.Empty<Category>();

            var data = await request.Content.ReadAsStringAsync();

            _categoriesStorage = JsonSerializer.Deserialize<IEnumerable<Category>>(data)
                                 ?? Enumerable.Empty<Category>();

            return _categoriesStorage;
        }
    }
}
