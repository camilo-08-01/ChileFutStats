namespace ChileFutStats.Models
{
    public class Partido
    {
        public long PartidoId { get; set; }
        public int TemporadaId { get; set; }
        public int EquipoLocalId { get; set; }
        public int EquipoVisitaId { get; set; }
        public int? Jornada { get; set; }
        public DateTime FechaHora { get; set; }
        public int? GolesLocal { get; set; }
        public int? GolesVisita { get; set; }
        public string Estado { get; set; } = string.Empty;
    }
}