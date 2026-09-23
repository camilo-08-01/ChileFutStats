namespace ChileFutStats.Models
{
    public class Equipo
    {
        public int EquipoId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Slug { get; set; }
        public string? NombreCorto { get; set; }
        public string? CodigoCorto { get; set; }
        public string? ColorPrimario { get; set; }
        public string? ColorSecundario { get; set; }
    }
}