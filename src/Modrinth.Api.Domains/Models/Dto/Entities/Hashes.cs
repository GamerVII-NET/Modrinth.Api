using System.Text.Json.Serialization;

namespace Modrinth.Api.Domains.Models.Dto.Entities
{
    public class Hashes
    {
        [JsonPropertyName("sha512")] public string Sha512 { get; set; }

        [JsonPropertyName("sha1")] public string Sha1 { get; set; }
    }
}
