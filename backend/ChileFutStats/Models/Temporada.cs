namespace ChileFutStats.Models
{
    public class Temporada
    {
        public int TemporadaId { get; set; }
        public int UniqueTournamentId { get; set; }
        public string? NombreTorneo { get; set; }
        public int? Anio { get; set; }
    }
}