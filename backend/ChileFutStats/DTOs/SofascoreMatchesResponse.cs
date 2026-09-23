using System.Text.Json.Serialization;

namespace ChileFutStats.DTOs
{
    public class SofascoreMatchesResponse
    {
        [JsonPropertyName("events")]
        public List<SofascoreEvent> Events { get; set; } = new();
    }

    public class SofascoreEvent
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("homeTeam")]
        public SofascoreTeam HomeTeam { get; set; } = new();

        [JsonPropertyName("awayTeam")]
        public SofascoreTeam AwayTeam { get; set; } = new();

        [JsonPropertyName("homeScore")]
        public SofascoreScore? HomeScore { get; set; }

        [JsonPropertyName("awayScore")]
        public SofascoreScore? AwayScore { get; set; }

        [JsonPropertyName("status")]
        public SofascoreStatus Status { get; set; } = new();

        [JsonPropertyName("roundInfo")]
        public SofascoreRoundInfo? RoundInfo { get; set; }

        [JsonPropertyName("startTimestamp")]
        public long StartTimestamp { get; set; }
       
        [JsonPropertyName("tournament")]
        public SofascoreEventTournament? Tournament { get; set; }

        [JsonPropertyName("season")]
        public SofascoreSeason? Season { get; set; }
    }

    public class SofascoreScore
    {
        [JsonPropertyName("current")]
        public int? Current { get; set; }
    }

    public class SofascoreStatus
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;
    }

    public class SofascoreRoundInfo
    {
        [JsonPropertyName("round")]
        public int Round { get; set; }
    }

    public class SofascoreEventTournament
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("uniqueTournament")]
        public SofascoreUniqueTournament? UniqueTournament { get; set; }
    }

    public class SofascoreUniqueTournament
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
    }

    public class SofascoreSeason
    {
        [JsonPropertyName("year")]
        public string Year { get; set; } = string.Empty;
    }
}