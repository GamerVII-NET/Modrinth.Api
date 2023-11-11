using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Modrinth.Api.Core.System
{
    public class FileLoader
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public async Task DownloadFileAsync(string url, string destinationDirectory, CancellationToken token)
        {
            Directory.CreateDirectory(destinationDirectory);

            var fileName = Path.GetFileName(url);
            var destinationPath = Path.Combine(destinationDirectory, fileName);

            var fileBytes = await _httpClient.GetByteArrayAsync(url);

            await File.WriteAllBytesAsync(destinationPath, fileBytes, token);
        }

        public Task DownloadFilesAsync(IEnumerable<string> urls, string destinationDirectory, CancellationToken token)
        {
            Directory.CreateDirectory(destinationDirectory);

            var downloadTasks = urls.Select(url => DownloadFileAsync(url, destinationDirectory, token));

            return Task.WhenAll(downloadTasks);
        }
    }
}
