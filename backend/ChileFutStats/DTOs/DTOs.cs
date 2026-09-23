using System.Text.Json.Serialization;

namespace ChileFutStats.DTOs
{
    public class SofascoreStandingsResponse
    {
        [JsonPropertyName("standings")]
        public List<SofascoreStandingGroup> Standings { get; set; } = new();
    }

    public class SofascoreStandingGroup
    {
        [JsonPropertyName("rows")]
        public List<SofascoreStandingRow> Rows { get; set; } = new();
    }

    public class SofascoreStandingRow
    {
        [JsonPropertyName("team")]
        public SofascoreTeam Team { get; set; } = new();

        [JsonPropertyName("position")]
        public int Position { get; set; }

        [JsonPropertyName("matches")]
        public int Matches { get; set; }

        [JsonPropertyName("wins")]
        public int Wins { get; set; }

        [JsonPropertyName("draws")]
        public int Draws { get; set; }

        [JsonPropertyName("losses")]
        public int Losses { get; set; }

        [JsonPropertyName("scoresFor")]
        public int ScoresFor { get; set; }

        [JsonPropertyName("scoresAgainst")]
        public int ScoresAgainst { get; set; }

        [JsonPropertyName("points")]
        public int Points { get; set; }
    }

    public class SofascoreTeam
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("slug")]
        public string? Slug { get; set; }

        [JsonPropertyName("shortName")]
        public string? ShortName { get; set; }

        [JsonPropertyName("nameCode")]
        public string? NameCode { get; set; }

        [JsonPropertyName("teamColors")]
        public SofascoreTeamColors? TeamColors { get; set; }
    }

    public class SofascoreTeamColors
    {
        [JsonPropertyName("primary")]
        public string? Primary { get; set; }

        [JsonPropertyName("secondary")]
        public string? Secondary { get; set; }
    }
}