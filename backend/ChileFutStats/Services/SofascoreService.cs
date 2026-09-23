using System.Text.Json;
using ChileFutStats.DTOs;

namespace ChileFutStats.Services
{
    public class SofascoreService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public SofascoreService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<SofascoreStandingsResponse?> GetStandingsAsync(int tournamentId, int seasonId)
        {
            var client = _httpClientFactory.CreateClient("Sofascore");

            var response = await client.GetAsync(
                $"tournaments/get-standings?type=total&tournamentId={tournamentId}&seasonId={seasonId}");

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<SofascoreStandingsResponse>(json, _jsonOptions);
        }
    }
}