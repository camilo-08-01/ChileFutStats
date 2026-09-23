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

        public async Task<SofascoreMatchesResponse?> GetMatchesAsync(int tournamentId, int seasonId, int pageIndex = 0)
        {
            return await FetchMatchesAsync("tournaments/get-matches", tournamentId, seasonId, pageIndex);
        }

        public async Task<SofascoreMatchesResponse?> GetNextMatchesAsync(int tournamentId, int seasonId, int pageIndex = 0)
        {
            return await FetchMatchesAsync("tournaments/get-next-matches", tournamentId, seasonId, pageIndex);
        }

        public async Task<SofascoreMatchesResponse?> GetLastMatchesAsync(int tournamentId, int seasonId, int pageIndex = 0)
        {
            return await FetchMatchesAsync("tournaments/get-last-matches", tournamentId, seasonId, pageIndex);
        }

        private async Task<SofascoreMatchesResponse?> FetchMatchesAsync(string endpoint, int tournamentId, int seasonId, int pageIndex)
        {
            var client = _httpClientFactory.CreateClient("Sofascore");

            var response = await client.GetAsync(
                $"{endpoint}?pageIndex={pageIndex}&tournamentId={tournamentId}&seasonId={seasonId}");

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<SofascoreMatchesResponse>(json, _jsonOptions);
        }
    }
}