using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeNewsDetectionCache.Authentication
{
    public class InMemoryGetApiKeyQuery : IGetApiKeyQuery
    {
        private readonly IDictionary<string, ApiKey> _apiKeys;
        

        public InMemoryGetApiKeyQuery()
        {
            var existingApiKeys = new List<ApiKey>
        {
            new ApiKey(1, "Extension", "C5BFF7F0-B4DF-475E-A331-F737424F013C", new DateTime(2019, 01, 01),
                new List<string>
                {
                    Roles.Extension,
                }),
            new ApiKey(2, "Developer", "FA872702-6396-45DC-89F0-FC1BE900591B", new DateTime(2019, 06, 01),
                new List<string>
                {
                    Roles.Developer
                }),
            new ApiKey(3, "ProcessingService", "DE420B29-CF80-4928-A463-0931F1D72118", new DateTime(2019, 12, 04),
                new List<string>
                {
                    Roles.ProcessingService
                })
        };

            _apiKeys = existingApiKeys.ToDictionary(x => x.Key, x => x);
        }

        public Task<ApiKey> Execute(string providedApiKey)
        {
            _apiKeys.TryGetValue(providedApiKey, out var key);
            return Task.FromResult(key);
        }
    }

}
