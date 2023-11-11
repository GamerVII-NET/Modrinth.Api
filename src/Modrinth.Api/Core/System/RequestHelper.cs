using System.Linq;
using System.Net.Http;

namespace Modrinth.Api.Core.System
{
    public static class RequestHelper
    {

        public static void UpdateApiRequestInfo(ModrinthApi api, HttpResponseMessage response)
        {
            var xRateLimit = response.Headers.FirstOrDefault(c => c.Key == "X-Ratelimit-Limit");
            var xRateRemaining = response.Headers.FirstOrDefault(c => c.Key == "X-Ratelimit-Remaining");
            var xRateReset = response.Headers.FirstOrDefault(c => c.Key == "X-Ratelimit-Reset");

            if (xRateLimit.Value.FirstOrDefault() is { } limitValue &&
                int.TryParse(limitValue, out int limit))
                api.Settings.RateLimit.Limit = limit;

            if (xRateRemaining.Value.FirstOrDefault() is { } xRateRemainingValue &&
                int.TryParse(xRateRemainingValue, out int remaining))
                api.Settings.RateLimit.Remaining = remaining;

            if (xRateReset.Value.FirstOrDefault() is { } xRateResetValue &&
                int.TryParse(xRateResetValue, out int reset))
                api.Settings.RateLimit.Reset = reset;
        }
    }
}
